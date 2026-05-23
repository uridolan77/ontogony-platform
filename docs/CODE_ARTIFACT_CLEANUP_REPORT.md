# Code & artifact cleanup report — ontogony-platform

**Date:** 2026-05-23  
**Scope:** Repository hygiene after documentation cleanup; shared mechanics only.

## Summary

Focused cleanup removed committed Python bytecode from donor quarantine, tightened donor/examples labeling, updated stale script doc references, refreshed `.gitignore`, and purged local build artifacts. No living contracts, package graph edges, or CI gates were removed.

## Files / folders deleted

| Item | Count / notes |
| --- | --- |
| `_donors/conexus/**/__pycache__/*.pyc` | **47** tracked bytecode files removed via `git rm` |
| Local ignored artifacts | `bin/`, `obj/`, `artifacts/`, `TestResults/`, `packages/**/node_modules`, etc. via `git clean -fdX` |

## Confirmed absent (prior doc cleanup)

| Path | Status |
| --- | --- |
| `_agent_prompts/` | Not on disk; not tracked |
| `_issue_bodies/` | Not on disk; not tracked |
| `docs/_incoming/` | Not on disk; not tracked |
| `docs/planning/` | Not on disk; not tracked |

## Files / folders preserved (and why)

| Area | Rationale |
| --- | --- |
| `_donors/agentor`, `_donors/athanor`, `_donors/conexus` | Historical donor reference; not in solution builds or package graph |
| `_donors/agentor/REPO_TRUTH.md` | Already carries QUARANTINE banner |
| `examples/Agentor.Program.fragment.cs`, `Athanor.Program.fragment.cs` | Archived migration fragments; labeled in new `examples/README.md` |
| All `src/Ontogony.*` shipping packages (27) | Required by build, tests, CI, consumer contracts |
| `Ontogony.SystemCompatibility` | In golden package map and solution; gate tests pass on fixtures |
| System compatibility fixtures under `tests/Ontogony.SystemCompatibility.Tests/Fixtures/` | Test-gated mechanical evidence |
| Canonical docs, ADRs, migrations, schemas | Contract and governance surface |

## Quarantine tightening

Added short **QUARANTINE README** banners:

- `_donors/agentor/README.md`
- `_donors/athanor/README.md`
- `_donors/conexus/README.md`

Added `examples/README.md` cataloging active vs archived examples.

## Generated artifacts

None regenerated in this pass. Existing generators unchanged (`scripts/generate-package-manifest.ps1`, public API snapshots, compatibility fixtures).

## `.gitignore` updates

Added under ontogony-platform `.gitignore`:

```gitignore
__pycache__/
*.pyc
*.pyo
```

Existing ignores for `bin/`, `obj/`, `TestResults/`, `artifacts/`, `*.nupkg`, `coverage/` confirmed adequate.

## Dead code removed

No public API or shipping source removed. Donor Python bytecode was the only committed dead artifact class removed.

Legacy trace header constants (`LegacyAgentorTraceId`, `LegacyAthanorTraceId`) **kept** — active interop contract in `Ontogony.Contracts` / `Ontogony.Observability`.

## Export / barrel fixes

None required; `validate-package-levels.ps1` passes with `Ontogony.SystemCompatibility` in golden map.

## Scripts updated

| Script | Change |
| --- | --- |
| `scripts/validate-nupkg-coordination-path-hygiene.ps1` | Doc refs → `docs/governance/NUGET_SOURCE_MAPPING.md`, `_donors/README.md` |
| `scripts/validate-dependency-baseline.ps1` | Doc ref → `docs/governance/PACKAGE_LEVEL_GOVERNANCE.md` |

No obsolete CI validators removed (all referenced validators still meaningful).

## Tests / builds run

| Command | Result |
| --- | --- |
| `validate-docs-links.ps1` | PASS |
| `validate-docs-api-names.ps1` | PASS |
| `validate-shipping-inventory.ps1` | PASS |
| `validate-package-levels.ps1` | PASS |
| `validate-ai-runtime-docs.ps1` | PASS |
| `dotnet build Ontogony.Platform.sln -c Release` | PASS (0 warnings) |
| `dotnet test Ontogony.Platform.sln -c Release --no-build` (excl. optional DevRoot integration naming) | PASS — **504+** tests across Infrastructure, PublicApi, Http, etc. |
| `dotnet test Ontogony.SystemCompatibility.Tests -c Release --no-build` | PASS — **23/23** |

## Failures still present

None introduced by this cleanup.

**Environmental note:** `SixRepoCompatibilityGateIntegrationTests` against live sibling repos at lock SHAs may still fail without full `DevRoot` layout — fixture-based gate tests pass locally.

## Remaining risks

1. **Donor .NET/Python source** under `_donors/` remains large; it can still confuse agents who skip README banners.
2. **Pre-existing unstaged edits** in `scripts/promote-system-rc.ps1` and `scripts/lib/system-rc-promotion.ps1` were not part of this pass.
3. **CHANGELOG** still mentions deleted `docs/planning/` paths historically — acceptable archive text.

## Follow-up items

1. Consider shrinking `_donors/conexus` Python tree to a minimal file set if archaeology value drops.
2. Run `run-six-repo-compatibility-gate.ps1` in CI/DevRoot when sibling repos are at lock SHAs.
3. Optional: add `validate-nupkg-coordination-path-hygiene.ps1` to local pre-pack checklist doc in `docs/TESTING.md`.
