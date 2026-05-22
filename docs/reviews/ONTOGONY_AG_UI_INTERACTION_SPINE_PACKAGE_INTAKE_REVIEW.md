# Ontogony AG-UI Interaction Spine Package — intake review

**Package:** `docs/_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22.zip`  
**Unpacked to:** `docs/_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/ontogony_ag_ui_interaction_spine_package_2026-05-22/` (nested folder inside outer extract dir)  
**Review date:** 2026-05-22  
**Intake verdict:** **GO for contract baseline** — package direction matches post–Evidence Spine / SYS-TIGHT operator reality; implementation must stay additive, client-first, and must not duplicate the cross-service Evidence Spine. **Do not start backend streaming or external AG-UI adapter work in the first milestone.**

**Runtime naming:** Use **Allagma** for governed execution everywhere. The package contains **no** `Agentor` references (compliant). Historical “Agentor” mentions in other repos stay confined to legacy/planning docs only.

**Locked baseline (informational):** `SYSTEM-ALPHA-006` per [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../allagma-dotnet/docs/system/ontogony-runtime.lock.json). AG-UI work does not require a lock promotion; register new protocol artifacts in [`system-protocol-registry.json`](../system/system-protocol-registry.json) when `PLAT-AGUI-000` lands.

---

## 1. Package contents summary

| Category | Paths | Count / notes |
|----------|-------|----------------|
| Executive / grounding | `00_EXECUTIVE_SUMMARY.md` … `14_BACKLOG_RISKS_AND_NON_GOALS.md`, `README.md`, `SOURCE_INDEX.md` | 16 markdown planning docs |
| Machine artifacts | `schemas/*.json`, `MANIFEST.json` | Event + session JSON Schema, event-family matrix |
| Contract sketches | `contracts/typescript/*`, `contracts/csharp/OntogonyAgentInteractionContracts.cs` | Reference only — not wired into builds |
| Fixtures | `fixtures/events/sample-run.jsonl`, `sample-human-gate-interrupt.jsonl` | **2** JSONL files shipped (plan doc `12_TESTING` names 5 categories — **not all present**) |
| AG-UI adapter | `adapters/ag-ui/*` | Strategy + TypeScript sketch (deferred milestone) |
| Implementation prompts | `prompts/PLAT-AGUI-000.md` … `OFE-AGUI-002.md`, `ALLAGMA/KANON/CONEXUS-AGUI-001` | Per-PR copy-paste scope |
| Repo copy map | `repo-copy-plan/README.md` | Destination paths per repo |

**Package goal (one line):** Add an **Ontogony-owned, event-sourced Agent Interaction Spine** (AG-UI-*concept*-compatible) on top of existing evidence/correlation/run surfaces — a **live co-execution timeline**, not another static graph browser.

**Milestone label in package:** `ONTOGONY-AGUI-000 — Agent Interaction Spine Baseline`.

---

## 2. What is immediately usable

| Artifact | Usability |
|----------|-----------|
| `schemas/ontogony-agent-interaction-event-v0.schema.json` | **Ready to publish** — draft 2020-12, matches platform schema style; `source` enum includes `fixture`; 47 event types + `CUSTOM` |
| `schemas/ontogony-agent-interaction-session-v0.schema.json` | **Ready** — references event schema via `$ref` |
| `schemas/agent-interaction-event.matrix.json` | **Ready** — service ownership + `existingSources` paths align with live Allagma/Kanon/Conexus routes |
| `03_ONTOGONY_AGENT_INTERACTION_SPINE_CONTRACT.md` | **Ready** as `docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md` after cross-links to Evidence Spine |
| `04_EVENT_TAXONOMY_AND_MAPPING.md` | **Ready** as operator appendix or sections merged into contract |
| `fixtures/events/*.jsonl` | **Validate as-is** — sample lines match schema constants |
| `05_REPO_PR_SEQUENCE.md` + prompts | **Ready** as implementation backlog |
| `contracts/typescript/*` | **Reference** for `ontogony-frontend` / `ontogony-ui` types — do not copy verbatim into platform |
| `contracts/csharp/OntogonyAgentInteractionContracts.cs` | **Reference** for Allagma projection DTOs — belongs in `allagma-dotnet` contracts/application, not platform |
| `adapters/ag-ui/AG_UI_COMPATIBILITY_STRATEGY.md` | **Planning only** — correct deferral |

---

## 3. What must be adapted to current code

### ontogony-platform

