# Documentation inventory

`/docs` is the source of truth for approved OWNIVERSE specifications.

**Status: awaiting the user's approved, updated documents.** No ARCH, DOMAIN, AI, DATA, API, DEV, IMPLEMENT, SPRINT, DEPLOY, QA or RESEARCH specifications were present in the initial workspace. This inventory is a setup aid, not a substitute specification.

| Specification | Status |
| --- | --- |
| ARCH-01 | Awaiting source |
| ARCH-02 | Awaiting source |
| DOMAIN-* (all approved documents) | Awaiting source |
| AI-01 | Awaiting source |
| AI-02 | Awaiting source |
| AI-03 | Awaiting source |
| DATA-01 | Awaiting source |
| DATA-02 | Awaiting source |
| API-01 | Awaiting source |
| API-02 | Awaiting source |
| DEV-01 | Awaiting source |
| IMPLEMENT-01 | Awaiting source |
| IMPLEMENT-02 | Awaiting source |
| SPRINT-01 (current scope) | Awaiting source |
| DEPLOY-* (all approved documents) | Awaiting source |
| QA (all approved documents) | Awaiting source |
| RESEARCH (all approved documents) | Awaiting source |

## Import procedure

1. Obtain the approved, updated source documents from the user. Preserve their contents and identifiers; do not infer missing specifications from filenames or conversation summaries.
2. Copy the originals into `/docs`, keeping meaningful filenames and relative links. Do not delete or move the source originals.
3. Update this inventory with each document's actual relative path and version/date where specified. Ask the user to resolve duplicate or conflicting versions.
4. Verify DEV-01 and SPRINT-01 define the rules and current scope, and verify the full required document set is present.
5. Review the Git diff and commit documentation checkpoint 0 before application scaffolding.
