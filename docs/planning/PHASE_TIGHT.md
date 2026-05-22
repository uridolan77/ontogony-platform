To push these repos **above 9**, the work is not “add more features.” It is **turn strong local-alpha repos into hard-gated, reproducible, operator-grade systems**.

> **Closeout index (done PRs + evidence):** [`PHASE_TIGHT_CLOSEOUT_2026-05-22.md`](./PHASE_TIGHT_CLOSEOUT_2026-05-22.md)

The current scores are high because the architecture is real. They are not above 9 because too much proof is still distributed across docs, local scripts, PR evidence, and manual discipline.

I would set the current scores as:

| Repo                | Score (2026-05-22) | Notes |
| ------------------- | -----------------: | --- |
| `ontogony-platform` |           **9.12** | PLATFORM-9-001/002/003 done; consumers adopt PROP tests |
| `allagma-dotnet`    |           **9.15** | ALLAGMA-9-001–004 + PROP-001 done; full cohesion PASS on Docker stack (streaming); restart sibling gate |
| `conexus-dotnet`    |           **8.95** | CONEXUS-PROP-001 done; alias manifest open |
| `kanon-dotnet`      |           **8.85** | KANON-PROP-001 done; lifecycle/replay gates open |
| `ontogony-frontend` |           **8.50** | Runtime posture panel v2 |
| `ontogony-ui`       |           **8.05** | Unchanged this slice |

**Completed PR index:** [`PHASE_TIGHT_CLOSEOUT_2026-05-22.md`](./PHASE_TIGHT_CLOSEOUT_2026-05-22.md).

Allagma deserves a score near **9.0+**, not lower, because the old hard-coded-model criticism is now fixed: it resolves model purpose to `ConexusModelAlias`, it already has a streaming path, and it now has a canonical system acceptance command plus propagation conformance tests.

---

# What “above 9” means

A repo goes above 9 only when it has:

```text
1. Clear boundary ownership
2. Runtime implementation
3. Contract snapshots / manifests
4. CI gates that fail on drift
5. Evidence artifacts
6. Local + durable mode proof
7. Cross-repo compatibility proof
8. Operator docs
9. Minimal stale planning residue
10. Known limitations are explicit and not confused with current state
```

Right now you have **1–8 mostly done** on the backend spine (platform gate, envelopes, propagation, Allagma acceptance, sibling PROP tests). Remaining for **9.2+**: **9–10** (operator docs freshness, explicit limitations) plus domain gates (Allagma evidence graph, Conexus alias manifest, Kanon lifecycle/replay matrices).

---

# 1. `ontogony-platform`: 9.12 (PLATFORM-9-001/002/003 done)

## Status (2026-05-22)

Platform is now the **hard compatibility spine**: mechanical gate (9-001), error envelope enforcement (9-002), frozen propagation contract + reusable conformance helpers (9-003). Score **above 9**.

Remaining gap is not platform code — it is **consumer repos** completing their domain-specific gates (Conexus alias manifest, Kanon lifecycle/replay matrices, Allagma evidence graph).

## Required moves

### PLATFORM-9-001 — System compatibility gate package ✅

Add a Platform-owned compatibility validator package:

```text
src/Ontogony.SystemCompatibility/
tests/Ontogony.SystemCompatibility.Tests/
docs/contracts/SYSTEM_COMPATIBILITY_GATE.md
```

It should read:

```text
Kanon compatibility manifest
Conexus OpenAPI snapshots
Allagma feature connection matrix
Frontend route/client matrix
Platform package versions
Expected headers/auth/env vars
```

Output:

```text
artifacts/system-compat/system-compatibility-summary.json
artifacts/system-compat/system-compatibility-summary.md
```

This would move Platform from “shared mechanics” to **shared mechanical enforcement**.

### PLATFORM-9-002 — Cross-service error envelope conformance ✅

Platform already owns error infrastructure. Now enforce it.

