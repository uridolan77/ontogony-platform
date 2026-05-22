# Mechanical contracts index

Platform-owned **mechanics** consumed across Ontogony services. Product semantics stay in Kanon, Allagma, and Conexus.

---

## Trace and correlation

| Document | Purpose |
| --- | --- |
| [`operators/TRACE_CORRELATION_CONTRACT.md`](./operators/TRACE_CORRELATION_CONTRACT.md) | Canonical trace/correlation headers and middleware behavior |
| [`operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](./operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) | Names and ownership boundaries |

Implementation: `Ontogony.Observability`, `Ontogony.Http` (outbound propagation).

---

## Errors

| Document | Purpose |
| --- | --- |
| [`contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](./contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md) | Cross-service error JSON shape and gate checks (PLATFORM-9-002) |
| [`contracts/error-envelope.matrix.json`](./contracts/error-envelope.matrix.json) | Machine-readable matrix |

Implementation: `Ontogony.Errors`.

---

## Headers and propagation

| Document | Purpose |
| --- | --- |
| [`contracts/HEADER_PROPAGATION_CONTRACT.md`](./contracts/HEADER_PROPAGATION_CONTRACT.md) | Frozen propagation header set (PLATFORM-9-003) |
| [`system/propagation-header.matrix.json`](./system/propagation-header.matrix.json) | Matrix validated by system-compat gate |

Test helpers: `Ontogony.Testing.HeaderPropagationConformanceAssertions`.

---

## System compatibility

| Document | Purpose |
| --- | --- |
| [`contracts/SYSTEM_COMPATIBILITY_GATE.md`](./contracts/SYSTEM_COMPATIBILITY_GATE.md) | Reads sibling-repo manifests; validates drift (PLATFORM-9-001) |

Package: `Ontogony.SystemCompatibility`. Script: `scripts/run-system-compatibility-gate.ps1`.

**Note:** The gate is mechanical. Kanon manifests, Conexus OpenAPI snapshots, and Allagma feature matrices are **authored in those repos**.

---

## Protocol registry

| Asset | Purpose |
| --- | --- |
| [`system/system-protocol-registry.json`](./system/system-protocol-registry.json) | Registered system protocols |
| [`system/README.md`](./system/README.md) | Registry operator notes |

Validated by `scripts/validate-system-protocol-registry.ps1`.

---

## AG-UI / evidence spine (mechanical)

| Document | Purpose |
| --- | --- |
| [`operators/AGENT_INTERACTION_SPINE_CONTRACT.md`](./operators/AGENT_INTERACTION_SPINE_CONTRACT.md) | Interaction spine contract |
| [`operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md`](./operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md) | Evidence resolver contract |
| [`operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md) | Cross-repo evidence spine index |
| [`operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](./operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md) | Identifier taxonomy |

Frontend and backend **implementations** live in their respective repos.

---

## Event envelopes and hashing

- **Envelopes:** `Ontogony.Contracts` — protocol-neutral event shape; conformance via `Ontogony.Testing.EnvelopeConformanceAssertions`.
- **Hashing:** `Ontogony.Hashing` — deterministic canonical JSON fingerprints.

Ingress normalization: `Ontogony.ProtocolIngress` (see package README under `src/`).

---

## ADRs

Breaking or structural decisions: [`adr/`](./adr/).

Migrations for consumer-visible changes: [`migrations/`](./migrations/).
