# Public API compatibility

Shipping libraries under `src/Ontogony.*` expose **public API baselines** checked in CI via [`tests/Ontogony.PublicApi.Tests`](../../../tests/Ontogony.PublicApi.Tests).

## Mechanism

- **PublicApiGenerator** emits a C#-shaped text view of each assembly’s public surface.
- **Verify** compares that text to checked-in `*.verified.txt` files (one per shipping package, keyed by assembly short name).

`dotnet test Ontogony.Platform.sln` runs these tests with all other tests; any unintended public API change fails the build until snapshots are updated.

## Changing the approved surface

1. Make the intentional API change in the relevant `src/Ontogony.*` project.
2. Run `dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj` (or the full solution).
3. Verify writes `*.received.txt` next to the matching `*.verified.txt` when the snapshot differs. Copy the received content into the verified file (or use your Verify diff tool), then commit **only** the updated `*.verified.txt` files (not `*.received.txt`; those are gitignored).

Breaking changes should still be documented in [`CHANGELOG.md`](../../CHANGELOG.md) and [`docs/migrations/`](../../migrations/) per repository policy.

## Related

- [PR-PLAT-004 spec](./pr-specs/PR-PLAT-004-public-api-approval-tests.md)
- [Framework baseline](../../FRAMEWORK_BASELINE.md) — versioning and upgrade expectations
