# Skill Version Applied Evidence Contract V0

## Purpose

`SkillVersionAppliedEvidence` records that a sandbox consumer **applied** a specific skill version (or explicit fallback) for one unit of work (e.g. one chat response). Supports reconstructability and rollback proof without production telemetry scope.

**Prerequisite:** [SKILL_RELEASE_GOVERNANCE.md](../protocols/SKILL_RELEASE_GOVERNANCE.md), [SKILL_CONSUMER_EXECUTION_CONTEXT_V0.md](./SKILL_CONSUMER_EXECUTION_CONTEXT_V0.md).

## Required fields

```json
{
  "eventId": "skillapplied_evt_001",
  "consumerId": "anti-sycophancy-chat-demo",
  "targetEnvironment": "sandbox",
  "skillArtifactId": "anti-sycophancy-response-style",
  "skillVersionId": "skillver_anti_syc_v2",
  "traceId": "00-abc-def-01",
  "correlationId": "corr_chat_session_001",
  "createdAtUtc": "2026-05-27T14:01:00Z"
}
```

## Optional lineage (when resolved)

```json
{
  "bindingId": "skillbind_release_sandbox_001",
  "sourcePromotionRequestId": "skillprom_demo_sandbox_001",
  "sourceOptimizationRunId": "skillopt_run_demo_001"
}
```

## Fallback evidence

When resolution was unresolved, include `unresolvedReason` and omit `bindingId`:

```json
{
  "eventId": "skillapplied_evt_002",
  "consumerId": "anti-sycophancy-chat-demo",
  "targetEnvironment": "sandbox",
  "skillArtifactId": "anti-sycophancy-response-style",
  "skillVersionId": "base",
  "unresolvedReason": "binding_paused",
  "traceId": "00-abc-def-02",
  "correlationId": "corr_chat_session_001",
  "createdAtUtc": "2026-05-27T14:02:00Z"
}
```

## Optional gateway refs (when Conexus is used)

```json
{
  "conexusModelCallId": "cmc_001",
  "routeDecisionId": "route_001"
}
```

## Evidence reference types (activation slice)

```text
skillConsumerBindingResolution
skillReleaseDeploymentBinding
skillPromotionRequest
skillOptimizationRun
skillVersionApplied
kanonDecision
conexusModelCall
```

## Related

- [SANDBOX_CONSUMER_ACTIVATION_EVENT_V0.md](./SANDBOX_CONSUMER_ACTIVATION_EVENT_V0.md)
- [SKILL_RELEASE_EVIDENCE_EXPORT_V0.md](./SKILL_RELEASE_EVIDENCE_EXPORT_V0.md)
