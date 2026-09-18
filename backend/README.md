# Backend development

Run commands below from `backend/`. Use the .NET SDK pinned by `global.json`.

## PostgreSQL configuration

Task 0.2 uses EF Core with Npgsql in `src/Persistence`, following the permitted
ORM choice in DATA-01 section 101 and the layout in DEV-01 section 7.
`OwniverseDbContext` intentionally has no business entities or tables.

Supply `DATABASE_URL` through the process environment before starting the API.
The name follows DEPLOY-01; its value is an Npgsql **connection string**, not a
`postgres://` URI. `Host`, `Database` and `Username` must be explicit. Credentials
must match an existing local development database; the example is a template.

```powershell
$env:DATABASE_URL = 'Host=localhost;Port=5432;Database=owniverse;Username=owniverse;Password=<local-password>'
$env:RABBITMQ_URL = 'amqp://<user>:<password>@localhost:5672/%2F'
$env:ASSET_STORAGE_PROVIDER = 'local'
$env:ASSET_STORAGE_PATH = 'local-data/assets'
dotnet restore Owniverse.slnx
dotnet build Owniverse.slnx --no-restore --configuration Release
dotnet run --project src/Api --configuration Release --no-build -- --urls http://localhost:5080
```

Do not commit real credentials. `.env.example` documents the logical variable;
`.env` is ignored by Git. ASP.NET Core does **not** automatically load `.env`:
export its value into the process environment through your local tooling.
Avoid putting actual passwords in checked-in scripts or shared terminal logs.

Missing, blank or malformed configuration prevents API startup with a
`DATABASE_URL` validation error. Valid configuration permits startup even when
the database is temporarily unavailable; readiness reports that failure.

## RabbitMQ configuration

`RABBITMQ_URL` is required by the API and worker, matching DEV-01 and the existing
repository convention (DEPLOY-01 calls this logical setting `QUEUE_URL`). Supply
an `amqp://` or `amqps://` URI with explicit host, username and password. Percent-
encode credentials and virtual-host names; `%2F` selects the `/` vhost. Query
parameters and fragments are rejected to keep backend/worker interpretation aligned.
Configuration errors stop startup without exposing the supplied URI.

`Shared/Messaging/IMessagePublisher` accepts opaque bytes, content type and a
destination. The RabbitMQ implementation in `Infrastructure/Messaging` sends to
an existing queue through the default exchange, with persistent delivery,
mandatory routing and publisher confirmations. It reuses a connection and a
serialized publisher channel; publication has a ten-second bound. Connection or
confirmation failures propagate as `MessagePublishException`; caller cancellation
propagates as cancellation. An interrupted publish can have an unknown outcome.
No automatic resend, Job retries or background connection-recovery loop is added.
A later operation can establish a new connection if the previous one is closed.

No queues/exchanges, Job schemas, consumers, outbox or Job state are created by
application startup. Destination provisioning belongs to a later task; no queue
name is fixed here. RabbitMQ is transport only. Persisted PostgreSQL Job state
will be authoritative when Job functionality is implemented.

## Local asset storage

Task 0.4 registers `IAssetStorage` from `Shared/Storage` with `LocalFileAssetStorage`
in `Infrastructure/Storage`. Operations follow DATA-02: `UploadAsync`,
`OpenReadAsync`, `ExistsAsync`, `DeleteAsync`. No asset metadata or authorization
workflow is implemented here. No cloud dependency or placeholder S3 class is added.

`ASSET_STORAGE_PROVIDER` defaults to `local`; other values are rejected.
`ASSET_STORAGE_PATH` is required. Relative paths resolve against the API content
root, not the shell working directory. With `dotnet run --project src/Api`, the
example uses `backend/src/Api/local-data/assets`. These directories are ignored
by Git. Absolute paths can be supplied locally for a mounted development volume.
The storage root must be outside compiled application output; directories are
created on first upload/readiness probe, not by configuration validation.