| Package assumption | Repo truth | Adaptation |
|--------------------|------------|------------|
| New schemas under `docs/schemas/` | Evidence Spine already owns `ontogony-cross-service-evidence-spine-bundle-v1.schema.json` + validators | Add **parallel** schemas; contract must state **Evidence Spine = graph authority**, Interaction Spine = **temporal event log** |
| `docs/system/agent-interaction-event.matrix.json` | `docs/system/system-evidence-spine-resolution.matrix.json` exists | Different artifacts — link both in contract index; do not merge matrices |
| Validation scripts in `11_PLATFORM_SCHEMAS` | Pattern: `scripts/validate-system-evidence-spine-contract.ps1` + `SystemEvidenceSpineContractTests.cs` | Implement `validate-agent-interaction-spine.ps1` + `SystemAgentInteractionSpineContractTests.cs`; register protocol in `system-protocol-registry.json` |
| Package fixtures path | Only 2 JSONL in zip | Copy to `docs/schemas/fixtures/agent-interaction/` (or validate from intake path in CI) |

### ontogony-frontend

| Package assumption | Repo truth | Adaptation |
|--------------------|------------|------------|
| Greenfield resolver | [`resolveEvidenceSpine.ts`](../../ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts) (~989 lines), deep graphs, export bundle, contract matrix tests | `evidenceSpineInteractionAdapter.ts` should **wrap** resolver output, not re-fetch |
| Greenfield correlation | [`resolveTraceCorrelation.ts`](../../ontogony-frontend/src/system/correlation/resolveTraceCorrelation.ts) | Reuse for lookup bar |
| New timeline UX | [`AllagmaRunCrossServiceTimeline.tsx`](../../ontogony-frontend/src/allagma/components/AllagmaRunCrossServiceTimeline.tsx) + `buildCrossServiceTimelineEvents` + `@ontogony/ui/observability` `OperatorTimeline` | OFE-AGUI-002 should **extend or compose** this — avoid two competing timelines on run detail |
| Route `/system/agent-interaction` | `/system/evidence-spine` exists in [`routes.tsx`](../../ontogony-frontend/src/app/routes.tsx) | Add sibling route; deep-link between workbenches |
| Human gate extraction | `extractHumanGateIdFromEvents` already used | Map to `INTERRUPT_CREATED` / `HUMAN_GATE_*` in adapter |

### ontogony-ui

| Package assumption | Repo truth | Adaptation |
|--------------------|------------|------------|
| New `./agent` export | Exports: `./system`, `./execution`, `./observability`, `./chat`, `./operator`, etc. — **no `./agent`** | Follow `check:exports`, `bundle:check`, Storybook patterns used by other slices |
| Eight new components | `./observability` already has `OperatorTimeline` | Agent timeline may wrap or fork observability patterns; share density/a11y gates |

### allagma-dotnet

| Package assumption | Repo truth | Adaptation |
|--------------------|------------|------------|
| Run/events/audit/resume/retry/cancel/replay | [`allagma-feature-connection.matrix.json`](../../allagma-dotnet/docs/system/allagma-feature-connection.matrix.json) `lastUpdated` 2026-05-21 — **matches** package table | Projection-only first; no new HTTP until JSONL export validates |
| Streaming v0 | `AllagmaRunStreamLifecyclePanel` + matrix note `fe-streaming-evidence-ui` (SYS-TIGHT-005): **metadata-only** stream lifecycle from run events | **Not** full interaction-event SSE — ALLAGMA-AGUI-002 must not conflate with existing panel |
| C# contracts in platform zip | Allagma owns runtime DTOs | Place records in Allagma.Contracts or Application, validated against platform schema |

### kanon-dotnet

| Package assumption | Repo truth | Adaptation |
|--------------------|------------|------------|
| Human gate / decision routes | [`KANON_EVIDENCE_SPINE_HANDOFF.md`](../../kanon-dotnet/docs/operators/KANON_EVIDENCE_SPINE_HANDOFF.md) + generated entrypoints | KANON-HITL-AGUI-001 = **mapping docs + tests** first; optional thin DTOs — avoid new ontology routes unless gap found |
| Resolve path | `POST /ontology/v0/actions/human-gates/{humanGateId}/resolve` | Package mapping **correct** |

### conexus-dotnet

| Package assumption | Repo truth | Adaptation |
|--------------------|------------|------------|
| Model-call evidence | [`MODEL_CALL_EVIDENCE_FLOW.md`](../../conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md) — project `GET /conexus/v0/model-calls/{id}` + admin drill-down | Package `04_*` correctly lists **both** project and admin routes; operator adapters should prefer **admin** + evidence-links for spine depth (same as Evidence Spine) |
| CONEXUS-AGUI-001 | Projection/tests/docs | No gateway behavior in Conexus core — stay in docs + fixture export tests |

