# Evidence Spine Operator Graph Spec

Evidence Spine should answer:

```text
What happened?
Which service produced it?
Which protocol act produced it?
Was it authoritative, draft-only, simulated, blocked, or local?
What evidence exists?
What evidence is missing?
```

## Supported roots

```text
runId
decisionId
modelCallId
routeDecisionId
traceId
correlationId
humanGateId
domainPackId
entityRef
```

## Node fields

```text
kind
label
service
protocolId
authorityMode
sideEffectLevel
id
status
links
```
