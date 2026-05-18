# Frontend UI Information Architecture

## Primary surface
Extend the existing Allagma run detail / audit / replay surface. Do not create a detached sandbox-only dashboard unless there is already a broader Allagma operations route.

## Recommended sections

1. Run summary: run ID, status, model purpose, planning decision ID, model call ID, trace/correlation IDs.
2. Sandbox evidence card: presence/absence state, dry-run side-effect ID, execute side-effect ID, replay-safe execute status, effect fingerprint, executor ref, marker relative path, raw content hidden note.
3. Side-effect ledger table: phase, status, sideEffectId, effectFingerprint, kanonDecisionId, registryVersion, executorRef, externalRefs, traceId, recordedAt, updatedAt, failure.
4. Event timeline: all five sandbox execute events.
5. Capability and limitation banner: real external execution BLOCKED, local sandbox execute LOCAL ONLY, production sandbox execute FORBIDDEN, retry/cancel/replay-trigger supported/unsupported/deferred.
6. Cross-service links: Kanon decision, human gate, Conexus model call, trace/correlation.
