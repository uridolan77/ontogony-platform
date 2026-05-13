# PLAT Final Open Items Closure Report

## Summary

- HEAD SHA: `75311d099918835eb863b4ca745fa982a07aaea3`
- Repository: `ontogony-platform`
- Scope: final governance, evidence, and status reconciliation pass for remaining `PLAT-NP-*` hardening items

## Files changed

- `CHANGELOG.md`
- `MIGRATION.md`
- `docs/VERSION_COMPATIBILITY_MATRIX.md`
- `docs/public-api-review.md`
- `docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md`
- `docs/planning/next-phase/backlog.json`
- `docs/planning/next-phase/NEXT_PHASE_SEQUENCE.md`
- `docs/planning/next-phase/CURRENT_REVIEW_AFTER_PLAT011.md`
- `docs/architecture/durability-boundaries.md`
- `README.md`
- `docs/start-here.md`
- `docs/architecture/index.md`
- `docs/operations/index.md`
- `scripts/validate-public-api-governance.ps1`
- `tests\Ontogony.Http.Tests\CorrelationHeadersDelegatingHandlerTests.cs`
- `tests\Ontogony.Http.Tests\DefaultRetryClassifierTests.cs`
- `tests\Ontogony.Http.Tests\ResilientIntegrationDelegatingHandlerTests.cs`
- `tests\Ontogony.Http.Tests\RetryBudgetTrackerTests.cs`
- `tests\Ontogony.Http.Tests\TransportResilienceRegistryTests.cs`

## Status table

| Item | Status | Evidence / note |
| --- | --- | --- |
| `PLAT-NP-001` | Closed | Tag `v0.3.0-alpha.1` publish proof is recorded in `docs/releases/PR-PLAT-NP-001-release-parity-evidence.md`. |
| `PLAT-NP-002` | Closed | Conexus package-mode compatibility proof remains recorded in `docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`. |
| `PLAT-NP-003` | Closed | Dependency graph enabled; dependency submission run `25795696616` succeeded; real PR dependency-review run `25795820305` succeeded on PR `#1`. |
| `PLAT-NP-004` | Closed | Package hygiene scan remains enforced in CI/release. |
| `PLAT-NP-005` | Closed, continuous maintenance | Accuracy baseline remains closed; docs still require normal ongoing maintenance. |
| `PLAT-NP-006` | Closed | Deferred item register remains explicit and current. |
| `PLAT-NP-007` | Closed | Secret reference parser remains implemented and covered. |
| `PLAT-NP-008` | Open (future maintenance guard) | Baseline warning coverage is complete for current public in-memory registrations; keep open to audit future additions and avoid warning noise on test-only fakes. |
| `PLAT-NP-009` | Implemented, pending CI proof | Governance script is hardened and locally proven; `ci.yml` runs it, but the current public head CI run is failed and no green CI URL exists yet for this exact revision. |
| `PLAT-NP-011` | Accepted / closed | This closure pass completed the remaining governance and evidence reconciliation work without adding new platform capabilities or product semantics. |

## Public API governance proof

- Script reviewed: `scripts/validate-public-api-governance.ps1`
- Real snapshot path: `tests/Ontogony.PublicApi.Tests/**/*.verified.txt`
- Diff sources covered in the script: unstaged, staged, `pull_request` against `origin/$GITHUB_BASE_REF`, and `push` against `HEAD^`
- Rename/delete handling: covered via `git diff --name-status --find-renames --diff-filter=AMDR`
- Changelog requirement: snapshot changes require `CHANGELOG.md` in the same change set
- Output policy: script user-facing output remains ASCII-only

### Local manual proof run

- Proof date: `2026-05-13`
- HEAD SHA: `75311d099918835eb863b4ca745fa982a07aaea3`
- Snapshot file used: `tests/Ontogony.PublicApi.Tests/ShippingAssemblyPublicApiTests.Public_api_matches_snapshot_assemblyShortName=Ontogony.AI.Contracts.verified.txt`
- Snapshot-only temporary edit: script failed with `FAIL: Public API snapshots changed but CHANGELOG.md was NOT updated` and exit code `1`
- Snapshot + temporary changelog edit: script passed with `PASS: Public API governance check passed` and exit code `0`
- Temporary proof edits were reverted immediately after validation

### CI proof

- CI wiring exists in `.github/workflows/ci.yml` (`./scripts/validate-public-api-governance.ps1` step is present)
- Current public head CI run for commit `75311d099918835eb863b4ca745fa982a07aaea3`: `https://github.com/uridolan77/ontogony-platform/actions/runs/25791922209` — **failed**
- Visible failure annotations on that run are stale `tests/Ontogony.Http.Tests` API mismatches on GitHub head. Those tests were repaired locally in this workspace, but no green public rerun exists yet, so there is still no green CI proof URL for the hardened revision.

## PLAT-NP-003 dependency-submission / dependency-review status

