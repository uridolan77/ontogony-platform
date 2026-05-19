# RP-003A — Live real provider completion evidence (platform)

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**

**Boundary:** Records RP-003A closeout. **Not production readiness.**

## Summary

RP-003 left a **classified** gap: Conexus `provider_transport_error` in Docker while host smoke passed. RP-003A identified runtime TLS trust as the cause, fixed Conexus `Dockerfile`, and proved:

```text
Conexus live OpenAI probe     PASS
Allagma real subject run      PASS (providerKey=openai)
Eval evidence export          PASS
Fake regression after kill    PASS
```

## Deliverables

| Repo | Item |
| --- | --- |
| `conexus-dotnet` | Runtime CA in `Dockerfile`; `docs/evidence/RP_003A_PROVIDER_TRANSPORT_ROOT_CAUSE_EVIDENCE.md` |
| `allagma-dotnet` | `docs/evidence/RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md` |
| `ontogony-platform` | `docker/local-working-system/scripts/run-rp-003a-live-provider-validation.ps1` |

## Operator entry

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
.\scripts\run-rp-003a-live-provider-validation.ps1
```

## Sequence update

```text
RP-003   DONE — classified transport failure acceptable for package
RP-003A  DONE — live completion proven; supersedes “no successful real call” gap
RP-004   DONE
RP-005   DONE (manual QA — see RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md)
RP-CLOSEOUT-001   NEXT
```

## Required statement

```text
Real provider path and failure classification were validated in RP-003.
One successful live real-provider completion was proven in RP-003A.
Not production readiness.
```
