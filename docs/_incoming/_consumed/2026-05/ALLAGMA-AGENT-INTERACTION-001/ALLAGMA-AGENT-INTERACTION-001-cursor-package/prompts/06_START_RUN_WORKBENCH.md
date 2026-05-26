# Cursor prompt — Start Run and Allagma overview polish

Implement the operator UX improvements around run creation and run list cards.

## Start Run

Add:

- request preview
- model purpose validation against runtime model-purpose config when available
- idempotency key explanation
- start button
- start and open run detail
- start and open Agent Interaction

## Idempotency copy

Use this meaning:

```text
Reuse the same idempotency key only when retrying the same request. Generate a new key for a new run.
```

## Run list polish

Fix:

- no unlabeled standalone `unknown`
- hide raw fake provider response from card summary
- show purpose, provider mode, ontology, player/context, planning decision ID, model-call ID, status
- structure stream-withheld output as output withheld / length / hash / reason
- rename `Failed runs (sample)` to truthful scope

## Tests

Add tests for:

- run card does not contain raw fake response text
- run card does not contain unlabeled unknown
- stream withheld renders structured fields
- Start Run preview updates
- Start and open Agent Interaction navigates correctly after creation
