# Decision Event Schema V0

## Purpose

`DecisionEvent` is a normalized, cross-service view of an agentic decision or action. It does not replace service-native records. It binds them into one reconstructable object.

## Required top-level fields

```ts
DecisionEventV0 {
  schemaVersion: "decision-event.v0";
  decisionEventId: string;
  decisionKind: DecisionKind;
  severity: DecisionSeverity;
  occurredAtUtc: string;
  serviceOfOrigin: "allagma" | "kanon" | "conexus" | "platform" | "frontend";
  correlationId?: string;
  traceId?: string;
  runId?: string;
  threadId?: string;
  evidenceFragments: EvidenceFragmentRef[];
  inputs?: DecisionInputs;
  policyBasis?: PolicyBasis;
  operatorIdentity?: OperatorIdentity;
  authorizationEnvelope?: AuthorizationEnvelope;
  reasoningEvidence?: ReasoningEvidence;
  outputAction?: OutputAction;
  postConditionState?: PostConditionState;
  relatedIds?: RelatedDecisionIds;
  metadata?: Record<string, unknown>;
}
```

## Decision kinds

Start with these kinds; extend only when a real existing action requires it.

```text
run_operation
human_gate_decision
model_route_decision
model_call
provider_fallback
agent_tool_call
semantic_decision
policy_evaluation
source_binding_mutation
operator_review_resolution
quality_snapshot_decision
workflow_transition
```

## Severity

```text
low        read-only, non-mutating, no external action
medium     state-affecting inside Ontogony or operator-visible decision
high       external side effect, data mutation, policy/security impact
critical   destructive, irreversible, regulated, production-facing, or financial action
```

## Evidence fragments

Every field should cite one or more fragments when possible.

```ts
EvidenceFragmentRef {
  fragmentId: string;
  fragmentType: string;
  service: string;
  sourceId: string;
  sourceRoute?: string;
  artifactUri?: string;
  hash?: string;
  capturedAtUtc?: string;
  trustLevel: "authoritative" | "derived" | "fixture" | "external" | "unknown";
}
```

## Inputs

```ts
DecisionInputs {
  userRequestHash?: string;
  normalizedInputHash?: string;
  promptTemplateId?: string;
  toolInputHash?: string;
  retrievedEvidenceIds?: string[];
  sourceBindingIds?: string[];
  redactionStatus?: "none" | "redacted" | "hashed_only";
  fragmentRefs: string[];
}
```

## Policy basis

```ts
PolicyBasis {
  policyId?: string;
  policyVersion?: string;
  ontologyId?: string;
  ontologyVersionId?: string;
  domainPackId?: string;
  kanonDecisionId?: string;
  ruleIds?: string[];
  evaluationResult?: "allowed" | "denied" | "requires_human" | "not_applicable";
  fragmentRefs: string[];
}
```

## Operator identity

```ts
OperatorIdentity {
  actorId?: string;
  actorKind: "human" | "agent" | "system" | "service" | "unknown";
  roles?: string[];
  delegatedBy?: string;
  authenticated?: boolean;
  authMethod?: string;
  fragmentRefs: string[];
}
```

## Authorization envelope

```ts
AuthorizationEnvelope {
  authority: "policy" | "human_gate" | "system_default" | "service_key" | "none" | "unknown";
  decision: "allow" | "deny" | "defer" | "unknown";
  scope?: string;
  expiresAtUtc?: string;
  humanGateId?: string;
  approvalId?: string;
  requiredRoles?: string[];
  satisfiedRoles?: string[];
  fragmentRefs: string[];
}
```

## Reasoning evidence

No hidden chain-of-thought. Use safe external evidence only.

```ts
ReasoningEvidence {
  reasoningEvidenceKind:
    | "safe_rationale"
    | "policy_summary"
    | "route_explanation"
    | "validator_report"
    | "human_note"
    | "deterministic_rule"
    | "not_available";
  summary?: string;
  modelVisibleRationaleHash?: string;
  validatorReportId?: string;
  fragmentRefs: string[];
}
```

## Output action

```ts
OutputAction {
  actionKind: string;
  actionId?: string;
  targetService?: string;
  targetResource?: string;
  toolName?: string;
  method?: string;
  status: "requested" | "executed" | "blocked" | "failed" | "rolled_back" | "unknown";
  outputHash?: string;
  fragmentRefs: string[];
}
```

## Post-condition state

```ts
PostConditionState {
  stateKind: "none" | "state_delta" | "snapshot" | "external_unknown" | "not_applicable";
  beforeHash?: string;
  afterHash?: string;
  deltaHash?: string;
  stateResource?: string;
  verificationStatus?: "verified" | "unverified" | "not_applicable";
  fragmentRefs: string[];
}
```

## Related IDs

```ts
RelatedDecisionIds {
  allagmaRunId?: string;
  allagmaOperationId?: string;
  humanGateId?: string;
  kanonDecisionId?: string;
  canonicalFactId?: string;
  semanticPlanId?: string;
  sourceBindingId?: string;
  conexusModelCallId?: string;
  conexusRouteDecisionId?: string;
  traceId?: string;
  correlationId?: string;
}
```