Create tests or analyzers that verify each backend uses the same envelope shape:

```json
{
  "code": "...",
  "message": "...",
  "system": "...",
  "stage": "...",
  "traceId": "...",
  "correlationId": "...",
  "retryable": true,
  "downstreamSystem": "..."
}
```

Score impact: high. This removes the current mixed v0 error-shape weakness.

### PLATFORM-9-003 — Header propagation contract ✅

Freeze and validate the headers:

```text
traceparent
X-Correlation-ID
X-Ontogony-Actor-Id
X-Ontogony-Actor-Type
X-Ontogony-Actor-Roles
X-Ontogony-Idempotency-Key
X-Allagma-Run-Id
```

Add reusable test helpers so Conexus, Kanon, and Allagma can prove propagation.

**Consumer adoption (done):** ALLAGMA-PROP-001, KANON-PROP-001, CONEXUS-PROP-001 — see closeout doc.

## Above-9 condition

**Met (9.12).** Platform provides the validator; all three backend repos have thin propagation conformance tests. Further lift to **9.2+** is documentation of consumer CI wiring, not new platform mechanics.

---

# 2. `conexus-dotnet`: 8.92 (CONEXUS-PROP-001 done; 9-001/002 open)

## Why it is close already

Conexus is the closest to 9. It has health/readiness docs, provider capability matrix, evidence spine, SLO starter, streaming usage/cost, contract/release discipline, capacity baseline, admin auth model, and known limitations. 

Its capacity baseline is also real: fake non-streaming, fake streaming, fallback, provider error storm, idempotency conflict under load, and optional Postgres scenarios are documented. 

The remaining gap is production-adjacent proof and cross-repo hardening.

## Required moves

### CONEXUS-PROP-001 — Outbound propagation conformance ✅

`tests/Conexus.Providers.OpenAI.Tests/ConexusOutboundPropagationConformanceTests.cs` — provider `AddOntogonyIntegrationHttpClient` path; documents ingress vs outbound split in `docs/architecture/BOUNDARIES.md`.

### CONEXUS-9-001 — Make model aliases system-contract objects

Conexus should publish a machine-readable alias contract:

```text
docs/generated/CONEXUS_MODEL_ALIAS_MANIFEST.json
docs/contracts/CONEXUS_MODEL_ALIAS_CONTRACT.md
```

For each alias:

```json
{
  "alias": "risk-summary-v0",
  "capabilities": ["chat", "json-mode"],
  "streamingAllowed": false,
  "toolCallsAllowed": false,
  "defaultPurpose": "summarize-player-risk",
  "fallbackPolicy": "...",
  "evidenceFields": ["modelCallId", "routeDecisionId", "usage"]
}
```

Allagma should validate against this before calling Conexus.

### CONEXUS-9-002 — Streaming evidence acceptance

Conexus already has streaming and streaming evidence docs. To go above 9, create an acceptance suite that proves:

```text
stream request accepted
first byte / first chunk recorded
chunk lifecycle recorded
stream completion recorded
usage/cost honesty preserved
evidence bundle includes streaming metadata
Allagma can correlate the stream to a run
```

### CONEXUS-9-003 — Durable idempotency / replay proof

If any idempotency path is still memory-only or local-only, make the durable limitation explicit and add Postgres proof for the important paths.

Above 9 requires:

```text
same idempotency key + same payload = replay
same key + different payload = conflict
restart does not duplicate durable side effects
conflict shape is stable
```

### CONEXUS-9-004 — Real-provider local acceptance pack

Not production. But real-provider acceptance should be formal:

```text
fake provider default
real provider opt-in
secret budget guard
no raw secrets in reports
provider smoke categories excluded by default
operator acceptance checklist
```

You already have parts of this; package it as a single gate.

## Above-9 condition

Conexus reaches **9.2** when model aliases, streaming evidence, idempotency, and provider acceptance are all machine-validated and consumed by Allagma.

---

