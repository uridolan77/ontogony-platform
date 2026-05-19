# Trace and correlation contract (operator)

**Scope:** Docker-local cross-service proof for `TRACE-CONTRACT-001`. **Not production readiness.**

## Canonical headers

| Header | Role |
| --- | --- |
| `X-Ontogony-Trace-Id` | End-to-end trace; echoed on every HTTP response |
| `X-Ontogony-Correlation-Id` | Operation-level correlation; distinct from trace when both are sent |
| `X-Allagma-Run-Id` | Allagma run spine; propagated to Kanon and Conexus metadata |

Legacy inbound aliases (`X-Athanor-Trace-Id`, `X-Agentor-Trace-Id`, `X-Conexus-Request-Id`, `X-Correlation-ID`) remain accepted during migration. See `docs/03_TRACE_CORRELATION_STANDARD.md`.

## Expected chain (probe)

```text
POST /allagma/v0/runs
  (X-Ontogony-Trace-Id, X-Ontogony-Correlation-Id)
    → response X-Ontogony-Trace-Id echoes trace
    → Allagma.RunCreated payload: traceId, correlationId
    → Kanon GET /decision-records/{planningDecisionId}
    → Kanon GET /decision-records/by-trace/{traceId}
    → Conexus GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
         metadata.allagma_run_id, metadata.correlation_id
```

W3C `traceparent` (OpenTelemetry/Jaeger) is related but **not identical** to Ontogony `X-Ontogony-Trace-Id`. For OTLP cross-service spans, see Allagma `docs/observability/CROSS_REPO_TRACE_LOOP.md`.

## Docker-local verification

Prerequisites: compose stack healthy on host ports **5081** (Kanon), **5082** (Conexus), **5083** (Allagma).

```powershell
cd C:\dev\ontogony-platform

.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
.\docker\local-working-system\scripts\validate-trace-correlation-evidence-report.ps1
```

Report (local, redacted): `docker/local-working-system/artifacts/trace-contract-001-evidence-report.json`

Tokens and ports load from `.env` / `.env.example`:

- `ALLAGMA_SERVICE_TOKEN`, `KANON_SERVICE_TOKEN`, `CONEXUS_ADMIN_API_KEY`
- `ALLAGMA_HOST_PORT`, `KANON_HOST_PORT`, `CONEXUS_HOST_PORT`

Optional guided replay (after seed or guided flow):

```powershell
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1 -SkipFrontend
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
```

## Operator questions

| Question | Answer |
| --- | --- |
| Why are trace and correlation different? | Trace is the stable request lineage id; correlation scopes a logical operation (often a run step batch). They must not collapse to the same value when both are explicitly set. |
| Where is trace on Kanon? | `traceId` on decision records; list via `GET /ontology/v0/decision-records/by-trace/{traceId}` with `ProvenanceReader` role. |
| Where is correlation on Conexus? | Execution journal metadata `correlation_id` (admin diagnostics route). |
| Host-local cohesion smoke? | Allagma `scripts/run-system-cohesion-smoke.ps1` scenario `correlation_chain` — same contract, host URLs. |

## Related docs

| Doc | Repo |
| --- | --- |
| `docs/03_TRACE_CORRELATION_STANDARD.md` | ontogony-platform |
| `docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md` | allagma-dotnet |
| `docs/integrations/IDEMPOTENCY_AND_TRACE_HEADERS.md` | kanon-dotnet |
| `docs/environments/.../TRACE-CONTRACT-001.md` | ontogony-platform (acceptance spec) |
| `ontogony-frontend/docs/operators/FRONTEND_DOCKER_LOCAL_CONTRACT.md` | ontogony-frontend (run detail UI checks) |
