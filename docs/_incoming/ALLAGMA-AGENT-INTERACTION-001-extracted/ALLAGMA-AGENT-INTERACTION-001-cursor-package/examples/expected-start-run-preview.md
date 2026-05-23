# Expected Start Run preview

## Request preview

```json
{
  "purpose": "summarize-player-risk",
  "objective": "Summarize risk status for player 123.",
  "ontologyVersionId": "gaming-core@0.1.0",
  "input": {
    "playerId": "123"
  },
  "topologyOverride": "centralized_orchestrator"
}
```

## Model purpose status

```text
Model purpose: summarize-player-risk
Alias: risk-summary-v0
Status: valid
```

or:

```text
Model purpose: summarize-player-risk
Status: unknown — backend did not expose model-purpose config
```

## Idempotency helper

```text
Reuse this idempotency key only when retrying the same request. Generate a new key for a new run.
```

## Actions

```text
Start run
Start and open run detail
Start and open Agent Interaction
```
