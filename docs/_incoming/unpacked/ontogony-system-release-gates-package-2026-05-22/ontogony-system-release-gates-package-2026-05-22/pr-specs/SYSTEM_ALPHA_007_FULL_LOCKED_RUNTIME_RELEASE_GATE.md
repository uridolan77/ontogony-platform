# SYSTEM-ALPHA-007 — Full locked-runtime release gate

## Goal

Provide one release command/workflow that validates the committed locked runtime across all four .NET repos and emits one release evidence bundle.

## Owner

Primary: `ontogony-platform`  
Runtime lock source: `allagma-dotnet/docs/system/ontogony-runtime.lock.json`

## Why

The system now has repo-local gates, runtime lock, package-mode proof, and system smoke scripts. They are not yet unified into a reproducible release-candidate gate.

## Deliverables

| Deliverable | Target path |
|---|---|
| Release evidence contract | `ontogony-platform/docs/releases/runtime-release-evidence/RUNTIME_RELEASE_EVIDENCE_CONTRACT.md` |
| Evidence schema | `ontogony-platform/schemas/runtime-release-evidence-bundle.schema.json` |
| Release gate script | `ontogony-platform/scripts/release/run-locked-runtime-release-gate.ps1` |
| Evidence validator | `ontogony-platform/scripts/release/validate-runtime-release-evidence.ps1` |
| Workflow | `ontogony-platform/.github/workflows/runtime-release-gate.yml` |
| Runbook | `ontogony-platform/docs/operators/RUNBOOK_FULL_LOCKED_RUNTIME_RELEASE_GATE.md` |
| First evidence closeout | `ontogony-platform/docs/evidence/SYSTEM_ALPHA_007_RELEASE_GATE_EVIDENCE.md` |

## Command shape

```powershell
./scripts/release/run-locked-runtime-release-gate.ps1 `
  -ReleaseId "SYSTEM-ALPHA-007" `
  -Mode Locked `
  -WorkspaceRoot "C:\dev\ontogony-release" `
  -RuntimeLockPath "allagma-dotnet/docs/system/ontogony-runtime.lock.json"
```

## Required gates

1. Parse runtime lock.
2. Checkout all repos at `lockedCommits`.
3. Validate repo commit SHA equals lock.
4. Run Platform build/test/pack.
5. Run Kanon build/test contract gates.
6. Run Conexus build/test default filter.
7. Run Allagma build/test/conformance.
8. Run package-mode release train.
9. Run system cohesion scheduled suite or consume a same-commit suite artifact.
10. Validate final evidence bundle.

## Acceptance criteria

- Fails if any pinned commit is missing.
- Fails if any repo is checked out at a different SHA than the lock.
- Fails if evidence bundle lacks package-mode summary.
- Fails if evidence bundle lacks system-cohesion summary.
- Fails if mode is not `Locked` and caller requests release verdict.
- Emits JSON and Markdown bundle.
- No secrets in evidence bundle.

## Boundary

This is release-gate automation only. It is not production readiness and does not enable real external tool execution.
