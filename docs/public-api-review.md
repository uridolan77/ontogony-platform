# Public API Review and Governance

## Overview

Ontogony.Platform ships **23 NuGet packages** with public APIs that downstream consumers (Conexus, Agentor, Athanor, etc.) depend on. Public API changes—including additions, removals, and signature modifications—must be intentional, documented, and reviewed.

## Public API Snapshots

This repository uses **public API snapshot testing** to detect changes to public API surface automatically:

- **Location:** `tests/Ontogony.PublicApi.Tests/`
- **Snapshot files:** `*.verified.txt` files named by assembly (for example, `Ontogony.Http.verified.txt`)
- **Test:** `ShippingAssemblyPublicApiTests.cs` compares each shipping package's public API surface against the approved snapshot
- **Tool:** Uses approval-style snapshots and `PublicApiGenerator` to extract and compare public types, methods, properties, and signatures

### What is tracked

- Public types and signatures
- Method and property declarations
- Visibility modifiers
- Generic constraints
- Attribute decorations that are part of the contract

### What is not tracked

- Internal types and members
- XML documentation strings
- Private implementation details
- Test-only types

## When Public API Snapshots Change

If a test fails with:

```text
Public_api_matches_snapshot_assemblyShortName=Ontogony.SomeName failed

The PublicAPI.txt snapshot file does not match the current source
```

It means the public API surface has changed. **Do not suppress this failure** — investigate and document the change.

### Acceptable reasons for public API changes

1. **Intentional addition:** A new public type, method, or property was added to extend the platform. This is **not** a breaking change if it is additive-only.
2. **Intentional removal:** A deprecated public member was removed. This **is** a breaking change.
3. **Signature modification:** A public method or property signature changed. This **is** a breaking change if downstream code may break.

### Not acceptable

- **Accidental visibility leaks:** Private or internal types accidentally becoming public.
- **Unreviewed signature changes:** Public members modified without changelog or migration notes.

## Review Checklist for Public API Changes

When you change a public API surface, ensure:

### Required

- [ ] **Public API snapshot** — The test framework detected the change; snapshot diff is in the PR.
- [ ] **Type of change** — Classify as **breaking** (removals, signature changes) or **non-breaking** (additions, internal restructurings).
- [ ] **CHANGELOG entry** — If the change is public and non-trivial, add an entry to `CHANGELOG.md` under **Unreleased**.
  - For additions: note the new public member.
  - For removals/breaking changes: note the impact and migration path.

### If breaking change

- [ ] **Migration note** — Add or update `docs/migrations/MIGRATION_v0.3_to_v0.4.md` (or current→next version) explaining:
  - What changed and why.
  - How consumers should update their code.
  - Any deprecation timeline.
- [ ] **Package README update** — If the change affects primary use cases in a shipping package, update the package README.

### If non-breaking addition

- [ ] **CHANGELOG entry** — Minimal note of the new public member.
- [ ] **Package README update** — If the addition is a primary use case, consider updating the package README with a usage example.

## Detecting Changes Locally

Run the public API tests locally:

```bash
dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj -c Release
```

If snapshots differ, the test output shows a diff. You can review the changes:

```bash
# Unix/macOS
cat tests/Ontogony.PublicApi.Tests/ShippingAssemblyPublicApiTests.Public_api_matches_snapshot_assemblyShortName=Ontogony.Http.received.txt

# Windows PowerShell
Get-Content tests/Ontogony.PublicApi.Tests/ShippingAssemblyPublicApiTests.Public_api_matches_snapshot_assemblyShortName=Ontogony.Http.received.txt
```

Compare the `.received.txt` (new) and `.verified.txt` (baseline) files.

## Approving Changes

If you have intentionally changed the public API and documented it:

1. Approve the test snapshot by renaming `.received.txt` → `.verified.txt` (or your test framework's approval mechanism).
2. Commit the updated `.verified.txt` file.
3. Include the updated snapshot file in your PR.

## CI Enforcement

The CI pipeline (`ci.yml`) runs `Ontogony.PublicApi.Tests` as part of the test suite and also runs `scripts/validate-public-api-governance.ps1`. A PR will fail CI if:

- A public API snapshot differs but has not been approved.
- The PR modifies a snapshot file **without** a corresponding CHANGELOG entry and (if breaking) migration note.

## Governance Script Scope and Limitations

`scripts/validate-public-api-governance.ps1` is intentionally conservative.

- It detects changed `.verified.txt` files under `tests/Ontogony.PublicApi.Tests/` from staged changes, unstaged changes, deleted snapshot files, renamed snapshot files, or CI PR/base diffs when available.
- If snapshot files changed, it requires `CHANGELOG.md` to change in the same diff/worktree.
- It does **not** fully classify removals vs additions yet; reviewers must still inspect the snapshot diff and decide whether a migration note is required.
- If CI cannot provide a comparable base diff and there are no local snapshot changes, the script exits cleanly rather than guessing.

## Manual Proof

You can validate the governance guard locally without committing anything:

1. Edit any `*.verified.txt` file under `tests/Ontogony.PublicApi.Tests/` and run `./scripts/validate-public-api-governance.ps1`.
  Expected result: the script fails if `CHANGELOG.md` was not also changed.
2. Make a corresponding edit to `CHANGELOG.md` and re-run the script.
  Expected result: the script passes.

### Recorded local proof (2026-05-13)

- Repository HEAD during the proof: `75311d099918835eb863b4ca745fa982a07aaea3`
- Snapshot file used: `tests/Ontogony.PublicApi.Tests/ShippingAssemblyPublicApiTests.Public_api_matches_snapshot_assemblyShortName=Ontogony.AI.Contracts.verified.txt`
- Result without `CHANGELOG.md` edit: **failed as expected** with `FAIL: Public API snapshots changed but CHANGELOG.md was NOT updated` and exit code `1`
- Result after temporary `CHANGELOG.md` edit: **passed as expected** with `PASS: Public API governance check passed` and exit code `0`
- Temporary proof edits were reverted after the check; the proof demonstrates local behavior only.

### CI proof status

`ci.yml` runs `./scripts/validate-public-api-governance.ps1`, so the gate is wired into CI. The current public head run for commit `75311d099918835eb863b4ca745fa982a07aaea3` is `https://github.com/uridolan77/ontogony-platform/actions/runs/25791922209`, and it is **failed**, so there is still no green CI proof URL for the rename/delete-aware script revision. The visible failure annotations are stale `tests/Ontogony.Http.Tests` API mismatches on GitHub head; local fixes exist in this workspace, but **PLAT-NP-009 remains implemented, pending external CI proof** until those fixes are pushed and CI reruns green.

## Future Public API Stability

Once Ontogony.Platform reaches a **stable release** (v1.0.0 or later):

- **Semantic Versioning** will be enforced: patch versions must not contain breaking public API changes.
- Public API governance will become stricter; deprecation periods will precede removals.
- Consumers will have minimum notice windows for breaking changes.

During **alpha releases** (`v0.3.0-alpha.1` and later):

- Public API changes may happen more frequently.
- Each change must still be intentional and documented.
- Alpha consumers should monitor CHANGELOG and migration notes carefully.

## Questions

If you have questions about whether a change is public, breaking, or requires migration documentation:

1. Check `Ontogony.Security`, `Ontogony.Http`, or `Ontogony.Configuration` package READMEs for primary use cases.
2. See [CHANGELOG.md](../CHANGELOG.md) for recent examples.
3. If uncertain, open a GitHub Discussion or ask in a PR.