- Historical CodeQL green proof URL: `https://github.com/uridolan77/ontogony-platform/actions/runs/25777405806`
- Current public head CodeQL run: `https://github.com/uridolan77/ontogony-platform/actions/runs/25791922210` — **failed** on stale `tests/Ontogony.Http.Tests` API usage
- Current public head Supply-chain green proof URL: `https://github.com/uridolan77/ontogony-platform/actions/runs/25791922188`
- Dependency submission success proof URL: `https://github.com/uridolan77/ontogony-platform/actions/runs/25795696616`
- Dependency-review real PR success proof URL: `https://github.com/uridolan77/ontogony-platform/actions/runs/25795820305` (PR `https://github.com/uridolan77/ontogony-platform/pull/1`)

## PLAT-NP-008 in-memory warning inventory

| Package | DI method | Implementation | Warn outside Development? | Test exists? | Intentionally test-only? |
| --- | --- | --- | --- | --- | --- |
| `Ontogony.Artifacts` | `AddOntogonyInMemoryArtifactStore` | `InMemoryArtifactStore` | Yes | Yes (`Ontogony.Artifacts.Tests`) | No |
| `Ontogony.Execution` | `AddOntogonyInMemoryExecutionJournal` | `InMemoryExecutionJournal` | Yes | Yes (`Ontogony.Execution.Tests`) | No |
| `Ontogony.Quotas` | `AddOntogonyInMemoryQuotaLedger` | `InMemoryQuotaLedger` | Yes | Yes (`Ontogony.Quotas.Tests`) | No |
| `Ontogony.Persistence` | `AddOntogonyInMemoryOutboxStore` | `InMemoryOutboxStore` | Yes | Yes (`Ontogony.Infrastructure.Tests`) | No |
| `Ontogony.Persistence` | none | `InMemoryIdempotencyLedger` | No public startup registration | No startup warning test required | Yes, direct/test usage only |
| `Ontogony.Testing` | none | `FakeClock`, `FakeIdGenerator`, `FakeEventPublisher`, `FakeCurrentActorAccessor` | No | No startup warning test required | Yes |

Judgment: baseline warning coverage is complete for current public in-memory DI registrations. Keep `PLAT-NP-008` open only as a maintenance guard for future additions.

## Coverage artifact status

- CI uploads `coverage.cobertura.xml` and `.trx` files via `.github/workflows/ci.yml`
- Coverage collection and upload are active
- Coverage thresholds remain intentionally deferred until the newer test projects mature

## Commands run and results

| Command | Result |
| --- | --- |
| `git fetch origin main` / `git checkout main` / `git pull --ff-only origin main` | Passed; current HEAD `75311d099918835eb863b4ca745fa982a07aaea3` |
| Repository searches for the historical package-name typo, `PLAT-NP-009`, `Dependency graph`, `Dependency submission`, `workflow_dispatch missing` | Completed; source/docs hits reviewed, no `workflow_dispatch missing` claim retained |
| `dotnet restore Ontogony.Platform.sln` | Passed |
| `dotnet build Ontogony.Platform.sln --no-restore -c Release` | Passed after replacing stale `tests/Ontogony.Http.Tests` coverage with current-surface tests |
| `dotnet test Ontogony.Platform.sln --no-build -c Release` | Passed; `553` tests, `0` failed |
| `./scripts/validate-docs-links.ps1` | Passed |
| `./scripts/validate-docs-api-names.ps1` | Passed |
| `./scripts/validate-ai-runtime-boundaries.ps1` | Passed |
| `./scripts/validate-shipping-inventory.ps1` | Passed |
| `./scripts/validate-ai-runtime-docs.ps1` | Passed |
| `./scripts/validate-package-levels.ps1` | Passed |
| `./scripts/validate-dependency-baseline.ps1` | Passed |
| `./scripts/validate-conexus-consumer-baseline-alignment.ps1` | Passed |
| `./scripts/validate-public-api-governance.ps1` (clean worktree) | Passed: `PASS: No public API snapshot changes detected` |
| `./scripts/validate-changelog.ps1 -PackageVersion "0.3.0-alpha.1" -Strict` | Passed |
| Local governance proof with temporary snapshot edit | Failed as expected until `CHANGELOG.md` also changed |
| Local governance proof with temporary snapshot + `CHANGELOG.md` edit | Passed as expected |
| `$env:PACKAGE_VERSION = "0.3.0-alpha.1"; ./scripts/pack-all.ps1 -NoBuild` | Passed; produced 23 packages |
| `./scripts/validate-nupkg-coordination-path-hygiene.ps1` | Passed |
| `./scripts/generate-package-manifest.ps1 -PackageVersion "0.3.0-alpha.1"` | Passed; manifest generated for 23 packages at commit `75311d099918835eb863b4ca745fa982a07aaea3` |
| `dotnet restore examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj -p:OntogonyPackageVersion=0.3.0-alpha.1` | Passed |
| `dotnet build examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj --no-restore -c Release -p:OntogonyPackageVersion=0.3.0-alpha.1` | Passed |
| Final repository-wide typo search after rebuild | Passed: `NO_MATCHES` |

## Skipped validations and exact reason

- A full new external CI rerun for `PLAT-NP-009` proof was not created from this workspace; only the targeted NP-003 runs were triggered/captured.

## Remaining open items

1. `PLAT-NP-008` remains intentionally open as a future maintenance guard, not because current baseline coverage is missing.
2. `PLAT-NP-009` remains implemented but pending external CI proof for this hardened script revision; the current public head CI run is failed, and the local HTTP-test fixes in this workspace have not yet been externally rerun.
