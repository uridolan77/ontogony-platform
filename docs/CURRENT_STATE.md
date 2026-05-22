# Current state — Ontogony.Platform

**Version line:** `0.3.0-alpha.1` (`Directory.Build.props`)  
**Shipping packages:** **27** `Ontogony.*` libraries under `src/` (validated by `scripts/validate-shipping-inventory.ps1`)  
**Target framework:** .NET 9 (`global.json` SDK `9.0.100`)

**Boundary:** This repo is **shared mechanics only**. It is not production-ready as a system, does not own semantic authority, model routing, or governed execution.

---

## What Platform provides (implemented)

| Area | Status | Entry |
| --- | --- | --- |
| Package graph + dependency rules | **Implemented** | [`ARCHITECTURE.md`](./ARCHITECTURE.md), [`architecture/package-levels.md`](./architecture/package-levels.md) |
| Trace / correlation | **Implemented** | [`operators/TRACE_CORRELATION_CONTRACT.md`](./operators/TRACE_CORRELATION_CONTRACT.md) |
| Error envelopes (cross-service gate) | **Implemented** | [`contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](./contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md) |
| Header propagation (frozen set) | **Implemented** | [`contracts/HEADER_PROPAGATION_CONTRACT.md`](./contracts/HEADER_PROPAGATION_CONTRACT.md) |
| System compatibility gate | **Implemented** | [`contracts/SYSTEM_COMPATIBILITY_GATE.md`](./contracts/SYSTEM_COMPATIBILITY_GATE.md), `Ontogony.SystemCompatibility` |
| Protocol registry | **Implemented** | [`system/system-protocol-registry.json`](./system/system-protocol-registry.json) |
| Per-package contracts | **Implemented** | [`packages/`](./packages/) |
| CI validation (build, test, docs, inventory, package levels) | **Implemented** | [`TESTING.md`](./TESTING.md) |

**Active alpha consumers:** Conexus.NET and Allagma.NET consume this stack (sibling `ProjectReference` or NuGet package mode). Kanon.NET references platform packages per its own `eng/Ontogony.References.props`. See [`INTEGRATION.md`](./INTEGRATION.md).

---

## Phase Tight platform deliverables (2026-05-22, done)

| ID | Deliverable |
| --- | --- |
| PLATFORM-9-001 | `Ontogony.SystemCompatibility`, `run-system-compatibility-gate.ps1`, [`SYSTEM_COMPATIBILITY_GATE.md`](./contracts/SYSTEM_COMPATIBILITY_GATE.md) |
| PLATFORM-9-002 | Cross-service error envelope matrix + conformance tests |
| PLATFORM-9-003 | Frozen propagation headers + `Ontogony.Testing` header assertions |

Evidence: [`evidence/PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md`](./evidence/PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md)

Cross-repo acceptance (cohesion, streaming, replay) lives in **product repos** — primarily [`allagma-dotnet/docs/e2e/`](https://github.com/uridolan77/allagma-dotnet/tree/main/docs/e2e).

---

## Partial / evolving

| Item | Status |
| --- | --- |
| HTTP resilience (Retry-After, jitter, circuit breaker) | **Partial** — baseline exists; richer policies still evolve ([`packages/Ontogony.Http.md`](./packages/Ontogony.Http.md)) |
| Public XML docs (`CS1591`) | **Partial** — Tier A enforced for Conexus baseline packages; Tier B deferred ([`quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](./quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md)) |
| PLAT-NP-008 in-memory DI warning coverage | **Partial** — maintenance guard when new public DI surfaces land |

---

## Not implemented (in this repo)

| Item | Notes |
| --- | --- |
| Production readiness | **Not started** — Docker-local and hardening milestones are operator programs, not prod charter |
| Replay engine | Contracts only (`Ontogony.Replay.Contracts`); no replay runtime here |
| Product semantics | Canonization, agent plans, provider routing, business rules — see ownership table in [`ARCHITECTURE.md`](./ARCHITECTURE.md) |

---

## Intentionally blocked (owned elsewhere)

| Concern | Owner |
| --- | --- |
| Semantic authority | **Kanon.NET** |
| Model gateway / routing | **Conexus.NET** |
| Governed execution / runs / gates | **Allagma.NET** |
| System runtime baseline lock | **Allagma.NET** — see [`allagma-dotnet/docs/evidence/README.md`](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/evidence/README.md) (e.g. SYSTEM-RC-001A) |

Platform must not become a runtime orchestrator or competing “system status” authority.

---

## Sister repos

| Repo | Documentation |
| --- | --- |
| allagma-dotnet | Governed execution, system cohesion, runtime lock |
| kanon-dotnet | Ontology, semantic decisions |
| conexus-dotnet | Model gateway |
| ontogony-frontend | Operator UI |
| ontogony-ui | Shared UI components |
