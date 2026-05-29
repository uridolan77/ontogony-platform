# Contract — Aisthesis required-edge matrix v2

## Purpose

Define operational reconstructability requirements beyond the v1 minimal 10-edge spine.

## Status values

| Status | Meaning |
|---|---|
| `present` | Matching edge exists and native IDs resolve |
| `missing` | Edge is required for this profile but absent |
| `not_applicable` | Edge is not required for this profile/trace; reason required |
| `ambiguous` | Candidate evidence exists but relation/native ID is insufficient |

## Profiles

| Profile | When used |
|---|---|
| `core-run-profile` | Allagma run with Kanon planning and Conexus model call |
| `metabole-first-profile` | Metabole pipeline/profile/mapping trace with cross-service continuation |
| `human-gated-run-profile` | Trace includes human-gate open/resolve lifecycle |
| `tool-side-effect-profile` | Trace includes tool execution or side effect |
| `model-fallback-profile` | Conexus fallback/error path exercised |
| `streaming-model-profile` | Conexus streaming path exercised |
| `retention-erasure-profile` | Trace includes erasure/tombstone/redaction action |

## V1 edges retained

```text
allagma.run_to_kanon.semantic_plan
allagma.run_to_kanon.decision
allagma.run_to_conexus.model_call
kanon.decision_to_semantic_plan
kanon.decision_to_policy
conexus.model_call_to_route_decision
conexus.model_call_to_provider_attempt
metabole.pipeline_to_profile
metabole.pipeline_to_mapping_candidate
metabole.mapping_candidate_to_artifact
```

## V2 additional edges

| Required edge ID | From producer | From native ID | To producer | To native ID | Relation | Required when |
|---|---|---|---|---|---|---|
| `allagma.run_to_human_gate` | allagma | `runId` | kanon | `humanGateId` | `blocked_by` | run opens human gate |
| `allagma.run_to_tool_intent` | allagma | `runId` | allagma | `toolIntentId` | `produced` | run plans tool intent |
| `allagma.tool_intent_to_tool_execution` | allagma | `toolIntentId` | allagma | `toolExecutionId` | `authorized` or `used` | tool intent executed or simulated |
| `allagma.tool_execution_to_side_effect` | allagma | `toolExecutionId` | allagma | `sideEffectId` | `produced` | real side effect occurred |
| `allagma.side_effect_to_artifact` | allagma | `sideEffectId` | allagma | `artifactId` | `materialized_as` | side effect produces artifact |
| `kanon.decision_to_canonical_fact` | kanon | `decisionId` | kanon | `canonicalFactId` | `used` or `derived_from` | decision depends on canonical fact |
| `kanon.decision_to_source_binding` | kanon | `decisionId` | kanon | `sourceBindingId` | `evaluated` | decision depends on source binding |
| `kanon.decision_to_replay_bundle` | kanon | `decisionId` | kanon | `replayBundleId` | `reconstructable_by` | replay bundle created |
| `kanon.human_gate_opened_to_resolved` | kanon | `humanGateId` | kanon | `humanGateResolutionId` | `resolved_by` | human gate resolved |
| `conexus.model_call_to_provider_fallback` | conexus | `modelCallId` | conexus | `fallbackId` | `fell_back_to` | fallback occurred |
| `conexus.model_call_to_provider_error` | conexus | `modelCallId` | conexus | `providerErrorId` | `failed_with` | provider error occurred |
| `conexus.model_call_to_usage_cost` | conexus | `modelCallId` | conexus | `usageCostId` | `measured_by` | usage/cost recorded |
| `conexus.streaming_session_to_completion_summary` | conexus | `streamingSessionId` | conexus | `modelCallId` | `summarized_as` | streaming path used |
| `metabole.pipeline_to_schema_extract` | metabole | `pipelineRunId` | metabole | `schemaExtractId` | `produced` | schema extraction ran |
| `metabole.profile_to_mapping_candidate` | metabole | `dataProfileId` | metabole | `mappingCandidateId` | `produced` | mapping candidate from profile |
| `metabole.mapping_candidate_to_review` | metabole | `mappingCandidateId` | metabole | `mappingReviewId` | `reviewed_by` | mapping review occurred |
| `metabole.mapping_review_to_artifact` | metabole | `mappingReviewId` | metabole | `artifactId` | `materialized_as` | review produces accepted/rejected artifact |
| `metabole.transformation_plan_to_artifact` | metabole | `transformationPlanId` | metabole | `artifactId` | `produced` | transformation plan materialized |
| `metabole.mapping_candidate_to_kanon_source_binding` | metabole | `mappingCandidateId` | kanon | `sourceBindingId` | `proposed_as` | mapping candidate submitted to Kanon |
| `aisthesis.erasure_to_tombstone` | aisthesis | `erasureRequestId` | aisthesis | `tombstoneId` | `produced` | erasure/tombstone action executed |

## Diagnostic requirement

Every `missing` or `ambiguous` edge must emit:

```json
{
  "requiredEdgeId": "...",
  "status": "missing",
  "severity": "error|warning",
  "producerOwner": "allagma|kanon|conexus|metabole|aisthesis",
  "message": "...",
  "suggestedProducerFix": "...",
  "profile": "..."
}
```
