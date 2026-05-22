# SYS-PROTOCOL-REGISTRY-001 evidence

**Date:** 2026-05-20  
**Baseline:** `SYSTEM-ALPHA-006`  
**Owner repo:** `ontogony-platform`

**Verdict:** PASS — registry validates against schema, paths resolve under DevRoot, baseline matches `ontogony-runtime.lock.json`.

**Non-claims:** Not production readiness; does not enable real external tool execution.

## Deliverables

| Item | Path |
| --- | --- |
| Registry | [`docs/system/system-protocol-registry.json`](../system/system-protocol-registry.json) |
| Schema | [`docs/system/schemas/system-protocol-registry.schema.json`](../system/schemas/system-protocol-registry.schema.json) |
| Validator | [`scripts/validate-system-protocol-registry.ps1`](../../scripts/validate-system-protocol-registry.ps1) |
| Sibling materializer (CI) | [`scripts/materialize-registry-siblings.ps1`](../../scripts/materialize-registry-siblings.ps1) |
| Index | [`docs/system/README.md`](../system/README.md) |

## Acceptance mapping

| Criterion | Result |
| --- | --- |
| Registry validates against schema | PASS — structural checks in validator |
| Each referenced file exists | PASS — `validate-system-protocol-registry.ps1` @ `DevRoot=C:\dev` |
| Answers route/auth/env/error/evidence artifact questions | PASS — `protocols[]` + `evidence[]` indexed by `kind` / `ownerRepo` |
| CI or script fails on drift / dead paths | PASS — validator wired in `.github/workflows/ci.yml` |

## Validator (local)

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-protocol-registry.ps1
# expect: system-protocol-registry.json OK (baseline=SYSTEM-ALPHA-006, DevRoot=C:\dev)
```

## Quarantines (registry snapshot)

| Id | Status | Notes |
| --- | --- | --- |
| B-012 | cleared | SYS-OBS-004A |
| Q-006-004 | open | Process restart script vs compose ports — non-blocking |

## Related

- Runtime lock: [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json)
- Closeout: [`allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md)
- Intake package absorbed into registry (2026-05-20); no `_incoming` archive in repo.
