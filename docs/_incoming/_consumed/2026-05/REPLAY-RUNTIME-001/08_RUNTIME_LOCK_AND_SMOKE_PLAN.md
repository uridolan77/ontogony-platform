# 08 — Runtime lock and smoke plan

## Principle

Replay smoke should become evidence before it becomes a hard gate.

Do not make replay a required PR gate initially. Make it opt-in/manual until the workflow is stable and fast.

## Smoke types

### 1. Local replay smoke

Goal: prove a local terminal Allagma run can produce replay evidence-only output.

Flow:

1. Start local Allagma with fake/local dependencies.
2. Create governed fake run.
3. Wait for terminal state.
4. Call `POST /allagma/v0/runs/{runId}/replay` with idempotency key.
5. Verify replay result contains `mode = evidence_only` or compatible legacy manifest mode.
6. Verify replay event was appended.
7. Export replay artifacts.

### 2. Governed fake replay smoke

Goal: prove the cross-service replay path without real providers/tools.

Flow:

1. Run existing governed fake E2E.
2. Capture run ID, trace ID, correlation ID, Kanon decision ID, Conexus model call ID if available.
3. Resolve Evidence Spine graph.
4. Classify replay eligibility.
5. Run deterministic simulation or evidence-only replay depending on source coverage.
6. Export replay evidence bundle.
7. Verify delta and safety posture.

### 3. Backend-only replay smoke

Goal: run without browser/frontend.

Commands should be PowerShell-first because current smoke infrastructure is PowerShell-heavy.

Artifacts:

- `artifacts/replay-runtime-001/<timestamp>/replay-request.json`
- `artifacts/replay-runtime-001/<timestamp>/replay-result.json`
- `artifacts/replay-runtime-001/<timestamp>/replay-evidence-bundle.json`
- `artifacts/replay-runtime-001/<timestamp>/replay-delta.json`
- `artifacts/replay-runtime-001/<timestamp>/replay-summary.json`
- `artifacts/replay-runtime-001/<timestamp>/replay-summary.md`

### 4. Optional frontend replay E2E

Goal: prove the operator workbench connects to live/fake services.

Flow:

1. Load Evidence Spine page.
2. Paste governed fake run ID.
3. Open Replay Workbench.
4. Verify eligibility modes.
5. Run evidence-only replay.
6. Export bundle.
7. Verify result link appears.

## Runtime-lock policy

### Stage 1

Required:

- schema/contract validation.

Not required:

- replay smoke.

### Stage 2

Required:

- Allagma replay unit/integration tests.
- Existing governed fake E2E remains valid.

Optional:

- backend-only replay smoke.

### Stage 3

Required for Kanon changes:

- Kanon semantic decision replay acceptance.

Optional:

- Allagma calls Kanon replay eligibility in a local smoke.

### Stage 4

Required for Conexus changes:

- Conexus replay/dry-run tests.
- Real provider blocking tests.

Optional:

- Conexus fake-provider replay smoke.

### Stage 5

Optional but recommended:

- cross-service replay bundle smoke.

### Stage 6

Replay smoke may become release-readiness evidence. It should still not be default PR gate until runtime cost and flake rate are acceptable.

## Existing artifacts to preserve

Do not break:

- Platform governed fake E2E evidence artifacts.
- Allagma governed fake E2E scripts.
- Runtime-lock governed fake wrapper.
- System compatibility matrix.
- Post-lock delta register.
- Existing Evidence Spine docker/live verification scripts.
