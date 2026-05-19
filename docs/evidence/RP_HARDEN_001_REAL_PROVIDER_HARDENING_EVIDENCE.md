# RP-HARDEN-001 — Real-provider hardening cross-repo evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — RP-005 follow-up gaps addressed or documented

**Boundary:** Hardening only after **Real provider validation v1 CLOSED / PASS**. **Not production readiness.** No secrets in git. No CI real-provider calls. Fake provider remains default.

## Scope

| Track | Repo | Outcome |
| --- | --- | --- |
| A — Guided-flow report | `allagma-dotnet` | Fixed `selectedProviderKey` → `providerKey` on route decision record |
| B — Token metrics | `conexus-dotnet`, `allagma-dotnet` | Stream usage capture + eval metric tests; `route_token_usage_available` when absent |
| C — Frontend visibility | `ontogony-frontend` | Unit tests PASS; Docker rebuild steps documented |

## Sibling evidence

| Repo | Doc |
| --- | --- |
| Allagma | `allagma-dotnet/docs/evidence/RP_HARDEN_001_REAL_PROVIDER_REPORTING_METRICS_EVIDENCE.md` |
| Conexus | `conexus-dotnet/docs/evidence/RP_HARDEN_001_TOKEN_USAGE_EVIDENCE.md` |
| Frontend | `ontogony-frontend/docs/evidence/RP_HARDEN_001_FRONTEND_PROVIDER_VISIBILITY_EVIDENCE.md` |

## Local `.env` posture

| Check | Expected |
| --- | --- |
| `docker/local-working-system/.env` gitignored | Yes |
| `CONEXUS_REAL_PROVIDER_ENABLED` default | `false` |
| OpenAI key | Local file only — never commit |

## Policy unchanged

- Fake provider default for compose/bootstrap
- `CONEXUS_REAL_PROVIDER_ENABLED` env-only opt-in (not BO UI)
- RP-005 results remain valid; this hardening narrows known limitations

## Next options

See [`docs/releases/REAL_PROVIDER_VALIDATION_NEXT_OPTIONS.md`](../releases/REAL_PROVIDER_VALIDATION_NEXT_OPTIONS.md) — `PROD-READINESS-001` remains separate.

## Required statement

```text
RP-HARDEN-001 hardens RP-005 reporting, token metrics, and frontend visibility documentation.
Not production readiness.
```
