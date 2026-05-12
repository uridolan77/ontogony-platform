# Package Versioning

This guide defines shared package versioning expectations for Ontogony.Platform.

See also the adoption index: [consumer-package-migration.md](./consumer-package-migration.md).

## Principles

- Shared packages evolve with explicit changelog entries.
- Breaking changes require migration notes in docs/migrations.
- Services adopt versions deliberately, not by automatic drift.

## Versioning Rules

- Patch: bug fixes and non-breaking behavior hardening.
- Minor: additive APIs and backward-compatible enhancements.
- Major: breaking API/behavior changes requiring service migration.

## Release Grouping

Refer to docs/Sprint1.md for release grouping proposals:

- 0.3 safe transport/observability
- 0.4 event substrate
- 0.5 operational readiness

## Adoption Workflow

1. Pin package versions in service repos.
2. Read CHANGELOG and migration notes before upgrading.
3. Run service integration tests after each package bump.
4. Validate trace/error/header compatibility externally.

## Do Not Do This

- Do not introduce silent breaking changes in minor versions.
- Do not ship behavior changes without changelog + migration note when needed.
