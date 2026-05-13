# PLAT-NP-003 — Supply-chain first-run evidence

Operational proof for **`uridolan77/ontogony-platform`** on `main` after workflow pin fixes (Trivy `v0.36.0`, dependency submission `advanced-security/component-detection-dependency-submission-action@v0.1.3`, `release-packages` `contents: write`). Evidence runs below are for **`8819d24f470f5c06771f5d76b5ffad819e28b758`** unless noted.

## CodeQL

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/codeql.yml` |
| Green run (same SHA as tag publish head) | [Run 25777405806](https://github.com/uridolan77/ontogony-platform/actions/runs/25777405806) |

## Supply chain (`supply-chain.yml`)

| Job | Evidence run |
| --- | --- |
| Workflow (aggregate) | [Run 25777405815](https://github.com/uridolan77/ontogony-platform/actions/runs/25777405815) |
| NuGet vulnerability audit | Same run — job **NuGet vulnerability audit** |
| SBOM (filesystem) | Same run — job **SBOM (filesystem)** uploads artifact **`sbom-cyclonedx`** (`sbom.cdx.json`) |
| Secret scan | Same run — job **Secret scan (gitleaks)** |

## Dependency submission

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/dependency-submission.yml` |
| Action pin | `advanced-security/component-detection-dependency-submission-action@v0.1.3` |
| Latest run | [Run 25791922202](https://github.com/uridolan77/ontogony-platform/actions/runs/25791922202) (fails: **Dependency graph disabled for this repository**) |

**Owner action (one-time):** enable **Dependency graph** for this repo in GitHub: open [Code security and analysis](https://github.com/uridolan77/ontogony-platform/settings/security_analysis) and turn on **Dependency graph**. The latest public failure annotation is explicit: `HttpError: The Dependency graph is disabled for this repository. Please enable it before submitting snapshots.` After it is on, re-run **Dependency submission** (`workflow_dispatch` or push to `main`) and replace the link above with the first green run.

Prior failure modes fixed in code:

1. **Trivy:** `aquasecurity/trivy-action@0.28.0` no longer resolves; use **`v0.36.0`** (immutable `v`-prefixed tags).
2. **Submission action:** `v0.0.5` threw `TypeError: Cannot read properties of null (reading 'Scheme')` during snapshot build; **`v0.1.3`** clears that path once the graph is enabled.

## Dependency review

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/dependency-review.yml` (runs on `pull_request` to `main`) |
| Note | PR-only by design; requires a populated dependency graph for meaningful diffs. No real PR proof URL is captured yet. Manual `workflow_dispatch` is not a supported shape for `dependency-review-action` (see historical run `25769346940`). |

## SBOM artifact retention

Confirm artifact **`sbom-cyclonedx`** on the green Supply chain run; upload uses `if-no-files-found: error`.

## Spec acceptance checklist

| Criterion | State |
| --- | --- |
| CodeQL green on `main` | **Previously met** — historical green run linked above. Current public head run `25791922210` for commit `75311d0` is failed due stale `tests/Ontogony.Http.Tests` API usage, so do not treat current head as externally green yet. |
| Supply chain green on `main` | **Met** — current public head run `25791922188` is green. |
| SBOM uploaded and retained | **Met** — artifact `sbom-cyclonedx` on green run. |
| Dependency submission green on `main` | **Blocked** — enable **Dependency graph** for the repository (see section above); link documents latest run until green. |
| Dependency review on real PR | **Open** — `dependency-review.yml` is `pull_request`-only (required base/head refs). Capture a real-PR run link when Dependency graph data enables meaningful diffs; until then this row is documentation-only, not a green proof artifact. |
