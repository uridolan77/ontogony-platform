# Executive Summary

The backend now has a meaningful governed side-effect evidence loop in Allagma: dry-run ledger, human gate binding, minimal local sandbox execute, execute idempotency reservation, replay-safe skip, runtime sandbox execute events, audit bundle `SandboxEvidence`, and no raw marker content in audit. Real external execution remains blocked.

The frontend/backend system is not yet aligned to this new evidence surface. That is the immediate next phase.

## Strategic decision

Do a bounded frontend/backend alignment bridge **before** eval-basing.

Reason: eval-basing will depend on the same primitives the UI now needs to display: run evidence, event timeline, replay-safe state, tool side-effect rows, capability truth, trace correlation, and redacted operator-safe payloads.

If these are not visible and contract-tested in the frontend, eval-basing will become a backend-only artifact layer that operators cannot inspect.

## Target outcome

At the end of this phase, an operator using `ontogony-frontend` should be able to open an Allagma run audit/replay view and see:

- whether sandbox execution was enabled
- whether real external execution was blocked
- dry-run side-effect row
- execute side-effect row
- marker relative path
- effect fingerprint
- executor ref
- replay-safe execute status
- `SandboxExecuteStarted`
- `SandboxExecuteCompleted`
- `SandboxExecuteBlocked`
- `SandboxExecuteFailed`
- `SandboxExecuteReplaySkipped`

No raw marker payload, secrets, tool args, provider headers, or unsafe content should be displayed by default.
