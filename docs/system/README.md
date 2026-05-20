# System protocol registry

**SYS-PROTOCOL-REGISTRY-001** — machine-readable index of cross-repo protocol surfaces (routes, auth, env, errors, trace, evidence, frontend catalogs, runtime lock).

| Artifact | Purpose |
| --- | --- |
| [`system-protocol-registry.json`](./system-protocol-registry.json) | Canonical registry for baseline **SYSTEM-ALPHA-006** |
| [`schemas/system-protocol-registry.schema.json`](./schemas/system-protocol-registry.schema.json) | JSON Schema (structural contract) |
| Runtime lock (authoritative SHAs) | [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json) |

## Validate locally

From a `DevRoot` with sibling repos (for example `C:\dev`):

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-protocol-registry.ps1
```

## Query protocol surfaces

Each `protocols[]` entry answers: *which repo owns this surface, what kind it is, and which files define it?*

```powershell
$reg = Get-Content docs/system/system-protocol-registry.json | ConvertFrom-Json
$reg.protocols | Where-Object kind -eq 'error' | Format-Table id, ownerRepo, paths
```

## CI

`scripts/validate-system-protocol-registry.ps1` runs in platform CI (full gate) and fails on baseline/lock drift, missing paths, or placeholder SHAs. See [`docs/evidence/SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md`](../evidence/SYSTEM_PROTOCOL_REGISTRY_001_EVIDENCE.md).

## Incoming packages (SYS-STALE-PACKAGE-GUARD-001)

Before adopting ZIPs under `docs/_incoming/`, run [`scripts/validate-stale-incoming-package.ps1`](../../scripts/validate-stale-incoming-package.ps1) and follow [`INCOMING_PACKAGE_RUNBOOK.md`](./INCOMING_PACKAGE_RUNBOOK.md). Patterns: [`stale-incoming-package-patterns.json`](./stale-incoming-package-patterns.json). Evidence: [`docs/evidence/SYS_STALE_PACKAGE_GUARD_001_EVIDENCE.md`](../evidence/SYS_STALE_PACKAGE_GUARD_001_EVIDENCE.md).

**Non-claims:** Registry completeness does not imply production readiness or unlocked real external tool execution.
