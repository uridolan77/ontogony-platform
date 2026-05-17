# Public API compatibility

Shipping libraries under `src/Ontogony.*` expose **public API baselines** checked in CI via [`tests/Ontogony.PublicApi.Tests`](../../../tests/Ontogony.PublicApi.Tests).

## Mechanism

- **PublicApiGenerator** emits a C#-shaped text view of each assembly’s public surface.
- **Verify** compares that text to checked-in `*.verified.txt` files (one per shipping package, keyed by assembly short name).

`dotnet test Ontogony.Platform.sln` runs these tests with all other tests; any unintended public API change fails the build until snapshots are updated.

## Breaking public API changes — required checklist

Updating `*.verified.txt` **only records** the new surface; it does **not** replace product communication.

Before merging a PR that **intentionally** breaks or narrows public API (removals, signature changes, sealed/virtual changes consumers rely on):

1. **[`CHANGELOG.md`](../../../CHANGELOG.md)** — add an **Unreleased** (or versioned) entry describing the break and the consumer impact.
2. **[`docs/migrations/`](../../../docs/migrations/)** — add a migration note when the change is not trivially discoverable from the changelog alone.
3. **`*.verified.txt`** — refresh snapshots after the above are drafted so reviewers see API + prose together.
4. **Phase 1 consumers** — note which downstream repos must react: **Conexus.NET**, **Kanon.NET**, **Allagma.NET** (see [`docs/governance/PHASE1_CONSUMER_COMPATIBILITY.md`](../../governance/PHASE1_CONSUMER_COMPATIBILITY.md)). Snapshot + changelog alone do not upgrade consumer repos; coordinate version bumps or sibling HEAD sync and their CI.

### Consumer-breaking change workflow

| Step | Action |
| --- | --- |
| Classify | Breaking vs additive-only (inspect snapshot diff) |
| Document | `CHANGELOG.md` + `docs/migrations/` when breaking |
| Notify | List affected consumers in PR description; link migration path |
| Prove | Platform: full checklist in [`PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md`](../../governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md) |
| Downstream | Each consumer runs its tests (Conexus/Allagma alignment scripts; Kanon package-mode CI) before adopting the new package line |

Snapshot-only PRs for **non-breaking** additions (new optional APIs, new types) still need a changelog entry if you want them called out in release notes, but migrations are usually unnecessary.

## Changing the approved surface

1. Make the intentional API change in the relevant `src/Ontogony.*` project.
2. Run `dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj` (or the full solution).
3. Verify writes `*.received.txt` next to the matching `*.verified.txt` when the snapshot differs. Copy the received content into the verified file (or use your Verify diff tool), then commit **only** the updated `*.verified.txt` files (not `*.received.txt`; those are gitignored).

For **breaking** changes, complete the [checklist above](#breaking-public-api-changes--required-checklist); do not ship snapshot updates without changelog/migration when the API change is breaking.

## Related

- [PR-PLAT-004 spec](./pr-specs/PR-PLAT-004-public-api-approval-tests.md)
- [Framework baseline](../../FRAMEWORK_BASELINE.md) — versioning and upgrade expectations
- [Phase 1 consumer compatibility](../../governance/PHASE1_CONSUMER_COMPATIBILITY.md)
- [Public API review](../../public-api-review.md)
- [Package compatibility checklist (0.3.0-alpha.1)](../../governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md)
