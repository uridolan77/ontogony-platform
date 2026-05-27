# Sandbox Consumer Activation Event Contract V0

## Purpose

`SandboxConsumerActivationEvent` is a protocol-neutral envelope for sandbox consumer activation milestones. Use for fixture golden paths, local JSONL evidence, and future event spine integration. Payload bodies conform to the v0 schemas in `docs/schemas/sandbox-consumer-activation/`.

**Prerequisite:** [SKILL_RELEASE_GOVERNANCE.md](../protocols/SKILL_RELEASE_GOVERNANCE.md).

## Envelope

```json
{
  "eventId": "sandboxact_evt_001",
  "eventType": "skill_version_applied",
  "schemaVersion": "sandbox-consumer-activation-event.v0",
  "occurredAtUtc": "2026-05-27T14:01:00Z",
  "consumerId": "anti-sycophancy-chat-demo",
  "targetEnvironment": "sandbox",
  "traceId": "00-abc-def-01",
  "correlationId": "corr_chat_session_001",
  "payload": {
    "skillVersionApplied": {
      "eventId": "skillapplied_evt_001",
      "consumerId": "anti-sycophancy-chat-demo",
      "targetEnvironment": "sandbox",
      "skillArtifactId": "anti-sycophancy-response-style",
      "skillVersionId": "skillver_anti_syc_v2",
      "bindingId": "skillbind_release_sandbox_001",
      "sourcePromotionRequestId": "skillprom_demo_sandbox_001",
      "sourceOptimizationRunId": "skillopt_run_demo_001",
      "traceId": "00-abc-def-01",
      "correlationId": "corr_chat_session_001",
      "createdAtUtc": "2026-05-27T14:01:00Z"
    }
  }
}
```

## `eventType` enum (v0)

```text
binding_resolution_requested
binding_resolved
binding_unresolved
execution_context_captured
skill_version_applied
```

## Rules

- `targetEnvironment` on the envelope must be `sandbox`, `local-demo`, or `fixture-only`.
- Production, live, and default-runtime activation events are out of scope.
- Consumers emit events; Allagma remains binding authority for `binding_*` types.

## Related

- [SKILL_VERSION_APPLIED_EVIDENCE_V0.md](./SKILL_VERSION_APPLIED_EVIDENCE_V0.md)
- [SANDBOX_CONSUMER_ACTIVATION.md](../protocols/SANDBOX_CONSUMER_ACTIVATION.md)
