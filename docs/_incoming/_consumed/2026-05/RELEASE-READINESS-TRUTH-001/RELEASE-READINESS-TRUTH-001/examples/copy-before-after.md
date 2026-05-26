# Copy Before/After Examples

## Page header

### Before

```text
Release readiness
Per-route operator readiness scorecard (FE-9-007). Regenerate with npm run readiness:sync.
```

### After

```text
Release readiness artifact
Generated route coverage and operator-readiness notes. This is not a release-candidate certification unless live validation is attached.
```

## Fixture/demo banner

### Before

```text
Fixture / demo only: Renders docs/generated/operator-release-readiness.json produced by npm run readiness:sync. No live backend client; RC posture is mechanical, not semantic.
```

### After

```text
Generated artifact only
This page renders docs/generated/operator-release-readiness.json. It does not validate live backend behavior or semantic evidence, so release-candidate posture is not assessed.
```

## Summary labels

### Before

```text
Ready 24
Partial 8
Gap 0
```

### After

```text
Artifact ready 24
Artifact partial 8
Artifact gaps 0
Release-candidate eligible 0
```

## Unknown source row

### Before

```text
/allagma/runs/:runId/audit   allagma   unknown   partial
```

### After

```text
/allagma/runs/:runId/audit
Data source: unknown
Artifact status: partial
Release impact: unresolved
Reason: Data source is not classified.
Next action: Classify the route data source and add route-level evidence.
```
