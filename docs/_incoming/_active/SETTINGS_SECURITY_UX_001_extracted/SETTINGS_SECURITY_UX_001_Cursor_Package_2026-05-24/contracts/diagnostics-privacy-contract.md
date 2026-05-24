# Diagnostics Privacy Contract

## Export metadata

```json
{
  "schemaVersion": "operator-diagnostics.v1",
  "generatedAtUtc": "2026-05-24T00:00:00Z",
  "privacy": {
    "containsClientDiagnostics": true,
    "containsRawSecrets": false,
    "redactionApplied": true,
    "clientFields": ["userAgent", "viewport", "timezone"],
    "secretHandling": "raw secrets omitted"
  }
}
```

## UI notice

```text
Diagnostics export contains local client diagnostics such as browser, viewport,
timezone, service URLs, health statuses, and redacted settings metadata.
It does not include raw API keys or raw secret values.
```

## Test requirement

Serialized diagnostics output must not contain mock raw secret strings used in tests.
