# Agent Interaction / AG-UI Spine Contract

## Purpose

Define the backend contract that makes an Ontogony governed run renderable, streamable, and replayable by an AG-UI/operator interaction surface.

The contract binds four backend repos:

```text
Platform = canonical event schema and validation
Allagma = run interaction export and stream owner
Kanon = semantic authority events, human gates, review/decision references
Conexus = model-call lifecycle and model evidence links
```

## Required event qualities

Every emitted interaction event must be:

- deterministic for a persisted run;
- ordered;
- timestamped;
- typed by stable phase/kind;
- redacted by default;
- linked to evidence identifiers where available;
- explicit about missing evidence;
- safe for operator UI rendering;
- safe for replay/export.

## Minimum event phases

| Phase | Owner | Description |
| --- | --- | --- |
| `run.created` | Allagma | Run accepted and persisted. |
| `task.classified` | Allagma | Task classification produced. |
| `topology.selected` | Allagma | Execution topology selected. |
| `kanon.plan.requested` | Allagma/Kanon | Semantic plan requested. |
| `kanon.plan.compiled` | Kanon | Semantic plan / decision ID available. |
| `policy.evaluated` | Kanon | Action/policy evaluation outcome. |
| `human_gate.interrupted` | Kanon/Allagma | Run paused for human decision. |
| `human_gate.resumed` | Kanon/Allagma | Human gate approved/resumed. |
| `human_gate.denied` | Kanon/Allagma | Human gate denied/terminated. |
| `model.requested` | Allagma/Conexus | Model call requested through Conexus. |
| `model.routed` | Conexus | Route decision / provider selected. |
| `model.stream.started` | Conexus/Allagma | Streaming lifecycle started. |
| `model.stream.chunk` | Conexus/Allagma | Metadata-only chunk event. |
| `model.stream.completed` | Conexus/Allagma | Stream terminal event. |
| `model.completed` | Conexus | Non-streaming model call completed. |
| `run.completed` | Allagma | Run terminal success. |
| `run.failed` | Allagma | Run terminal failure with classified reason. |
| `evidence.linked` | All repos | Evidence link/path emitted. |

## Required identifiers

Events should include applicable identifiers:

```text
runId
traceId
correlationId
eventId
sequence
kanonDecisionId
humanGateId
modelCallId
routeDecisionId
evaluationRunId
baselineComparisonId
artifactId
evidenceBundlePath
```

## Redaction rules

Events must not contain:

```text
raw prompt
raw model completion
provider API key
project API key
service token
admin key
connection string
unapproved PII or raw domain payload
```

If textual content is required for UI demonstration, use deterministic fake fixtures or redacted summaries only.

## Stream/export equivalence

The JSONL export and SSE stream must be logically equivalent for a persisted run:

```text
same event order
same event ids
same phase sequence
same evidence references
same redaction state
```

The SSE stream may include keep-alive comments, but those must not alter the logical event stream.

## Resume semantics

SSE resume must support `Last-Event-ID` or documented equivalent behavior.

Acceptance:

```text
client receives events 1..N
client disconnects after event K
client resumes with Last-Event-ID=K
client receives K+1..N only
no duplicate side effects
no duplicated terminal event
```

## Relationship to Evidence Spine

AG-UI events are not a replacement for Evidence Spine. They are a presentation/replay layer that points to evidence objects.

Every event with a major external reference should include an evidence slot or link hint:

```text
kanonDecisionId -> Kanon decision/provenance route
modelCallId -> Conexus model-call evidence bundle/detail
routeDecisionId -> Conexus route-decision detail
runId -> Allagma run/audit/events
traceId/correlationId -> cross-service trace/evidence resolver
```

## Certification threshold

The AG-UI backend spine is certified when:

- schema validation passes;
- golden JSONL export passes;
- golden SSE stream passes;
- resume behavior passes;
- Conexus projection includes evidence-bundle links;
- Kanon human-gate/review semantics are representable;
- redaction report passes;
- missing-evidence negative cases are explicit and structured.
