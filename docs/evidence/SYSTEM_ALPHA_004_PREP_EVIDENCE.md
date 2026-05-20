# SYSTEM-ALPHA-004 prep evidence

**Status:** IN PROGRESS — validation recorded below as commands complete. Do not treat as baseline PASS until all sections show PASS and `ontogony-runtime.lock.json` is refreshed.

## Baseline transition

| Field | Value |
| --- | --- |
| Previous baseline | `SYSTEM-ALPHA-003` |
| Target baseline | `SYSTEM-ALPHA-004` |
| Scope | Sprint 1 service-hardening (items 5–10) + FE-SANDBOX-LABELS-001 |

## Sprint 1 items included

- `KANON-POSTGRES-LOCAL-001`
- `KANON-EVIDENCE-001`
- `CONEXUS-TOOLS-001`
- `CONEXUS-GOV-DRILLDOWN-001`
- `ALLAGMA-TOOL-TRUST-001`
- `ALLAGMA-SANDBOX-OBS-001`
- `FE-SANDBOX-LABELS-001` (frontend execution-mode operator labels)

## Repos and commits to lock

Recorded after validation from `main` (or working tree if uncommitted):

| Repo | Commit (working tree at validation time) |
| --- | --- |
| `ontogony-platform` | `cdc87304ed711b52842cea5e367922bfa8e59561` |
| `conexus-dotnet` | `0e2040835fc5cfc78bc903734e68b2cc810e959b` |
| `kanon-dotnet` | `9ce1f44707e786cd34a6feebf4d3481ab9f7fe7f` |
| `allagma-dotnet` | `1b82f702a301a4ff219d4f6b0712c2be9c2ca5d3` |
| `ontogony-frontend` | `7e1ad2797f30dd4b7da9c24a0b6dd3922b9985a5` (uncommitted FE-SANDBOX-LABELS-001 changes) |

## Governance

| Check | Result |
| --- | --- |
| `03_ACCEPTANCE_MATRIX.md` Status column populated | PASS (2026-05-20) |
| Sprint 1 addendum reconciled | PASS (existing + FE follow-up in progress) |

## Frontend (FE-SANDBOX-LABELS-001)

| Check | Result |
| --- | --- |
| `formatToolExecutionMode.ts` + Vitest | PASS |
| Capabilities tool mode catalog | Implemented |
| Run event timeline execution-mode badge | Implemented (`AllagmaRunEventStream`) |
| Audit sandbox primary/observed modes | Implemented |
| Evidence export preserves execution-mode fields | PASS (audit export spreads bundle) |
| Evidence doc | `ontogony-frontend/docs/evidence/FE_SANDBOX_LABELS_001_EVIDENCE.md` |

## Validation commands

### Frontend

```powershell
cd C:\dev\ontogony-frontend
npm run openapi:sync:allagma
npm run openapi:gen
npm test -- --run src/allagma/sandbox src/allagma/adapters/allagmaSandboxEvidenceAdapters.test.ts src/allagma/adapters/allagmaExecutionAdapters.test.ts src/allagma/components/AllagmaSandboxEvidencePanel.test.tsx
```

### Backend (representative)

```powershell
cd C:\dev\kanon-dotnet
dotnet build Kanon.sln -c Release
./scripts/run-postgres-local-acceptance.ps1 -WriteEvidenceSummary

cd C:\dev\conexus-dotnet
dotnet build Conexus.sln -c Release
dotnet test Conexus.sln --filter "FullyQualifiedName~Tools|FullyQualifiedName~ModelCall|FullyQualifiedName~Governance|FullyQualifiedName~Drilldown"

cd C:\dev\allagma-dotnet
dotnet build Allagma.sln -c Release
dotnet test Allagma.sln --filter "FullyQualifiedName~ToolTrust|FullyQualifiedName~SandboxObs|FullyQualifiedName~Capabilities|FullyQualifiedName~Audit"
```

### Docker-local

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build conexus-api kanon-api allagma-api ontogony-frontend
docker compose up -d

cd C:\dev\ontogony-platform\docker\scripts
./seed-and-verify-local-working-system.ps1
./run-evidence-spine-008a-docker-live-verification.ps1 -Seed
```

## Docker-local rebuild evidence

| Check | Result | Artifact |
| --- | --- | --- |
| Stack rebuild API images | PASS (2026-05-20) | `docker compose build conexus-api kanon-api allagma-api` |
| Frontend image rebuild | FAIL | `npm run build` fails on pre-existing Vitest/TS errors in unrelated `*.test.tsx` files |
| Conexus/Kanon/Allagma health | PASS | ports 5081–5083 healthy |
| Conexus model-call admin list/detail 200 | PASS | Bearer `cx-conexus-admin-dev` |
| Allagma capabilities `toolExecutionModes` | PASS | GET `/allagma/v0/capabilities` returns four modes + `defaultMode` |

## System E2E

| Check | Result | Artifact |
| --- | --- | --- |
| Timestamped system E2E summary | _pending_ | `artifacts/system-e2e/<timestamp>/summary.json` |

## Restart survival

| Check | Result | Artifact |
| --- | --- | --- |
| Data survives service restart | _pending_ | `artifacts/restart-e2e/<timestamp>/summary.json` |

## Observability

| Check | Result | Artifact |
| --- | --- | --- |
| Trace/correlation/model-call evidence | _pending_ | `artifacts/observability/<timestamp>/observability-summary.json` |

## Evidence Spine Docker-live

| Check | Result | Artifact |
| --- | --- | --- |
| Cross-service links (Allagma/Kanon/Conexus) | _pending_ | `docker/local-working-system/artifacts/evidence-spine-008a-docker-live-report.json` |
| Conexus model-call routes not 404 | _pending_ | |

## Allagma browser QA

| Check | Result | Artifact |
| --- | --- | --- |
| Actionability workbench on rebuilt images | _pending_ | `ontogony-frontend/docs/evidence/ALLAGMA_ACTION_007_*` |
| Sandbox execution-mode labels visible | _pending_ | |

## Open blockers

- **Runtime lock not refreshed** — `ontogony-runtime.lock.json` remains `SYSTEM-ALPHA-003` until full E2E/restart/observability/evidence-spine/browser QA pass on a rebuilt stack including frontend.
- **Frontend Docker build** — `tsc -b` includes test files with stale mocks (`StartRunWorkbenchPage.test.tsx`, `ConexusChatPage.test.tsx`, etc.); blocks `ontogony-frontend` image rebuild.
- **Evidence spine script** — `run-evidence-spine-008a-docker-live-verification.ps1 -Seed` exited early during stack start (needs rerun after compose stable).
- **Allagma filtered test suite** — broader filter hit `RunAuditEvidenceDocsTests` vocabulary snapshot drift (1 failure); `SandboxObs` / capabilities tests **PASS** (9/9).
- Real external tool execution remains blocked by design.

## Safety

Real external tool execution remains blocked. `RealExecutionEnabled=true` still fails startup validation until SANDBOX-003.
