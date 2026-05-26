# Decision reconstructability HTTP surfaces (v0)

**Status:** implemented 2026-05-26 (closure Option 1 + spine slices 004–007).  
**Index:** [DECISION_RECONSTRUCTABILITY_PROTOCOL_V0.md](DECISION_RECONSTRUCTABILITY_PROTOCOL_V0.md)

Kanon is the classifier and preferred read API. Allagma and Conexus expose decision-event exports aligned with [DECISION_EVENT_SCHEMA_V0.md](DECISION_EVENT_SCHEMA_V0.md). The operator console may call Kanon directly or via BFF patterns documented in `ontogony-frontend`.

---

## Kanon (semantic authority)

| Method | Path | Role |
| --- | --- | --- |
| GET | `/ontology/v0/decision-events/{decisionEventId}/reconstructability` | Assembled event + report |
| GET | `/ontology/v0/reconstructability/by-trace/{traceId}` | Trace-scoped reconstruction |
| POST | `/ontology/v0/reconstructability/classify` | Classify one event |
| POST | `/ontology/v0/reconstructability/classify-batch` | Batch classify (Evidence Spine, run panels) |
| POST | `/ontology/v0/reconstructability/report-artifacts` | Persist report artifact (DEC-RECON-007) |
| POST | `/ontology/v0/reconstructability/report-artifacts/batch` | Batch persist |
| GET | `/ontology/v0/reconstructability/report-artifacts/{artifactId}` | Fetch artifact |
| GET | `/ontology/v0/reconstructability/report-artifacts/by-decision-event/{decisionEventId}` | Lookup by event |

Route inventory: `kanon-dotnet/docs/generated/fragments/ontology-v0-route-list.md`.

---

## Allagma (governed execution exports)

| Method | Path | Role |
| --- | --- | --- |
| GET | `/allagma/v0/runs/{runId}/decision-events` | Run-scoped decision events for classification |

Response schema: `ontogony-allagma-run-decision-events-v1`. See `allagma-dotnet/docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json`.

---

## Conexus (model gateway exports)

| Method | Path | Role |
| --- | --- | --- |
| GET | `/admin/v0/model-calls/{modelCallId}/decision-events` | Dedicated decision-event export |
| GET | `/admin/v0/model-calls/{modelCallId}/evidence-bundle` | Preferred bundle; includes `decisionEvents` slice |

See `conexus-dotnet/docs/contracts/CONEXUS_DECISION_EVENTS_V1.md`.

---

## Operator console routes (frontend)

| Path | Data source |
| --- | --- |
| `/allagma/runs/:runId/reconstructability` | Allagma decision-events + Kanon classify |
| `/conexus/model-calls/:modelCallId/reconstructability` | Conexus decision-events + Kanon classify |
| `/kanon/reconstructability/:decisionEventId` | Kanon GET reconstructability |
| `/system/evidence-spine/:traceId/reconstructability` | Kanon by-trace |

---

## Error semantics

| Status | Meaning |
| --- | --- |
| `404` | No decision event or source id |
| `422` | Source exists but cannot be assembled into a decision event |
| `200` + `FAIL` | Event classified; governance failed |
| `200` + `WARN` | Reconstructable for non-critical workflows; non-blocking gaps |

Align with Kanon middleware and [`CROSS_SERVICE_ERROR_CONTRACT.md`](CROSS_SERVICE_ERROR_CONTRACT.md) for cross-service calls.
