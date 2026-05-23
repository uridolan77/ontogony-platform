# Cursor prompt — E2E and tests

Add focused tests for `ALLAGMA-AGENT-INTERACTION-001`.

## Frontend unit/component tests

Required:

- mode switch renders all modes
- live mode default when backend reachable
- fixture mode badge
- imported JSONL badge
- session summary renders linked IDs
- Allagma events map to readable timeline phases
- tool/gate events render as first-class cards
- Conexus provider panel renders fake provider details
- Kanon decision panel renders planning decision
- unresolved reasons are shown
- export bundle has no duplicate sections
- run list hides raw fake provider response
- no unlabeled unknown in run cards
- Start Run preview and navigation actions

## Backend tests

Only if backend routes/fields are added. Add targeted tests for:

- new interaction endpoint
- new linked identifier fields
- redaction/not-recorded markers
- model-call ID persistence/exposure

## Manual E2E

Document one manual run:

1. Start local stack.
2. Create run for `summarize-player-risk`.
3. Open Agent Interaction in live mode.
4. Verify timeline and enrichments.
5. Export bundle.
6. Switch to fixture and verify demo badge.

Save validation notes under:

```text
artifacts/allagma-agent-interaction-001/<timestamp>/validation-summary.md
```
