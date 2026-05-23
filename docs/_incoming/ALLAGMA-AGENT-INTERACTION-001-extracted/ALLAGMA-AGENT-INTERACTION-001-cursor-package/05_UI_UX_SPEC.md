# UI / UX specification

## Page header

Preferred title:

```text
Agent Interaction
```

Subtitle:

```text
Inspect a real Allagma run as an interaction timeline: messages, model calls, tool decisions, human gates, and evidence links.
```

## Mode switch

Use a segmented control or tabs:

```text
Live lookup | Fixture replay | Imported JSONL
```

### Live lookup badge

```text
Data source: live
```

### Fixture replay badge

```text
Demo fixture — not live evidence
```

Secondary text:

```text
Fixture sessions are useful for UI replay only. They do not prove service readiness or cross-service connectivity.
```

### Imported JSONL badge

```text
Imported replay — offline artifact
```

## Lookup area

Fields:

- Identifier input.
- Kind selector/auto-detection if available.
- Resolve button.
- Use latest completed run.
- Use latest failed run.
- Open in Evidence Spine.

## Session summary

Show:

- run ID
- status
- started/completed timestamps
- objective/purpose
- actor/historical actor
- trace/correlation IDs
- ontology version
- planning decision ID
- action decision IDs
- model call ID
- provider mode
- evaluation/baseline IDs
- data source/mode

## Timeline layout

Recommended card fields:

```text
[time] [phase badge] [status badge]
Title
Short operator-readable explanation
Evidence: source endpoint / linked entity / copy ID
Details expander
```

Example:

```text
06:41:19.259  Planning  requested
Kanon plan requested
Allagma requested a semantic plan from Kanon for ontology gaming-core@0.1.0.
Evidence: /allagma/v0/runs/{runId}/events
```

## Messages panel

Sections:

- Operator/user objective
- Allagma instruction/context
- Kanon plan summary
- Conexus model-call messages
- Provider output summary

Use redaction states:

```text
visible
redacted
withheld
not_recorded
not_available
```

## Tool and gate panel

Show a compact summary above detailed timeline rows:

```text
Tool intents: 1 proposed, 1 blocked, 0 executed
Human gates: 0 waiting, 0 approved, 0 denied
```

## Provider panel

Show:

```text
Purpose: summarize-player-risk
Alias: risk-summary-v0
Provider mode: fake
Provider: fake
Provider model: fake.chat
Fallback used: no
Tokens: 13 in / 12 out
Latency: ...
Cost: ...
```

If unavailable, label exactly what is unavailable.

## Run list card summary

Bad current shape:

```text
unknown
Fake Conexus provider response to: Purpose: summarize-player-risk Input: {...}
```

Target shape:

```text
Purpose: summarize-player-risk
Status: completed
Provider mode: fake
Ontology: gaming-core@0.1.0
Player: 123
Planning decision: decision_...
Model call: chatcmpl-...
```

Raw response belongs behind:

```text
View raw response
```

## Start Run UX

Add:

- request preview card
- idempotency key explanation
- model purpose validity status
- action buttons:
  - Start run
  - Start and open run detail
  - Start and open Agent Interaction
