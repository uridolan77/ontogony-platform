# Validation matrix — Contract Discipline > 9

## Repo-local required gates

| Repo | Gates |
| --- | --- |
| `ontogony-platform` | package inventory, system compatibility gate, protocol registry, error envelope, header propagation, public API snapshots |
| `conexus-dotnet` | OpenAPI snapshots, DTO golden snapshots, compatibility manifest, package-mode client proof, provider matrix tests |
| `kanon-dotnet` | route inventory, client coverage, doc fragments, OpenAPI baseline, manifest, server-only policy, v0 freeze, v1 guard |
| `allagma-dotnet` | route inventory, OpenAPI snapshot, runtime lock, post-lock delta register, feature connection matrix, package-mode proof |

## Cross-repo required gates

```powershell
# Platform
pwsh ./scripts/run-system-compatibility-gate.ps1

# Kanon
pwsh ./scripts/update-route-doc-fragments.ps1
pwsh ./scripts/update-kanon-openapi-baseline.ps1
pwsh ./scripts/update-kanon-compatibility-manifest.ps1

# Allagma
./scripts/validate-runtime-lock.ps1 -ReleaseMode
./scripts/validate-feature-connection-matrix.ps1 -DevRoot C:/dev
./scripts/architecture-conformance/run-cross-repo-conformance.ps1
```

## Closeout acceptance

- No route-count contradictions between generated manifests and narrative docs.
- Runtime lock and system compatibility matrix agree.
- Package versions agree across Allagma lock and repo package docs.
- Intentional error-shape exceptions are documented and tested.
- Header propagation conformance is tested in consumers.
