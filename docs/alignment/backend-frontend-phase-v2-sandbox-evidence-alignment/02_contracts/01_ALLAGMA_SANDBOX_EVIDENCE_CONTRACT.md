# Allagma Sandbox Evidence Contract

The frontend must consume Allagma's audit bundle sandbox evidence as a typed, redacted evidence section.

## Backend source concept

`AgentRunAuditBundleContract` includes optional `SandboxEvidence?: AgentRunAuditSandboxEvidenceContract`.

## UI meaning

`SandboxEvidence` proves what happened around a governed local sandbox side effect.

It should answer:

- Was there a dry-run?
- Was there an execute?
- Was execute replay-safe?
- Which side-effect ledger rows exist?
- What effect fingerprint was approved/executed?
- Which executor produced the effect?
- Which relative marker path was produced?
- Was content withheld/redacted?

## Required frontend behavior

If `SandboxEvidence` is absent, show "No sandbox evidence for this run", do not show an error, and do not infer execution.

If present, show dry-run side-effect ID, execute side-effect ID, `HasReplaySafeExecute`, `MarkerRelativePath`, `EffectFingerprint`, `ExecutorRef`, and ledger rows.

## Ledger row display fields

Show phase, status, sideEffectId, effectFingerprint, kanonDecisionId, registryVersion, executorRef, externalRefs, traceId, recordedAtUtc, updatedAtUtc, failureClass, and failureMessage.

Hide raw marker content, raw tool args, raw user prompt, secrets, provider headers, API keys, and filesystem absolute paths.

## External refs

The backend accepts both of these key shapes for marker relative path extraction:

```text
sandbox.file:marker_relative_path
sandbox_file
```

The frontend adapter should also tolerate both if it reads from raw ledger rows.
