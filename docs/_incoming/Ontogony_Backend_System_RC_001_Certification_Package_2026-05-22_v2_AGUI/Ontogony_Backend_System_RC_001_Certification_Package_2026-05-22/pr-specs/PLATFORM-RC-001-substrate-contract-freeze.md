# PLATFORM-RC-001 — Substrate Contract Freeze

## Owner

`ontogony-platform`

## Problem

Platform is a strong alpha substrate, but above-9 shared mechanics require a clear RC contract, current protocol registry, and hard consumer compatibility validation.

## Goal

Freeze the Platform substrate contract for the current backend alpha-RC.

## Required files

```text
docs/releases/ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md
docs/evidence/PLATFORM_RC_001_SUBSTRATE_CONTRACT_FREEZE_EVIDENCE.md
docs/system/system-protocol-registry.json
```

## Required contract sections

```text
package_version_line
public_api_freeze_scope
allowed_breaking_change_process
consumer_validation_commands
protocol_registry_refresh_policy
evidence_spine_contract_status
operator_failure_taxonomy_status
non_semantic_boundary_statement
```

## Required validations

```powershell
dotnet test Ontogony.Platform.sln -c Release
pwsh ./scripts/validate-system-protocol-registry.ps1
pwsh ./scripts/validate-system-evidence-spine-contract.ps1
pwsh ./scripts/validate-system-operator-failure-taxonomy.ps1
pwsh ./scripts/validate-package-levels.ps1
```

If public API snapshots exist:

```powershell
dotnet test tests/Ontogony.PublicApi.Tests -c Release
```

## Consumer compatibility expectations

Evidence must point to:

```text
Conexus package-mode or source-mode validation
Kanon package-mode or source-mode validation
Allagma package-mode certification
```

## Acceptance criteria

- Protocol registry reflects current runtime lock.
- No product semantics are added to Platform.
- Public API diff is reviewed.
- Migration/changelog exists for any behavior-affecting change.
- Evidence spine and operator failure taxonomy validators pass.
- Package levels validator passes.

## Non-goals

- No Kanon semantic logic.
- No Allagma orchestration semantics.
- No Conexus routing policy.
- No product-specific workflow logic.
