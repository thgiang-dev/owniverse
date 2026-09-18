"""Verify real failure paths; successful broker connectivity is opt-in."""

import os
import socket
import subprocess
import sys
import unittest
from unittest.mock import patch

from owniverse_worker.messaging import connection_parameters


def run_worker(value):
    environment = os.environ.copy()
    environment.pop("RABBITMQ_URL", None)
    if value is not None:
        environment["RABBITMQ_URL"] = value
    return subprocess.run([sys.executable, "-m", "owniverse_worker", "--check-rabbitmq"],
                          env=environment, capture_output=True, text=True, timeout=15)


class MessagingTests(unittest.TestCase):
    def test_invalid_configuration_does_not_leak_credentials(self):
        secret = "smoke-secret-do-not-log"
        cases = [None, "", "   ", f"http://user:{secret}@localhost",
                 f"amqp://user:{secret}@localhost:99999/%2F",
                 f"amqp://:{secret}@localhost/%2F", "amqp://localhost/%2F",
                 f"amqp://user:{secret}@localhost/a/b",
                 f"amqp://user:{secret}@localhost/%2F?heartbeat=1"]
        for index, value in enumerate(cases):
            with self.subTest(case=index):
                result = run_worker(value)
                self.assertEqual(result.returncode, 2)
                self.assertIn("RABBITMQ_URL is required", result.stderr)
                self.assertNotIn(secret, result.stdout + result.stderr)

    def test_unavailable_broker_fails_without_leaking_credentials(self):
        with socket.socket() as reservation:
            reservation.bind(("127.0.0.1", 0))
            result = run_worker(
                f"amqp://test:smoke-secret-do-not-log@127.0.0.1:{reservation.getsockname()[1]}/%2F")
            self.assertEqual(result.returncode, 1)
            self.assertIn("RabbitMQ connection failed", result.stderr)
            self.assertNotIn("smoke-secret-do-not-log", result.stdout + result.stderr)

    def test_percent_encoded_credentials_and_virtual_host(self):
        with patch.dict(os.environ, {"RABBITMQ_URL": "amqp://user:p%40ss@localhost/%2F"}):
            parameters = connection_parameters()
        self.assertEqual(parameters.credentials.password, "p@ss")
        self.assertEqual(parameters.virtual_host, "/")
        for suffix in ("", "/"):
            with patch.dict(os.environ, {"RABBITMQ_URL": "amqp://user:pass@localhost" + suffix}):
                self.assertEqual(connection_parameters().virtual_host, "/")

    @unittest.skipUnless(os.environ.get("RABBITMQ_URL"), "No RABBITMQ_URL: authenticated broker check skipped")
    def test_authenticated_broker(self):
        result = run_worker(os.environ["RABBITMQ_URL"])
        self.assertEqual(result.returncode, 0, result.stderr)


if __name__ == "__main__":
    unittest.main(verbosity=2)
