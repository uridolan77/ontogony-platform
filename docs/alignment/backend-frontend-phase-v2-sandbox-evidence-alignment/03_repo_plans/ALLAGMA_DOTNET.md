# Repo Plan — allagma-dotnet

## Objective
Finish Phase 3 RC closeout and expose clean OpenAPI-backed contracts for frontend consumption.

## PRs

### ALLAGMA-FBA2-001 — Phase 3 RC validator and closeout
Scope: mark P3-SB-006 DONE when validator/tag is complete, keep P3-SB-005 evidence clean, generate current OpenAPI snapshot, prove tests and scripts, document remaining limitations.

Acceptance: `RealExecutionEnabled=true` still rejected; `SandboxExecutionEnabled=true` still forbidden in production; P3 entry guard passes; P3 restart/audit tests pass; OpenAPI contains audit bundle sandbox evidence shape.

### ALLAGMA-FBA2-002 — Capability metadata endpoint
Expose run operation support, sandbox evidence support, local sandbox execute state, real external execution blocked state, and contract version.

### ALLAGMA-FBA2-003 — Audit contract snapshot
Document and snapshot `AgentRunAuditBundleContract`, `AgentRunAuditSandboxEvidenceContract`, `AgentRunAuditSideEffectLedgerEntryContract`, and sandbox execute events.

### ALLAGMA-FBA2-004 — Evidence artifact route if needed
If frontend cannot easily get sandbox evidence from the existing audit route, add a stable read-only route: `GET /runs/{runId}/sandbox-evidence`. Prefer existing audit bundle route.
