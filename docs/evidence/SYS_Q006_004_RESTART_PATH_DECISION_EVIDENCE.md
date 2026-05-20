# SYS-Q006-004-RESTART-PATH-DECISION evidence (platform)

**Date:** 2026-05-21  
**Baseline:** `SYSTEM-ALPHA-006`  
**Owner repo:** `ontogony-platform` (canonical doc); `allagma-dotnet` (script guard)

**Verdict:** PASS — operators have a single canonical Docker-local restart path.

**Non-claims:** Not production readiness.

## Deliverables

| Item | Path |
| --- | --- |
| Canonical operator runbook | [`docs/operators/CANONICAL_RESTART_PATH.md`](../operators/CANONICAL_RESTART_PATH.md) |
| Allagma decision + guard | [allagma-dotnet `SYS_Q006_004_RESTART_PATH_DECISION_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/SYS_Q006_004_RESTART_PATH_DECISION_EVIDENCE.md) |
| Protocol registry | `quarantines[Q-006-004].status = cleared` in [`system-protocol-registry.json`](../system/system-protocol-registry.json) |

## Canonical command

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose restart allagma-api
.\scripts\wait-local-working-system.ps1
```

## Related

- [docker/local-working-system/README.md](../../docker/local-working-system/README.md)
