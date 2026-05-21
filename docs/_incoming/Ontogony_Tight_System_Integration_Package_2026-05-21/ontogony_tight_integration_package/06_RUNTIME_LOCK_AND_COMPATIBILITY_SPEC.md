# 06 — Runtime lock and compatibility spec

## Lock principles

1. `expectedRefs` express branch intent, usually `main`.
2. `lockedCommits` express reproducible runtime pins.
3. Package versions must match release-mode package build inputs.
4. Evidence paths are part of the lock, not optional decoration.
5. Moving-main deltas after a lock cut must be classified before the next lock promotion.

## Required lock shape

```json
{
  "system": "ontogony-alpha-governed-runtime",
  "baseline": "SYSTEM-TIGHT-001",
  "expectedRefs": {
    "ontogony-platform": "main",
    "conexus-dotnet": "main",
    "kanon-dotnet": "main",
    "allagma-dotnet": "main"
  },
  "lockedCommits": {
    "ontogony-platform": "<sha>",
    "conexus-dotnet": "<sha>",
    "kanon-dotnet": "<sha>",
    "allagma-dotnet": "<sha>"
  },
  "packageVersions": {
    "Ontogony": "0.3.0-alpha.1",
    "Kanon.Client": "0.1.0-alpha.0",
    "Kanon.Contracts": "0.1.0-alpha.0",
    "Conexus.Client": "0.1.0-alpha.1",
    "Conexus.Contracts": "0.1.0-alpha.1"
  },
  "requiredScenarios": [
    "completed_run",
    "idempotent_run_start",
    "human_gate_waiting",
    "human_gate_approved",
    "human_gate_denied",
    "kanon_conexus_assistance",
    "conexus_fallback",
    "streaming_lifecycle",
    "operator_audit_journey",
    "restart_survival"
  ],
  "evidence": {
    "systemCohesionSummary": "artifacts/system-e2e/<timestamp>/summary.json",
    "restartSummary": "artifacts/restart-e2e/<timestamp>/summary.json",
    "operatorAuditSummary": "artifacts/operator-audit/<timestamp>/summary.json",
    "observabilitySummary": "artifacts/observability/<timestamp>/observability-summary.json"
  }
}
```

## Compatibility rules

### Kanon

- All Allagma `kanonCalls` must be subset of Kanon route inventory.
- All typed-client Kanon calls must be classified `Client` in Kanon client coverage.
- Kanon manifest schema changes require Allagma conformance updates.

### Conexus

- All Allagma Conexus calls must compile against `Conexus.Client` package mode.
- Streaming contract changes require package-mode and streaming smoke.
- Model aliases used by Allagma model purposes must exist in Conexus seed/bootstrap for local stack.

### Platform

- Shared contract package version must be pinned in runtime lock.
- Cross-service error envelope or evidence-spine schema changes require downstream adapter tests.

### Frontend/operator

- Route catalogs must pass parity against backend route inventories for any locked backend update.
- Operator evidence-spine tests must pass for all required identifier types.

## Post-lock delta classification

Every post-lock commit touching a repo in the runtime must be classified before next cut:

| Class | Meaning |
|---|---|
| `docs_only` | no runtime/API effect |
| `test_only` | no runtime/API effect but gate behavior may change |
| `api_additive_safe` | new optional API, no breaking change |
| `api_breaking` | requires coordinated migration |
| `runtime_behavior_change` | must be validated in smoke |
| `package_surface_change` | requires package-mode validation |
| `operator_surface_change` | requires frontend/operator validation |
| `security_surface_change` | requires security review |
