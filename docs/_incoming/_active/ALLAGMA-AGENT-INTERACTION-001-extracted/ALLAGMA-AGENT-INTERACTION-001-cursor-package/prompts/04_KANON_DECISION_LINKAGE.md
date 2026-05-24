# Cursor prompt — Kanon decision linkage

Inspect `kanon-dotnet` and frontend Kanon clients for decision/provenance routes.

Agent Interaction must show semantic authority involvement in the run.

Use existing routes first:

```text
GET /ontology/v0/decision-records/{decisionId}
GET /ontology/v0/decision-records/{decisionId}/provenance
GET /ontology/v0/semantic-graph
```

## Required display data

- planning decision ID
- action-evaluation decision ID(s), if present
- decision kind/type
- ontology version ID
- policy or gate outcome when available
- provenance status
- links to Kanon decision/provenance pages

## Distinguish decisions

Do not conflate:

```text
planningDecisionId
kanonDecisionId
actionDecisionId
humanGateDecisionId
```

If the backend currently records only planning decision, display that truthfully and mark action decisions as `not_recorded` when applicable.

## Tests

Add frontend tests for:

- planning decision visible
- action decision distinct when present
- missing action decision reason shown
- ontology version shown as full ID, e.g. `gaming-core@0.1.0`
