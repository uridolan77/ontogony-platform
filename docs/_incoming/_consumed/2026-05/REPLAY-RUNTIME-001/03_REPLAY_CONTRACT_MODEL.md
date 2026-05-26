# 03 — Replay contract model

## Shared enum: `ReplayMode`

```json
[
  "exact_replay",
  "deterministic_simulation",
  "dry_run",
  "reconstructed",
  "evidence_only",
  "unavailable"
]
```

## Shared enum: `ReplayTargetKind`

```json
[
  "allagma.run",
  "allagma.audit_bundle",
  "allagma.interaction_stream",
  "kanon.decision",
  "kanon.semantic_plan",
  "kanon.provenance",
  "conexus.model_call",
  "conexus.route_decision",
  "conexus.provider_attempt",
  "platform.trace",
  "platform.correlation",
  "evidence_spine_bundle"
]
```

## Core DTOs

### `ReplayTarget`

```json
{
  "kind": "allagma.run",
  "identifier": "run-demo-001",
  "displayIdentifier": "allagma.run:run-demo-001",
  "ownerService": "allagma",
  "root": true
}
```

### `ReplayEligibility`

```json
{
  "target": {},
  "eligibleModes": ["evidence_only", "reconstructed", "deterministic_simulation"],
  "recommendedMode": "evidence_only",
  "unavailableModes": [
    {
      "mode": "exact_replay",
      "reasonCode": "external_provider_not_replayable",
      "message": "The original model call used a real external provider and no deterministic provider snapshot is available."
    }
  ],
  "requiredSourceData": [],
  "availableSourceData": [],
  "missingSourceData": [],
  "safetyPosture": {},
  "confidence": "medium"
}
```

### `ReplayRequest`

```json
{
  "replayId": "replay_...",
  "rootIdentifier": "run-demo-001",
  "rootKind": "allagma.run",
  "requestedMode": "evidence_only",
  "requestedBy": {
    "actorId": "local-operator",
    "roles": ["Admin"]
  },
  "providerExecutionPolicy": "forbid_real_providers",
  "toolExecutionPolicy": "forbid_real_tools",
  "redactionPolicy": "operator_default",
  "createdAt": "2026-05-24T00:00:00Z"
}
```

### `ReplayResult`

```json
{
  "replayId": "replay_...",
  "rootIdentifier": "run-demo-001",
  "target": {},
  "mode": "evidence_only",
  "status": "completed",
  "verdict": "inconclusive",
  "serviceAttempts": [],
  "originalEvidenceRefs": [],
  "replayEvidenceRefs": [],
  "deltaRef": "replay-delta.json",
  "bundleRef": "replay-evidence-bundle.json",
  "safetyPosture": {},
  "startedAt": "...",
  "completedAt": "..."
}
```

### `ReplayServiceAttempt`

```json
{
  "service": "kanon",
  "operation": "decision_replay_bundle_export",
  "target": "kanon.decision:kanon-decision-demo-001",
  "status": "succeeded",
  "mode": "evidence_only",
  "route": "GET /ontology/v0/provenance/decisions/{decisionId}/replay-bundle",
  "durationMs": 42,
  "evidenceRefs": [],
  "skippedReason": null
}
```

### `ReplayDelta`

```json
{
  "replayId": "replay_...",
  "originalRootIdentifier": "run-demo-001",
  "mode": "deterministic_simulation",
  "verdict": "matched_with_expected_drift",
  "comparisons": [
    {
      "field": "conexus.routeDecision.provider",
      "status": "matched",
      "original": "fake",
      "replay": "fake"
    }
  ],
  "uncompared": [],
  "notes": []
}
```

## Target matrix

| Target | Identifier | Owner | Required source data | Allowed modes | Output artifact | Evidence Spine link | Safety constraints |
|---|---|---|---|---|---|---|---|
| `allagma.run` | `runId` | Allagma | run record, event stream, operations, audit bundle, Kanon decision IDs, Conexus model call IDs | `evidence_only`, `reconstructed`, `dry_run`, `deterministic_simulation` when all dependencies are fake/pinned | replay result bundle | node `replay.run_result` linked to run | no real tools/providers unless explicitly allowed and gated |
| `allagma.audit_bundle` | `runId` | Allagma | audit bundle | `evidence_only`, `reconstructed` | audit replay view | edge to original audit bundle | read-only |
| `allagma.interaction_stream` | `runId` | Allagma | interaction events JSONL/SSE source | `reconstructed`, `evidence_only` | replay timeline | edge to Agent Interaction | read-only |
| `kanon.decision` | `decisionId` | Kanon | decision record, provenance, replay bundle | `evidence_only`, `deterministic_simulation` when semantic snapshots exist | Kanon replay result | edge to Kanon decision/provenance | no model/provider authority |
| `kanon.semantic_plan` | `planId`/decision relation | Kanon | ontology version, domain pack, source bindings, input payload | `deterministic_simulation`, `evidence_only`, `unavailable` | semantic plan replay result | edge to semantic plan | pinned ontology required |
| `kanon.provenance` | `provenanceId` or decision/entity | Kanon | provenance envelope | `evidence_only`, `reconstructed` | provenance export | edge to provenance | read-only |
| `conexus.model_call` | `modelCallId` | Conexus | model call record, request fingerprint, route decision, provider attempts, output fingerprint | `evidence_only`, `reconstructed`, `dry_run`, `deterministic_simulation` only for fake/local provider | model-call replay result | edge to model-call evidence bundle | no real provider by default |
| `conexus.route_decision` | `routeDecisionId` | Conexus | route decision record, routing config snapshot, model alias | `deterministic_simulation`, `dry_run`, `evidence_only` | route-decision replay result | edge to route decision | no provider execution needed |
| `conexus.provider_attempt` | `providerAttemptId` | Conexus | provider attempt record, provider kind, error/output fingerprint | `evidence_only`, `reconstructed`, fake-only `deterministic_simulation` | provider-attempt replay result | edge to provider attempt | real provider attempts blocked |
| `platform.trace` | `traceId` | Platform contract/frontend resolver | Evidence Spine graph | `reconstructed`, `evidence_only` | cross-service replay bundle | root trace node | read-only unless subtargets selected |
| `platform.correlation` | `correlationId` | Platform contract/frontend resolver | Evidence Spine graph | `reconstructed`, `evidence_only` | cross-service replay bundle | root correlation node | read-only unless subtargets selected |
| `evidence_spine_bundle` | bundle id/path/hash | Platform contract/frontend resolver | evidence bundle | `evidence_only`, `reconstructed` | replay bundle summary | bundle node | read-only |

## Route contract principle

Routes may be service-local, but DTO semantics must be shared. A replay result emitted by Kanon or Conexus must be embeddable in an Allagma cross-service replay bundle without custom lossy translation.
