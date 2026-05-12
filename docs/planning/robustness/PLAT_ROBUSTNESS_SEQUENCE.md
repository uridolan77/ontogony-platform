# Platform robustness PR sequence

1. PR-PLAT-001 — Publish packages to internal GitHub Packages feed.
2. PR-PLAT-002 — Consumer compatibility test using Conexus.
3. PR-PLAT-003 — Typed HTTP client support for Ontogony HTTP resilience.
4. PR-PLAT-004 — Public API approval tests.
5. PR-PLAT-005 — Dependency baseline alignment.
6. PR-PLAT-006 — XML docs for active Conexus-used APIs.
7. PR-PLAT-007 — Security/supply-chain workflow pack.
8. PR-PLAT-008 — Donor folder hygiene.
9. PR-PLAT-009 — In-memory startup warnings.
10. PR-PLAT-010 — README resilience contradiction fix.
11. PR-PLAT-011 — Generic secret-value resolver.
12. PR-PLAT-012 — Durable quota ledger design spike.

## Follow-ups (post-001 / post-002)

- **PR-PLAT-001.1** — Operational: first successful tag run on GitHub Packages; confirm feed entries, manifest vs artifacts, and Release attachments (see [Package publishing](./PACKAGE_PUBLISHING_GITHUB_PACKAGES.md#first-tag-publish-operational-proof)). Release workflow parity with `ci.yml` validation is tracked in `.github/workflows/release-packages.yml`.
- **PR-PLAT-002.1** — Full Conexus.NET build/tests against published packages; spec: [pr-specs/PR-PLAT-002.1-real-conexus-dotnet-package-consumption.md](./pr-specs/PR-PLAT-002.1-real-conexus-dotnet-package-consumption.md) (preferred implementation: Conexus repo CI).
