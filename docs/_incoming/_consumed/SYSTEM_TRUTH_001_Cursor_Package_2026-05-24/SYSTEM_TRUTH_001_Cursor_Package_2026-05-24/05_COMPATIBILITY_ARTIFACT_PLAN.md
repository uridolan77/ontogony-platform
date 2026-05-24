# 05 — Compatibility Artifact Plan

## Purpose

The console currently says compatibility is unknown because no summary artifact is on disk. SYSTEM-TRUTH-001 should provide a machine-readable artifact that turns that unknown into a precise pass/warning/fail state.

## Proposed files

In platform or frontend generated docs:

```text
docs/generated/operator-system-compatibility.json
docs/generated/operator-system-compatibility.md
```

If cross-repo generated docs are difficult, initially generate under:

```text
ontogony-frontend/docs/generated/operator-system-compatibility.json
```

## Summary shape

See `contracts/system-compatibility-summary.schema.json`.

Minimum:

```json
{
  "schemaVersion": "ontogony.systemCompatibility.summary.v1",
  "generatedAtUtc": "2026-05-24T00:00:00Z",
  "baseline": "SYSTEM-ALPHA-006",
  "overallStatus": "warning",
  "services": [
    {
      "service": "conexus",
      "baseUrl": "http://localhost:5082",
      "connectivity": "live",
      "readiness": "not_ready",
      "healthContract": "valid",
      "version": "0.1.0-alpha.local",
      "warnings": ["OpenAI credentials optional provider not configured"],
      "failures": []
    }
  ],
  "compatibilityChecks": [
    {
      "id": "service.version.metadata",
      "status": "passed",
      "expected": "Every service exposes health.v1 version field",
      "actual": "Conexus/Kanon/Allagma expose version"
    }
  ]
}
```

## Status policy

Overall status rules:

```text
failed:
  any required service offline
  any required readiness check failed
  health contract invalid for required service
  compatibility manifest mismatch

warning:
  optional provider warning
  generated artifact stale
  live_with_fallback used for non-critical fields
  missing optional version metadata

passed:
  required services live
  required readiness ready
  health/ready contracts valid
  manifest satisfied
  no critical fallback/fixture data in required cards

unknown:
  artifact missing
  manifest missing
  insufficient data to classify
```

`fixture_only` must not contribute to `passed`.
