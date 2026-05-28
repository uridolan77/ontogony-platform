# Allagma Run Gates and Workflow Semantics

## Role

Allagma should not own provider decoding. It should record and govern workflow consequences.

## Required event integration

When a Conexus call happens inside an Allagma-managed run, emit or persist events equivalent to:

- `SamplingPolicyResolved`
- `SamplingPolicyWarningRaised`
- `SamplingPolicyViolationRaised`
- `SamplingProfileOverrideRequested`
- `SamplingProfileOverrideApproved`
- `SamplingProfileOverrideRejected`

Use existing event patterns. Do not invent a second run timeline model.

## Side-effect rule

`DiversityProbe` and `CreativeIdeation` outputs must not directly execute side effects. If a workflow node produces alternatives, Allagma should require a deterministic selection/approval node before any external side effect.

Recommended rule:

```text
if effectiveProfileId in [CreativeIdeation, DiversityProbe]
and nextOperation.sideEffecting == true
then require deterministic selection step or human gate
```

## Human gate trigger candidates

A human gate is required or recommended when:

- requested profile violates local/Kanon policy;
- high-risk operation requests non-deterministic profile;
- diversity output is about to become an action;
- raw ad-hoc sampling overrides exceed profile envelope;
- profile override changes `directExecutionAllowed` from false to true.

## Workflow evidence example

```json
{
  "eventType": "SamplingProfileOverrideRequested",
  "runId": "run_123",
  "stepId": "step_4",
  "requestedProfileId": "CreativeIdeation",
  "effectiveProfileIdBeforeOverride": "DeterministicContract",
  "reason": "Operator requested creative rewrite alternatives.",
  "createdAtUtc": "2026-05-28T00:00:00Z"
}
```

## Tests

1. Run records `SamplingPolicyResolved` for each Conexus call.
2. Direct side-effect after `DiversityProbe` is blocked.
3. Human approval enables a bounded override.
4. Rejected override leaves deterministic profile in place.
5. Replay/reconstructability includes sampling policy events.
