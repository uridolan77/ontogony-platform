# DEC-RECON-007 acceptance checklist

**Verdict:** PASS only when all applicable sections are checked on the same seeded or documented baseline run.

## Prerequisites

- [ ] Local working system: Allagma + Kanon + Conexus (+ frontend `:5175`) healthy
- [ ] Baseline run with decision events (ENV-SEED-001 or documented substitute)
- [ ] Service tokens match compose `.env`

## Kanon — artifact persistence

- [ ] `POST /ontology/v0/reconstructability/report-artifacts` creates an artifact for a valid `DecisionEventContract`
- [ ] `POST /ontology/v0/reconstructability/report-artifacts/batch` preserves input order
- [ ] `GET /ontology/v0/reconstructability/report-artifacts/{artifactId}` returns stored artifact
- [ ] Fetch missing `artifactId` → structured 404
- [ ] Invalid event → 422 (`DecisionEventNotClassifiable` or equivalent), same pattern as classify
- [ ] Artifact `schema` = `ontogony-reconstructability-report-artifact-v1`
- [ ] Artifact includes deterministic `decisionEventHash` (SHA-256 lowercase hex)
- [ ] Same canonical input → same `decisionEventHash` and same `artifactId`
- [ ] Material input change → different `decisionEventHash`
- [ ] Artifact records `classifierVersion` (non-empty, stable for classifier release)
- [ ] Artifact stores full `report` (`ReconstructabilityReportContract`)
- [ ] Artifact includes `decisionEventSnapshot` (or canonical snapshot) and correlation fields when present on input
- [ ] Optional: `GET …/by-decision-event/{decisionEventId}` returns latest artifact (or documented indexed query)

## Kanon — non-regression

- [ ] `POST /ontology/v0/reconstructability/classify` unchanged for DEC-RECON-004 consumers
- [ ] `POST /ontology/v0/reconstructability/classify-batch` unchanged for DEC-RECON-004 / 006
- [ ] `dotnet test … --filter "Reconstructability"` passes (new + existing tests)
- [ ] Route inventory / OpenAPI guards updated intentionally and pass

## Allagma — export (unchanged contract)

- [ ] `GET /allagma/v0/runs/{runId}/decision-events` still returns `ontogony-allagma-run-decision-events-v1`
- [ ] At least one exported event used to generate a persisted artifact

## Frontend — progressive enhancement

- [ ] Evidence Spine still renders **Decision reconstructability** section (DEC-RECON-006)
- [ ] When artifacts available: node shows `artifactId`, `decisionEventHash`, `classifierVersion`, `classifiedAtUtc` (or subset per UX spec)
- [ ] When artifact API unavailable: classify-batch fallback still works; panel opens
- [ ] Run audit `run-decision-reconstruction-panel` still works (DEC-RECON-004)
- [ ] `npm run openapi:check`, `npm test` (decision reconstructability / graph append filters) pass

## Live smoke

- [ ] Smoke: Allagma GET decision-events → Kanon POST report-artifacts/batch → Kanon GET by artifactId
- [ ] Evidence Spine page shows decision-event section with artifact metadata when smoke used UI path
- [ ] `docker/local-working-system/artifacts/dec-recon-007-smoke-report.json` written with `verdict: PASS`

## Evidence artifacts

| Repo | Path |
| --- | --- |
| Kanon | `docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_KANON_EVIDENCE.md` |
| Frontend | `docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_EVIDENCE.md` |
| Platform | `docker/local-working-system/artifacts/dec-recon-007-smoke-report.json` |

## Explicit non-goals (must remain true)

- [ ] No Conexus decision-event export added in this slice
- [ ] No hidden chain-of-thought in stored artifacts or API responses
- [ ] DEC-RECON-004 smoke still passes or failure documented with environment blocker
