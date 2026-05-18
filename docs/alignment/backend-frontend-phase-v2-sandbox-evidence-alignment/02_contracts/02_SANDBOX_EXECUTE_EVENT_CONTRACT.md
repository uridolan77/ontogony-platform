# Sandbox Execute Event Contract

The frontend should render these five Allagma event types in the run timeline:

```text
Allagma.SandboxExecuteStarted
Allagma.SandboxExecuteCompleted
Allagma.SandboxExecuteBlocked
Allagma.SandboxExecuteFailed
Allagma.SandboxExecuteReplaySkipped
```

## Timeline semantics

- `SandboxExecuteStarted`: execute reservation was created and dispatch is about to start.
- `SandboxExecuteCompleted`: local sandbox effect completed and ledger row was marked completed.
- `SandboxExecuteBlocked`: execute was blocked before side effect. This is often an expected policy outcome, not a system failure.
- `SandboxExecuteFailed`: executor attempted but failed; ledger row terminal status failed.
- `SandboxExecuteReplaySkipped`: completed execute row already exists and executor was not called again. This is a positive idempotency signal, not failure.

## UI treatment

Started = info. Completed = success. Blocked = warning/policy. Failed = error. ReplaySkipped = neutral/success.

Never display raw marker content from an event payload.
