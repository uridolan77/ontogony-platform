# TRACE-CONTRACT-001 — Cross-service trace/correlation contract

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** (documentation + script contract)  
**Statement:** Post-Docker-local hardening operator proof for Allagma → Kanon → Conexus trace/correlation; **not production readiness**.

## Scope

Document and automate the cross-service trace/correlation contract for Docker-local stacks. Add `inspect-trace-correlation-evidence.ps1` (correlation probe + optional guided replay) and report validator. No service `src/` changes unless an evidence gap is proven (none required for this slice).

## Delivered

```text
docker/local-working-system/scripts/_docker-local-env.ps1 (Conexus config helpers)
docker/local-working-system/scripts/inspect-trace-correlation-evidence.ps1
docker/local-working-system/scripts/validate-trace-correlation-evidence-report.ps1
docker/local-working-system/README.md
docs/operators/TRACE_CORRELATION_CONTRACT.md
docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/TRACE-CONTRACT-001.md
docs/environments/compose-to-docker-closeout-package-v2/01_PR_SEQUENCE.md
docs/environments/compose-to-docker-closeout-package-v2/04_STATUS_BOARD.md
docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md
docs/evidence/TRACE_CONTRACT_001_EVIDENCE.md
allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md (link)
allagma-dotnet/docs/observability/CROSS_REPO_TRACE_LOOP.md (link)
kanon-dotnet/docs/integrations/IDEMPOTENCY_AND_TRACE_HEADERS.md (link)
```

## Probe checks

| Layer | Check |
| --- | --- |
| Allagma | Distinct inbound trace + correlation; response echoes trace; `RunCreated` payload matches |
| Kanon | Planning decision `traceId`, `correlationId`, `allagmaRunId`; by-trace index includes planning decision |
| Conexus | Execution journal `allagma_run_id`, `correlation_id` match probe |

## Commands (operator)

```powershell
cd C:\dev\ontogony-platform

.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
.\docker\local-working-system\scripts\validate-trace-correlation-evidence-report.ps1

Select-String -Path .\docker\local-working-system\artifacts\trace-contract-001-evidence-report.json `
  -Pattern 'allagma-dev-service-token|kanon-dev-service-token|cx-conexus-admin-dev|_local_pw'
# Expected: no matches
```

## Validation checks (repo diff)

| Check | Result |
| --- | --- |
| No Allagma/Kanon/Conexus `src/` changes | **yes** |
| No workflow changes | **yes** |
| No secrets committed | **yes** |
| JSON report local only | **yes** |
| Production readiness claimed | **no** |

## Live validation (2026-05-19)

Stack healthy on host ports **5081** / **5082** / **5083**.

| Check | Result |
| --- | --- |
| `inspect-trace-correlation-evidence.ps1` | **PASS** |
| `validate-trace-correlation-evidence-report.ps1` | **PASS** |
| Probe run status | `Completed` |
| Kanon by-trace decision count | 2 (includes planning) |
| Guided replay | **PASS** (`docker-guided-main-flow-report.json`, subject trace indexed) |

Artifact (local, not committed): `docker/local-working-system/artifacts/trace-contract-001-evidence-report.json`

## Follow-up

| PR | Focus |
| --- | --- |
| `FE-HARDEN-001` | DONE — Frontend hardening beyond walkthrough |
