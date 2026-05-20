# SYS-POSTLOCK-DELTA-REGISTER-001 evidence

**Date:** 2026-05-21  
**Baseline:** `SYSTEM-ALPHA-006`  
**Owner repo:** `ontogony-platform`

**Verdict:** PASS — every companion repo has classified post-lock groups; Conexus pre-lock batch remains validated at ALPHA-006.

**Non-claims:** Not production readiness; does not enable real external tool execution.

## Issue

`SYS-POSTLOCK-DELTA-REGISTER-001` — Post-lock delta register ahead of Operator V1 RC.

## Repos changed

| Repo | Role |
| --- | --- |
| `ontogony-platform` | Canonical register, validator, CI docs-gate, this evidence |
| (indexed) | `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, `ontogony-frontend`, `ontogony-ui` |

## Files changed

| Path | Change |
| --- | --- |
| `docs/system/post-lock-delta-register.json` | Machine-readable cross-repo register |
| `scripts/validate-post-lock-delta-register.ps1` | Structure + lock SHA alignment validator |
| `tests/Ontogony.Infrastructure.Tests/SysPostLockDeltaRegister001Tests.cs` | Register guard tests |
| `.github/workflows/ci.yml` | `docs-gates` step |
| `docs/evidence/README.md` | Index link |

## Tests run

```powershell
cd C:\dev\ontogony-platform
dotnet test Ontogony.Platform.sln -c Release --filter "FullyQualifiedName~SysPostLockDeltaRegister001"
.\scripts\validate-post-lock-delta-register.ps1
```

## Docker/browser validation

Not required — classification and governance only.

## Operator behavior

Operators and integrators use the register to see which post-ALPHA-006 deltas are **moving-main** vs **validated at lock**, and which Conexus APIs are safe but not yet wired in the frontend.

## Safety statement

- Real external tool execution remains blocked.
- Semantic authority remains in Kanon; register does not change runtime policy.
- No production-readiness claim is made.

## Known limitations

- `movingMainHeadSha` values are point-in-time (2026-05-21); refresh before the next lock cut.
- Register does not auto-diff git; run `git log <lock>..HEAD` per repo when updating.
- `ontogony-frontend` / `ontogony-ui` are companion repos (not in `ontogony-runtime.lock.json` `lockedCommits`).

## Next step

`ALLAGMA-OPERATOR-RUNS-001` (Sprint B).

## Related

| Repo | Detail evidence |
| --- | --- |
| Conexus | [CONEXUS_POSTLOCK_001_CLASSIFICATION.md](../../../conexus-dotnet/docs/evidence/CONEXUS_POSTLOCK_001_CLASSIFICATION.md) |
| Allagma lock | [SYSTEM_ALPHA_006_CLOSEOUT.md](../../../allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md) |
| Register | [`post-lock-delta-register.json`](../system/post-lock-delta-register.json) |
