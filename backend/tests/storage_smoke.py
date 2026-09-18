"""Filesystem, DI, startup configuration and readiness checks for local asset storage."""

import subprocess
import tempfile
from pathlib import Path
import unittest

from persistence_smoke import BACKEND, get, run_api, wait_for_api
from messaging_smoke import DATABASE, PROBE


class StorageSmokeTests(unittest.TestCase):
    def test_storage_operations_and_security(self):
        result = subprocess.run(["dotnet", str(PROBE), "storage-tests"], cwd=BACKEND,
                                capture_output=True, text=True, encoding="utf-8", timeout=30)
        self.assertEqual(result.returncode, 0, result.stdout + result.stderr)
        self.assertIn("Temporary test assets cleaned up", result.stdout)

    def test_invalid_storage_configuration_prevents_startup(self):
        cases = [({"ASSET_STORAGE_PROVIDER": "s3"}, "ASSET_STORAGE_PROVIDER"),
                 ({"ASSET_STORAGE_PROVIDER": ""}, "ASSET_STORAGE_PROVIDER"),
                 ({"ASSET_STORAGE_PATH": None}, "ASSET_STORAGE_PATH"),
                 ({"ASSET_STORAGE_PATH": "   "}, "ASSET_STORAGE_PATH")]
        for settings, expected in cases:
            with self.subTest(settings=settings), run_api(DATABASE, storage_settings=settings) as (process, _, log):
                self.assertNotEqual(process.wait(timeout=20), 0)
                log.seek(0)
                self.assertIn(expected, log.read())

    def test_unusable_storage_root_preserves_liveness(self):
        with tempfile.TemporaryDirectory(prefix="owniverse-storage-failure-") as directory:
            blocker = Path(directory) / "file"
            blocker.write_text("unchanged", encoding="utf-8")
            with run_api(DATABASE, storage_settings={"ASSET_STORAGE_PATH": str(blocker)}) as (process, port, _):
                wait_for_api(process, port)
                self.assertEqual(get(port, "/health/ready"), (503, "Unhealthy"))
                self.assertEqual(get(port, "/health/live"), (200, "Healthy"))
            self.assertEqual(blocker.read_text(encoding="utf-8"), "unchanged")


if __name__ == "__main__":
    unittest.main(verbosity=2)
