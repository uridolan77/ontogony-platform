# Critical System E2E Flows

These flows are the first candidates for replacing manual regression testing.

## E2E-001 — Full governed run completion

```text
Client
  -> Allagma POST /allagma/v0/runs
    -> Kanon compile semantic plan
    -> Kanon evaluate action/policy
    -> Conexus chat completion via configured alias/purpose
    -> Allagma records completed run + events
```

Assertions:

- Response is successful.
- Run ID exists.
- Planning decision ID exists where supported.
- Model call ID or model evidence exists where supported.
- Event sequence contains planning, model, and completion phases.
- Trace/correlation ID is returned or discoverable.
- Evidence bundle is written.

## E2E-002 — Idempotent run creation replay

Same request + same `Idempotency-Key`:

- returns same logical result;
- does not create duplicate run;
- does not create duplicate model call;
- does not create duplicate Kanon decision.

Same key + different payload:

- returns conflict;
- identifies idempotency conflict with stable error code.

## E2E-003 — Human gate waiting/approval/denial

Flow:

1. Submit run requiring human gate.
2. Verify paused/waiting state.
3. Resolve gate in Kanon as approved.
4. Resume run in Allagma.
5. Verify completion.
6. Repeat with denied outcome.

Assertions:

- The run does not continue before approval.
- Approval and denial are both durable.
- Events clearly show gate requested/resolved.
- Denied gate does not call downstream model/tool actions.

## E2E-004 — Kanon Conexus assistance

Flow:

```text
Client -> Kanon assistance route -> Conexus -> Kanon decision record
```

Assertions:

- Assistance can be enabled/disabled by config.
- Missing `AllowedFields` fails.
- Disallowed roles fail.
- Redaction occurs before Conexus call.
- Output is marked advisory / `draft_only`.
- Decision record includes fingerprints and provenance.

## E2E-005 — Conexus fallback through Allagma

Flow:

1. Configure primary fake provider to fail.
2. Configure fallback fake/local provider to succeed.
3. Start Allagma run.
4. Verify run completes.
5. Verify Conexus records fallback path.

Assertions:

- Failure is not hidden.
- Fallback is observable.
- Usage/cost telemetry remains coherent.
- Allagma response does not depend on provider-specific details.

## E2E-006 — Restart survival

Flow:

1. Start a run and pause it at a known point if possible.
2. Restart Allagma.
3. Query run/events.
4. Resume or inspect.

Assertions:

- Run state remains valid.
- Events are not lost.
- Idempotency state remains correct.
- Evidence export remains possible.

## E2E-007 — Metabole data-spine/evolution flow

Initial placeholder until route calibration.

Expected shape:

```text
Input artifact/state -> Metabole transformation/evolution step -> durable output -> provenance -> downstream use
```

Assertions:

- Transformation is deterministic for fixed inputs where required.
- Versioning/provenance are emitted.
- Invalid input fails with stable error.
- Outputs can be referenced by downstream service flows.

## E2E-008 — Aisthesis phenomenological memory flow

Initial placeholder until route calibration.

Expected shape:

```text
experience trace -> memory encoding -> retrieval/reflection -> Kanon semantic alignment or decision support
```

Assertions:

- Memory write is durable.
- Retrieval returns expected trace/context.
- Temporal/phenomenological metadata are preserved.
- Kanon-aligned semantic constraints are respected.
- No private/sensitive context is leaked into unrelated flows.

## E2E-009 — Frontend service coverage journey

Flow:

1. Open console.
2. Navigate to each service area.
3. Verify data loads from backend or mock state is clearly marked.
4. Execute safe read-only action.
5. Verify error handling for unavailable backend.

Assertions:

- UI exposes Allagma, Kanon, Conexus, Metabole, and Aisthesis where intended.
- UI calls current backend endpoints.
- Missing backend capability is shown as unavailable, not silently hidden.
