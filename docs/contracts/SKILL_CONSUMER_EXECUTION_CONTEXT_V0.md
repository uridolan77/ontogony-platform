# Skill Consumer Execution Context Contract V0

## Purpose

`SkillConsumerExecutionContext` captures what a sandbox consumer **believes it is running under** after binding resolution and before applying behavior (e.g. chat response generation). It is consumer-local context for debug panels and downstream evidence; it is not the Allagma binding record.

**Prerequisite:** [SKILL_CONSUMER_BINDING_RESOLUTION_V0.md](./SKILL_CONSUMER_BINDING_RESOLUTION_V0.md).

## Required fields

```json
{
  "consumerId": "anti-sycophancy-chat-demo",
  "targetEnvironment": "sandbox",
  "skillArtifactId": "anti-sycophancy-response-style",
  "resolutionKind": "resolved",
  "skillVersionId": "skillver_anti_syc_v2",
  "bindingId": "skillbind_release_sandbox_001",
  "sourcePromotionRequestId": "skillprom_demo_sandbox_001",
  "sourceOptimizationRunId": "skillopt_run_demo_001",
  "capturedAtUtc": "2026-05-27T14:00:00Z"
}
```

## `resolutionKind` enum

```text
resolved
unresolved_fallback
forbidden_target
```

## Unresolved fallback example

```json
{
  "consumerId": "anti-sycophancy-chat-demo",
  "targetEnvironment": "sandbox",
  "skillArtifactId": "anti-sycophancy-response-style",
  "resolutionKind": "unresolved_fallback",
  "skillVersionId": "base",
  "fallbackReason": "binding_paused",
  "capturedAtUtc": "2026-05-27T14:05:00Z"
}
```

When `resolutionKind` is `unresolved_fallback`, `bindingId` and source lineage fields are omitted.

## Rules

- Consumers must set `skillVersionId` from resolution or explicit fallback only.
- `forbidden_target` is used when the consumer attempted a forbidden `targetEnvironment` before calling Allagma (client-side guard in 001C).
- Do not label this context as production deployment or live behavior.

## Related

- [SKILL_VERSION_APPLIED_EVIDENCE_V0.md](./SKILL_VERSION_APPLIED_EVIDENCE_V0.md)
- [SANDBOX_CONSUMER_ACTIVATION.md](../protocols/SANDBOX_CONSUMER_ACTIVATION.md)
