# Acceptance matrix

## Global acceptance

| Gate | Acceptance |
|---|---|
| Aisthesis fixture regression | Fixture smoke remains PASS |
| Producer tests | Each producer repo passes Release tests |
| Required edges | Live trace has present=10, missing=0 |
| Overall grade | Live trace reconstructability v2 grade is `complete` |
| Bundle | Live bundle has fingerprint, producer summary, identifier summary, timeline, graph, missingEdges=[] |
| Honesty | Live proof is not claimed if live mode returns NOT_RUN |

## Producer acceptance

| Producer | Required proof |
|---|---|
| Allagma | Emits run envelopes and edges to Kanon plan/decision and Conexus model call |
| Kanon | Emits semantic plan, decision, policy evaluation, and decision→plan/policy edges |
| Conexus | Emits route decision, model call, provider attempt identifier/edge |
| Metabole | Emits pipeline, profile, mapping candidate, artifact and produced/materialized edges |
