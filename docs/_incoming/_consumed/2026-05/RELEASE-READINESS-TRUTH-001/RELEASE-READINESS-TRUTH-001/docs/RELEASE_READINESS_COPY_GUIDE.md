# Release Readiness Copy Guide

## Replace misleading copy

### Before

```text
Release readiness
Per-route operator readiness scorecard.
Ready 24 / Partial 8 / Gap 0
```

### After

```text
Release readiness artifact
Generated route coverage and operator-readiness notes. This is not a release-candidate certification unless live validation is attached.
Artifact ready 24 / Artifact partial 8 / Artifact gaps 0
```

## Posture copy

### Current generated-only state

```text
Release-candidate posture: Not assessed
This page is generated from route metadata. No live backend or semantic validation is attached to this scorecard.
```

### Stale artifact

```text
The generated readiness artifact is stale. Regenerate with npm run readiness:sync before using this page for release planning.
```

### Fixture-only route

```text
Demo only. This route renders from fixture/generated data and does not count toward release readiness.
```

### Unknown source

```text
Unresolved. This route's data source is not classified, so it cannot be counted toward release posture.
```

### Live-with-fallback

```text
Needs review. Live data is mixed with fallback values; inspect the fallback reason before treating this route as ready.
```

## Words to avoid

Avoid unless qualified:

```text
ready
release-ready
passed
green
certified
validated
production-ready
```

Use instead:

```text
artifact-ready
generated-only
live validation not attached
candidate after live validation
needs review
unresolved
```
