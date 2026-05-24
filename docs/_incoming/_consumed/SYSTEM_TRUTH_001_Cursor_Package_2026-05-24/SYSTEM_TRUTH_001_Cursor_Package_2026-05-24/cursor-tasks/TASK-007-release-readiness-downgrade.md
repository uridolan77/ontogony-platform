# TASK-007 — Release Readiness Truth

## Goal

Prevent generated/fixture readiness from looking like release readiness.

## Requirements

- Rename artifact-only surface if needed.
- Fixture-only routes cannot count as ready for release posture.
- Generated route readiness must be marked generated.
- Release posture should be `not_assessed` when no live validation exists.

## Acceptance

No route/readiness page overstates release posture.
