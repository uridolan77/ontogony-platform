# Capability Contract

The frontend should not guess what the backend can do.

## Required capability groups

### Allagma run operations

- startRun
- resume
- retry
- cancel
- replayTrigger
- exportAuditBundle
- listRunEvents
- sandboxEvidence

Each should expose `supported`, `status`, `reason`, `contractVersion`, `requiresAuth`, and `requiresHumanGate`.

### Sandbox execution capability

```json
{
  "realExternalExecution": {
    "enabled": false,
    "status": "blocked",
    "reason": "Real external execution remains blocked"
  },
  "localSandboxExecution": {
    "enabled": true,
    "status": "local_only",
    "productionAllowed": false,
    "executor": "LocalSandboxFileMarkerExecutor.v1"
  }
}
```

## Frontend rule

Every limitation banner must map to explicit backend unsupported capability, missing backend route, degraded backend dependency, unknown backend version, or frontend not yet adapted.