# 3. `kanon-dotnet`: 8.85 (KANON-PROP-001 done; 9-001/002 open)

## Why it is not above 9 yet

Kanon’s boundary is excellent. It owns ontology, canonical facts, semantic plans, action policies, human gates, decision provenance, and domain packs; it explicitly does not own LLM routing, workflow orchestration, or provider execution. 

The gap is not conceptual. It is **semantic lifecycle enforcement**.

## Required moves

### KANON-PROP-001 — Conexus assistance outbound propagation ✅

`tests/Kanon.Tests/KanonConexusAssistancePropagationConformanceTests.cs` — trace, correlation, actor, idempotency; explicitly **no** `X-Allagma-Run-Id` on Kanon → Conexus.

### KANON-9-001 — Domain pack lifecycle hardening

Introduce explicit lifecycle states:

```text
draft
validated
reviewed
accepted
active
deprecated
archived
```

And require state transitions to produce decision records.

This is the biggest Kanon move toward 9+ because Kanon is the meaning authority. Meaning must have lifecycle governance.

### KANON-9-002 — Semantic decision replay acceptance ✅

Prove that every important decision can be replayed:

```text
semantic plan compile
canonical fact resolution
action policy evaluation
human gate check
domain pack activation
contradiction resolution
```

For each decision:

```text
input fingerprint
ontology version
domain pack version
source binding version
policy version
decision ID
provenance
replay bundle
```

**Implementation:** `tests/Kanon.Tests/KanonSemanticDecisionReplayAcceptanceTests.cs`; `scripts/run-semantic-decision-replay-acceptance.ps1`; `docs/e2e/KANON_SEMANTIC_DECISION_REPLAY_ACCEPTANCE.md`. See [`kanon-dotnet/docs/evidence/KANON_9_002_SEMANTIC_DECISION_REPLAY_ACCEPTANCE_EVIDENCE.md`](../../kanon-dotnet/docs/evidence/KANON_9_002_SEMANTIC_DECISION_REPLAY_ACCEPTANCE_EVIDENCE.md).

**Operator verified (2026-05-22):** category tests **6/6 PASS**; full filter **9/9 PASS**; summary artifact `artifacts/kanon-semantic-decision-replay-acceptance/20260522T192918Z/summary.json`.

### KANON-9-003 — Error envelope normalization

If any v0 routes still return local `{ error = ... }`, bare `NotFound`, or endpoint-specific shapes, normalize them or explicitly freeze them as transitional.

Above 9 requires clients to trust failure shapes.

### KANON-9-004 — Conexus assistance full E2E — partial ✅

Kanon’s Conexus assistance boundary is good; **included in system acceptance** via ALLAGMA-9-001 / `kanon_conexus_assistance` cohesion scenario. Remaining: dedicated Kanon-only acceptance matrix if desired.

```text
Kanon assistance request
field allowlist
role allowlist
redaction
Conexus model call
draft_only result
decision record
provenance lookup
```

This proves Conexus can assist without becoming semantic authority.

## Above-9 condition

Kanon reaches **9.1–9.2** when semantic lifecycle, replay, policy decision provenance, and assistance boundaries are all enforced by tests and manifests, not just docs.

---

# 4. `allagma-dotnet`: 9.15 (ALLAGMA-9-001–004 + PROP-001 done)

## Why it is close

Allagma is the integration center: run lifecycle, resume/retry/cancel/replay, evaluations, operator runtime posture v2, Kanon/Conexus dependencies, Platform packages, and canonical acceptance. Model purpose routing (`Allagma:ModelPurposes:[purpose]:ConexusModelAlias`), streaming, and trust gates are exercised by `run-system-cohesion-acceptance.ps1` on the Docker `local-working-system` stack.

Remaining lift to **9.2+**: cohesion `restart_survival` PASS in-summary (or lock-linked compose restart artifact refresh) without port conflicts.

## Required moves

### ALLAGMA-9-001 — Single system acceptance command ✅

