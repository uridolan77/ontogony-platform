# Evidence Spine graph taxonomy

**Status:** Canonical node and edge kinds for cross-service export (EVIDENCE-SPINE-REPLAY-KANON-001)  
**Implementation:** `ontogony-frontend/src/evidence-spine/evidenceGraphTypes.ts`  
**Export schema:** [`ontogony-cross-service-evidence-spine-bundle-v1.schema.json`](../schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json) (open `kind` strings; this document is normative)

## Node kinds

| Kind | Service | Role |
| --- | --- | --- |
| `allagma.run` | Allagma | Governed execution run |
| `allagma.replayRequest` | Allagma | Cross-service replay orchestration request |
| `allagma.replayResult` | Allagma | Completed replay result |
| `allagma.replayEvidenceBundle` | Allagma | Redacted replay export bundle |
| `allagma.replayDelta` | Allagma | Original vs replay comparison |
| `allagma.replayServiceAttempt` | Allagma | Per-service replay attempt |
| `conexus.modelCall` | Conexus | Model invocation evidence |
| `conexus.routeDecision` | Conexus | Route / model selection decision |
| `kanon.decision` | Kanon | Decision record (may carry lifecycle metadata) |
| `kanon.canonicalFact` | Kanon | Canonical fact artifact |
| `kanon.semanticPlan` | Kanon | Semantic query plan |
| `kanon.semanticQualitySnapshot` | Kanon | Semantic quality snapshot |
| `kanon.reviewItem` | Kanon | Operator review queue item |
| `kanon.ontologyVersion` | Kanon | Ontology version anchor |
| `kanon.sourceBinding` | Kanon | Source binding |
| `kanon.domainPack` | Kanon | Domain pack |
| `platform.trace` | Platform | Trace anchor |
| `platform.correlation` | Platform | Correlation anchor |

## Edge kinds

### Replay

| Kind | From → To |
| --- | --- |
| `replay_requested_for` | replay request → replay target (run, bundle, etc.) |
| `replay_resolved_to` | replay request → resolved target node |
| `replay_attempted_service` | replay result → service attempt |
| `replay_produced_bundle` | replay result → evidence bundle |
| `replay_produced_delta` | replay result → delta |

### Kanon lifecycle and review

| Kind | From → To |
| --- | --- |
| `decision_has_lifecycle` | decision → lifecycle summary (or metadata on node) |
| `decision_supersedes` | decision → superseded decision |
| `decision_reviewed_by` | decision → review source decision |
| `decision_resolves_review` | decision → review resolution decision |

### Semantic artifacts

| Kind | From → To |
| --- | --- |
| `artifact_belongs_to_ontology_version` | artifact → ontology version |
| `source_binding_supports_fact` | source binding → canonical fact |

### Conexus → Kanon

| Kind | From → To |
| --- | --- |
| `model_call_informed_decision` | model call → decision |
| `route_decision_selected_model` | route decision → model call |

### Legacy aliases (export canonicalization)

These kinds may appear in resolver output during migration; export maps them to the kinds above:

| Legacy | Canonical |
| --- | --- |
| `has_replay_result` | `replay_produced_bundle` (result linkage; context-specific) |
| `has_replay_bundle` | `replay_produced_bundle` |
| `has_replay_delta` | `replay_produced_delta` |
| `replay_service_attempt` | `replay_attempted_service` |
| `used_model_call` | `model_call_informed_decision` |
| `used_route_decision` | `route_decision_selected_model` |
| `used_kanon_decision` | `model_call_informed_decision` (run→decision) or retain with `derived_from` |
| `used_ontology_version` | `artifact_belongs_to_ontology_version` |
| `reviewed_by_decision` | `decision_reviewed_by` |

## Confidence

`direct` | `derived` | `weak` | `unresolved` — unchanged from v1 bundle schema.
