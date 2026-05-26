# 07 — Evidence Spine and export plan

## Evidence Spine integration

Replay must extend Evidence Spine, not fork it.

### New node kinds

- `replay.request`
- `replay.result`
- `replay.delta`
- `replay.evidence_bundle`
- `replay.service_attempt`
- `replay.safety_posture`

### New edge kinds

- `replays`
- `uses_original_evidence`
- `produces_replay_evidence`
- `compares_against`
- `attempted_by_service`
- `blocked_by_policy`
- `skipped_due_to_missing_evidence`

### Identifier examples

```text
replay.request:replay_01HX...
replay.result:replay_01HX...
replay.delta:replay_01HX...
replay.evidence_bundle:replay_01HX...
```

## Export artifacts

Every replay export directory should contain:

```text
replay-request.json
replay-result.json
replay-evidence-bundle.json
replay-delta.json
replay-summary.json
replay-summary.md
```

Optional:

```text
service-attempts/
  allagma.json
  kanon.json
  conexus.json
source-evidence/
  evidence-graph-snapshot.json
  original-run-audit.json
  original-model-call-evidence.json
```

## `replay-request.json`

Required fields:

- `replayId`
- `rootIdentifier`
- `rootKind`
- `requestedMode`
- `requestedBy`
- `requestedAt`
- `providerExecutionPolicy`
- `toolExecutionPolicy`
- `redactionPolicy`
- `clientIdempotencyKeyHash`
- `sourceEvidenceGraphFingerprint`

## `replay-result.json`

Required fields:

- `replayId`
- `status`
- `mode`
- `verdict`
- `target`
- `startedAt`
- `completedAt`
- `serviceAttempts`
- `originalEvidenceRefs`
- `replayEvidenceRefs`
- `deltaRef`
- `bundleRef`
- `safetyPosture`
- `unavailableReasons`
- `buildMetadata`
- `runtimeMetadata`

## `replay-evidence-bundle.json`

Required fields:

- `schemaVersion`
- `bundleId`
- `originalRootIdentifier`
- `replayTarget`
- `replayMode`
- `replayVerdict`
- `serviceAttempts`
- `sourceEvidenceReferences`
- `resultEvidenceReferences`
- `unavailableReasons`
- `skippedReasons`
- `safetyPosture`
- `redactionMetadata`
- `buildMetadata`
- `runtimeMetadata`
- `evidenceSpineLinks`

## `replay-delta.json`

Required fields:

- `replayId`
- `mode`
- `verdict`
- `comparisons`
- `uncompared`
- `expectedDrift`
- `unexpectedDrift`
- `notes`

## `replay-summary.md/json`

Summary must be operator-readable:

- What was replayed.
- What mode was used.
- What was not replayed.
- Whether real providers/tools were blocked.
- Key matched/diverged/skipped results.
- Evidence Spine links.
- Export path/hash.

## Redaction metadata

Every bundle must include:

- redaction policy name;
- fields redacted;
- payloads omitted;
- whether raw prompts/messages are included;
- whether model output text is included or only hashed;
- whether user/tool secrets were excluded.

## Build/runtime metadata

Include:

- repo refs when known;
- service version/build hash when available;
- environment name;
- runtime-lock compatibility ref when applicable;
- OpenAPI/protocol version.
