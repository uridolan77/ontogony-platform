# 08 — Test Plan

## Backend tests

### Health contract tests

For each service:

```text
GET /health
assert HTTP 200
assert JSON
assert schemaVersion == health.v1
assert service matches expected
assert version is present
assert checkedAtUtc is present
```

### Readiness contract tests

For each service:

```text
GET /ready
assert HTTP 200 or appropriate readiness status with JSON body
assert schemaVersion == ready.v1
assert status in allowed enum
assert checks array exists
assert every check has id, label, status, severity
```

### Conexus local fake-provider readiness

Scenario:

```text
fake provider registered
risk-summary-v0 routes to fake
OpenAI provider missing credentials
```

Expected:

```text
connectivity: live
readiness: ready or degraded, depending exact local policy
OpenAI credential check: warning optional
fake provider check: ready
routing alias check: ready
```

If current strict readiness must remain not_ready, ensure the failing required check is explicit and justified.

## Frontend unit tests

Add tests for:

```text
parseHealthV1(valid)
parseHealthV1(missing version)
parseHealthV1(non-json)
parseReadyV1(valid)
parseReadyV1(failing required check)
aggregateServiceTruth()
aggregateSystemCompatibility()
fixtureOnlyCannotBeReleaseReady()
unknownCannotBePassed()
```

## Frontend integration tests

Use mocked service payloads:

1. All live/ready/valid → Home shows passed.
2. Conexus live/not_ready/valid → Home shows warning; no "all services healthy".
3. Missing version metadata → compatibility cannot verify.
4. Missing compatibility artifact → system compatibility unknown/warning.
5. Fixture-only release readiness → release posture not assessed/warning.

## Smoke tests

Use scripts:

```text
scripts/smoke/system_truth_smoke.ps1
scripts/smoke/system_truth_smoke.sh
```

Expected output:

```text
service, healthStatus, healthContract, readiness, version, warnings
conexus, healthy, valid, not_ready, 0.1.0-alpha.local, [...]
kanon, healthy, valid, ready, 0.1.0-alpha.local, []
allagma, healthy, valid, ready, 0.1.0-alpha.local, [...]
overall: warning
```
