"""RabbitMQ configuration/readiness regression checks against the built backend."""

import os
from pathlib import Path
import socket
import subprocess
import unittest

from persistence_smoke import BACKEND, get, run_api, wait_for_api

PROBE = BACKEND / "tests/InfrastructureSmoke/bin/Release/net10.0/InfrastructureSmoke.dll"
DATABASE = os.environ.get("DATABASE_URL", "Host=127.0.0.1;Port=1;Database=test;Username=test;Timeout=1")


def probe(mode, database, rabbitmq):
    environment = os.environ.copy()
    environment.update(DATABASE_URL=database, RABBITMQ_URL=rabbitmq)
    return subprocess.run(["dotnet", str(PROBE), mode], env=environment, cwd=BACKEND,
                          capture_output=True, text=True, encoding="utf-8", timeout=25)


class MessagingSmokeTests(unittest.TestCase):
    def test_invalid_configuration_fails_without_exposing_credentials(self):
        secret = "smoke-secret-do-not-log"
        cases = [None, "", "   ", f"http://user:{secret}@localhost",
                 f"amqp://user:{secret}@localhost:99999/%2F",
                 f"amqp://:{secret}@localhost/%2F",
                 "amqp://localhost/%2F", f"amqp://user:{secret}@localhost/a/b",
                 f"amqp://user:{secret}@localhost/%2F?heartbeat=1"]
        for index, value in enumerate(cases):
            with self.subTest(case=index), run_api(DATABASE, value) as (process, _, log):
                self.assertNotEqual(process.wait(timeout=20), 0)
                log.seek(0)
                output = log.read()
                self.assertIn("RABBITMQ_URL is required", output)
                self.assertNotIn(secret, output)

    def test_unavailable_rabbitmq_does_not_affect_liveness(self):
        with socket.socket() as reservation:
            reservation.bind(("127.0.0.1", 0))
            url = f"amqp://test:smoke-secret-do-not-log@127.0.0.1:{reservation.getsockname()[1]}/%2F"
            with run_api(DATABASE, url) as (process, port, log):
                wait_for_api(process, port)
                self.assertEqual(get(port, "/health/ready"), (503, "Unhealthy"))
                self.assertEqual(get(port, "/health/live"), (200, "Healthy"))
                log.seek(0)
                self.assertNotIn("smoke-secret-do-not-log", log.read())
            result = probe("publisher-unavailable", DATABASE, url)
            self.assertEqual(result.returncode, 0, result.stdout + result.stderr)

    @unittest.skipUnless(os.environ.get("DATABASE_URL"), "No DATABASE_URL: real PostgreSQL probe skipped")
    def test_postgresql_independently_of_rabbitmq(self):
        result = probe("postgresql", DATABASE, "amqp://test:test@127.0.0.1:1/%2F")
        self.assertEqual(result.returncode, 0, result.stdout + result.stderr)
        with socket.socket() as reservation:
            reservation.bind(("127.0.0.1", 0))
            unreachable = f"Host=127.0.0.1;Port={reservation.getsockname()[1]};Database=test;Username=test;Timeout=1"
            result = probe("postgresql", unreachable, "amqp://test:test@127.0.0.1:1/%2F")
            self.assertEqual(result.returncode, 1)
            self.assertIn("postgresql: Unhealthy", result.stdout)

    @unittest.skipUnless(os.environ.get("RABBITMQ_URL"), "No RABBITMQ_URL: real broker checks skipped")
    def test_authenticated_broker_and_publisher(self):
        for mode in ("rabbitmq", "publisher"):
            with self.subTest(mode=mode):
                result = probe(mode, DATABASE, os.environ["RABBITMQ_URL"])
                self.assertEqual(result.returncode, 0, result.stdout + result.stderr)


if __name__ == "__main__":
    unittest.main(verbosity=2)
