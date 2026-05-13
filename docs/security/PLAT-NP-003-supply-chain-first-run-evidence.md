# PLAT-NP-003 — Supply-chain first-run evidence

Operational proof for **`uridolan77/ontogony-platform`** on `main` after workflow pin fixes (Trivy `v0.36.0`, dependency submission `advanced-security/component-detection-dependency-submission-action@v0.1.3`, `release-packages` `contents: write`). Evidence runs below are for **`8819d24f470f5c06771f5d76b5ffad819e28b758`** unless noted.

## CodeQL

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/codeql.yml` |
| Green run (same SHA as tag publish head) | https://github.com/uridolan77/ontogony-platform/actions/runs/25777405806 |

## Supply chain (`supply-chain.yml`)

| Job | Evidence run |
| --- | --- |
| Workflow (aggregate) | https://github.com/uridolan77/ontogony-platform/actions/runs/25777405815 |
| NuGet vulnerability audit | Same run — job **NuGet vulnerability audit** |
| SBOM (filesystem) | Same run — job **SBOM (filesystem)** uploads artifact **`sbom-cyclonedx`** (`sbom.cdx.json`) |
| Secret scan | Same run — job **Secret scan (gitleaks)** |

## Dependency submission

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/dependency-submission.yml` |
| Action pin | `advanced-security/component-detection-dependency-submission-action@v0.1.3` |
| Latest run | https://github.com/uridolan77/ontogony-platform/actions/runs/25777405818 (fails: **Dependency graph disabled for this repository**) |

**Owner action (one-time):** enable **Dependency graph** for this repo in GitHub: open [Code security and analysis](https://github.com/uridolan77/ontogony-platform/settings/security_analysis) and turn on **Dependency graph**. The REST `security_and_analysis.dependency_graph` field is not returned for this user-owned repo until enabled in the UI; after it is on, re-run **Dependency submission** (`workflow_dispatch` or push to `main`) and replace the link above with the first green run.

Prior failure modes fixed in code:

1. **Trivy:** `aquasecurity/trivy-action@0.28.0` no longer resolves; use **`v0.36.0`** (immutable `v`-prefixed tags).
2. **Submission action:** `v0.0.5` threw `TypeError: Cannot read properties of null (reading 'Scheme')` during snapshot build; **`v0.1.3`** clears that path once the graph is enabled.

## Dependency review

| Field | Value |
| --- | --- |
| Workflow | `.github/workflows/dependency-review.yml` (runs on `pull_request` to `main`) |
| Note | Exercised automatically on each PR; requires a populated dependency graph for meaningful diffs. Manual `workflow_dispatch` is not a supported shape for `dependency-review-action` (see historical run `25769346940`). |

## SBOM artifact retention

Confirm artifact **`sbom-cyclonedx`** on the green Supply chain run; upload uses `if-no-files-found: error`.
