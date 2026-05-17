# allagma-dotnet Alignment Plan

## Objective

Make Allagma expose governed runtime capabilities explicitly and typed enough for the operator console.

## PRs

### ALLAGMA-BFA-001 — Run operation capability endpoint
Expose operation support metadata for startRun, resume, retry, cancel, and replayTrigger.

### ALLAGMA-BFA-002 — Retry/cancel/replay-trigger decision
Implement supported operations or explicitly mark unsupported/deferred. Add idempotency, auth, events, audit metadata, OpenAPI, and tests.

### ALLAGMA-BFA-003 — Typed replay/evidence API
Add stable response schema for replay/evidence workbench.

### ALLAGMA-BFA-004 — Event payload contracts
Type or document failure, human gate, Conexus model call/stream, retry/cancel/replay events.

### ALLAGMA-BFA-005 — Frontend consumption
Refresh OpenAPI and drive operation panel from backend metadata.

## Acceptance

- Frontend run operation panel is driven by backend capability truth.
- Replay/evidence UI can consume typed schemas.
