# Reconstructability Report V0

## Purpose

A `ReconstructabilityReport` evaluates a `DecisionEvent` property by property. It is deterministic and explainable.

## Classification enum

```text
F = fully_fillable
P = partially_fillable
S = structurally_unfillable
O = opaque
```

## Report contract

```ts
ReconstructabilityReportV0 {
  schemaVersion: "reconstructability-report.v0";
  reportId: string;
  decisionEventId: string;
  generatedAtUtc: string;
  classifierVersion: string;
  decisionKind: string;
  severity: string;
  propertyResults: ReconstructabilityPropertyResult[];
  desStrictCompletenessPct: number;
  ontogonyGovernanceStatus: "PASS" | "WARN" | "FAIL";
  blockingReasons: string[];
  warnings: string[];
  missingEvidence: MissingEvidenceDiagnostic[];
  linkedEvidenceFragments: EvidenceFragmentRef[];
  safeReasoningPolicy: SafeReasoningPolicySummary;
  metadata?: Record<string, unknown>;
}
```

## Property result

```ts
ReconstructabilityPropertyResult {
  property:
    | "inputs"
    | "policyBasis"
    | "operatorIdentity"
    | "authorizationEnvelope"
    | "reasoningEvidence"
    | "outputAction"
    | "postConditionState";
  classification: "F" | "P" | "S" | "O";
  score: 1.0 | 0.5 | 0.0;
  rationale: string;
  fragmentRefs: string[];
  requiredForGovernance: boolean;
  blocking: boolean;
  remediationHint?: string;
}
```

## Score calculation

```text
score(F) = 1.0
score(P) = 0.5
score(S) = 0.0
score(O) = 0.0

desStrictCompletenessPct = round(100 * sum(property scores) / 7, 1)
```

## Governance status calculation

For `high` and `critical` severity decisions:

- `inputs` must be `F`.
- `policyBasis` must be `F`.
- `operatorIdentity` must be `F`.
- `authorizationEnvelope` must be `F`.
- `outputAction` must be `F`.
- `postConditionState` must be `F`, or `S` only when the action is explicitly non-mutating / not-applicable.
- `reasoningEvidence` may be `P` if a safe surrogate exists.
- `reasoningEvidence` should not fail governance solely because hidden chain-of-thought is unavailable.

For `medium` severity decisions, missing `postConditionState` may produce `WARN` rather than `FAIL`.

For `low` severity read-only decisions, missing `policyBasis` may produce `WARN` if a documented default policy applies.

## Safe reasoning policy summary

```ts
SafeReasoningPolicySummary {
  hiddenChainOfThoughtCaptured: false;
  safeSurrogateAvailable: boolean;
  surrogateKinds: string[];
  note: string;
}
```

The implementation must always set `hiddenChainOfThoughtCaptured` to `false` unless an explicit future governance decision changes the rule. This package does not authorize hidden chain-of-thought capture.
