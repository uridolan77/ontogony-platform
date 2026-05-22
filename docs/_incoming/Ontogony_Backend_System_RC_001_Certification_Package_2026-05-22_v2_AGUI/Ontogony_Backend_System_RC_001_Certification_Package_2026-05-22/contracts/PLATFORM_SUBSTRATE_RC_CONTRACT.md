# Platform Substrate RC Contract

## Owner

`ontogony-platform`

## Purpose

Define the contract needed for Platform mechanics to score above 9.

## Required sections in Platform release doc

```text
package_version_line
public_api_freeze_scope
allowed_breaking_change_process
migration_and_changelog_policy
consumer_validation_commands
package_level_matrix
protocol_registry_status
evidence_spine_contract_status
operator_failure_taxonomy_status
non_semantic_boundary_statement
```

## Hard boundary

Platform owns mechanics only.

Allowed:

```text
trace propagation
correlation
idempotency primitives
error envelope mechanics
HTTP resilience
hashing/fingerprints
redaction/secrets abstractions
execution journal facts
protocol registry
operator taxonomy adapter
```

Forbidden:

```text
canonical truth
ontology meaning
agent planning semantics
provider routing policy
business approval rules
iGaming/product logic
```

## Required validators

```powershell
dotnet test Ontogony.Platform.sln -c Release
pwsh ./scripts/validate-system-protocol-registry.ps1
pwsh ./scripts/validate-system-evidence-spine-contract.ps1
pwsh ./scripts/validate-system-operator-failure-taxonomy.ps1
pwsh ./scripts/validate-package-levels.ps1
```
