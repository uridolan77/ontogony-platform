# Add a provider or model alias (Conexus)

> **Audience:** backend developer (Conexus)  
> **Applies to:** `conexus-dotnet`, local Docker bootstrap  
> **Source of truth:** Conexus admin API, dev bootstrap scripts  
> **Last verified:** 2026-05-25

## Overview

**Conexus** owns providers, deployments, **model aliases**, and route resolution. Allagma calls through the gateway with an alias — not raw provider endpoints.

## Local dev (fake provider)

Governed smokes bootstrap a fake provider + `gpt-4o-mini` alias automatically:

```powershell
# Called from cohesion / governed-fake scripts against Conexus admin API
POST http://localhost:5082/admin/v0/dev/bootstrap
```

Docker: start script seeds via compose env — see [`../../docker/local-working-system/README.md`](../../docker/local-working-system/README.md).

## Checklist (new alias or provider)

1. **Provider adapter** — Implement in Conexus (not platform mechanics).
2. **Admin config** — Register provider, credentials via secret references (platform helpers).
3. **Alias** — Map product alias → provider model id.
4. **Routing** — Route policy, fallback, quota if applicable.
5. **OpenAPI** — Update Conexus snapshot + `ontogony-frontend` `openapi:sync:conexus`.
6. **Allagma** — Wire `conexusModelAlias` on run templates / purposes if needed.
7. **UI** — Conexus observability + alias admin pages.
8. **Tests** — Provider unit tests; optional RP-003A live validation for real keys (local only).

## Real provider validation (optional)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-rp-003a-live-provider-validation.ps1
```

Requires `CONEXUS_PROVIDER_OPENAI_API_KEY` — not for CI by default.

## Operator capability truth (v2)

After adding or changing a provider, verify inventory shows **`CapabilityProfile`** (support states: `supported`, `partial`, `simulated`, `unavailable`, `rejected`, `planned`, …) — not boolean guesses.

| Doc | Path |
| --- | --- |
| Capability matrix | `conexus-dotnet/docs/providers/PROVIDER_CAPABILITY_MATRIX.md` |
| Parity closure | `conexus-dotnet/docs/evidence/CONEXUS_PROVIDER_PARITY_001_EVIDENCE.md` |
| Console UI | Conexus observability + provider inventory chips (`ontogony-frontend/src/conexus/`) |

Checks:

```powershell
cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Application.Tests --filter FullyQualifiedName~ProviderParity001

cd C:\dev\ontogony-frontend
npm run conexus:route-parity:check
npm run conexus:operator-ui-coverage:check
```

## References

- [07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md](./07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md)
- `conexus-dotnet/AGENTS.md`
- `conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`
