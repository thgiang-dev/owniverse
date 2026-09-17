# OWNIVERSE Engineering Rules

Read `/docs` before making architectural decisions. `/docs` is the source of truth for approved project specifications. Workspace setup notes are not approved architecture or sprint specifications.

## Locked stack

- Frontend: Vue 3 + Vite + TypeScript
- Backend: ASP.NET Core Web API
- Database: PostgreSQL
- Queue: RabbitMQ
- AI Worker: Python
- Asset Storage: IAssetStorage
- Development Storage: LocalFileAssetStorage
- Production-like Storage: S3AssetStorage

## Core rules

- Follow DEV-01.
- Follow the current SPRINT-01 scope.
- Do not invent architecture.
- Do not change public API contracts without specification.
- Do not change database schema without migrations.
- Do not refactor unrelated code.
- AI Worker must never mutate Canon directly.
- Candidate is not Canon.
- RabbitMQ is not the Job source of truth.
- Frontend is not the source of truth.
- GenerationContextV1 is the AI application contract.
- Raw technical prompts belong to AI adapters.
- Context Snapshots are immutable.
- Retry and Regeneration are different.
- Binary assets must go through IAssetStorage.
- Never commit real secrets or local `.env` files.
- Use one implementation agent at a time. A separate reviewer may review the diff; resolve findings with the implementation agent before committing.

## Before coding

1. Read relevant specifications, including DEV-01 and the current SPRINT-01.
2. If required specifications are missing or conflicting, ask the user to supply or clarify them before implementing the dependent work. Do not fill gaps with invented requirements.
3. Inspect existing code.
4. State a short implementation plan.
5. Implement only the requested task.

## After coding

1. Build affected projects.
2. Run relevant tests.
3. Fix failures caused by the change.
4. Review the diff.
5. Report files changed, tests, migrations and remaining risks.

## Current phase

Workspace and documentation preparation only. Do not scaffold applications or implement business features during this phase. Begin Sprint 0 - Task 0.1 only after the approved documentation is imported, checkpoint 0 is committed, and the user requests the next task.
