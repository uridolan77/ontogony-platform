# Frontend contract handoff

## Goal

Ensure `ontogony-frontend` can consume Aisthesis live evidence without fabricated data.

## Required routes

```text
GET /aisthesis/v0/capabilities
GET /aisthesis/v0/evidence/lookup?traceId={traceId}
GET /aisthesis/v0/evidence/traces/{traceId}/timeline
GET /aisthesis/v0/evidence/traces/{traceId}/graph
GET /aisthesis/v0/evidence/traces/{traceId}/reconstructability/v2
GET /aisthesis/v0/evidence/traces/{traceId}/bundle
POST /aisthesis/v0/evaluation/traces/{traceId}/runs
GET /aisthesis/v0/evaluation/traces/{traceId}/runs/latest
```

## Required UI states

- trace not found;
- Aisthesis unavailable;
- reconstructability partial;
- missing required edge;
- ambiguous required edge;
- bundle export unavailable;
- producer coverage incomplete;
- protected payload reference warning;
- live proof redacted summary.

## Required components

```text
AisthesisTraceSearch
AisthesisTraceIdentityCard
AisthesisProducerCoveragePanel
AisthesisRequiredEdgeDiagnosticsPanel
AisthesisReconstructabilityScorePanel
AisthesisTimelinePanel
AisthesisGraphPanel
AisthesisBundleExportAction
AisthesisRawEvidenceDrawer
```

## Non-negotiable

The frontend must not fabricate evidence. Demo data must be visibly marked as demo and disabled in live mode.
