# PRODUCT-MANUAL-QA-002 follow-up prompts (2026-05-19)

Use this file to open targeted follow-up issues/PR prompts from the `PARTIAL / BLOCKED`
execution recorded in `2026-05-19_FULL_MANUAL_QA_RESULTS.md`.

## Execution update (2026-05-19 addendum)

- `PMQA002-001` backend scope completed: Docker-local .NET image rebuild path now succeeds with trusted local CA injection (no TLS bypass).
- Rebuilt-image probes for prior inconclusive Allagma routes now return `200` (`/evaluations`, `/evaluations/{id}/evidence`, `/evaluations/baseline-comparisons`, `/evaluation-datasets`).
- Previous `404` observations are reclassified as stale-image/version-skew caused by rebuild failure.
- Full-stack compose rebuild remains blocked by a separate frontend image build error (`ontogony-frontend`: `tsc: not found`), outside NuGet TLS trust scope.

## Defect prompts

### 1) Docker build trust chain blocker

- ID: `PMQA002-001`
- Classification: `blocking defect`
- Severity: `high`
- Summary: Docker rebuild path fails during restore (`NU1301`) with TLS certificate chain `PartialChain` to `https://api.nuget.org/v3/index.json`.
- Prompt text:

```text
Fix Docker-local build restore trust chain failures (NU1301 PartialChain) for
allagma/kanon/conexus images so `start-local-working-system.ps1 -Build` succeeds
from a clean environment without manual certificate bypasses.

Acceptance:
- `reset-local-working-system.ps1 -Force` then `start-local-working-system.ps1 -Build` succeeds.
- No insecure TLS bypass flags added.
- Root cause + remediation documented in operator docs.
```

### 2) Allagma evaluation list returned 404 (inconclusive)

- ID: `PMQA002-002`
- Classification: `inconclusive (possible version-skew)`
- Severity: `high`
- Route: `GET /allagma/v0/evaluations`
- Prompt text:

```text
After Docker rebuild trust is fixed, verify whether
`GET /allagma/v0/evaluations` is present in rebuilt current images.
Current 404 was observed after rebuild failure and may indicate stale-image/version-skew.

Acceptance:
- Rebuilt stack (`start-local-working-system.ps1 -Build`) succeeds first.
- Endpoint returns 200 with list payload (or explicit typed empty list) on rebuilt images,
  or a source-backed route decision is documented.
- Frontend `/allagma/evaluations` behavior is re-verified with fresh probe.
```

### 3) Eval evidence export route mismatch

- ID: `PMQA002-003`
- Classification: `docs/runtime mismatch (inconclusive until rebuild succeeds)`
- Severity: `high`
- Route: `GET /allagma/v0/evaluations/{evaluationRunId}/evidence`
- Prompt text:

```text
Align runtime and docs for evaluation evidence export route.
Current docs and preflight expect:
`GET /allagma/v0/evaluations/{evaluationRunId}/evidence`
but Docker-local runtime returned 404 after rebuild failure.

Acceptance:
- Rebuilt stack (`start-local-working-system.ps1 -Build`) succeeds first.
- Either route is implemented and returns 200 for valid evaluation IDs,
  or docs/contracts/frontend are corrected consistently to the true route.
- One canonical route only; no ambiguous dual wording.
```

### 4) Baseline comparison list returned 404 (inconclusive)

- ID: `PMQA002-004`
- Classification: `inconclusive (possible version-skew)`
- Severity: `medium`
- Route: `GET /allagma/v0/evaluations/baseline-comparisons`
- Prompt text:

```text
After Docker rebuild trust is fixed, verify baseline comparison list route availability on rebuilt images:
`GET /allagma/v0/evaluations/baseline-comparisons`.

Acceptance:
- Rebuilt stack (`start-local-working-system.ps1 -Build`) succeeds first.
- Endpoint returns 200 with list payload and supports expected query shape.
- Frontend baseline list page uses live route without degraded fallback by default.
```

### 5) Evaluation dataset list returned 404 (inconclusive)

- ID: `PMQA002-005`
- Classification: `inconclusive (possible version-skew)`
- Severity: `medium`
- Route: `GET /allagma/v0/evaluation-datasets`
- Prompt text:

```text
After Docker rebuild trust is fixed, verify dataset list route availability on rebuilt images:
`GET /allagma/v0/evaluation-datasets`.

Acceptance:
- Rebuilt stack (`start-local-working-system.ps1 -Build`) succeeds first.
- Endpoint returns 200 with dataset list payload (or explicit typed empty response).
- Frontend dataset route remains read-only and renders honest live state.
```

## Re-run gate for PRODUCT-MANUAL-QA-002

Do not mark rerun as PASS until all are true:

- [ ] Fresh rebuild path succeeds (`start-local-working-system.ps1 -Build`)
- [ ] `GET /allagma/v0/evaluations` returns 200
- [ ] `GET /allagma/v0/evaluations/{evaluationRunId}/evidence` returns 200 (or route/docs aligned)
- [ ] `GET /allagma/v0/evaluations/baseline-comparisons` returns 200
- [ ] `GET /allagma/v0/evaluation-datasets` returns 200
- [ ] Full checklist re-executed from fresh stack with new artifacts only
- [ ] Boundary preserved: not production readiness, no secrets in evidence
