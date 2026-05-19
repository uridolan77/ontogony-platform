# 08 — Run detail evidence journey checklist

Primary route: `/allagma/runs/{subjectRunId}`

## Run detail checks

- [ ] Route resolves for guided `subjectRunId`
- [ ] Run status, actor, and timing fields render
- [ ] Event timeline renders with stable ordering
- [ ] Triage panels and suggested actions render when relevant
- [ ] Supported run operation controls behave as expected

## Evidence journey checks

- [ ] Evaluation journey section links to `/allagma/evaluations/{id}`
- [ ] Replay journey section links to `/allagma/replay?runId={subjectRunId}`
- [ ] Cross-service evidence links (Conexus/Kanon) render when IDs are present
- [ ] Links preserve trace/correlation identifiers through navigation

## Limitation/degraded checks

- [ ] Unsupported replay trigger/cancel/retry actions are shown as limitations when absent
- [ ] Failed dependent fetches show explicit degraded cards, not blank content
- [ ] Redacted failure payloads do not leak secrets

## Export checks

- [ ] Run triage evidence export panel is available
- [ ] Export action emits artifact or explicit error
- [ ] Exported payload contains expected run identifiers and redaction behavior

## Evidence

- [ ] Screenshot: run detail top section with run ID
- [ ] Screenshot: journey links section
- [ ] Artifact: run evidence export (or explicit failure proof)
- [ ] Note: trace/correlation continuity observation
