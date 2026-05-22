# SYSTEM-RC-001 Backend Certification Package — intake review

**Package zip:** `docs/_incoming/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22_v2_AGUI.zip`  
**Unpacked to:** `docs/_incoming/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22_v2_AGUI/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22/`  
**Package id:** `Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22` (`v2-agui`)  
**Review date:** 2026-05-22  
**Intake verdict:** **GO for staged certification execution** — treat as a **certification package**, not a feature package. **Do not implement in this intake step.** **Do not change runtime lock SHAs.** **Do not enable real external tool execution.** **Do not claim production readiness.**

---

## Executive classification

| Question | Answer |
| --- | --- |
| Is this a feature package? | **No** — it certifies existing alpha backend behavior at pinned commits. |
| Is this frontend AG-UI work? | **No** — `SYSTEM-RC-001F` certifies **backend** Agent Interaction Spine surfaces (JSONL export, SSE stream, schema, redaction, evidence links). Frontend AG-UI is out of scope for this package. |
| What is SYSTEM-RC-001F? | **Agent Interaction / AG-UI spine certification** — backend contract + golden artifacts after evidence-spine golden run. |
| First executable PR? | **`SYSTEM-RC-001A` runtime lock promotion** in `allagma-dotnet` — **only after** reviewing current `SYSTEM-ALPHA-006` lock, post-lock deltas, and moving-main drift. |
| Current runtime baseline | **`SYSTEM-ALPHA-006`** ([`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json)) — unchanged by this intake. |

---

## 1. Package purpose

Move the four backend repos from strong alpha (`8.x` self-assessment) toward **certified alpha-RC (`9+`)** through machine-gated validation, not new product semantics:

1. **Pin** latest selected backend heads (runtime lock promotion).
2. **Prove** sibling-source cohesion, package-mode builds, observability PASS, and a golden evidence-spine journey.
3. **Hard-certify** Conexus gateway, Kanon semantic authority, and Platform substrate contracts.
4. **Certify** backend Agent Interaction Spine (AG-UI-compatible export/stream) as **`SYSTEM-RC-001F`**.
5. **Close out** with `validate-system-tight-rc-{prep,readiness,evidence}.ps1` (Allagma) and indexed release evidence.

Guiding principle from package README: *prove the current system is pinned, buildable, integrated, restart-safe, observable, evidence-resolvable, and explicit about non-goals.*

**Boundary preservation (frozen):**

| Repo | Owns |
| --- | --- |
| `allagma-dotnet` | Governed runtime, orchestration, **runtime lock**, cohesion/evidence artifacts |
| `kanon-dotnet` | Semantic authority (ontology, facts, policy, human gates, provenance) |
| `conexus-dotnet` | Model gateway (routing, fallback, quota, streaming, model-call evidence) |
| `ontogony-platform` | Shared mechanics, protocol registry, agent-interaction **schema** (not product semantics) |

---

## 2. File inventory (29 files)

Nested under `Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22/`:

| Path | Role |
| --- | --- |
| `00_README_EXECUTION_ORDER.md` | Package overview, repo roles, execution order, AG-UI v2 addendum |
| `01_CURRENT_STATE_AND_SCORE_TARGETS.md` | Score targets (`8.x` → `9+`), what certification means |
| `02_PR_SEQUENCE.md` | Cross-repo PR order including **SYSTEM-RC-001F** placement |
| `metadata/package-manifest.json` | Machine manifest (`v2-agui`, file list, non-goals) |
| **Contracts (8)** | Certification matrices and spine contracts (copy/adapt into each repo when executing) |
| `contracts/AGENT_INTERACTION_AGUI_SPINE_CONTRACT.md` | Backend AG-UI spine contract (Platform schema + Allagma export/stream + Kanon/Conexus projections) |
| `contracts/AGUI_BACKEND_OPERATOR_SURFACE_MATRIX.md` | Operator/backend surface matrix for AG-UI certification |
| `contracts/CONEXUS_GATEWAY_CERTIFICATION_MATRIX.md` | Gateway certification rows |
| `contracts/EVIDENCE_SPINE_GOLDEN_RUN_CONTRACT.md` | Golden evidence journey contract |
| `contracts/OBSERVABILITY_PASS_CONTRACT.md` | Observability PASS artifact contract |
| `contracts/PLATFORM_SUBSTRATE_RC_CONTRACT.md` | Platform alpha-RC substrate freeze |
| `contracts/RUNTIME_ORCHESTRATION_CORRECTNESS_MATRIX.md` | Allagma orchestration correctness rows |
| `contracts/SEMANTIC_AUTHORITY_CERTIFICATION_MATRIX.md` | Kanon semantic authority rows |
| **PR specs (9)** | Executable scope per PR |
| `pr-specs/SYSTEM-RC-001A-runtime-lock-promotion.md` | Lock promotion only — no features |
| `pr-specs/SYSTEM-RC-001B-full-cohesion-evidence.md` | Sibling-source cohesion evidence |
| `pr-specs/SYSTEM-RC-001C-package-mode-certification.md` | Package-mode build/test evidence |
| `pr-specs/SYSTEM-RC-001D-observability-pass.md` | Live observability PASS artifacts |
| `pr-specs/SYSTEM-RC-001E-evidence-spine-golden-run.md` | Golden evidence spine (Platform + Allagma) |
| `pr-specs/SYSTEM-RC-001F-agent-interaction-agui-spine-certification.md` | **Backend AG-UI spine certification** |
| `pr-specs/CONEXUS-RC-001-gateway-certification-matrix.md` | Conexus gateway matrix + evidence |
| `pr-specs/KANON-RC-001-semantic-authority-certification.md` | Kanon semantic matrix + evidence |
| `pr-specs/PLATFORM-RC-001-substrate-contract-freeze.md` | Platform RC contract + registry refresh |
| **Validation (4)** | Commands and gates |
| `validation/REQUIRED_COMMANDS.md` | All-repo certification commands |
| `validation/AGUI_REQUIRED_COMMANDS.md` | SYSTEM-RC-001F-specific commands |
| `validation/ACCEPTANCE_CHECKLIST.md` | Full package acceptance (+ AG-UI section) |
| `validation/NON_GOALS_AND_HARD_BLOCKS.md` | Hard blocks and AG-UI non-goals |
| **Templates (4)** | Closeout doc patterns |
| `templates/RELEASE_EVIDENCE_CLOSEOUT_TEMPLATE.md` | Release evidence closeout |
| `templates/AGUI_EVIDENCE_CLOSEOUT_TEMPLATE.md` | AG-UI certification closeout |
| `templates/ARTIFACT_INDEX_TEMPLATE.md` | Artifact index |
| `templates/STALE_DOC_CLEANUP_TEMPLATE.md` | Stale doc cleanup |

**Manifest gap:** `metadata/package-manifest.json` `pr_sequence` omits `SYSTEM-RC-001F`; `02_PR_SEQUENCE.md` and `00_README_EXECUTION_ORDER.md` include it. **Prefer `02_PR_SEQUENCE.md` as authoritative** when executing.

---

## 3. Proposed PR sequence (with owning repo)

| Order | PR id | Owner repo | Depends on | Purpose (summary) |
| ---: | --- | --- | --- | --- |
| 1 | **SYSTEM-RC-001A** | `allagma-dotnet` | — | Runtime lock promotion; post-lock deltas; **no features** |
| 2 | **SYSTEM-RC-001B** | `allagma-dotnet` | 001A | Full sibling-source cohesion evidence |
| 3 | **SYSTEM-RC-001C** | `allagma-dotnet` (+ upstream packs) | 001A | Package-mode certification |
| 4 | **SYSTEM-RC-001D** | `allagma-dotnet` | 001A, prefer 001B | Observability PASS (`observability-summary.json`) |
| 5 | **SYSTEM-RC-001E** | `ontogony-platform` + `allagma-dotnet` | 001B | Evidence spine golden run + redaction |
| 6 | **CONEXUS-RC-001** | `conexus-dotnet` | 001A (verify with 001B) | Gateway certification matrix |
| 7 | **KANON-RC-001** | `kanon-dotnet` | 001A (verify with 001B) | Semantic authority certification matrix |
| 8 | **PLATFORM-RC-001** | `ontogony-platform` | 001–007 | Substrate contract freeze + protocol registry refresh |
| 9 | **SYSTEM-RC-001F** | `allagma-dotnet` (Platform/Conexus/Kanon participation) | **001E** | **Backend AG-UI / Agent Interaction Spine certification** |
| — | Final closeout | `allagma-dotnet` (+ indexes) | All above | `validate-system-tight-rc-*.ps1` + `SYSTEM_RC_001_CLOSEOUT` evidence |

**Hard rule from package:** Do not merge a later PR if an earlier RC gate is red (unless the later PR explicitly fixes the failing gate).

**First executable PR recommendation:** Start with **SYSTEM-RC-001A** only after:

- Reading [`ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json) and [`post-lock-deltas.json`](../../../allagma-dotnet/docs/system/post-lock-deltas.json).
- Running `validate-runtime-lock.ps1` and `-ReleaseMode` on moving `main` heads.
- Classifying drift in post-lock delta register (Platform: [`post-lock-delta-register.json`](../system/post-lock-delta-register.json)).

Do **not** promote the lock in the intake step.

---

## 4. What applies directly to `ontogony-platform`

| Package artifact | Platform action (when executing, not now) |
| --- | --- |
| `SYSTEM-RC-001E` | Golden evidence spine artifacts; may extend existing evidence-spine validators/fixtures |
| `PLATFORM-RC-001` | Publish `ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md`; refresh [`system-protocol-registry.json`](../system/system-protocol-registry.json); evidence `PLATFORM_RC_001_*` |
| `SYSTEM-RC-001F` (Platform slice) | Run `validate-agent-interaction-spine.ps1`; golden AG-UI fixtures under `artifacts/agui/` — **schema/matrix authority only** |
| Contracts in `contracts/` | Copy or merge into `docs/operators/` / `docs/system/` as certification matrices — adapt to live paths (see reconciliation below) |

**Already landed (do not re-implement blindly):**

- Agent Interaction Spine baseline: [`AGENT_INTERACTION_SPINE_CONTRACT.md`](../operators/AGENT_INTERACTION_SPINE_CONTRACT.md), schemas, matrix, [`validate-agent-interaction-spine.ps1`](../../scripts/validate-agent-interaction-spine.ps1).
- Closeout: [`AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md`](../evidence/AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md) (local implementation accepted; **not** same as SYSTEM-RC-001F machine certification).
- Prior intake: [`ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md`](./ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md).

**SYSTEM-RC-001F vs prior AG-UI package:** Prior package (`ontogony_ag_ui_interaction_spine_package_2026-05-22`) drove **implementation** (PLAT-AGUI-000 … ADAPTER-AGUI-001). **SYSTEM-RC-001F** drives **backend certification evidence** (golden JSONL, SSE transcript, redaction report, resume report) at lock-pinned commits — complementary, not a duplicate feature sprint.

---

## 5. Instructions for sibling repos (not executed here)

### `allagma-dotnet`

- **001A–001D, 001B, 001C:** Lock, cohesion, package-mode, observability — primary owner.
- **001E:** Co-run golden evidence journey; produce `artifacts/evidence-spine/`.
- **001F:** Own `GET /allagma/v0/runs/{runId}/interaction-events` (JSONL) and `.../interaction-events/stream` (SSE); golden `artifacts/agui/`; tests `AgentRunInteractionEvent*`, `AgentRunInteractionStream*`.
- Final gates: `scripts/validate-system-tight-rc-{prep,readiness,evidence}.ps1` (exist today under SYS-TIGHT-007 lineage).
- **Non-goals:** No real external tool execution; no feature work in lock PR.

### `kanon-dotnet`

- **KANON-RC-001:** Semantic authority certification matrix, manifest gates, v0 freeze, negative paths, Postgres smoke.
- **001F participation:** Human-gate / review-queue / assistance-review IDs mappable to interaction interrupt events; tests per `AGUI_REQUIRED_COMMANDS.md`.
- **Boundary:** No model authority leak; assistance remains non-authoritative.

### `conexus-dotnet`

- **CONEXUS-RC-001:** Gateway matrix (fallback, quota, idempotency, streaming, evidence bundle, usage drilldown, auth isolation).
- **001F participation:** Model-call interaction projector; evidence-bundle links; no raw prompt/completion in projected events.

### `ontogony-frontend` (optional per package, not RC-blocking)

- `validation/REQUIRED_COMMANDS.md` lists route parity and docker-live e2e — **recommended**, not part of SYSTEM-RC-001 backend certification scope.

---

## 6. SYSTEM-RC-001F — AG-UI / Agent Interaction Spine certification scope

**Label:** `SYSTEM-RC-001F — Agent Interaction / AG-UI spine certification`  
**Placement:** After `SYSTEM-RC-001E`, before final SYSTEM-RC-001 closeout.

**Definition in package:** AG-UI means the **backend** event/export/streaming contract that makes a governed run renderable and replayable by an agent-interaction UI — **not** a frontend component sprint.

**Must produce (per pr-spec):**

```text
artifacts/agui/<timestamp>/golden-run-interaction-events.jsonl
artifacts/agui/<timestamp>/golden-run-interaction-stream-transcript.txt
artifacts/agui/<timestamp>/agui-redaction-report.json
artifacts/agui/<timestamp>/agui-resume-report.json
docs/evidence/SYSTEM_RC_001F_AGUI_SPINE_CERTIFICATION_EVIDENCE.md
```

**Must validate:**

- Platform: `ontogony-agent-interaction-event-v0` schema + matrix (`validate-agent-interaction-spine.ps1`).
- Allagma: JSONL export determinism; SSE stream; `Last-Event-ID` resume without duplicates.
- Conexus: model-call lifecycle projection with evidence-bundle links.
- Kanon: human-gate interrupt/resume/deny representable in event phases.
- Redaction: no raw prompts, completions, secrets, provider keys.

**Relationship to Evidence Spine:** AG-UI events **link** to evidence spine / audit / Conexus / Kanon objects by ID — do **not** duplicate the evidence graph as a second AG-UI graph.

---

## 7. Hard non-goals and hard blocks

From `validation/NON_GOALS_AND_HARD_BLOCKS.md` (package-wide + AG-UI):

```text
production readiness
enterprise IAM runtime
real external tool execution
full cloud deployment
full provider parity
frontend redesign (as RC deliverable)
new product semantics
new ontology v1
new Conexus provider routing model
new Platform product abstractions
```

**AG-UI-specific:**

- Not a frontend AG-UI implementation sprint.
- No duplicate Evidence Spine graph for AG-UI.
- No raw prompts/completions/secrets in interaction events.
- No Kanon/Conexus public error contract changes for AG-UI convenience.
- No AG-UI certification claim without JSONL + SSE + schema + redaction + evidence links proven.

**Hard blocks:**

- Real external tool execution remains blocked.
- Runtime lock updates only in dedicated lock-promotion PRs (not feature PRs).
- No stale PASS claims without artifact links.
- Platform remains mechanics-only.
- Allagma owns model purpose; Conexus owns routing; Kanon owns semantic truth.

---

## 8. Reconciliation with current repo state (read before 001A)

| Package assumption | Current truth | Intake guidance |
| --- | --- | --- |
| Greenfield AG-UI spine | **PLAT-AGUI-000 closed locally** | 001F = **certify at lock**, not re-build spine |
| `validate-agent-interaction-spine.ps1` | **Exists** | Use as 001F Platform gate |
| Runtime lock at latest `main` | **`SYSTEM-ALPHA-006` pins** | 001A must classify deltas first |
| `validate-system-tight-rc-*.ps1` | **Exist in Allagma** | Final closeout gates ready |
| Observability PASS | May be pending on moving-main | 001D must produce artifact-linked PASS |
| `package-manifest.json` pr_sequence | **Missing 001F** | Use `02_PR_SEQUENCE.md` |
| Stale incoming specs | Multiple packages under `docs/_incoming/` | Run [`validate-stale-incoming-package.ps1`](../../scripts/validate-stale-incoming-package.ps1) before adopting commands verbatim |

**Do not execute stale incoming package specs blindly** — cross-check against [`docs/CURRENT_IMPLEMENTATION_STATUS.md`](../CURRENT_IMPLEMENTATION_STATUS.md), Allagma lock evidence, and [`AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md`](../evidence/AGUI_SPINE_CLOSEOUT_001_EVIDENCE.md).

---

## 9. Intake step acceptance (this PR)

| Criterion | Status |
| --- | --- |
| Zip unpacked under `docs/_incoming/..._v2_AGUI/` | Done |
| No source/runtime behavior changes | Done |
| Intake review states certification (not feature) package | This document |
| SYSTEM-RC-001F called out explicitly | §6 |
| First executable PR = 001A after repo state review | §3, §8 |
| Runtime lock SHAs unchanged | Done |
| Real tool execution not enabled | Done |
| Production readiness not claimed | Done |

---

## 10. Related indexes

| Doc | Link |
| --- | --- |
| AG-UI interaction spine intake (implementation package) | [`ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md`](./ONTOGONY_AG_UI_INTERACTION_SPINE_PACKAGE_INTAKE_REVIEW.md) |
| Agent Interaction Spine contract (live) | [`../operators/AGENT_INTERACTION_SPINE_CONTRACT.md`](../operators/AGENT_INTERACTION_SPINE_CONTRACT.md) |
| Runtime lock (authoritative) | [`../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json) |
| Allagma release evidence index | [`../../../allagma-dotnet/docs/releases/RELEASE_EVIDENCE_INDEX.md`](../../../allagma-dotnet/docs/releases/RELEASE_EVIDENCE_INDEX.md) |
| Unpacked package root | [`../_incoming/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22_v2_AGUI/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22/`](../_incoming/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22_v2_AGUI/Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22/00_README_EXECUTION_ORDER.md) |