---

## 4. Stale or thin assumptions

| Item | Issue |
|------|--------|
| `12_TESTING_AND_FIXTURE_HARNESS.md` | Lists `sample-model-call-route.jsonl`, `sample-missing-evidence.jsonl`, `sample-replay.jsonl` — **not in MANIFEST or zip** |
| `01_CURRENT_STATE_GROUNDING.md` | Accurate as of package date; **kanon-dotnet** / **allagma-dotnet** may have moved on `main` since intake — re-verify endpoint matrix before ALLAGMA/KANON PRs |
| `04_EVENT_TAXONOMY` Conexus row `GET /conexus/v0/model-calls/{modelCallId}` | Valid for project scope; operator UI already uses admin paths in resolver — matrix should document **operator-preferred** vs **project-scoped** |
| `system-protocol-registry.json` pins | Registry still lists ALPHA-006 SHAs; AG-UI protocol entry not present yet — expected |
| CopilotKit / .NET AG-UI SDK | Correctly deferred in `14_BACKLOG` — do not import dependencies in baseline |
| “No live AG-UI compatibility” | Still true — adapter is milestone 10 |

---

## 5. Risky or over-ambitious slices

| Slice | Risk | Mitigation |
|-------|------|------------|
| Full v0 enum (47+ types) in PR 1 | Contract paralysis, empty adapters | **Phase event families:** RUN, STEP, INTERRUPT, DECISION, MODEL_CALL, EVIDENCE_* first; MESSAGE_DELTA / UI_INTENT_* documented but optional in first adapters |
| `OUI-AGENT-001` (8 components + Storybook) | Large UI PR | Split: timeline + interrupt + evidence links first; state diff + UI intent renderer later |
| `OFE-AGUI-003` unified workbench | Duplicates Evidence Spine + cross-service timeline | Merge navigation; shared lookup identifiers; single “unresolved” vocabulary |
| `MESSAGE_*` / streaming | Allagma stream panel is metadata-only | Client-synthesize from run events; no raw token deltas unless existing event vocabulary exposes them |
| `UI_INTENT_*` | Arbitrary generative UI | Registry-only; defer renderer beyond stub |
| `ALLAGMA-AGUI-002` SSE | Backend before reducer stable | **Explicit defer** per package — minimum milestone 9 |
| `ADAPTER-AGUI-001` | Two canonical event models | After internal spine proven with fixtures |
| Payload `type: object` + `payload: {}` in schema | Weak drift detection | Add optional `payloadSchemaRef` per event family in matrix v0.1 or document payloads in contract appendix |

---

## 6. Recommended final PR sequence

Aligned with package `05_REPO_PR_SEQUENCE.md`, trimmed for repo reality:

| Order | ID | Repo | Scope | Depends on |
|-------|-----|------|-------|------------|
| 1 | **PLAT-AGUI-000** | ontogony-platform | Contract index, schemas, matrix, fixtures dir, validation script + tests, evidence doc, protocol registry entry | — |
| 2a | **OUI-AGENT-001** (split ok) | ontogony-ui | `./agent` export: timeline, interrupt card, tool card, evidence links | PLAT-AGUI-000 types/constants |
| 2b | **OFE-AGUI-001** | ontogony-frontend | `src/agent-interaction/`, JSONL replay, reducer, `/system/agent-interaction` | PLAT-AGUI-000; can parallel 2a |
| 3 | **OFE-AGUI-002** | ontogony-frontend | Adapters from Allagma/Kanon/Conexus APIs + Evidence Spine + correlation; embed on run/observability/decisions | OUI-AGENT-001 + OFE-AGUI-001 |
| 4 | **ALLAGMA-AGUI-001** | allagma-dotnet | `IAgentInteractionEventProjector`, JSONL export endpoint or operator command, tests | PLAT-AGUI-000 |
| 5 | **KANON-HITL-AGUI-001** | kanon-dotnet | Interrupt mapping doc + tests (+ optional DTOs) | PLAT-AGUI-000 |
| 6 | **CONEXUS-AGUI-001** | conexus-dotnet | Model-call projection doc + fixture export tests | PLAT-AGUI-000 |
| 7 | **OFE-AGUI-003** | ontogony-frontend | Integrated workbench, JSONL import/export, cross-links | 2–6 |
| — | ALLAGMA-AGUI-002 | allagma-dotnet | SSE stream | **Defer** until 7 accepted |
| — | ADAPTER-AGUI-001 | frontend/platform | External AG-UI mapping | **Defer** to backlog |

