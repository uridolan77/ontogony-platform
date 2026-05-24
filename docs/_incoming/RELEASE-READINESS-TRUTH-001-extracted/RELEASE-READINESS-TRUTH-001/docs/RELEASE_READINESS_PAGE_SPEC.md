# Release Readiness Page Spec

## Purpose

The page should be an operator-facing scorecard for the current route readiness artifact, while making clear whether that artifact has any live release-candidate authority.

## Required sections

### 1. Page header

Recommended copy:

```text
Release readiness artifact
Generated route coverage and operator-readiness notes. This is not a release-candidate certification unless live validation is attached.
```

### 2. Posture panel

Fields:

```text
Release-candidate posture: Not assessed | Not ready | Candidate | Passed
Basis: Generated artifact only | Live validation attached | Mixed
Reason: concise explanation
```

For current observed state:

```text
Release-candidate posture: Not assessed
Basis: Generated artifact only
Reason: No live backend or semantic validation is attached to this scorecard.
```

### 3. Artifact source panel

Fields:

```text
Artifact path
Generated timestamp
Freshness
Regeneration command
Source type
```

Example:

```text
Artifact: docs/generated/operator-release-readiness.json
Generated: 2026-05-23T17:18:00.362Z
Freshness: fresh
Regenerate: npm run readiness:sync
Source: generated route artifact
```

### 4. Summary cards

Recommended cards:

```text
Generated routes
Artifact ready
Artifact partial
Artifact gaps
Fixture/demo only
Unknown source
Live validation
```

Avoid unqualified `Ready`, `Partial`, `Gap`.

### 5. Route table

Columns:

```text
Route
Area
Data source
Artifact status
Release impact
Reason
Next action
```

If table width is limited, use expandable detail rows for reason/next action.

## Status rules

```text
fixture_only => demo-only, never release-ready
unknown => unresolved, needs classification
live_with_fallback => needs review unless fallback is explicitly non-critical
live => can be release-candidate eligible only with live validation
```

## Empty/error states

### Missing artifact

```text
No generated readiness artifact found.
Run npm run readiness:sync, then reload this page.
```

### Invalid artifact

```text
The generated readiness artifact could not be parsed.
Regenerate it with npm run readiness:sync. If the problem persists, check the artifact schema.
```

### Stale artifact

```text
This readiness artifact is stale. Regenerate before using it for planning.
```
