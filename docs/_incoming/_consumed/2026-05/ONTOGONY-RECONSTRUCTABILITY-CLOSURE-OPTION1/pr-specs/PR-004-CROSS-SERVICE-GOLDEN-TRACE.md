# PR-004 — Cross-service golden trace

## Repo focus

Primary orchestrator:

```text
C:\dev\allagma-dotnet
```

Involved services:

```text
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
```

## Goal

Produce one cross-service evidence bundle showing a representative governed run and reconstructability classification across services.

## Scenario

Use fake/local provider. No external provider keys.

Minimum flow:

```text
1. Start Kanon, Conexus, Allagma.
2. Start Allagma run with model purpose routed to Conexus fake provider.
3. Kanon creates/evaluates planning/policy decision.
4. Conexus records route/model call evidence.
5. Allagma completes or enters/resolves a human gate depending on fixture mode.
6. Fetch Allagma decision-events.
7. Fetch/project Conexus decision-events.
8. Send combined events to Kanon classify-batch.
9. Write golden trace bundle.
```

## Required artifacts

```text
artifacts/reconstructability/golden-trace/<timestamp>/golden-trace.json
artifacts/reconstructability/golden-trace/<timestamp>/summary.json
docs/evidence/CROSS_SERVICE_RECONSTRUCTABILITY_GOLDEN_TRACE.md
```

## Scripts

Add:

```text
scripts/system/run-reconstructability-golden-trace.ps1
scripts/system/validate-reconstructability-golden-trace.ps1
```

## Summary must include

```json
{
  "schema": "ontogony-cross-service-reconstructability-summary-v1",
  "traceId": "...",
  "correlationId": "...",
  "runId": "...",
  "allagmaEvents": 0,
  "conexusEvents": 0,
  "kanonClassificationResults": 0,
  "highCriticalFailures": 0,
  "status": "PASS"
}
```

## Acceptance criteria

```text
Golden trace runs in fake-provider local mode.
highCriticalFailures == 0.
Artifacts are redaction-safe.
Docs explain that this is alpha/local evidence, not production certification.
```
