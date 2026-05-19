# RP-002 — Conexus real provider local mode evidence (platform)

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — Conexus gate shipped; platform evidence pointer

**Boundary:** Records RP-002 closeout for the cross-repo package. **No platform runtime changes.** **No real-provider calls from this repo.** **Not production readiness.**

## Conexus deliverable (sibling repo)

| Item | Location |
| --- | --- |
| Implementation + tests | `uridolan77/conexus-dotnet` — commit on `main` (RP-002) |
| Conexus evidence | [`conexus-dotnet/docs/evidence/RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md`](https://github.com/uridolan77/conexus-dotnet/blob/main/docs/evidence/RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md) |
| Operator script | `conexus-dotnet/scripts/validate-real-provider-local.ps1` |
| Policy (unchanged authority) | [`docs/operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](../operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md) |

## Acceptance mapping

| Criterion | Result |
| --- | --- |
| One real call succeeds **or** failure precisely classified | **PASS** — `403` classes for disabled gate / missing key; live call optional via local script |
| Route / model-call evidence | **PASS** (automated gate tests); live evidence when operator runs script with local key |
| Fake-provider regression | **PASS** |
| No secrets | **PASS** |
| Not production readiness | **PASS** |

## Operator command (Docker-local stack)

```powershell
# From conexus-dotnet with API at compose port 5082
.\scripts\validate-real-provider-local.ps1 -BaseUrl http://localhost:5082 `
  -ProjectApiKey cx-dev-key-change-me -AdminApiKey cx-conexus-admin-dev
```

Compose placeholders: `docker/local-working-system/.env.example` (`CONEXUS_REAL_PROVIDER_ENABLED=false`).

## Next step

**`RP-003`** — Allagma real-provider guided flow (`prompts/RP-003_ALLAGMA_REAL_PROVIDER_GUIDED_FLOW.md`).

## Required statement

```text
RP-002 adds Conexus local real-provider opt-in gates after RP-001 policy.
Fake provider remains the default.
No CI real-provider calls.
Not production readiness.
```
