# Decision Event Profiles v1

## allagma.run_operation

Expected severity:

```text
low for completed/cancelled
critical for failed/denied
```

Required:

```text
run record fragment
objective hash when available
planning Kanon decision fragment when available
operator identity
terminal output action
verified run status post-condition
trace/correlation/run IDs
```

## allagma.human_gate_decision

Expected severity:

```text
high
```

Required:

```text
human gate record fragment
Kanon human-gate decision fragment
resume/deny decision fragment when resolved
operator identity
authorization envelope with scope/humanGateId
output action: human_gate_resolution
verified post-condition state
```

## allagma.agent_tool_call

Expected severity:

```text
medium when allowed
high when blocked
critical only if real side-effect execution becomes enabled in a future package
```

Required:

```text
tool intent record fragment
Kanon tool policy fragment when available
tool name/scope
authorization envelope
output action: allowed/blocked
verified post-condition state
```

## allagma.policy_evaluation

Expected severity:

```text
critical for exceeded
medium for warning
low for within_policy/unknown unless profile says otherwise
```

Required:

```text
budget snapshot fragment
budget policy id
evaluation result
operator identity
authorization envelope
verified post-condition state
```

## conexus.route_decision

Expected severity:

```text
high if provider fallback or policy deny affects model access
medium otherwise
```

Required:

```text
route decision id
project id hash/ref
model alias
selected provider/route
capability profile
policy/fallback basis
output action
post-condition state
```

## conexus.model_call

Expected severity:

```text
high if failed/denied/quota blocked
medium if completed
```

Required:

```text
modelCallId
routeDecisionId
model alias
provider id
input hash
output hash or interruption status
usage/cost metadata
redaction status
post-condition state
```

## conexus.quota_policy_evaluation

Expected severity:

```text
critical if quota exceeded
medium if warning
low if within policy
```

Required:

```text
quota policy id
project id hash/ref
usage counters
decision
post-condition state
```

## conexus.cache_decision

Expected severity:

```text
low normally
medium if stale bypass/error affects output
```

Required:

```text
cache policy id
cache key hash
hit/miss/bypass
route/model call relation
```

## workflow_transition

Expected severity:

```text
low normally
medium/high only if transition gates execution or causes terminal failure
```

Required:

```text
checkpoint/workflow event fragment
node id when available
output action
post-condition state
```
