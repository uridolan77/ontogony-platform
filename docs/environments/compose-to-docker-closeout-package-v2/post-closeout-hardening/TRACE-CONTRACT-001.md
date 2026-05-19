# TRACE-CONTRACT-001 — Cross-service trace/correlation contract (Docker-local)

## Purpose

Make the **Allagma → Kanon → Conexus** trace and correlation contract **operator-verifiable** in Docker-local flows after `KANON-OP-002`.

Operators must be able to prove:

```text
POST /allagma/v0/runs (distinct X-Ontogony-Trace-Id + X-Ontogony-Correlation-Id)
  → Allagma run/events carry traceId + correlationId
  → Kanon decision records (planning + by-trace index) match
  → Conexus execution journal metadata matches runId + correlationId
```

## Timing

Post-`ENV-DOCKER-CLOSEOUT-001`. Does not block the closed Docker-local milestone.

## Boundary

- Operator visibility, probe scripts, and documentation **first**.
- No new trace middleware behavior unless an evidence gap requires a small fix (not expected).
- No OTLP/Jaeger requirement for this slice (see Allagma `docs/observability/CROSS_REPO_TRACE_LOOP.md`).
- No real provider keys, no real external execution.
- **Not production readiness.**

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-platform` | `_docker-local-env.ps1` (Conexus config), `inspect-trace-correlation-evidence.ps1`, `validate-trace-correlation-evidence-report.ps1`, README, spec, evidence, `docs/operators/TRACE_CORRELATION_CONTRACT.md` |
| `allagma-dotnet` | Link from `docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md` and `docs/observability/CROSS_REPO_TRACE_LOOP.md` |
| `kanon-dotnet` | Link from `docs/integrations/IDEMPOTENCY_AND_TRACE_HEADERS.md` |

## Canonical contract (summary)

| ID | Header / field | Rule |
| --- | --- | --- |
| Trace | `X-Ontogony-Trace-Id` | Canonical; echoed on every HTTP response |
| Correlation | `X-Ontogony-Correlation-Id` | Distinct from trace when both sent; stored as Kanon `correlationId` and Conexus journal `correlation_id` |
| Run | `X-Allagma-Run-Id` / `runId` | Allagma-owned; propagated to Kanon `allagmaRunId` and Conexus `allagma_run_id` |
| W3C | `traceparent` | Telemetry lineage; related but not identical to Ontogony trace id |

Full standard: `docs/03_TRACE_CORRELATION_STANDARD.md`. Cross-repo matrix: `allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md`.

## Acceptance criteria

| # | Topic | Expected |
| --- | --- | --- |
| 1 | Probe run | `POST /allagma/v0/runs` with distinct trace + correlation ids succeeds |
| 2 | Response echo | Response includes `X-Ontogony-Trace-Id` matching inbound trace |
| 3 | Allagma events | `Allagma.RunCreated` payload `traceId` / `correlationId` match probe values; correlation ≠ trace |
| 4 | Kanon planning | `GET /decision-records/{planningDecisionId}` has matching `traceId`, `correlationId`, `allagmaRunId` |
| 5 | Kanon by-trace | `GET /decision-records/by-trace/{traceId}` lists planning decision |
| 6 | Conexus journal | `GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}` metadata has `allagma_run_id` + `correlation_id` |
| 7 | Guided replay (optional) | When guided/seed report exists, subject run events expose a trace id queryable on Kanon |
| 8 | Report safety | No raw service tokens or API keys in JSON artifacts |
| 9 | No unjustified runtime changes | Docs + scripts only unless evidence gap proven |

## Operator verification

```powershell
cd C:\dev\ontogony-platform

# Prerequisite: stack healthy (5081 Kanon, 5082 Conexus, 5083 Allagma)
.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend

# Correlation probe + cross-service evidence
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
.\docker\local-working-system\scripts\validate-trace-correlation-evidence-report.ps1
```

Optional: run after guided flow for replay section:

```powershell
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1 -SkipFrontend
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-platform` | `docs/evidence/TRACE_CONTRACT_001_EVIDENCE.md` |
| `ontogony-platform` | `docs/operators/TRACE_CORRELATION_CONTRACT.md` |

## Follow-up

| PR | Focus |
| --- | --- |
| `FE-HARDEN-001` | Frontend hardening beyond walkthrough |
