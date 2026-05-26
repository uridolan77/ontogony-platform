# Missing Evidence Diagnostic V0

## Purpose

Missing evidence should not merely say “field missing.” It should tell the developer/operator which emitting service must be fixed and what fragment is required.

## Contract

```ts
MissingEvidenceDiagnosticV0 {
  diagnosticId: string;
  decisionEventId: string;
  property: string;
  classification: "P" | "S" | "O";
  severity: "info" | "warning" | "blocking";
  message: string;
  likelyOwnerService: "allagma" | "kanon" | "conexus" | "platform" | "frontend" | "unknown";
  missingFragmentTypes: string[];
  suggestedFix: string;
  relatedRoute?: string;
  relatedContract?: string;
  fragmentRefs: string[];
}
```

## Example diagnostics

### Missing policy basis

```json
{
  "property": "policyBasis",
  "classification": "O",
  "severity": "blocking",
  "message": "No policy, ontology version, domain pack, or Kanon decision was bound to this action.",
  "likelyOwnerService": "kanon",
  "missingFragmentTypes": ["policy_evaluation", "semantic_authority_binding"],
  "suggestedFix": "Persist the Kanon policy evaluation id or ontology version id on the originating decision event."
}
```

### Partial post-condition state

```json
{
  "property": "postConditionState",
  "classification": "P",
  "severity": "warning",
  "message": "The action reports execution but no verified state-after snapshot or delta hash is linked.",
  "likelyOwnerService": "allagma",
  "missingFragmentTypes": ["state_after_snapshot", "state_delta_hash"],
  "suggestedFix": "Capture a post-action state snapshot or explicit not-applicable marker for non-mutating operations."
}
```

### Opaque reasoning evidence

```json
{
  "property": "reasoningEvidence",
  "classification": "O",
  "severity": "info",
  "message": "No safe rationale surrogate is available. Hidden chain-of-thought is not required or captured.",
  "likelyOwnerService": "conexus",
  "missingFragmentTypes": ["safe_route_explanation", "validator_report"],
  "suggestedFix": "Add a short route-decision explanation or policy-evaluation summary when the action is high impact."
}
```
