# PRODUCT-MANUAL-QA-002 follow-up prompts (2026-05-19)

Use this file to open targeted follow-up issues/PR prompts from the `PARTIAL / BLOCKED`
execution recorded in `2026-05-19_FULL_MANUAL_QA_RESULTS.md`.

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

### 2) Missing Allagma evaluation list route

- ID: `PMQA002-002`
- Classification: `product bug`
- Severity: `high`
- Route: `GET /allagma/v0/evaluations`
- Prompt text:

```text
Restore/implement `GET /allagma/v0/evaluations` so the global evaluation list
contract is available in Docker-local runtime and matches current docs/frontend expectations.

Acceptance:
- Endpoint returns 200 with list payload (or explicit typed empty list).
- Frontend `/allagma/evaluations` works in live mode without hidden fallback.
- Contract verified by API test and manual QA probe.
```

### 3) Eval evidence export route mismatch

- ID: `PMQA002-003`
- Classification: `docs mismatch` (possibly runtime bug)
- Severity: `high`
- Route: `GET /allagma/v0/evaluations/{evaluationRunId}/evidence`
- Prompt text:

```text
Align runtime and docs for evaluation evidence export route.
Current docs and preflight expect:
`GET /allagma/v0/evaluations/{evaluationRunId}/evidence`
but Docker-local runtime returned 404.

Acceptance:
- Either route is implemented and returns 200 for valid evaluation IDs,
  or docs/contracts/frontend are corrected consistently to the true route.
- One canonical route only; no ambiguous dual wording.
```

### 4) Missing baseline comparison list route

- ID: `PMQA002-004`
- Classification: `product bug`
- Severity: `medium`
- Route: `GET /allagma/v0/evaluations/baseline-comparisons`
- Prompt text:

```text
Restore/implement baseline comparison list route:
`GET /allagma/v0/evaluations/baseline-comparisons`.

Acceptance:
- Endpoint returns 200 with list payload and supports expected query shape.
- Frontend baseline list page uses live route without degraded fallback by default.
```

### 5) Missing evaluation dataset list route

- ID: `PMQA002-005`
- Classification: `product bug`
- Severity: `medium`
- Route: `GET /allagma/v0/evaluation-datasets`
- Prompt text:

```text
Restore/implement evaluation dataset list route:
`GET /allagma/v0/evaluation-datasets`.

Acceptance:
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
