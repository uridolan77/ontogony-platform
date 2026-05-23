# Start Run operator contract

## Purpose

Make starting an Allagma run understandable and immediately connected to Agent Interaction.

## UI fields

- purpose/model purpose
- player/context input
- ontology version
- topology override, if supported
- objective
- idempotency key
- actor context summary

## Request preview

Before submit, show:

```json
{
  "purpose": "summarize-player-risk",
  "objective": "Summarize risk status for player 123.",
  "ontologyVersionId": "gaming-core@0.1.0",
  "input": {
    "playerId": "123"
  }
}
```

Redact secrets and do not show hidden credentials.

## Idempotency explanation

```text
Reuse this idempotency key only when retrying the same request. Generate a new key for a new run.
```

## Actions

```text
Start run
Start and open run detail
Start and open Agent Interaction
```

## Validation

If runtime posture exposes valid model purposes, validate selected purpose before submit.

States:

```text
valid
invalid
unknown — backend did not expose purpose config
```

## Post-submit

The response should surface:

- run ID
- status
- planning decision ID if returned
- model call ID if returned or discovered later
- links to run detail and Agent Interaction
