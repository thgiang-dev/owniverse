# OWNIVERSE

Workspace preparation for OWNIVERSE. No application skeleton or business feature has been implemented.

## Start here

Read [AGENTS.md](AGENTS.md), then the approved specifications in [docs](docs/README.md). `/docs` is the source of truth. The document inventory identifies specifications that still need to be supplied; setup notes do not replace them.

## Repository layout

```text
OWNIVERSE/
|-- frontend/       Vue 3 + Vite + TypeScript (not scaffolded)
|-- backend/        ASP.NET Core Web API (not scaffolded)
|-- ai-worker/      Python worker (not scaffolded)
|-- tests/          Cross-project tests (not implemented)
|-- deployment/     Deployment configuration (not implemented)
|-- docs/           Approved specifications and document inventory
|-- scripts/        Development scripts (not implemented)
|-- .github/        Repository workflows (not implemented)
|-- .env.example
|-- .gitignore
|-- AGENTS.md
`-- README.md
```

## Environment

Install Git, VS Code or an IDE with a coding agent, Docker Desktop, Node.js with npm/pnpm, .NET SDK and Python. A GitHub account is required for remote collaboration. PostgreSQL and RabbitMQ will run in Docker later; do not install them directly on Windows for this setup.

```powershell
git --version
node --version
npm --version
dotnet --version
python --version
docker --version
docker info
```

Installed tool versions do not establish project version requirements. Select and pin application/runtime versions from the approved specifications during the authorized skeleton task.

`.env.example` contains configuration names and safe example defaults. A local `.env` is ignored by Git. It is not necessary to create one during documentation setup.

## Development workflow

Use one implementation agent for the initial phase:

1. Read relevant specifications and agree on the requested task scope.
2. Implement the scoped task.
3. Build and run relevant tests.
4. Have a separate review session inspect the diff.
5. Fix findings, verify affected behavior, and commit.

## Documentation checkpoint

Import all approved, updated specifications listed in [docs/README.md](docs/README.md), resolve missing documents and confirm the current sprint. Then review staged files and create checkpoint 0:

```powershell
git add .
git diff --cached --check
git diff --cached --stat
git diff --cached
git commit -m "docs: initialize OWNIVERSE specifications"
```

A workspace-only commit does not replace this specifications checkpoint.

## Next task (not started)

After documentation checkpoint 0 and a new user instruction, Sprint 0 - Task 0.1 will initialize only:

- Vue 3 + Vite + TypeScript frontend.
- ASP.NET Core backend solution.
- Python AI worker package.

Verify all three projects can start/build independently. Do not implement Project, Story, Character, AI generation or other business features in that task.
