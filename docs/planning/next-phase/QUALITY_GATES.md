# Ontogony.Platform next-phase quality gates

## Every PR

- `dotnet restore Ontogony.Platform.sln`
- `dotnet build Ontogony.Platform.sln --no-restore -c Release`
- `dotnet test Ontogony.Platform.sln --no-build -c Release`
- `./scripts/validate-docs-links.ps1`
- `./scripts/validate-docs-api-names.ps1`
- `./scripts/validate-ai-runtime-boundaries.ps1`
- `./scripts/validate-shipping-inventory.ps1`
- `./scripts/validate-ai-runtime-docs.ps1`
- `./scripts/validate-package-levels.ps1`
- `./scripts/validate-dependency-baseline.ps1`
- `./scripts/validate-conexus-consumer-baseline-alignment.ps1`

## Package/release PRs

- `./scripts/pack-all.ps1 -NoBuild`
- `./scripts/validate-nupkg-coordination-path-hygiene.ps1` (after pack; rejects donor/agent/incoming paths inside `.nupkg` archives)
- `./scripts/generate-package-manifest.ps1 -PackageVersion <version>`
- Conexus package smoke restore/build from local `.nupkg`
- Manifest contains all expected packages and SHA-256 hashes.
- No package includes `_agent_prompts`, `_issue_bodies`, `docs/_incoming_packages`, `.tmp`, or donor material.

## Public API PRs

- Public API snapshot diffs must be reviewed.
- Intentional public API change requires:
  - changelog entry;
  - migration note if breaking;
  - README/package docs update if consumer-facing.

## Security workflow PRs

- CodeQL green on `main`.
- Supply chain green on `main`.
- Dependency submission green on `main`.
- Dependency review tested on a real PR.
- SBOM artifact uploaded and named predictably.

## Boundary rule

If the proposed change mentions a provider brand, model, route, price catalog, gateway policy, billing policy, safety policy, or product quota tier, it probably belongs in Conexus, not Ontogony.
