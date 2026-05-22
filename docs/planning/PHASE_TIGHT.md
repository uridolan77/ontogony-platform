To push these repos **above 9**, the work is not “add more features.” It is **turn strong local-alpha repos into hard-gated, reproducible, operator-grade systems**.

The current scores are high because the architecture is real. They are not above 9 because too much proof is still distributed across docs, local scripts, PR evidence, and manual discipline.

I would set the current missing number as:

| Repo                | Current score |
| ------------------- | ------------: |
| `ontogony-platform` |       **8.5** |
| `conexus-dotnet`    |       **8.8** |
| `kanon-dotnet`      |       **8.6** |
| `allagma-dotnet`    |       **8.7** |

Allagma deserves **8.7**, not lower, because the old hard-coded-model criticism is now fixed: it resolves model purpose to `ConexusModelAlias`, and it already has a streaming path. 

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

Right now you have **1–6 mostly done**. To cross 9, focus on **7–10**.

---

# 1. `ontogony-platform`: 8.5 → 9.1+

## Why it is not above 9 yet

Platform is architecturally clean. Its rule is correct: share mechanics, not meaning. It explicitly allows correlation, idempotency, errors, HTTP, hosting, telemetry, startup guards, current actor context, hashing, and configuration validation, while forbidding product semantics and service-specific logic. 

The gap is that Platform is still partly a **library repo** plus **coordination docs**. To score above 9, it must become the **hard compatibility spine** for the whole Ontogony system.

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

## Above-9 condition

Platform reaches **9.1–9.2** when every repo consumes Platform mechanics **and** Platform provides the validator that proves the system has not drifted.

---

# 2. `conexus-dotnet`: 8.8 → 9.2+

## Why it is close already

Conexus is the closest to 9. It has health/readiness docs, provider capability matrix, evidence spine, SLO starter, streaming usage/cost, contract/release discipline, capacity baseline, admin auth model, and known limitations. 

Its capacity baseline is also real: fake non-streaming, fake streaming, fallback, provider error storm, idempotency conflict under load, and optional Postgres scenarios are documented. 

The remaining gap is production-adjacent proof and cross-repo hardening.

## Required moves

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

# 3. `kanon-dotnet`: 8.6 → 9.1+

## Why it is not above 9 yet

Kanon’s boundary is excellent. It owns ontology, canonical facts, semantic plans, action policies, human gates, decision provenance, and domain packs; it explicitly does not own LLM routing, workflow orchestration, or provider execution. 

The gap is not conceptual. It is **semantic lifecycle enforcement**.

## Required moves

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

### KANON-9-002 — Semantic decision replay acceptance

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

### KANON-9-003 — Error envelope normalization

If any v0 routes still return local `{ error = ... }`, bare `NotFound`, or endpoint-specific shapes, normalize them or explicitly freeze them as transitional.

Above 9 requires clients to trust failure shapes.

### KANON-9-004 — Conexus assistance full E2E

Kanon’s Conexus assistance boundary is good, but to score above 9 it should be included in system acceptance:

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

# 4. `allagma-dotnet`: 8.7 → 9.2+

## Why it is close

Allagma is now the integration center. Its feature matrix maps run lifecycle, resume/retry/cancel/replay, evaluations, baseline comparisons, runtime posture, Kanon/Conexus dependencies, Platform packages, smoke tests, and frontend routes. 

It also documents model purpose routing, including `Allagma:ModelPurposes:[purpose]:ConexusModelAlias`, streaming flags, and persisted-stream-output flags. 

The main thing holding it below 9 is that it is still **integration-rich but not yet hard-gated enough**.

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

This alone probably pushes Allagma to **9.0+**.

### ALLAGMA-9-002 — Evidence graph acceptance

Allagma should prove a run can be traversed as a graph:

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

This is exactly what your system is about. Make it executable.

### ALLAGMA-9-003 — Real execution trust model, no real execution yet

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

### ALLAGMA-9-004 — Runtime posture must become operator-grade

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

## Above-9 condition

Allagma reaches **9.2** when it becomes the one-command proof that the system is coherent.

---

# The actual 9+ roadmap

## Sprint A — Hard gates, not features

```text
SYSTEM-9A-001 Platform compatibility validator
SYSTEM-9A-002 Allagma system acceptance command
SYSTEM-9A-003 Conexus model alias manifest
SYSTEM-9A-004 Kanon domain pack lifecycle manifest
SYSTEM-9A-005 Frontend/backend route-client drift gate
```

Expected result:

```text
Platform: 8.5 → 8.9
Conexus: 8.8 → 9.0
Kanon: 8.6 → 8.9
Allagma: 8.7 → 9.0
```

## Sprint B — Evidence and replay

```text
SYSTEM-9B-001 End-to-end evidence graph acceptance
SYSTEM-9B-002 Kanon semantic decision replay bundle acceptance
SYSTEM-9B-003 Conexus streaming evidence acceptance
SYSTEM-9B-004 Allagma restart/idempotency/human-gate acceptance
SYSTEM-9B-005 Frontend evidence journey E2E from live artifacts
```

Expected result:

```text
Platform: 9.0
Conexus: 9.1
Kanon: 9.1
Allagma: 9.2
Frontend: 8.7–8.9
```

## Sprint C — Operator-grade polish

```text
SYSTEM-9C-001 Runtime posture dashboard
SYSTEM-9C-002 Shared error envelope conformance
SYSTEM-9C-003 Header propagation contract
SYSTEM-9C-004 Real execution trust model, still disabled
SYSTEM-9C-005 UI status taxonomy + EvidenceExportPanel consolidation
```

Expected result:

```text
Platform: 9.1+
Conexus: 9.2+
Kanon: 9.2
Allagma: 9.2+
Frontend/UI: 9.0-
```

---

# My recommended priority

Do this first:

```text
1. Allagma one-command system cohesion acceptance
2. Platform compatibility validator
3. Conexus model alias manifest
4. Kanon domain-pack lifecycle/replay hardening
5. Frontend evidence journey live-artifact E2E
```

Why this order? Because **Allagma is the integration spine**. If Allagma can prove the full system loop, every other repo’s next hardening target becomes obvious.

The goal is not “all repos have more stuff.” The goal is:

```text
One command proves the Ontogony runtime still works as a governed, observable,
idempotent, replayable, policy-mediated system.
```

That is what gets the repos above 9.