Add one canonical command:

```powershell
.\scripts\system\run-system-cohesion-acceptance.ps1
```

It should start or verify:

```text
Platform packages
Kanon
Conexus
Allagma
optional frontend
Postgres durable mode
```

And prove:

```text
start run
Kanon semantic plan
Kanon action evaluation
Conexus model call
run completed
events emitted
audit bundle exported
evidence graph resolvable
idempotent retry safe
restart durable
human gate allow/deny path
streaming path if enabled
```

Output:

```text
artifacts/system-cohesion/summary.json
artifacts/system-cohesion/summary.md
```

**Evidence (2026-05-22):** local acceptance PASS — `artifacts/system-cohesion/summary.json`; full cohesion `run-20260522T185400Z` on Docker compose (`-UseExistingServices`; streaming PASS; `restart_survival` DEFERRED). See [`PHASE_TIGHT_CLOSEOUT_2026-05-22.md`](./PHASE_TIGHT_CLOSEOUT_2026-05-22.md) and [`allagma-dotnet/docs/evidence/ALLAGMA_COH_FULL_ACCEPTANCE_2026-05-22_EVIDENCE.md`](../../allagma-dotnet/docs/evidence/ALLAGMA_COH_FULL_ACCEPTANCE_2026-05-22_EVIDENCE.md).

### ALLAGMA-PROP-001 — Outbound propagation conformance ✅

`tests/Allagma.Tests/AllagmaOutboundPropagationConformanceTests.cs` — proves Allagma → Kanon (full frozen set) and Allagma → Conexus (trace/correlation/idempotency/run-id; no actor per privacy rule) via `Ontogony.Testing.HeaderPropagationConformanceAssertions`.

### ALLAGMA-9-002 — Evidence graph acceptance ✅

Allagma proves a run can be traversed as a graph (executable):

```text
Allagma run
→ Kanon plan decision
→ Kanon action/gate decisions
→ Conexus model call
→ Conexus route decision
→ Allagma audit bundle
→ evaluation/baseline if present
→ frontend evidence links
```

**Implementation:** `scripts/lib/evidence-graph-acceptance.ps1`; cohesion scenario `evidence_graph_acceptance`; `scripts/system/run-evidence-graph-acceptance.ps1`; artifact schema `allagma-evidence-graph-acceptance-v1`. See [`allagma-dotnet/docs/e2e/EVIDENCE_GRAPH_ACCEPTANCE.md`](../../allagma-dotnet/docs/e2e/EVIDENCE_GRAPH_ACCEPTANCE.md).

### ALLAGMA-9-003 — Real execution trust model, no real execution yet ✅

Allagma currently keeps real execution disabled, which is correct.  To go above 9, do not enable it yet. Instead, formalize the trust model:

```text
deny by default
capability declaration
human gate requirement
dry-run / execute split
side-effect ledger
network/filesystem allowlists
per-tool secret scope
timeout/cancellation
kill switch
replay no-reexecution rule
audit export
```

Then add tests proving real execution cannot be enabled accidentally.

**Implementation:** `tests/Allagma.Tests/Allagma9003RealExecutionTrustModelTests.cs`; `scripts/system/run-real-execution-trust-acceptance.ps1`; phase `real_execution_trust_acceptance` in ALLAGMA-9-001. See [`allagma-dotnet/docs/e2e/REAL_EXECUTION_TRUST_ACCEPTANCE.md`](../../allagma-dotnet/docs/e2e/REAL_EXECUTION_TRUST_ACCEPTANCE.md).

### ALLAGMA-9-004 — Runtime posture must become operator-grade ✅

The runtime posture endpoint exists.  Expand it into an operator-grade system posture:

```text
Kanon reachable
Conexus reachable
model aliases configured
streaming purposes configured
persistence mode
Postgres migration state
real execution disabled/enabled
manual evaluation enabled/disabled
known degraded dependencies
last compatibility check result
```

