# Reconstructability report artifact contract

**Schema id:** `ontogony-reconstructability-report-artifact-v1`  
**Owner:** Kanon (`kanon-dotnet`) — HTTP under `/ontology/v0/reconstructability/report-artifacts*`  
**Related:** `DecisionEventContract`, `ReconstructabilityReportContract` (existing classify output)

## Purpose

A durable, hash-addressable record of a single classification run: exact input snapshot, classifier version, timestamps, governance summary, and full report payload for audit and UI replay.

## Artifact document

```json
{
  "schema": "ontogony-reconstructability-report-artifact-v1",
  "artifactId": "recon_art_<stable-hex>",
  "decisionEventId": "<uuid>",
  "decisionKind": "run_operation",
  "serviceOfOrigin": "allagma",
  "sourceRef": "allagma:run:<runId>",
  "decisionEventHash": "<sha256-hex-lowercase>",
  "classifierVersion": "kanon-decision-reconstructability-classifier-v1",
  "classifiedAtUtc": "2026-05-26T12:00:00.000Z",
  "traceId": "<optional>",
  "runId": "<optional>",
  "correlationId": "<optional>",
  "relatedIds": { },
  "inputSchemaVersion": "<decision-event schema version>",
  "reportSchemaVersion": "<reconstructability-report schema version>",
  "decisionEventSnapshot": { },
  "report": { },
  "ontogonyGovernanceStatus": "PASS",
  "strictCompletenessPct": 100.0,
  "safeReasoningNotice": "Hidden chain-of-thought is not stored or exposed.",
  "createdAtUtc": "2026-05-26T12:00:00.000Z",
  "updatedAtUtc": "2026-05-26T12:00:00.000Z"
}
```

### Field rules

| Field | Required | Notes |
| --- | --- | --- |
| `schema` | yes | Constant `ontogony-reconstructability-report-artifact-v1` |
| `artifactId` | yes | Deterministic from `schema` + `decisionEventHash` + `classifierVersion`; **exclude** `classifiedAtUtc` |
| `decisionEventId` | yes | From input event |
| `decisionKind` | yes | From input event |
| `serviceOfOrigin` | yes | e.g. `allagma` |
| `sourceRef` | yes | Stable source pointer (run, trace, export batch, etc.) |
| `decisionEventHash` | yes | SHA-256 of canonical serialized `DecisionEventContract` |
| `classifierVersion` | yes | Kanon classifier release id |
| `classifiedAtUtc` | yes | When classification ran (wall clock, UTC) |
| `traceId` | no | From request metadata or event when available |
| `runId` | no | Allagma run id when applicable |
| `correlationId` | no | Platform correlation when available |
| `relatedIds` | no | Map of auxiliary ids (tool call id, gate id, etc.) |
| `inputSchemaVersion` | yes | Decision event contract version |
| `reportSchemaVersion` | yes | Report contract version |
| `decisionEventSnapshot` | yes | Full `DecisionEventContract` or JSON-safe canonical snapshot |
| `report` | yes | Full `ReconstructabilityReportContract` from classifier |
| `ontogonyGovernanceStatus` | yes | `PASS` \| `WARN` \| `FAIL` (denormalized from report) |
| `strictCompletenessPct` | yes | Denormalized from report (`desStrictCompletenessPct` or repo name) |
| `safeReasoningNotice` | yes | Fixed safe notice; no chain-of-thought |
| `createdAtUtc` | yes | First persist time |
| `updatedAtUtc` | yes | Last upsert time |

## Hashing

**`decisionEventHash`:**

1. Serialize `DecisionEventContract` with deterministic JSON (property order, stable null handling) per repo canonical JSON rules.
2. SHA-256 → lowercase hex string (64 chars).

**`artifactId`:**

```text
payload = schema + ":" + decisionEventHash + ":" + classifierVersion
artifactId = "recon_art_" + <first 32 or 48 hex chars of sha256(payload)>
```

Idempotent re-create with identical input and classifier version must return the same `artifactId` and preserve `createdAtUtc`.

## HTTP shapes (Kanon)

### Create single

`POST /ontology/v0/reconstructability/report-artifacts`

Request body (minimum):

```json
{
  "decisionEvent": { },
  "sourceRef": "allagma:run:<runId>",
  "runId": "<optional>",
  "traceId": "<optional>",
  "correlationId": "<optional>"
}
```

Response: `200` or `201` with `ReconstructabilityReportArtifactContract`.

### Create batch

`POST /ontology/v0/reconstructability/report-artifacts/batch`

```json
{
  "decisionEvents": [ { }, { } ],
  "sourceRef": "allagma:run:<runId>",
  "runId": "<optional>"
}
```

Response: artifacts in **same order** as `decisionEvents`.

### Get by id

`GET /ontology/v0/reconstructability/report-artifacts/{artifactId}`

### Get by decision event (optional)

`GET /ontology/v0/reconstructability/report-artifacts/by-decision-event/{decisionEventId}`

Returns latest artifact for that event id, or 404.

## Safety

**Forbidden in artifact storage and API responses:**

- Hidden chain-of-thought
- Raw model internal reasoning traces

**Allowed in `report` and snapshots:**

- `safeReasoningPolicy` and other existing safe surrogates on `ReconstructabilityReportContract`
- Policy summaries, route explanations, validation outputs, human notes, evidence fragment references, classifier diagnostics

## Platform promotion (post-implementation)

When Kanon contracts stabilize, promote a durable copy to `docs/contracts/` and link from `docs/INDEX.md`. Until then, this intake doc is the source of truth for DEC-RECON-007.
