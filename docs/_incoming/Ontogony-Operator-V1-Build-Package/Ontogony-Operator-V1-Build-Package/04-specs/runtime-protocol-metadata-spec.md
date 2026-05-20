# Runtime Protocol Metadata Spec

Do not protocolize everything. Add metadata only to evidence-producing cross-service acts.

## Minimal fields

```json
{
  "protocolId": "kanon.action.evaluate.v1",
  "authorityMode": "authoritative",
  "sideEffectLevel": "semantic_decision",
  "producerService": "allagma",
  "consumerService": "kanon"
}
```

## Suggested protocol ids

```text
allagma.run.start.v1
allagma.run.resume.v1
allagma.runtime-posture.v1
kanon.semantic-plan.compile.v1
kanon.action.evaluate.v1
kanon.human-gate.check.v1
kanon.semantic-graph.resolve.v1
conexus.chat-completion.create.v1
conexus.chat-completion.stream.v1
conexus.model-call.evidence.v1
conexus.route-decision.resolve.v1
ontogony.evidence-spine.resolve.v1
```

## Authority modes

```text
authoritative
draft_only
simulation_only
blocked
local_only
observational
gateway
unknown
```

## Side-effect levels

```text
none
read_only
evidence_record
semantic_decision
run_state_transition
model_call
local_sandbox_effect
real_external_blocked
unknown
```
