# Migration — PLATFORM-RC-001 substrate contract freeze (2026-05-22)

## Summary

Documents the **protocol registry baseline promotion** from `SYSTEM-ALPHA-006` to `SYSTEM-RC-001A` and publication of the **0.4 alpha-RC substrate contract** without changing the shipped NuGet line (`0.3.0-alpha.1`).

## Who must react

| Repo | Action |
| --- | --- |
| `ontogony-platform` | Use [`ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md`](../releases/ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md) as RC substrate authority |
| `allagma-dotnet` | Runtime lock already at `SYSTEM-RC-001A`; no lock change in this PR |
| `kanon-dotnet`, `conexus-dotnet`, `ontogony-frontend` | None for registry refresh; continue RC evidence at lock SHAs |
| CI / operators | Run `validate-system-protocol-registry.ps1` with `DevRoot` containing siblings at lock commits when validating paths |

## Breaking changes

**None** to public NuGet APIs. Registry `baseline` string changed; scripts that hard-coded `SYSTEM-ALPHA-006` on the registry must accept `SYSTEM-RC-001A` (validator updated).

## Validation

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-protocol-registry.ps1
```
