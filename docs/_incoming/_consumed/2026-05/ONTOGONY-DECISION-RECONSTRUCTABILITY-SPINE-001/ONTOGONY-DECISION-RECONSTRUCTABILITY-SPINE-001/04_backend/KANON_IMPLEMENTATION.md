# Kanon Implementation Plan

## Role

Kanon should own semantic authority and reconstructability classification.

## Slice A — Contracts

Add or extend DTOs/contracts for:

- `DecisionEventContract`
- `EvidenceFragmentRefContract`
- `ReconstructabilityReportContract`
- `ReconstructabilityPropertyResultContract`
- `MissingEvidenceDiagnosticContract`

Keep names aligned with existing Kanon contract style. If Kanon already has an Evidence Spine contract namespace, place them there.

## Slice B — Classifier

Implement a deterministic classifier, for example:

```text
DecisionReconstructabilityClassifier
DecisionEventReconstructabilityService
MissingEvidenceDiagnosticFactory
```

Classification logic should be simple and testable.

### Inputs

`F` when:

- at least one authoritative input fragment exists;
- input hash/source ids are present;
- redaction status is known.

`P` when:

- input is known only as a hash without source binding;
- source exists but has no fragment reference;
- evidence is fixture/external only.

`O` when:

- the decision likely consumed input but none is persisted.

`S` rarely applies to inputs.

### Policy basis

`F` when:

- policy id/version or ontology version/domain pack/Kanon decision is linked;
- evaluation result is present.

`P` when:

- policy id exists but no version/evaluation result;
- ontology version exists but no rule/policy binding.

`O` when:

- action should have a policy basis but no evidence exists.

`S` when:

- action kind is explicitly outside policy scope and has documented default behavior.

### Operator identity

`F` when:

- actor id, kind, roles, and authenticated/delegated context are known.

`P` when:

- actor id exists without roles or auth context.

`O` when:

- action occurred but originator cannot be identified.

`S` when:

- deterministic scheduled system transition explicitly has no human/agent operator, but service identity is still recommended.

### Authorization envelope

`F` when:

- allow/deny/defer decision is known;
- authority source is known;
- required/satisfied roles or human gate/policy id are present.

`P` when:

- decision result exists but scope/authority is incomplete.

`O` when:

- action executed but no authorization evidence exists.

`S` when:

- explicitly documented as not requiring authorization. Use sparingly.

### Reasoning evidence

Never require hidden chain-of-thought.

`F` when:

- deterministic rule or explicit policy summary fully explains the decision path; or
- human approval note plus policy summary is sufficient for the action kind.

`P` when:

- safe surrogate exists but is incomplete or generic.

`O` when:

- no safe surrogate exists and the decision involved model/tool selection or semantic judgment.

`S` when:

- deterministic system transition has no reasoning slot.

### Output action

`F` when:

- action kind, status, and target/tool/resource are known.

`P` when:

- action kind/status exist but target is missing.

`O` when:

- the event implies an action but no action record exists.

`S` rarely applies.

### Post-condition state

`F` when:

- verified state after, delta hash, or state snapshot exists.

`P` when:

- execution status is known but state after is unverified.

`O` when:

- mutating action executed and no state-after evidence exists.

`S` when:

- action is explicitly non-mutating / not-applicable.

## Slice C — Governance status

Implement `PASS/WARN/FAIL` separately from the strict score. For critical actions, missing policy/actor/auth/action/input must be blocking. ReasoningEvidence should not block solely due to absent hidden chain-of-thought if a safe surrogate exists.

## Slice D — Missing-evidence diagnostics

Return diagnostics that identify likely owner service and suggested fix. This will make the console useful to developers.

## Slice E — Routes

Preferred Kanon endpoints:

```http
GET /ontology/v0/decision-events/{decisionEventId}/reconstructability
GET /ontology/v0/reconstructability/by-run/{runId}
GET /ontology/v0/reconstructability/by-trace/{traceId}
GET /ontology/v0/reconstructability/by-model-call/{modelCallId}
```

Regenerate route inventory and OpenAPI baseline if the repo uses those gates.

## Required tests

- Classifier tests for all properties and all F/P/S/O outcomes.
- Governance status tests for low/medium/high/critical severity.
- Safe reasoning tests proving hidden chain-of-thought is not required or stored.
- Missing diagnostic tests with owner-service hints.
- Endpoint contract tests.
- Evidence graph link tests if the Evidence Spine already has graph expansion routes.

## Docs

Add:

```text
docs/contracts/DECISION_EVENT_SCHEMA_V0.md
docs/contracts/RECONSTRUCTABILITY_REPORT_V0.md
docs/governance/DECISION_RECONSTRUCTABILITY_SPINE.md
docs/operators/DECISION_RECONSTRUCTION_OPERATOR_GUIDE.md
```
