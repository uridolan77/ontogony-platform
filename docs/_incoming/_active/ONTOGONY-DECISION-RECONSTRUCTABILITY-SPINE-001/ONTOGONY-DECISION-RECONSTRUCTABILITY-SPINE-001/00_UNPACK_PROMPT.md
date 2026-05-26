# Generic Cursor Unpack Prompt

You are working on the Ontogony multi-repo system. Unpack and implement the package `ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001`.

## Objective

Add a governed, cross-service **Decision Reconstructability Spine** so that every critical agent action can be reconstructed from explicit evidence fragments. The implementation must distinguish ordinary telemetry from governance-grade reconstructability.

## Repositories

Review and update only what is necessary in these repos:

- `ontogony-platform`
- `allagma-dotnet`
- `kanon-dotnet`
- `conexus-dotnet`
- `ontogony-frontend`
- `ontogony-ui`

## First pass: inspect before editing

Before making changes:

1. Inspect current Evidence Spine docs, contracts, route inventories, OpenAPI baselines, and console pages.
2. Identify existing objects that already represent decisions, traces, routes, gates, audit records, model calls, source bindings, quality snapshots, and semantic graph nodes.
3. Reuse existing contracts and naming patterns wherever possible.
4. Do not invent parallel abstractions if an existing Ontogony object can be extended cleanly.
5. Write brief working notes in `docs/_incoming/_active/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/IMPLEMENTATION_NOTES.md`.

## Required implementation shape

Implement the smallest strong vertical slice that proves the architecture:

1. A canonical `DecisionEvent` contract or equivalent DTO shape.
2. A `ReconstructabilityReport` contract.
3. A classifier that produces `F`, `P`, `S`, or `O` per property.
4. Missing-evidence diagnostics with actionable remediation hints.
5. At least one Allagma run operation/gate action reconstructed end-to-end.
6. At least one Conexus model-call/route-decision action reconstructed end-to-end.
7. At least one Kanon semantic decision/policy-basis action reconstructed end-to-end.
8. Frontend reconstruction panel rendering score, property table, missing evidence, linked fragments, and state-before/state-after where available.
9. Golden tests and fixtures proving PASS/FAIL cases.

## Reconstructability properties

Use these seven canonical properties:

- `inputs`
- `policyBasis`
- `operatorIdentity`
- `authorizationEnvelope`
- `reasoningEvidence`
- `outputAction`
- `postConditionState`

The `reasoningEvidence` property must never require hidden model chain-of-thought. It may be `P` when a safe surrogate exists, such as a route rationale, policy-evaluation summary, explicit model output rationale, validator report, or human note. It is `O` when no external safe evidence exists. It is `S` only when the decision path is deterministic and structurally has no model-deliberation slot.

## Classification semantics

- `F` — Fully fillable from persisted evidence; enough to reconstruct the property without guessing.
- `P` — Partially fillable; some evidence exists but is incomplete, indirect, lossy, or missing a binding.
- `S` — Structurally unfillable; the system path does not produce this property, and that absence is explicit.
- `O` — Opaque; the property may exist but is not observable from persisted artifacts.

Score mapping:

```text
F = 1.0
P = 0.5
S = 0.0
O = 0.0
```

Keep two scores separate:

1. `desStrictCompletenessPct` — strict property score over the seven properties.
2. `ontogonyGovernanceStatus` — `PASS`, `WARN`, or `FAIL` based on blocking properties, explicit safety rules, and whether a safe rationale surrogate is adequate for the action type.

## Blocking rules for critical actions

For critical actions, governance must fail unless these are `F`:

- `inputs`
- `policyBasis`
- `operatorIdentity`
- `authorizationEnvelope`
- `outputAction`

`postConditionState` must be `F` or explicitly `S` with a non-state-changing action type. `reasoningEvidence` may be `P` if a safe surrogate exists; it must not block solely because hidden chain-of-thought is unavailable.

## Documentation requirement

Update repo docs to explain:

- the distinction between trace presence and reconstructability;
- the `F/P/S/O` classification;
- how critical actions are gated;
- how missing evidence is remediated;
- how frontend operators interpret the reconstruction panel.

## Testing requirement

Add tests for:

- all four classification values;
- strict score computation;
- blocking governance rules;
- missing evidence diagnostics;
- route/API contract stability;
- frontend rendering for full, partial, structurally missing, and opaque evidence;
- a cross-service golden fixture linking Allagma, Kanon, and Conexus fragments.

## Finish condition

The package is complete only when:

- tests pass in modified repos;
- route inventories/OpenAPI snapshots are regenerated where required;
- docs reflect the actual implementation rather than this package's proposal;
- the frontend has no fake-only labels unless explicitly marked as fixture data;
- a local operator can open one decision and see reconstructability status, missing evidence, and linked supporting fragments.
