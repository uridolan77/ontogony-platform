# PR-002 — Conexus decision-event emitters

## Repo focus

```text
C:\dev\conexus-dotnet
```

## Goal

Expose normalized reconstructability decision events for Conexus model-access decisions.

## Event kinds to implement

Start with these:

```text
conexus.route_decision
conexus.model_call
conexus.quota_policy_evaluation
conexus.cache_decision
conexus.streaming_lifecycle
```

Add later if easy:

```text
conexus.provider_attempt
conexus.fallback_decision
conexus.admin_mutation
```

## Implementation choices

Prefer a projector over existing model-call/admin evidence records:

```text
ConexusDecisionEventProjector
```

Avoid duplicating journal storage unless needed.

## Contract

Create:

```text
src/Conexus.Contracts/Reconstructability/ConexusDecisionEventContracts.cs
docs/contracts/CONEXUS_DECISION_EVENTS_V1.md
```

The contract should align with Kanon's classifier expectations:

```text
schemaVersion
decisionEventId
decisionKind
severity
occurredAtUtc
serviceOfOrigin = "conexus"
evidenceFragments[]
traceId
correlationId
inputs
policyBasis
projectIdentity/operatorIdentity
authorizationEnvelope
reasoningEvidence
outputAction
postConditionState
relatedIds
```

## Endpoint strategy

Choose one:

### Preferred if existing evidence endpoint is mature

Add `decisionEvents` to model-call evidence bundle.

### Alternative

Add:

```text
GET /conexus/v0/model-calls/{modelCallId}/decision-events
GET /conexus/v0/traces/{traceId}/decision-events
```

## Redaction rules

Never export:

```text
raw prompt
raw model completion
API key
authorization token
provider secret
connection string
unredacted PII
```

Use:

```text
promptHash
completionHash
routeDecisionId
modelCallId
modelAlias
provider id
usage/cost
latency
stream status
```

## Tests

Add tests for:

```text
route decision projects with model alias, selected route, provider, capability profile
model call projects with input/output hashes, usage/cost, post-condition
quota exceeded projects as high/critical
cache hit/miss projects without raw prompt
streaming lifecycle projects final hash/interrupted status
all fragment refs resolve
no forbidden raw values appear in JSON
route inventory / OpenAPI updated if new endpoint added
```

## Acceptance criteria

```text
Conexus emits decision events for at least route_decision and model_call.
No raw prompt/completion/secret leakage.
Every referenced fragment exists.
Docs and current-state updated.
```
