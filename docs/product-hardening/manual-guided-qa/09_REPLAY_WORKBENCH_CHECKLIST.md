# 09 — Replay workbench checklist

Primary route: `/allagma/replay`

Lookup modes:

- run: `?runId={subjectRunId}`
- trace: `?mode=trace&traceId={traceId}`
- decision: `?mode=decision&decisionId={planningDecisionId}`

## Core lookup checks

- [ ] Run lookup resolves using guided `subjectRunId`
- [ ] Trace lookup resolves using guided `traceId`
- [ ] Decision lookup resolves using guided planning decision ID (when available)
- [ ] Invalid lookup input yields validation feedback, not a crash

## Replay bundle checks

- [ ] Replay bundle panel renders when bundle exists
- [ ] Explicit unavailable state renders when no bundle exists
- [ ] Correlation source evidence panel renders with trace context
- [ ] Export panel is reachable from replay workbench

## Limitation checks

- [ ] Live replay trigger absence is shown via explicit limitation banner
- [ ] No fake replay trigger button/path is offered when backend route is absent
- [ ] Language clearly separates lookup/evidence from mutation capabilities

## Redaction/security checks

- [ ] Preview/export content redacts secrets as expected
- [ ] API keys/tokens do not appear in replay payload displays

## Evidence

- [ ] Screenshot: run lookup success
- [ ] Screenshot: trace or decision lookup success
- [ ] Screenshot: limitation banner for trigger absence
- [ ] Artifact: replay evidence export file (or fail capture)
