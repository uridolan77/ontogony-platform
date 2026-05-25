# AGM-DEPTH-001 to AGM-DEPTH-007 — Allagma implementation-depth slice

## Objective

Raise `allagma-dotnet` implementation depth from 8.5 to 9.1+ by closing runtime gaps: replay, topology E2E, streaming path, MAF depth, and lock evidence.

## AGM-DEPTH-001 — Postgres replay records

Add Postgres persistence for replay requests, eligibility summaries, bundles, deltas, and replay run/event idempotency evidence.

Acceptance:

- replay request survives Allagma restart;
- replay run/event idempotency verified after restart;
- no raw prompt/completion is stored;
- in-memory mode remains valid for local dev.

## AGM-DEPTH-002 — Conexus route-decision dry-run orchestration

Extend cross-service replay coordinator to request Conexus route-decision dry-run where route decision ID exists, merge result into replay bundle/eligibility, and surface failures as `CrossServiceErrorEnvelope`.

Acceptance:

- tests cover model-call-only, route-decision-only, both, and unavailable Conexus;
- evidence graph links Allagma replay request to Conexus dry-run.

## AGM-DEPTH-003 — Topology authorization E2E automation

Automate low-risk paths where topology decision is intentionally null and high-risk authorization paths where Kanon topology decision exists.

Acceptance:

- system cohesion runner can include topology authorization scenario;
- evidence distinguishes `not_required` from `missing`.

## AGM-DEPTH-004 — Streaming governed path as formal smoke

Add `run-system-cohesion-smoke.ps1 -IncludeStreaming` and verify stream start/chunk/completion/interruption events, idempotency-header omission, and run correlation without raw content storage by default.

## AGM-DEPTH-005 — MAF workflow depth 002

Deepen adapter proof with branch/condition workflow fixture, checkpoint/resume fixture, human gate in MAF workflow path, and evidence graph linkage. Core projects must still have no MAF references.

## AGM-DEPTH-006 — Alias readiness check integration

After Conexus alias readiness exists, Allagma readiness/system smoke should verify configured `Allagma:ModelPurposes:*:ConexusModelAlias` values are registered and capability-compatible.

## AGM-DEPTH-007 — Runtime lock promotion preparation

Update runtime lock only after all above passes. Refresh evidence pointers and reduce/reset post-lock deltas.

## Validation commands

```powershell
./scripts/bootstrap-solution.ps1
dotnet restore Allagma.sln
dotnet build Allagma.sln -c Release --no-restore
dotnet test Allagma.sln -c Release --no-build --filter "Category!=CrossRepo&Category!=PersistenceSmoke"
./scripts/architecture-conformance/run-cross-repo-conformance.ps1
./scripts/validate-runtime-lock.ps1
./scripts/validate-feature-connection-matrix.ps1 -DevRoot C:/dev
./scripts/system/run-system-cohesion-acceptance.ps1
./scripts/smoke/run-governed-maf-e2e.ps1 -Strict
```
