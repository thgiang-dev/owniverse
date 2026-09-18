"""Black-box persistence checks. Build the Release solution before running."""

from contextlib import contextmanager
import os
from pathlib import Path
import socket
import subprocess
import tempfile
import time
import unittest
from urllib.error import HTTPError, URLError
from urllib.request import ProxyHandler, build_opener

BACKEND = Path(__file__).resolve().parents[1]
API = BACKEND / "src/Api/bin/Release/net10.0/Owniverse.Api.dll"
HTTP = build_opener(ProxyHandler({}))


def get(port, path):
    try:
        response = HTTP.open(f"http://127.0.0.1:{port}{path}", timeout=10)
    except HTTPError as error:
        response = error
    with response:
        return response.status, response.read().decode()


@contextmanager
def run_api(connection, rabbitmq_url=os.environ.get("RABBITMQ_URL", "amqp://test:test@127.0.0.1:1/%2F"), storage_settings=None):
    with socket.socket() as reservation:
        reservation.bind(("127.0.0.1", 0))
        port = reservation.getsockname()[1]
    environment = os.environ.copy()
    environment.pop("DATABASE_URL", None)
    environment.pop("RABBITMQ_URL", None)
    if rabbitmq_url is not None:
        environment["RABBITMQ_URL"] = rabbitmq_url
    if connection is not None:
        environment["DATABASE_URL"] = connection
    environment["ASPNETCORE_ENVIRONMENT"] = "Production"
    with tempfile.TemporaryDirectory(prefix="owniverse-api-storage-") as storage_root, \
            tempfile.TemporaryFile(mode="w+", encoding="utf-8") as log:
        environment["ASSET_STORAGE_PROVIDER"] = "local"
        environment["ASSET_STORAGE_PATH"] = storage_root
        for key, value in (storage_settings or {}).items():
            if value is None:
                environment.pop(key, None)
            else:
                environment[key] = value
        process = subprocess.Popen(
            ["dotnet", str(API), "--urls", f"http://127.0.0.1:{port}"],
            cwd=BACKEND, env=environment, stdout=log, stderr=subprocess.STDOUT,
        )
        try:
            yield process, port, log
        finally:
            if process.poll() is None:
                process.terminate()
            try:
                process.wait(timeout=10)
            except subprocess.TimeoutExpired:
                process.kill()
                process.wait(timeout=5)


def wait_for_api(process, port):
    deadline = time.monotonic() + 20
    while time.monotonic() < deadline:
        if process.poll() is not None:
            raise AssertionError("API exited before becoming live; check database configuration.")
        try:
            if get(port, "/health/live")[0] == 200:
                return
        except (URLError, TimeoutError):
            pass
        time.sleep(0.1)
    raise AssertionError("API did not become live within 20 seconds.")


class PersistenceSmokeTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        if not API.is_file():
            raise RuntimeError("Build backend/Owniverse.slnx in Release configuration first.")

    def test_invalid_configuration_fails_without_exposing_credentials(self):
        sentinel = "smoke-secret-do-not-log"
        cases = [
            None, "", "   ",
            f"postgres://user:{sentinel}@localhost/db",
            f"Host=localhost;Database=owniverse;Username=test;Password={sentinel};Port=invalid",
            f"Database=owniverse;Username=test;Password={sentinel}",
            f"Host=localhost;Username=test;Password={sentinel}",
            f"Host=localhost;Database=owniverse;Password={sentinel}",
        ]
        for index, connection in enumerate(cases):
            with self.subTest(case=index), run_api(connection) as (process, _, log):
                self.assertNotEqual(process.wait(timeout=20), 0)
                log.seek(0)
                output = log.read()
                self.assertIn("DATABASE_URL is required", output)
                self.assertNotIn(sentinel, output)

    def test_unavailable_database_is_not_ready_but_api_stays_live(self):
        # Reserve a port without listening so no real database can answer it.
        with socket.socket() as unavailable:
            unavailable.bind(("127.0.0.1", 0))
            db_port = unavailable.getsockname()[1]
            connection = (
                f"Host=127.0.0.1;Port={db_port};Database=owniverse;Username=test;"
                "Password=smoke-secret-do-not-log;Timeout=1"
            )
            with run_api(connection) as (process, port, log):
                wait_for_api(process, port)
                self.assertEqual(get(port, "/health/ready"), (503, "Unhealthy"))
                self.assertEqual(get(port, "/health/live"), (200, "Healthy"))
                log.seek(0)
                self.assertNotIn("smoke-secret-do-not-log", log.read())

    @unittest.skipUnless(os.environ.get("DATABASE_URL") and os.environ.get("RABBITMQ_URL"),
                         "DATABASE_URL and RABBITMQ_URL required: combined readiness check skipped")
    def test_configured_dependencies_are_ready(self):
        with run_api(os.environ["DATABASE_URL"]) as (process, port, _):
            wait_for_api(process, port)
            self.assertEqual(get(port, "/health/ready"), (200, "Healthy"))


if __name__ == "__main__":
    unittest.main(verbosity=2)
