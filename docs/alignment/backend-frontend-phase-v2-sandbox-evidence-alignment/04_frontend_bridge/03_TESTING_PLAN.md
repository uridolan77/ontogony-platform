# Frontend Testing Plan

## Unit tests

Adapter tests: full evidence, missing evidence, dry-run only, completed execute, failed execute, replay-safe execute, marker path from `MarkerRelativePath`, marker path from `externalRefs["sandbox_file"]`, marker path from `externalRefs["sandbox.file:marker_relative_path"]`, unknown phase/status, redaction.

Timeline tests: renders Started, Completed, Blocked, Failed, ReplaySkipped, and safe fallback for unknown events.

Capability tests: real external execution blocked, local sandbox execute enabled/disabled, retry/cancel/replay-trigger unsupported/deferred.

## Integration tests

Run detail loads audit bundle, sandbox evidence card visible, timeline events visible, empty state visible, limitation banners match capability metadata.

## E2E smoke

Start local backend stack if feasible, open frontend run detail route, verify sandbox evidence section, verify no raw content displayed.
