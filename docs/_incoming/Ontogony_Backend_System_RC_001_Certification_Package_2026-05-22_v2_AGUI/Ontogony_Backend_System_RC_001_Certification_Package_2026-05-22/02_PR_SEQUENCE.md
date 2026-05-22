# PR Sequence

## PR 1 — SYSTEM-RC-001A-runtime-lock-promotion

**Repo:** `allagma-dotnet`  
**Purpose:** Pin latest selected backend heads and classify all movement since `SYSTEM-ALPHA-006`.

Depends on: none.

Primary files:

```text
docs/system/ontogony-runtime.lock.json
docs/system/post-lock-deltas.json
docs/system/POST_LOCK_DELTA_REGISTER.md
docs/releases/RELEASE_EVIDENCE_INDEX.md
```

Must not add features.

---

## PR 2 — SYSTEM-RC-001B-full-cohesion-evidence

**Repo:** `allagma-dotnet`  
**Purpose:** Run and record full sibling-source cohesion with Kanon assistance, Conexus fallback, and streaming evidence.

Depends on: PR 1.

Primary artifacts:

```text
artifacts/system-e2e/<timestamp>/summary.json
docs/evidence/SYSTEM_RC_001B_FULL_COHESION_EVIDENCE.md
```

---

## PR 3 — SYSTEM-RC-001C-package-mode-certification

**Repo:** `allagma-dotnet`, with upstream package involvement  
**Purpose:** Prove Allagma builds/tests against packed Ontogony/Kanon/Conexus package versions from lock.

Depends on: PR 1.

Primary artifacts:

```text
artifacts/ci-evidence/package-mode/summary.json
docs/evidence/SYSTEM_RC_001C_PACKAGE_MODE_CERTIFICATION.md
```

---

## PR 4 — SYSTEM-RC-001D-observability-pass

**Repo:** `allagma-dotnet`, with Platform docs cross-link  
**Purpose:** Produce real observability PASS artifact for all three backend nodes.

Depends on: PR 1 and preferably PR 2.

Primary artifacts:

```text
artifacts/observability/<timestamp>/observability-summary.json
docs/evidence/SYSTEM_RC_001D_OBSERVABILITY_PASS.md
```

---

## PR 5 — SYSTEM-RC-001E-evidence-spine-golden-run

**Repo:** `ontogony-platform` + `allagma-dotnet`  
**Purpose:** Prove golden evidence journey from Allagma run to Kanon decisions, Conexus model call, route decision, usage/cost, replay, and redaction.

Depends on: PR 2.

Primary artifacts:

```text
artifacts/evidence-spine/<timestamp>/golden-run-evidence-bundle.json
artifacts/evidence-spine/<timestamp>/golden-run-evidence-graph.json
artifacts/evidence-spine/<timestamp>/golden-run-redaction-report.json
```

---

## PR 6 — CONEXUS-RC-001-gateway-certification-matrix

**Repo:** `conexus-dotnet`  
**Purpose:** Hard-certify gateway behavior: fallback, quota, idempotency, streaming, evidence bundle, usage drilldown, and auth isolation.

Can run after PR 1, but should be verified with PR 2.

---

## PR 7 — KANON-RC-001-semantic-authority-certification

**Repo:** `kanon-dotnet`  
**Purpose:** Hard-certify semantic authority behavior: manifest gates, v0 freeze, negative semantic cases, Postgres smoke, assistance review loop.

Can run after PR 1, but should be verified with PR 2.

---

## PR 8 — PLATFORM-RC-001-substrate-contract-freeze

**Repo:** `ontogony-platform`  
**Purpose:** Freeze Platform alpha-RC substrate contract, public API discipline, protocol registry refresh, and consumer compatibility expectations.

Should run after PRs 1–7 so registry reflects the certified state.

---

## Final closeout

After all PRs:

```powershell
cd C:\dev\allagma-dotnet
pwsh ./scripts/validate-system-tight-rc-prep.ps1
pwsh ./scripts/validate-system-tight-rc-readiness.ps1
pwsh ./scripts/validate-system-tight-rc-evidence.ps1
```

Then create:

```text
docs/evidence/SYSTEM_RC_001_CLOSEOUT.md
```


---

## SYSTEM-RC-001F — Agent Interaction / AG-UI spine certification

**Owner:** `allagma-dotnet` with Platform, Conexus, and Kanon contract participation.  
**Placement:** after `SYSTEM-RC-001E-evidence-spine-golden-run`, before final evidence closeout.

**Purpose:** certify that the backend can produce a stable AG-UI-compatible interaction stream from a governed run without leaking raw prompts, completions, or secrets.

**Must produce:**

```text
contracts/AGENT_INTERACTION_AGUI_SPINE_CONTRACT.md
contracts/AGUI_BACKEND_OPERATOR_SURFACE_MATRIX.md
artifacts/agui/<timestamp>/golden-run-interaction-events.jsonl
artifacts/agui/<timestamp>/golden-run-interaction-stream-transcript.txt
artifacts/agui/<timestamp>/agui-redaction-report.json
artifacts/agui/<timestamp>/agui-resume-report.json
```

**Must validate:**

```text
Platform schema validation
Allagma JSONL export
Allagma SSE stream and Last-Event-ID resume
Conexus model-call interaction projection
Kanon human-gate / review interrupt mapping
Redaction scan
Evidence-bundle links on model-call events
```

**Score impact:** raises Evidence / audit / operator spine and Cross-repo integration by proving the run is not only persisted, but narratable as a deterministic agent interaction session.