---

## 7. Cross-repo dependency order

```text
PLAT-AGUI-000 (schemas + contract + gates)
    ├─► OUI-AGENT-001 ──┐
    └─► OFE-AGUI-001 ───┼─► OFE-AGUI-002 ──► OFE-AGUI-003
                        │
    ├─► ALLAGMA-AGUI-001 ─┤
    ├─► KANON-HITL-AGUI-001 ─┤ (docs/tests; feeds adapters)
    └─► CONEXUS-AGUI-001 ────┘
```

**Parallelizable after PLAT-AGUI-000:** `OUI-AGENT-001`, `OFE-AGUI-001`, and backend doc/projection PRs (4–6) that only emit JSONL matching schema.

**Consumer package order (if publishing UI):** platform docs → `@ontogony/ui` alpha → `ontogony-frontend` bump.

---

## 8. Files likely copied into each repo

Do **not** blind-copy; use as authoritative sources.

### ontogony-platform (`PLAT-AGUI-000`)

| Source (intake) | Destination |
|-----------------|-------------|
| `03_ONTOGONY_AGENT_INTERACTION_SPINE_CONTRACT.md` | `docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md` |
| `schemas/ontogony-agent-interaction-event-v0.schema.json` | `docs/schemas/ontogony-agent-interaction-event-v0.schema.json` |
| `schemas/ontogony-agent-interaction-session-v0.schema.json` | `docs/schemas/ontogony-agent-interaction-session-v0.schema.json` |
| `schemas/agent-interaction-event.matrix.json` | `docs/system/agent-interaction-event.matrix.json` |
| `fixtures/events/*.jsonl` | `docs/schemas/fixtures/agent-interaction/*.jsonl` |
| `04_EVENT_TAXONOMY_AND_MAPPING.md` | Sections in contract or `docs/operators/AGENT_INTERACTION_EVENT_MAPPING.md` |
| (new) | `scripts/validate-agent-interaction-spine.ps1`, `.sh` |
| (new) | `docs/evidence/PLAT_AGUI_000_EVIDENCE.md` |
| (new) | `tests/.../SystemAgentInteractionSpineContractTests.cs` |
| (edit) | `docs/system/system-protocol-registry.json` — new `agent-interaction-spine-contract` protocol |

### ontogony-ui (`OUI-AGENT-001`)

| Source | Destination |
|--------|-------------|
| `contracts/typescript/ontogony-agent-interaction-types.ts` | Inform `src/components/agent/types.ts`, `eventViewModels.ts` |
| `07_UI_PACKAGE_PLAN.md` | PR description only |

### ontogony-frontend (`OFE-AGUI-001/002/003`)

| Source | Destination |
|--------|-------------|
| `contracts/typescript/interaction-reducer-sketch.ts` | `src/agent-interaction/reducers/interactionReducer.ts` |
| `fixtures/events/*.jsonl` | `src/agent-interaction/fixtures/` |
| `06_FRONTEND_IMPLEMENTATION_PLAN.md` | PR plan |
| `adapters/ag-ui/ag-ui-adapter-sketch.ts` | Defer to `src/agent-interaction/adapters/agUiAdapter.ts` |

### allagma-dotnet (`ALLAGMA-AGUI-001`)

| Source | Destination |
|--------|-------------|
| `contracts/csharp/OntogonyAgentInteractionContracts.cs` | Application/Contracts DTOs (rename namespace to Allagma/Ontogony conventions) |
| `08_ALLAGMA_ADAPTER_PLAN.md` | PR plan + `docs/migrations/` note if HTTP export added |

### kanon-dotnet (`KANON-HITL-AGUI-001`)

| Source | Destination |
|--------|-------------|
| `09_KANON_HITL_PLAN.md` | `docs/operators/KANON_AGENT_INTERACTION_INTERRUPT_MAPPING.md` (suggested name) |
| Link from | `KANON_EVIDENCE_SPINE_HANDOFF.md` |

### conexus-dotnet (`CONEXUS-AGUI-001`)

| Source | Destination |
|--------|-------------|
| `10_CONEXUS_TRACE_EVENTS_PLAN.md` | `docs/operators/CONEXUS_AGENT_INTERACTION_EVENT_PROJECTION.md` |
| Link from | `MODEL_CALL_EVIDENCE_FLOW.md` |

---

## 9. Tests and drift gates required before implementation

### Platform (`PLAT-AGUI-000` — blocking for downstream)

