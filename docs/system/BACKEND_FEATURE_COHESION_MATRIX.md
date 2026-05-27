# Backend feature cohesion matrix

**Manifest:** [`backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json`](./backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json)  
**Protocol:** [`docs/protocols/BACKEND_SYSTEM_COHESION.md`](../protocols/BACKEND_SYSTEM_COHESION.md)

Matrix view of feature spines × runtime repos. **Status** values come from the manifest (`closed`, `in_progress`, `open`, `deferred`).

## Repo roles

| Repo | Role |
| --- | --- |
| `ontogony-platform` | Contract/fixture/manifest authority; mechanical spines |
| `allagma-dotnet` | Governed execution |
| `kanon-dotnet` | Semantic authority |
| `conexus-dotnet` | Model gateway |
| `metabole-dotnet` | Data transformation |

## Spine matrix

| Spine | Status | Owner | Participants | Protocol / contract |
| --- | --- | --- | --- | --- |
| Decision reconstructability | closed | Kanon | Allagma, Conexus, Platform | [`DECISION_RECONSTRUCTABILITY_PROTOCOL_V0.md`](../contracts/DECISION_RECONSTRUCTABILITY_PROTOCOL_V0.md) |
| Workflow lifecycle | in_progress | Allagma | Kanon, Conexus | Backend cohesion protocol |
| Budget / cost governance | in_progress | Allagma | Conexus | Backend cohesion protocol |
| Skill optimization | closed | Allagma | Kanon, Conexus, Platform | [`SKILL_OPTIMIZATION_SPINE.md`](../protocols/SKILL_OPTIMIZATION_SPINE.md) |
| Skill release governance | closed | Allagma, Kanon | Platform | [`SKILL_RELEASE_GOVERNANCE.md`](../protocols/SKILL_RELEASE_GOVERNANCE.md) |
| Sandbox consumer activation | closed | Allagma | Platform | [`SANDBOX_CONSUMER_ACTIVATION.md`](../protocols/SANDBOX_CONSUMER_ACTIVATION.md) |
| Kanon semantic authority | in_progress | Kanon | Allagma | Backend cohesion protocol |
| Conexus routing / model-call evidence | in_progress | Conexus | Allagma | Backend cohesion protocol |
| Metabole transformation / evolution | in_progress | Metabole | Kanon, Conexus | Backend cohesion protocol |
| Cross-service error envelope | closed | Platform | All runtimes | [`CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](../contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md) |
| Trace / correlation / idempotency | closed | Platform | All runtimes | [`MECHANICAL_PROTOCOL_REGISTRY.md`](../contracts/MECHANICAL_PROTOCOL_REGISTRY.md) |
| OpenAPI / route inventory | in_progress | Platform | All runtimes | [`system-protocol-registry.json`](./system-protocol-registry.json) |
| Evidence export / read-model | closed | Allagma | Kanon, Conexus, Platform | [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](../operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md) |

## Ownership rules

- **Owner** repo implements the spine’s primary behavior and golden-path tests.
- **Participant** repos supply contracts, calls, or evidence consumed by the owner.
- Platform **closed** spines mean contract + conformance evidence exist here; runtime repos still run alignment tests against the manifest.
- A spine marked **closed** in this matrix requires `evidenceDocs` in the manifest (enforced by schema tests).

## Evidence pointers (closed spines)

| Spine | Evidence |
| --- | --- |
| Decision reconstructability | [`PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md`](../evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md) |
| Skill optimization | [`ONTOGONY_SKILL_OPTIMIZATION_SPINE_001_CLOSEOUT.md`](../evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001_CLOSEOUT.md) |
| Skill release governance | [`ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md`](../evidence/ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md) |
| Sandbox consumer activation | [`ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001G.md`](../evidence/ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001G.md) |
| Error envelope | [`PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md`](../evidence/PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md) |
| Trace / correlation / idempotency | [`TRACE_CONTRACT_001_EVIDENCE.md`](../evidence/TRACE_CONTRACT_001_EVIDENCE.md) |
| Evidence export / read-model | [`SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md`](../evidence/SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md) |

Full index: [`BACKEND_COHESION_EVIDENCE_INDEX.md`](../evidence/BACKEND_COHESION_EVIDENCE_INDEX.md).
