# Governed Fake E2E Contract

## Run input

```json
{
  "ontologyVersionId": "gaming-core@0.1.0",
  "actorId": "local-operator",
  "actorType": "human",
  "actorRoles": ["Admin"],
  "objective": "Summarize player risk for local governed fake E2E proof.",
  "context": {
    "playerId": "123"
  },
  "modelPurpose": "summarize-player-risk"
}
```

## Expected model routing

```text
modelPurpose: summarize-player-risk
alias: risk-summary-v0
providerKey: fake
providerModel: fake.chat
```

## Expected evidence chain

```text
allagma.run
  -> kanon.decision
  -> conexus.modelCall
  -> conexus.routeDecision
  -> conexus.providerAttempt
  -> platform.trace
  -> platform.correlation
```

## Required export metadata

```json
{
  "schema": "ontogony-cross-service-evidence-spine-bundle-v1",
  "scenario": "GOVERNED-FAKE-E2E-001",
  "source": "live",
  "fixture": false
}
```
