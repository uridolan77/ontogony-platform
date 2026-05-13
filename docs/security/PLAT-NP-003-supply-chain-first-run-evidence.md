# PLAT-NP-003 — Supply-chain first-run evidence

This document records **green workflow runs** on `uridolan77/ontogony-platform` `main` after the workflow pin fixes in the same change set (Trivy `v0.36.0`, dependency submission `v0.1.3`, release `contents: write`).

## CodeQL

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/codeql.yml` |
| Evidence run | _filled after push — see maintainer note below_ |

## Supply chain (`supply-chain.yml`)

| Job | Purpose | Evidence run |
| --- | --- | --- |
| NuGet vulnerability audit | `dotnet list package --vulnerable` | _TBD_ |
| SBOM (filesystem) | CycloneDX via Trivy → artifact **`sbom-cyclonedx`** | _TBD_ |
| Secret scan | `gitleaks/gitleaks-action` (skipped on fork PRs) | _TBD_ |

## Dependency submission

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/dependency-submission.yml` |
| Action | `advanced-security/component-detection-dependency-submission-action@v0.1.3` |
| Evidence run | _TBD_ |

## Dependency review

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/dependency-review.yml` (runs on `pull_request` to `main`) |
| Note | Opens or updates a PR summary per repo settings; verify on the next non-fork PR. |

## SBOM artifact retention

The **Supply chain** workflow uploads `sbom-cyclonedx` with `actions/upload-artifact@v4` (`if-no-files-found: error`). Confirm the artifact appears on the green **SBOM (filesystem)** job.

## Fixes applied (why prior runs failed)

1. **Trivy action:** `aquasecurity/trivy-action@0.28.0` no longer resolves; upstream standardized on **`v`-prefixed** immutable tags (see [trivy-action releases](https://github.com/aquasecurity/trivy-action/releases)).
2. **Dependency submission:** `component-detection-dependency-submission-action@v0.0.5` failed with `TypeError: Cannot read properties of null (reading 'Scheme')` during snapshot submission; **`v0.1.3`** includes later component-detection / action fixes.

---

**Maintainer:** After merging the workflow changes, replace `_TBD_` rows with `https://github.com/uridolan77/ontogony-platform/actions/runs/<id>` for the first **success** run on `main` for each workflow name.