Keys are logical server-generated identifiers, for example
`projects/abc/images/123.png`, never physical paths or original upload filenames.
Segments start with an ASCII letter/digit and contain only ASCII letters, digits,
periods, underscores and hyphens (maximum 255 characters each). Empty segments,
trailing periods, Windows device names, absolute paths, backslashes, colons and
percent-encoded input are rejected. Keys are not URL-decoded. Full paths are
canonicalized and checked against the root boundary; existing symlinks/junctions
and other reparse points in the path, root or ancestors are rejected.

The configured directory and its ancestors must be controlled by the application
or trusted administrators. These portable filesystem checks are not an isolation
boundary against another OS process swapping directories/links concurrently.
Use filesystem permissions to prevent such mutation. Keep server-generated key
casing consistent because filesystem case sensitivity depends on the host.

Upload reads from the caller's current stream position without closing it. It
streams to a temporary file in the destination directory, then moves it to the
final key with overwrite disabled. Existing keys fail with `IOException` and
remain unchanged. Interrupted/failed uploads do not expose a partial object;
temporary files are removed during normal exception/cancellation cleanup. A hard
process crash can leave a staging file; no background cleanup job is introduced.
Read returns a caller-owned stream. Missing read throws `FileNotFoundException`,
exists returns false, and delete is idempotent for missing objects. Delete removes
only the physical object; future business code must enforce reference/provenance
rules. Access URLs belong to later asset access/API work.

## Health checks

- `GET /health/live`: HTTP 200 while the API is serving requests; no dependency probes.
- `GET /health/ready`: runs PostgreSQL `SELECT 1` and checks RabbitMQ via an
  authenticated connection plus an AMQP channel open/close round trip, plus a
  local storage write/read/delete probe required by SPRINT-01 section 14.
  HTTP 200 / `Healthy` requires all three dependencies; any failure returns
  HTTP 503 / `Unhealthy`. Each check has a five-second timeout and omits secrets.

```powershell
Invoke-WebRequest http://localhost:5080/health/live
Invoke-WebRequest http://localhost:5080/health/ready
```

These checks currently cover the process, PostgreSQL, RabbitMQ and local asset storage. Additional
infrastructure belongs to its own tasks.

## Verification

After building, run the dependency-free HTTP smoke tests with Python:

```powershell
python -m unittest discover -s tests -p "*_smoke.py" -v
```

The tests cover infrastructure configurations, credential redaction, unavailable dependency
readiness and independent liveness. `DATABASE_URL` enables a real PostgreSQL probe;
`RABBITMQ_URL` enables authenticated broker and publisher tests. Both enable the
combined HTTP readiness success test. Missing integration settings produce explicit
skips; supplied but unreachable dependencies fail their integration tests.

`tests/InfrastructureSmoke` is a dependency-free test harness (no test-framework
packages). It can check `postgresql` or `rabbitmq` individually, or run a
`publisher` round trip using opaque test bytes in a temporary exclusive queue,
then verify that an unroutable publication fails. This is not a business message.
Test hosts and temporary resources are disposed after checks. Database probes only
run `SELECT 1` and do not change schema or data.

```powershell
dotnet run --project tests/InfrastructureSmoke --configuration Release --no-build -- postgresql
dotnet run --project tests/InfrastructureSmoke --configuration Release --no-build -- rabbitmq
```

Task 0.3 does not provision a broker or Docker Compose. Successful authenticated
connectivity must be checked against a running instance; failure-path tests do not
substitute for that verification.

Asset tests need no PostgreSQL/RabbitMQ. They use temporary directories and verify
streaming integrity, nested keys, missing objects, overwrite rejection, concurrent
uploads, failed/cancelled upload cleanup, unsafe keys, junction/symlink rejection,
configuration validation and storage readiness. API smoke tests also isolate
storage under temporary directories and clean them after stopping the test host.

```powershell
dotnet run --project tests/InfrastructureSmoke --configuration Release --no-build -- storage-tests
python -m unittest discover -s tests -p "storage_smoke.py" -v
```

## Migrations

No migration is needed to initialize this empty EF Core model. No database,
business table, seed data or migration history table is created by this task.
Startup does not call `EnsureCreated` or `Migrate`. Add version-controlled EF Core
migrations and design-time tooling with the first actual schema change, following
DEV-01 and DEPLOY-01. Docker/container provisioning is outside Task 0.2.
