# Risks and non-goals

## Non-goals

- Do not enable real external tools.
- Do not implement SANDBOX-003.
- Do not add arbitrary external I/O.
- Do not make Agent Interaction a replacement for Evidence Spine.
- Do not build a full streaming workbench in this slice; only design extension points and avoid blocking future streaming.
- Do not overbuild backend APIs if existing routes already contain sufficient data.

## Risks

### Risk: fixture data continues to look live

Mitigation:

- hard badge
- explicit mode
- data-source field in export
- fixture never counts for readiness

### Risk: backend data is insufficient for messages

Mitigation:

- render `not_recorded` / `redacted` / `not_available`
- avoid inventing messages from summaries
- add backend fields only if necessary

### Risk: raw prompts or secrets are exposed

Mitigation:

- default to summaries and redaction
- expand raw data only behind explicit details
- never render secret-looking fields unredacted

### Risk: “unknown” remains everywhere

Mitigation:

- classify unknown what: task type, provider mode, readiness, source, decision, message availability
- tests should catch standalone unlabeled `unknown` in cards

### Risk: timeline becomes graph UI

Mitigation:

- Agent Interaction should tell the chronological interaction story
- Evidence Spine remains the graph/cross-service resolver
- Links between the two should be clear
