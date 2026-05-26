# DEC-RECON-007 — implementation prompt

Use this document after the platform intake package exists. Implement in order: **Kanon → frontend → verification/closeout**.

## Context

**Closed slices:**

- DEC-RECON-004: `GET /allagma/v0/runs/{runId}/decision-events` → `POST /ontology/v0/reconstructability/classify-batch` → run audit panel.
- DEC-RECON-006: Evidence Spine appends `allagma.decisionEvent` nodes with PASS/WARN/FAIL overlay and opens `ReconstructabilityReportPanel`.

**Existing Kanon routes (must remain compatible):**

```text
GET  /ontology/v0/decision-events/{decisionEventId}/reconstructability
GET  /ontology/v0/reconstructability/by-trace/{traceId}
POST /ontology/v0/reconstructability/classify
POST /ontology/v0/reconstructability/classify-batch
```

## Kanon (`C:\dev\kanon-dotnet`)

### 1. Inspect existing code

- `ReconstructabilityEndpoints.cs`
- `DecisionReconstructabilityService.cs`
- `DecisionReconstructabilityClassifier.cs`
- `DecisionEventContractValidator.cs`
- `DecisionReconstructabilityContracts.cs`
- Repository / persistence patterns
- OpenAPI, route inventory, client coverage tests
- Authorization: `ServerOnly` / `ProvenanceRead` conventions

### 2. Classifier versioning

Add a version constant or provider, for example:

```text
kanon-decision-reconstructability-classifier-v1
```

Expose as `DecisionReconstructabilityClassifierVersion.Current` (or repo-consistent equivalent).

### 3. Canonical input hashing

Implement deterministic SHA-256 (lowercase hex) over canonical `DecisionEventContract` JSON:

- Reuse `Ontogony.Hashing` / existing canonical JSON helpers if present.
- Include `schemaVersion` and evidence fields; do not drop fields without documented justification.
- Tests: identical semantic payloads → identical hash; material payload change → different hash.

Suggested API:

```csharp
DecisionEventCanonicalHasher.ComputeSha256(DecisionEventContract decisionEvent)
```

### 4. Artifact contracts

See [02_ARTIFACT_CONTRACT.md](./02_ARTIFACT_CONTRACT.md). Suggested C# names:

- `ReconstructabilityReportArtifactContract`
- `CreateReconstructabilityReportArtifactRequest`
- `CreateReconstructabilityReportArtifactsBatchRequest`
- `ReconstructabilityReportArtifactResponse`
- `ReconstructabilityReportArtifactsBatchResponse`

### 5. API routes

Prefer:

```text
POST /ontology/v0/reconstructability/report-artifacts
POST /ontology/v0/reconstructability/report-artifacts/batch
GET  /ontology/v0/reconstructability/report-artifacts/{artifactId}
GET  /ontology/v0/reconstructability/report-artifacts/by-decision-event/{decisionEventId}   (optional)
```

Same auth policy as existing reconstructability endpoints.

**Create flow:**

```text
validate DecisionEventContract
→ compute decisionEventHash
→ classify (existing classifier)
→ build ReconstructabilityReportContract
→ build artifact (classifierVersion, metadata, snapshot, report)
→ persist (idempotent by deterministic artifactId)
→ return artifact
```

**Deterministic artifactId** (do not include `classifiedAtUtc`):

```text
recon_art_<hex prefix of sha256(schema + ":" + decisionEventHash + ":" + classifierVersion)>
```

### 6. Persistence

- Upsert / idempotent create by `artifactId`
- Fetch by `artifactId`
- Optional: latest by `decisionEventId`
- Preserve `createdAtUtc` on idempotent re-create; update `updatedAtUtc` when appropriate

### 7. Tests

Filter: `Reconstructability`

Cover: create, batch order, fetch, 404, 422, hash stability, artifactId stability, existing classify/classify-batch unchanged.

### 8. Evidence

`docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_KANON_EVIDENCE.md`

---

## Frontend (`C:\dev\ontogony-frontend`)

After Kanon ships artifact endpoints and OpenAPI is regenerated:

1. Regenerate Kanon client (`npm run openapi:check` / project convention).
2. Add artifact client helpers (create batch, get by id, optional get by decisionEventId).
3. **Evidence Spine** (`loadAllagmaRunDecisionReconstruction`, `appendAllagmaDecisionEventReconstructabilityGraph`):
   - Try `POST …/report-artifacts/batch` when loading a run graph.
   - On success: attach `artifactId`, `decisionEventHash`, `classifierVersion`, `classifiedAtUtc` to graph node metadata.
   - On failure / 404 / unavailable: fall back to existing `classify-batch` path (DEC-RECON-006 behavior).
4. **UI:** Show artifact metadata in decision-event nodes when present; keep `ReconstructabilityReportPanel` working from embedded report or on-demand classify.
5. Extend or add DEC-RECON-007 smoke (see [03_VERIFICATION_PLAN.md](./03_VERIFICATION_PLAN.md)).

Evidence: `docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_EVIDENCE.md`

---

## Platform (`C:\dev\ontogony-platform`)

- Add or extend smoke script under `scripts/decision-reconstructability/` (mirror DEC-RECON-004 pattern).
- Write `docker/local-working-system/artifacts/dec-recon-007-smoke-report.json` on PASS.
- Update this package README working log and move to `_consumed/2026-05/` when closeout completes.

---

## Safety constraint

Do **not** store or expose hidden chain-of-thought. Artifacts may include only safe reasoning surrogates: policy summaries, route explanations, validation outputs, human notes, evidence fragment references, classifier diagnostics, and `safeReasoningPolicy` from the report contract.
