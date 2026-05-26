# Merge gates

## Required before merge

- Static validator passes.
- New/updated docs tests pass.
- Acceptance matrix contains all required scenario ids.
- No required scenario has `FAIL`.
- Any deferral includes reason, owner, and next gate.
- Real tools remain blocked.
- Closeout report exists or the PR clearly states this is a preparatory partial.

## Required before alpha closure

- `validate-system-coh-001.ps1 -ReleaseMode` passes.
- `validate-runtime-lock.ps1 -ReleaseMode` passes.
- At least one live governed run evidence artifact exists.
- Correlation chain proof exists or is explicitly deferred.
- Kanon assistance and Conexus fallback are either proved or explicitly classified.
- Operator Evidence Spine visibility is either proved or explicitly classified.

## Do not merge if

- The package duplicates existing matrices instead of linking/updating them.
- Any service boundary is blurred.
- Real tool execution becomes enabled.
- Model provider routing appears in Allagma.
- Actor headers are sent to Conexus without an explicit privacy review.
- Release mode silently skips a required scenario.
