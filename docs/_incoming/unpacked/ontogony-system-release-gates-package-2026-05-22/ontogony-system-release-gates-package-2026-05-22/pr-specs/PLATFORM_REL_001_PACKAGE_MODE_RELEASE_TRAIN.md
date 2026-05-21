# PLATFORM-REL-001 — Package-mode release train

## Goal

Prove the Ontogony backend can be consumed as packages, not only sibling source references.

## Owner

Primary implementation: `allagma-dotnet`  
Governance and contract docs: `ontogony-platform`

## Existing baseline

Allagma already has package-mode CI logic that packs Platform/Kanon/Conexus into a local feed and restores Allagma with package flags. PLATFORM-REL-001 turns this into a formal release train with evidence, schemas, validators, and lock alignment.

## Deliverables

| Deliverable | Target path |
|---|---|
| Package-mode contract | `ontogony-platform/docs/releases/package-mode/PACKAGE_MODE_RELEASE_CONTRACT.md` |
| Package-mode summary schema | `allagma-dotnet/schemas/package-mode-release-summary.schema.json` |
| Package train script | `allagma-dotnet/scripts/release/run-package-mode-release-train.ps1` |
| Package summary validator | `allagma-dotnet/scripts/release/validate-package-mode-release-summary.ps1` |
| Workflow | `allagma-dotnet/.github/workflows/package-mode-release-train.yml` or reusable job in CI |
| Evidence closeout | `allagma-dotnet/docs/evidence/PLATFORM_REL_001_PACKAGE_MODE_RELEASE_TRAIN_EVIDENCE.md` |

## Required package lines

From runtime lock:

```text
Ontogony.*        0.3.0-alpha.1
Kanon.Client      0.1.0-alpha.0
Kanon.Contracts   0.1.0-alpha.0
Conexus.Client    0.1.0-alpha.1
Conexus.Contracts 0.1.0-alpha.1
```

## Required assertions

1. Package versions match runtime lock.
2. Allagma restore uses package flags.
3. Local feed contains required packages.
4. Allagma does not restore from sibling Platform/Kanon/Conexus paths.
5. Package-mode build passes.
6. Package-mode tests pass.
7. Summary JSON validates.

## Acceptance criteria

- Hard fails on version mismatch.
- Hard fails on missing package.
- Hard fails on sibling ProjectReference usage in package mode.
- Hard fails on restore/build/test failure.
- Emits package-mode release summary.
- Summary is included in SYSTEM-ALPHA-007 evidence bundle.
