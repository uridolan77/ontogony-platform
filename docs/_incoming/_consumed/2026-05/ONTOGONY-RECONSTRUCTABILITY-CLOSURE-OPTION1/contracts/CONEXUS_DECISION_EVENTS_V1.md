# Conexus Decision Events v1

## Purpose

Conexus must expose reconstructable model-access decisions without leaking raw prompts, completions, keys, or provider secrets.

## Endpoint options

Choose one of these based on current Conexus architecture:

### Option A — dedicated endpoint

```text
GET /conexus/v0/model-calls/{modelCallId}/decision-events
GET /conexus/v0/traces/{traceId}/decision-events
```

### Option B — evidence bundle extension

Add `decisionEvents[]` to existing model-call evidence/admin detail responses.

Preferred first implementation: Option B if it avoids route sprawl.

## Event kinds

```text
conexus.route_decision
conexus.provider_attempt
conexus.fallback_decision
conexus.model_call
conexus.quota_policy_evaluation
conexus.cache_decision
conexus.streaming_lifecycle
conexus.admin_mutation
```

## Required redaction

Fields allowed:

```text
modelCallId
routeDecisionId
projectId hash/ref
model alias
provider id
route id
inputHash
outputHash
usage/cost
latency
capability profile
cache key hash
quota policy id
traceId
correlationId
```

Fields forbidden:

```text
raw prompt
raw completion
API key
provider secret
connection string
authorization header
unredacted user content
```

## Classifier expectation

High/critical Conexus events must classify PASS/WARN through Kanon, not FAIL.
