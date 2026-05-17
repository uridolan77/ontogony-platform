# Trace and Correlation Contract

## Goal

Make cross-service navigation deterministic:

Allagma run → Kanon decision/provenance/replay → Conexus model call/request → back to Allagma events.

## Canonical IDs

| ID | Owner |
|---|---|
| `traceId` | Platform/ingress |
| `correlationId` | Platform/integration context |
| `runId` | Allagma |
| `decisionId` | Kanon |
| `modelCallId` | Conexus |
| `requestId` | Conexus |
| `humanGateId` | Allagma/Kanon gate resolution |
| `replayBundleId` | Kanon/Allagma replay |

## Required propagation

Allagma → Kanon must include traceId, runId, actorId, tenantId and capture decisionId.

Allagma → Conexus must include traceId, runId, decisionId if available and capture modelCallId/requestId.

Kanon replay bundles must include decisionId, traceId, related run/model-call IDs where known, and provenance references.

Conexus diagnostics must include requestId, modelCallId, traceId, runId, decisionId, provider, model, status, and safe usage/token summary.
