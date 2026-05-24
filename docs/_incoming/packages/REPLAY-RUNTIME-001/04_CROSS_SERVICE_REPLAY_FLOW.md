# 04 — Cross-service replay flow

## Primary operator flow

### 1. Enter root identifier

The operator pastes or selects any supported identifier:

- run ID;
- trace ID;
- correlation ID;
- model call ID;
- route decision ID;
- provider attempt ID;
- Kanon decision ID;
- Evidence Spine bundle ID/path.

Frontend uses the existing Evidence Spine resolver first. Manual target kind override remains supported for ambiguous identifiers.

### 2. Resolve Evidence Spine graph

The resolver builds the current graph:

- Allagma run/events/audit/interaction stream;
- Kanon decision/provenance/replay bundle;
- Conexus model call/route decision/provider attempts;
- trace/correlation identifiers;
- human gate/eval/baseline links when present.

The graph is the source of replay target discovery. Replay must not create a parallel evidence graph.

### 3. Classify replay eligibility

Frontend calls Allagma replay orchestration eligibility.

Allagma:

1. normalizes the root target;
2. reads local Allagma evidence when the root is a run/audit/interaction stream;
3. asks Kanon for semantic target eligibility when Kanon nodes are present;
4. asks Conexus for model-call/route-decision/provider-attempt eligibility when Conexus nodes are present;
5. returns one merged eligibility summary.

### 4. Select replay mode

The UI displays only allowed modes:

- exact replay is hidden unless fully proven;
- deterministic simulation is enabled for governed fake/pinned cases;
- dry-run is enabled for planning/routing-only cases;
- reconstructed/evidence-only are always safer defaults when evidence exists;
- unavailable explains missing evidence or safety blocks.

### 5. Preview

The preview must show:

- replay target;
- mode;
- services involved;
- what data will be read;
- what will be simulated;
- what will be skipped;
- whether real providers are blocked;
- whether real tools are blocked;
- redaction posture;
- expected output artifacts.

### 6. Confirm risky mode

Any mode that could execute computation, even fake/local deterministic simulation, should show a confirmation dialog. Any mode that requests real provider/tool execution must be blocked in this package unless explicitly deferred behind future trust model work.

### 7. Run replay/simulation

Allagma creates a replay record and performs the orchestration:

- Allagma local attempt: run/audit/interaction reconstruction.
- Kanon attempt: decision/provenance replay bundle and optional semantic simulation.
- Conexus attempt: model-call evidence and model-call dry-run when orchestrated from Allagma; route-decision dry-run via Conexus admin when the UI or operator calls it directly (**not** auto-appended by Allagma in REPLAY-RUNTIME-002 — see `docs/_incoming/_active/REPLAY-RUNTIME-002/IMPLEMENTATION_NOTES.md`).
- Platform contract: normalize evidence references and graph links.

### 8. Produce result bundle

Allagma returns:

- replay request;
- eligibility snapshot;
- service attempts;
- result summary;
- delta;
- Evidence Spine links;
- export bundle references;
- unavailable/skipped reasons.

### 9. Compare original vs replay evidence

Comparison starts conservative:

- original status vs replay status;
- event count/timeline hash;
- Kanon decision verdict/action/gate result;
- Conexus route decision provider/model alias;
- fake-provider output fingerprint;
- cost/usage estimate if present;
- evidence completeness.

### 10. Link result back to Evidence Spine

Replay result becomes a new Evidence Spine node:

```text
replay.result:{replayId}
```

Edges:

- `replays` -> original target;
- `uses_original_evidence` -> source evidence nodes;
- `produced` -> replay result/delta/bundle artifacts;
- `attempted_service` -> Allagma/Kanon/Conexus attempt nodes;
- `blocked_by` -> safety/unavailable reasons when applicable.

## Existing page entrypoints

Replay should be linked from:

- Evidence Spine graph node actions;
- Allagma Run Detail;
- Allagma Audit Journey;
- Agent Interaction timeline;
- Conexus model-call evidence detail;
- Kanon decision/provenance detail;
- governed fake evidence artifacts.

Replay should not be linked from:

- generic home page cards;
- route inventory tables unless the route row is itself a replay target;
- settings pages;
- readiness/status dashboards except as evidence links.
