# CI-compatible five-service smoke

## Goal

Create a smoke harness that can later run in CI without relying on manual local API windows.

## Requirements

The CI smoke must support:

```powershell
./scripts/system/run-five-service-ci-smoke.ps1 -Mode Preflight
./scripts/system/run-five-service-ci-smoke.ps1 -Mode Fixture
./scripts/system/run-five-service-ci-smoke.ps1 -Mode Live
```

## Modes

### Preflight

- verify repo paths;
- verify scripts exist;
- verify ports/config expected;
- do not require services to be running;
- exit 0 if preconditions are documentable.

### Fixture

- start Aisthesis or require Aisthesis ready;
- ingest required-edge fixture;
- query reconstructability/lookup/bundle;
- require complete grade.

### Live

- require all services ready;
- trigger configured live workflow only if environment variables define how;
- otherwise return `NOT_RUN` with reason;
- never fake PASS.

## Summary output

Write:

```text
artifacts/five-service-ci-smoke/<timestamp>/summary.json
```

The summary must include status, services, traceId, producersObserved, requiredEdges, reconstructability, bundleFingerprintPresent, and redaction flags.

## CI truth

A CI smoke that cannot trigger a live workflow may pass preflight but must not claim live proof.