**Implementation:** `GetAllagmaRuntimePostureService.GetAsync` (schema v2); downstream `/health` probes; migration reporter; compatibility snapshot reader. See [`allagma-dotnet/docs/e2e/RUNTIME_POSTURE_ACCEPTANCE.md`](../../allagma-dotnet/docs/e2e/RUNTIME_POSTURE_ACCEPTANCE.md).

## Above-9 condition

Allagma reaches **9.2** when it becomes the one-command proof that the system is coherent.

---

# The actual 9+ roadmap

## Sprint A — Hard gates, not features

```text
SYSTEM-9A-001 Platform compatibility validator          ✅ PLATFORM-9-001
SYSTEM-9A-002 Allagma system acceptance command         ✅ ALLAGMA-9-001
SYSTEM-9A-003 Conexus model alias manifest            ⬜ CONEXUS-9-001
SYSTEM-9A-004 Kanon domain pack lifecycle manifest      ⬜ KANON-9-001
SYSTEM-9A-005 Frontend/backend route-client drift gate ✅
```

Actual (2026-05-22 closeout):

```text
Platform: 8.5 → 9.12
Conexus: 8.8 → 8.92
Kanon: 8.6 → 8.85
Allagma: 8.7 → 9.15
```

## Sprint B — Evidence and replay

```text
SYSTEM-9B-001 End-to-end evidence graph acceptance          ✅ ALLAGMA-9-002
SYSTEM-9B-002 Kanon semantic decision replay bundle acceptance ✅ KANON-9-002
SYSTEM-9B-003 Conexus streaming evidence acceptance         ⬜ CONEXUS-9-002 (streaming PASS in ALLAGMA cohesion; formal Conexus pack open)
SYSTEM-9B-004 Allagma restart/idempotency/human-gate acceptance ⬜ (human-gate + streaming PASS; restart DEFERRED in cohesion / compose sibling gate)
SYSTEM-9B-005 Frontend evidence journey E2E from live artifacts ✅
```

Actual (partial Sprint B):

```text
Platform: 9.12
Conexus: 8.92
Kanon: 8.85
Allagma: 9.15
Frontend: 8.50
```

## Sprint C — Operator-grade polish

```text
SYSTEM-9C-001 Runtime posture dashboard                    ⬜
SYSTEM-9C-002 Shared error envelope conformance          ✅ PLATFORM-9-002
SYSTEM-9C-003 Header propagation contract                ✅ PLATFORM-9-003
SYSTEM-9C-004 Real execution trust model, still disabled   ✅ ALLAGMA-9-003
SYSTEM-9C-005 UI status taxonomy + EvidenceExportPanel     ⬜
```

Actual (partial Sprint C):

```text
Platform: 9.12 (9C-002 + 9C-003 done)
Conexus: 8.92
Kanon: 8.85
Allagma: 9.15
Frontend/UI: 8.50 / 8.05
```

---

# My recommended priority

Do this first:

```text
1. ✅ Allagma one-command system cohesion acceptance (ALLAGMA-9-001)
2. ✅ Platform compatibility validator (PLATFORM-9-001)
3. ✅ Header propagation contract + consumer tests (PLATFORM-9-003, *-PROP-001)
4. ✅ Allagma evidence graph acceptance (ALLAGMA-9-002)
5. ⬜ Conexus model alias manifest (CONEXUS-9-001)
6. ⬜ Kanon domain-pack lifecycle hardening (KANON-9-001); ✅ replay acceptance (KANON-9-002)
7. ✅ Frontend evidence journey live-artifact E2E (SYSTEM-9B-005)
```

Why this order? Because **Allagma is the integration spine**. If Allagma can prove the full system loop, every other repo’s next hardening target becomes obvious.

The goal is not “all repos have more stuff.” The goal is:

```text
One command proves the Ontogony runtime still works as a governed, observable,
idempotent, replayable, policy-mediated system.
```

That is what gets the repos above 9.
