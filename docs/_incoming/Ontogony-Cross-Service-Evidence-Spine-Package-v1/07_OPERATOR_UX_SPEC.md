# Operator UX spec

## Entry points

Add a top-level or System-level route:

```text
/evidence
/evidence/spine
/system/evidence-spine
```

Recommended label:

```text
Evidence spine
```

## Workbench layout

```text
[Paste any ID ___________________________________] [Resolve]

Detected as: modelCallId | runId | traceId | decisionId | evalId | manual override

Resolution summary:
- 7 nodes resolved
- 9 edges resolved
- 2 expected links missing
- 1 service warning

Graph:
[Allagma Run] → [Kanon Decision] → [Conexus Route Decision] → [Conexus Model Call]
       ↓
[Evaluation] → [Evidence Export]
       ↓
[Baseline Comparison]

Actions:
- Open run
- Open eval
- Open Conexus observability
- Open Kanon decision
- Export evidence spine
```

## Node cards

Every node should show:
- node kind
- ID
- owner service
- status/verdict
- created/completed time if available
- confidence
- source API
- open link
- copy ID

## Missing edges

Do not just omit missing links. Show:

```text
Kanon decision not resolved
Reason: no planningDecisionId on run and trace lookup returned 404.
```

## Page embedding

Evidence spine lookup should appear on:
- Command center
- Conexus observability
- Allagma run detail
- Allagma evaluation detail
- Kanon decision/provenance pages
- Diagnostics export modal
