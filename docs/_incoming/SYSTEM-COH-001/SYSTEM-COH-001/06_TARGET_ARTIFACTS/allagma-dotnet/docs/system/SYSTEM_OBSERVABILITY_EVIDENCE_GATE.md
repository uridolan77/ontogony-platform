# SYSTEM-COH-001 — Observability evidence gate

## Purpose

Define what “observable alpha runtime” means for the Ontogony governed loop.

## Required observability dimensions

| Dimension | Expected proof |
|---|---|
| Service health | `/health` and `/ready` snapshots for Allagma, Kanon, Conexus |
| Run lifecycle | Allagma run events show planning, model request, completion/failure, gate states |
| Semantic decisions | Kanon decisions can be looked up by id and trace where applicable |
| Model calls | Conexus model call / execution journal can be looked up by request/model call id |
| Correlation | One correlation id joins Allagma event, Kanon decision, Conexus model metadata |
| Errors | Downstream failures classify into stable cohesion codes |
| Evidence Spine | Operator graph can start from run/trace/decision/modelCall/reviewItem roots |
| Replay/restart | Durable state survives restart where scenario requires it |

## Evidence artifact

The acceptance runner should write:

```json
{
  "schema": "ontogony-system-observability-evidence-v1",
  "timestampUtc": "...",
  "services": {
    "allagma": { "health": "PASS", "ready": "PASS" },
    "kanon": { "health": "PASS", "ready": "PASS" },
    "conexus": { "health": "PASS", "ready": "PASS" }
  },
  "correlation": {
    "traceId": "...",
    "correlationId": "...",
    "allagmaRunId": "...",
    "kanonDecisionId": "...",
    "conexusModelCallId": "..."
  },
  "notes": []
}
```

## Deferral rule

If full dashboard/SLO evidence is deferred, mark `observability_evidence_gate` as `DEFERRED_WITH_REASON` and link the next observability package. Do not silently omit it.
