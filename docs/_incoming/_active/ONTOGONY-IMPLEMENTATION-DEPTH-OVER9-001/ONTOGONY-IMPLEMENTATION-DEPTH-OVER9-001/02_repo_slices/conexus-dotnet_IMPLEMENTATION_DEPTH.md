# CX-DEPTH-001 to CX-DEPTH-005 — Conexus implementation-depth slice

## Objective

Raise `conexus-dotnet` implementation depth from 8.5 to 9.1+ by making gateway operations deeper, more durable, and more testable.

## CX-DEPTH-001 — Durable maintenance-job history

Implement durable maintenance-run history for Postgres mode.

Scope:

- retention run history;
- project-erasure run history;
- maintenance status endpoint reads durable history when Postgres is enabled;
- in-memory mode keeps current behavior but reports non-durable mode explicitly.

Suggested files:

```text
src/Conexus.Infrastructure/Persistence/Entities/MaintenanceRunEntity.cs
src/Conexus.Infrastructure/Persistence/Stores/EfMaintenanceRunStore.cs
src/Conexus.Application/Maintenance/*
tests/Conexus.Tests/Maintenance/*
docs/evidence/CX_DEPTH_001_DURABLE_MAINTENANCE_HISTORY_EVIDENCE.md
```

Acceptance:

- restart test proves maintenance history survives process restart in Postgres mode;
- maintenance APIs do not leak secrets;
- known limitation updated or narrowed.

## CX-DEPTH-002 — Streaming posture hardening

Do not implement durable streaming replay yet unless explicitly designed. Instead, make streaming posture deep and operator-visible.

Scope:

- stream lifecycle evidence completeness;
- first-byte and interruption metrics;
- streamed tool delta classification;
- explicit reason when streaming idempotency is rejected;
- admin model-call detail exposes streaming lifecycle summary.

Acceptance:

- streaming evidence lifecycle tests pass;
- `Idempotency-Key` remains rejected for streaming;
- docs explain why streaming replay is not equivalent to non-stream replay.

## CX-DEPTH-003 — Provider capability profile v3

Deepen provider capability enforcement for JSON mode, tool calling, streamed tool deltas, multimodal limits, fake-provider boundaries, and route-preview explanations.

Acceptance:

- capability registry tests cover OpenAI-compatible, OpenAI, Anthropic, Gemini, fake;
- route-preview surfaces capability gates before runtime call;
- provider capability matrix generated or validated.

## CX-DEPTH-004 — Routing/fallback evidence pack

Make fallback behavior operationally provable:

- deterministic fake primary failure + fallback success fixture;
- admin evidence route shows route decision chain;
- Allagma-initiated completion can correlate to route decision.

Acceptance:

- local/system smoke can exercise fallback;
- Conexus telemetry records primary failure and fallback success;
- no raw prompt/response logs by default.

## CX-DEPTH-005 — Alias readiness and deployment guard

Expose a read-only alias readiness check for Allagma model-purpose aliases. No Allagma dependency.

Acceptance:

- `risk-summary-v0` and `risk-summary-stream-v0` style aliases can be validated in local dev;
- failures are typed and operator-readable.

## Validation commands

```powershell
dotnet restore Conexus.sln
dotnet build Conexus.sln --no-restore -c Release -p:NoWarn=CS1591
dotnet test Conexus.sln --no-build -c Release -p:NoWarn=CS1591 --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline"
powershell -NoProfile -File scripts/check-doc-freshness.ps1
```
