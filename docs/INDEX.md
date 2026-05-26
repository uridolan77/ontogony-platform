# Documentation index — Ontogony.Platform

Practical map of **canonical** platform documentation. For intake packages, see [`_incoming/_active/MANIFEST.md`](./_incoming/_active/MANIFEST.md).

---

## Start here

| Document | Purpose |
| --- | --- |
| [`README.md`](./README.md) | What this repo owns, doc layout, sister repos |
| [`CURRENT_STATE.md`](./CURRENT_STATE.md) | Single source of platform truth |
| [`ARCHITECTURE.md`](./ARCHITECTURE.md) | Package layers, forbidden edges |
| [`CONTRACTS.md`](./CONTRACTS.md) | Mechanical contracts index |
| [`DEVELOPMENT.md`](./DEVELOPMENT.md) | Build, local ports, Docker, CI scripts |
| [`TESTING.md`](./TESTING.md) | Tests, conformance kits, CI gates |
| [`INTEGRATION.md`](./INTEGRATION.md) | Consumer blueprints, package mode |
| [`KNOWN_LIMITATIONS.md`](./KNOWN_LIMITATIONS.md) | Non-goals and limitations |

---

## Architecture and decisions

| Area | Path |
| --- | --- |
| Package dependency matrix | [`architecture/package-levels.md`](./architecture/package-levels.md) |
| Architecture index | [`architecture/index.md`](./architecture/index.md) |
| Durability boundaries | [`architecture/durability-boundaries.md`](./architecture/durability-boundaries.md) |
| ADRs | [`adr/`](./adr/) |
| Migrations | [`migrations/`](./migrations/) |
| Closeout reviews | [`reviews/`](./reviews/) |

---

## Contracts (mechanical)

| Topic | Path |
| --- | --- |
| Contracts hub | [`CONTRACTS.md`](./CONTRACTS.md) |
| Error envelope gate | [`contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](./contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md) |
| Header propagation | [`contracts/HEADER_PROPAGATION_CONTRACT.md`](./contracts/HEADER_PROPAGATION_CONTRACT.md) |
| System compatibility | [`contracts/SYSTEM_COMPATIBILITY_GATE.md`](./contracts/SYSTEM_COMPATIBILITY_GATE.md) |
| Contract discipline | [`contracts/CONTRACT_DISCIPLINE_STANDARD.md`](./contracts/CONTRACT_DISCIPLINE_STANDARD.md) |
| Replay runtime | [`contracts/REPLAY_RUNTIME_CONTRACT.md`](./contracts/REPLAY_RUNTIME_CONTRACT.md) |
| Protocol registry | [`system/system-protocol-registry.json`](./system/system-protocol-registry.json) |
| JSON schemas | [`schemas/`](./schemas/) |

---

## Observability, errors, HTTP

| Topic | Path |
| --- | --- |
| Trace / correlation | [`operators/TRACE_CORRELATION_CONTRACT.md`](./operators/TRACE_CORRELATION_CONTRACT.md) |
| Operator contracts index | [`operators/README.md`](./operators/README.md) |
| Evidence spine (mechanical) | [`operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md) |
| Terminology / boundaries | [`operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](./operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) |
| Six-repo doc standard | [`operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](./operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) |

Implementation packages: [`packages/Ontogony.Observability.md`](./packages/Ontogony.Observability.md), [`packages/Ontogony.Http.md`](./packages/Ontogony.Http.md), [`packages/Ontogony.Errors.md`](./packages/Ontogony.Errors.md).

---

## Idempotency, persistence, execution

| Topic | Path |
| --- | --- |
| Package index | [`packages/index.md`](./packages/index.md) |
| Idempotency / outbox patterns | [`packages/Ontogony.Persistence.md`](./packages/Ontogony.Persistence.md), [`adoption/conformance-kits.md`](./adoption/conformance-kits.md) |
| Execution journal | [`packages/Ontogony.Execution.md`](./packages/Ontogony.Execution.md) |
| AI runtime guardrails | [`ai-runtime/boundary-guardrails.md`](./ai-runtime/boundary-guardrails.md) |

---

## Hosting and local environments

| Topic | Path |
| --- | --- |
| Docker local (compose) | [`../docker/local-working-system/README.md`](../docker/local-working-system/README.md) |
| Environment docs | [`environments/docker-local-working-system/`](./environments/docker-local-working-system/) |
| Canonical restart | [`operators/CANONICAL_RESTART_PATH.md`](./operators/CANONICAL_RESTART_PATH.md) |

---

## Testing and evidence

| Topic | Path |
| --- | --- |
| Testing guide | [`TESTING.md`](./TESTING.md) |
| Conformance adoption | [`adoption/conformance-kits.md`](./adoption/conformance-kits.md) |
| Platform evidence | [`evidence/README.md`](./evidence/README.md) |
| Quality / coverage policy | [`quality/`](./quality/) |

---

## Release, governance, consumers

| Topic | Path |
| --- | --- |
| Consumer blueprints | [`consumer-blueprints/`](./consumer-blueprints/) |
| Governance / RC | [`governance/`](./governance/) |
| Learning path (all repos) | [`learn/INDEX.md`](./learn/INDEX.md) |
| Consumer compatibility | [`governance/PHASE1_CONSUMER_COMPATIBILITY.md`](./governance/PHASE1_CONSUMER_COMPATIBILITY.md) |

---

## Intake (non-canonical)

| Topic | Path |
| --- | --- |
| Intake policy + **archive to `_consumed`** | [`_incoming/README.md`](./_incoming/README.md) |
| Active packages | [`_incoming/_active/MANIFEST.md`](./_incoming/_active/MANIFEST.md) |
| Consumed archive | [`_incoming/_consumed/MANIFEST.md`](./_incoming/_consumed/MANIFEST.md) |

---

## Sister repos (product semantics)

| Repo | Role |
| --- | --- |
| `kanon-dotnet` | Semantic authority |
| `allagma-dotnet` | Governed execution |
| `conexus-dotnet` | Model gateway |
| `ontogony-frontend` | Operator console |
| `ontogony-ui` | Shared UI mechanics |
