# Allagma actionability workbench — known limitations

Consolidated from ACTION-000 through ACTION-007. Not a production readiness statement.

## Run operations v2 (006)

- **Retry** only when run status is `Failed`; creates a new run from immutable inputs.
- **Cancel** only for non-terminal runs; does not stop in-flight Conexus calls retroactively.
- **Replay** records a manifest on terminal runs; does not re-execute planning/model work.
- **Deny human gate** has no Allagma route — operators use Kanon resolve, then optional Allagma resume.
- Per-run contract: `GET /allagma/v0/runs/{runId}/operations` — UI falls back to OpenAPI-only inference if route missing (older deployments).

## Evaluation and baseline

- **Manual evaluation write** (`POST …/evaluations`) is policy-gated and disabled in production configs.
- **Promote baseline/eval** is not an HTTP mutation — `promotionRecommendation` is read-only metadata.
- Baseline compare create requires valid baseline + subject run + scenario identifiers.

## Evidence and export

- Run audit and eval evidence exports are **per-resource**; no bulk export route.
- Related eval evidence from run detail depends on evaluations existing for that run.
- Replay & evidence page exports client-built bundles; live replay trigger uses run detail operations panel when deployed.

## Operator environment

- Local service tokens and fake Conexus provider are **Docker-local posture** only.
- **Docker image lag:** if `allagma-api` was not rebuilt after ACTION-006, operations/retry/cancel/replay return 404 at the host edge even though source and unit tests pass.
- Browser verification requires rebuilt `ontogony-frontend` image with provenance env args.

## Cross-service

- Cross-links to Kanon decisions/domain packs depend on fields present on Allagma run GET (often sparse).
- Evidence Spine package is separate — this workbench does not implement a unified graph resolver.
