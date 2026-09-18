# Worker infrastructure smoke check

Run commands from `ai-worker/` using the existing virtual environment:

```powershell
.venv/Scripts/python -m pip install -e .
$env:RABBITMQ_URL = 'amqp://<user>:<password>@localhost:5672/%2F'
.venv/Scripts/python -m owniverse_worker --check-rabbitmq
.venv/Scripts/python -m unittest discover -s tests -v
```

`RABBITMQ_URL` follows DEV-01 and the repository environment convention (the
logical `QUEUE_URL` in DEPLOY-01). Use an existing broker account and virtual host.
Percent-encode credentials and virtual-host names; `%2F` selects the `/` vhost.
Use `amqps` for TLS. Query parameters and fragments are not accepted, so backend
and worker interpret the same connection settings. `.env` is not loaded
automatically; export settings into the process environment. Never commit secrets.

The entrypoint validates configuration, authenticates, opens/closes a channel and
exits. The default invocation performs the same connectivity check. Exit codes:
0 = connected, 2 = invalid/missing configuration, 1 = connection failed. Errors
omit credentials. Connection attempts are bounded and are not automatically retried.

This checks infrastructure connectivity only, not readiness for AI workload.
It does not declare queues, consume/publish messages, load models or process Jobs.
RabbitMQ is transport; future PostgreSQL Job records will be authoritative.

Unit/smoke tests require no broker. The authenticated connectivity test is skipped
unless `RABBITMQ_URL` is supplied; a supplied but unreachable broker fails the test.
