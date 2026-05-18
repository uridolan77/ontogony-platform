# Trace and Identifier Contract

The frontend must correlate evidence across services and within Allagma.

## Minimum identifiers

For Allagma sandbox evidence: runId, toolIntentId, sideEffectId, kanonDecisionId, registryVersion, effectFingerprint, executorRef, traceId, and correlationId if available.

For cross-service views: Allagma runId, Kanon decisionId, Kanon humanGateId if available, Conexus modelCallId if available, traceId, and replayBundleId if available.

## UI behavior

The UI should allow drill-down from:

```text
Run -> timeline event -> side-effect ledger row -> Kanon decision -> trace/correlation
```

If the linked backend route is missing, show: "Linked ID available; backend detail route not yet supported." Do not hide the ID.
