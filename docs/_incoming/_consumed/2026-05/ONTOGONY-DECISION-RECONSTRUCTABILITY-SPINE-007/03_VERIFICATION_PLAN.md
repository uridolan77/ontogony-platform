# DEC-RECON-007 verification plan

## Unit and integration tests

### Kanon

```powershell
cd C:\dev\kanon-dotnet
dotnet build Kanon.sln -c Release --no-incremental
dotnet test tests\Kanon.Tests\Kanon.Tests.csproj -c Release --no-build --filter "Reconstructability"
```

Also run (if separate from filter):

- Route inventory / OpenAPI snapshot guards
- Client coverage / compatibility manifest tests

**Expected:**

- New artifact tests pass
- Existing `classify` / `classify-batch` tests pass
- Hash stability and deterministic `artifactId` tests pass

### Frontend

```powershell
cd C:\dev\ontogony-frontend
npm run openapi:check
npm run build
npm test -- decisionReconstructability
npm test -- appendAllagmaDecisionEventReconstructabilityGraph
```

Add DEC-RECON-007-specific tests when artifact client and spine wiring land.

### Platform hygiene

```powershell
cd C:\dev\ontogony-platform
powershell -NoProfile -File ./scripts/validate-docs-incoming-hygiene.ps1
```

## Live stack smoke (target)

Prerequisites: `docker compose` local working system; seeded run with decision events.

**Flow:**

```text
1. GET  /allagma/v0/runs/{runId}/decision-events
2. POST /ontology/v0/reconstructability/report-artifacts/batch
3. GET  /ontology/v0/reconstructability/report-artifacts/{artifactId}
4. (optional) GET …/by-decision-event/{decisionEventId}
5. Open Evidence Spine for same runId — decision reconstructability section + artifact metadata
```

**Machine-readable output:**

```text
ontogony-platform/docker/local-working-system/artifacts/dec-recon-007-smoke-report.json
```

Suggested report fields:

| Field | Purpose |
| --- | --- |
| `schema` | Smoke report schema id |
| `recordedAtUtc` | When smoke ran |
| `runId` | Baseline run |
| `traceId` | From export or seed report |
| `eventCount` | Decision events exported |
| `artifactCount` | Artifacts created |
| `reportCount` | Reports embedded in artifacts |
| `worstGovernanceStatus` | PASS / WARN / FAIL across artifacts |
| `classifierVersion` | Observed classifier version |
| `sampleArtifactId` | First artifact id for manual inspection |
| `sampleDecisionEventHash` | Matching hash |
| `steps[]` | Per-step pass/fail |
| `verdict` | `PASS` or `FAIL` |

### Script placement (to implement in slice 2–4)

Mirror DEC-RECON-004:

| Repo | Likely path |
| --- | --- |
| `ontogony-frontend` | `scripts/decision-reconstructability/run-dec-recon-007-api-smoke.mjs` |
| `ontogony-frontend` | `npm run dec-recon:smoke:007:api` (name TBD) |
| `ontogony-platform` | extend `scripts/validate-decision-reconstructability-local.ps1` or sibling |

Reference: [`dec-recon-004`](../../../docker/local-working-system/artifacts/dec-recon-004-smoke-report.json) smoke report and [`ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004`](../../_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/README.md).

## Non-regression gates

| Gate | Command / check |
| --- | --- |
| DEC-RECON-004 | `npm run dec-recon:smoke:api` or `npm run docker:smoke:dec-recon` in `ontogony-frontend` |
| DEC-RECON-006 | Evidence Spine unit tests + manual `/system/evidence-spine?id={runId}&kind=allagmaRunId` |
| Classify-batch | Same run: batch classify still returns 1:1 reports without persisting |

Document any skipped command with reason (ENV-SEED blocker, stack down, etc.). **Do not claim PASS for commands not run.**

## Evidence documents (on closeout)

| Repo | Path |
| --- | --- |
| Kanon | `docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_KANON_EVIDENCE.md` |
| Frontend | `docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_EVIDENCE.md` |
| Platform | Update [README.md](./README.md) working log; archive package to `_consumed/2026-05/` |

## Closeout commit plan (suggested)

```text
kanon-dotnet:       feat(reconstructability): persist report artifacts
ontogony-frontend:  feat(evidence): show reconstructability report artifacts
ontogony-platform:  docs(dec-recon): close persisted artifact evidence
```

Do not commit unrelated local changes.