- [ ] `validate-agent-interaction-spine.ps1`: schema JSON valid; matrix `schema` + families; fixture JSONL lines validate against event schema (structural validator — follow evidence-spine script style if no ajv in repo)
- [ ] `SystemAgentInteractionSpineContractTests.cs`: artifact existence + matrix required families
- [ ] `system-protocol-registry.json` entry with validators list
- [ ] Contract cross-links: `SYSTEM_EVIDENCE_SPINE_CONTRACT.md`, `TRACE_CORRELATION_CONTRACT.md`, sibling handoff docs
- [ ] Redaction / hidden-reasoning / no-raw-prompt rules explicit (acceptance checklist §Platform)

### Frontend (before OFE-AGUI-002 merge)

- [ ] `interactionReducer.test.ts` — deterministic replay of both shipped fixtures
- [ ] `jsonlFixtureAdapter.test.ts` — parse + schema validation hook (optional: copy minimal validator)
- [ ] `evidenceSpineInteractionAdapter.test.ts` — does not duplicate HTTP calls already made by resolver
- [ ] Playwright smoke: `/system/agent-interaction?fixture=sample-run` renders timeline

### ontogony-ui

- [ ] `check:exports` includes `./agent`
- [ ] Component unit tests + Storybook for interrupt approval a11y
- [ ] `check:smoke-named-exports` for new subpath

### Backend projection PRs

- [ ] JSONL output validated against platform schema (CI step or test harness reading schema from platform path / submodule)
- [ ] No new forbidden dependencies (Allagma/Kanon/Conexus AGENTS.md)

### Explicit non-gates for baseline

- Production SSE/WebSocket hardening
- External AG-UI client interoperability
- CopilotKit integration

---

## 10. Go / no-go for `PLAT-AGUI-000`

| Criterion | Status |
|-----------|--------|
| Evidence Spine baseline shipped | **Yes** — [`CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md`](../releases/CROSS_SERVICE_EVIDENCE_SPINE_CLOSEOUT.md), resolver tests |
| Allagma run surface matches package | **Yes** — feature connection matrix |
| Kanon human-gate / decision handoff | **Yes** — `KANON_EVIDENCE_SPINE_HANDOFF.md` |
| Conexus model-call evidence flow | **Yes** — `MODEL_CALL_EVIDENCE_FLOW.md` |
| Frontend correlation + resolver | **Yes** |
| Package schemas internally consistent | **Yes** — fixtures validate against enum/source rules |
| Competing “Agentor” runtime | **No** — Allagma is canonical |
| Blocking gaps | **None** for docs-only PLAT-AGUI-000 |

### Recommendation

**GO** — Start **`PLAT-AGUI-000`** as the first implementation slice.

Conditions:

1. Publish schemas and contract as **additive v0** alongside Evidence Spine v1 bundle schema.
2. Register validators in `system-protocol-registry.json`.
3. Do **not** implement runtime services, UI, or streaming in the same PR.
4. Add the two shipped JSONL fixtures under `docs/schemas/fixtures/agent-interaction/`; track missing fixture categories as follow-up in `PLAT-AGUI-000` evidence doc.

**NO-GO (for now)** on **`ALLAGMA-AGUI-002`**, **`ADAPTER-AGUI-001`**, and full **`OFE-AGUI-003`** until client-side replay + adapters demonstrate stable event shapes on real run data.

---

## 11. Recommended first implementation slice (after intake)

**`PLAT-AGUI-000`** in `ontogony-platform` only:

- Copy/adapt contract + schemas + matrix per §8.
- Implement validation script + xUnit gate mirroring SYS-TIGHT-002 evidence spine pattern.
- Evidence doc with command output and explicit relationship to Evidence Spine.
- Optional: add `docs/migrations/2026-05-22-agent-interaction-spine-v0.md` changelog pointer for consumers.

**Second slice (parallel once PLAT merges):** `OUI-AGENT-001` + `OFE-AGUI-001` — fixture replay workbench proves reducer and UI primitives without backend changes.

**Third slice:** `OFE-AGUI-002` — wire existing APIs and **`buildCrossServiceTimelineEvents`** / **`resolveEvidenceSpine`**; treat as the highest-value operator increment.

---

## 12. Intake archive location

Keep the unpacked package under:

`docs/_incoming/ontogony_ag_ui_interaction_spine_package_2026-05-22/`

Do not delete automatically. Implementation PRs should cite intake paths until artifacts are promoted into canonical `docs/` locations.

---

*End of intake review. Ready to commit to `ontogony-platform` on request.*
