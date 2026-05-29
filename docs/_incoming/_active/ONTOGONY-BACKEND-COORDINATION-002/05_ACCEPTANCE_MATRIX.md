# Acceptance matrix — ONTOGONY-BACKEND-COORDINATION-002

Status values: `PASS` | `FAIL` | `DEFERRED_WITH_REASON` | `NOT_APPLICABLE_FOR_ALPHA` | `NOT_STARTED`

Last updated: **2026-05-29** (package intake)

---

## Parent package gates

| ID | Gate | Status | Evidence |
| --- | --- | --- | --- |
| PARENT-001 | All eight slice closeouts exist | `NOT_STARTED` | — |
| PARENT-002 | Boundary matrix updated post-sprint | `NOT_STARTED` | `allagma-dotnet/docs/system/BACKEND_REPO_BOUNDARY_MATRIX.md` |
| PARENT-003 | Runtime lock validates in ReleaseMode | `NOT_STARTED` | `validate-runtime-lock.ps1` output |
| PARENT-004 | No slice introduced boundary violation | `NOT_STARTED` | Architecture conformance |
| PARENT-005 | Package archived to `_consumed` with CONSUMED.md | `NOT_STARTED` | — |

---

## Slice 1 — BACKEND-REPO-DOCS-ORDER-002

| ID | Criterion | Status |
| --- | --- | --- |
| DOC-001 | All six repos: README links to docs/README + cleanup/cohesion status | `NOT_STARTED` |
| DOC-002 | All six repos: docs/README has Status, Reviews, Evidence, Deferrals, Incoming | `NOT_STARTED` |
| DOC-003 | `BACKEND_REPO_DOCS_INDEX.md` links resolve | `NOT_STARTED` |
| DOC-004 | `validate-docs-incoming-hygiene.ps1` passes per repo | `NOT_STARTED` |
| DOC-005 | `validate-docs-links.ps1` passes OR deferrals filed per repo | `NOT_STARTED` |

---

## Slice 2 — SYSTEM-COMPATIBILITY-MATRIX-001

| ID | Criterion | Status |
| --- | --- | --- |
| COMPAT-001 | Matrix repo rows match runtime lock | `NOT_STARTED` |
| COMPAT-002 | Port matrix: Kanon 5081, Conexus 5082, Allagma 5083, Metabole 5084, Aisthesis 5085 | `NOT_STARTED` |
| COMPAT-003 | Package version row matches Directory.Packages.props | `NOT_STARTED` |
| COMPAT-004 | Route counts match generated inventories (± documented delta) | `NOT_STARTED` |
| COMPAT-005 | `run-cross-repo-conformance.ps1` PASS or deferrals classified | `NOT_STARTED` |

---

## Slice 3 — SHARED-ERROR-CONTRACT-001

| ID | Criterion | Status |
| --- | --- | --- |
| ERR-001 | Contract promoted to platform `docs/contracts/` | `NOT_STARTED` |
| ERR-002 | Middleware-mapped 4xx/5xx use envelope in all six APIs | `NOT_STARTED` |
| ERR-003 | `SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` complete | `NOT_STARTED` |
| ERR-004 | System compat error gate PASS | `NOT_STARTED` |
| ERR-005 | Kanon typed validation DTOs documented as intentional exception | `NOT_STARTED` |

---

## Slice 4 — CROSS-REPO-IDENTITY-CORRELATION-001

| ID | Criterion | Status |
| --- | --- | --- |
| IDN-001 | Propagation contract v1 in platform docs | `NOT_STARTED` |
| IDN-002 | Trace ID present on Allagma→Kanon→Conexus chain in smoke | `NOT_STARTED` |
| IDN-003 | Idempotency key forwarded on mutating cross-service calls | `NOT_STARTED` |
| IDN-004 | `SYSTEM_TRACE_CONTEXT_MATRIX.md` matches code constants | `NOT_STARTED` |
| IDN-005 | Actor header documented (alpha/dev modes) | `NOT_STARTED` |

---

## Slice 5 — ALLAGMA-CONEXUS-MODEL-ALIAS-001

| ID | Criterion | Status |
| --- | --- | --- |
| ALIAS-001 | No provider model IDs in Allagma `src/` | `NOT_STARTED` |
| ALIAS-002 | `conexus-model-alias-manifest.snapshot.json` validates | `NOT_STARTED` |
| ALIAS-003 | All model purposes map to Conexus aliases in config | `NOT_STARTED` |
| ALIAS-004 | Conexus docs describe consumer alias contract | `NOT_STARTED` |
| ALIAS-005 | Tests use `test-alias` or manifest aliases only | `NOT_STARTED` |

---

## Slice 6 — BACKEND-SYSTEM-E2E-001

| ID | Criterion | Status |
| --- | --- | --- |
| E2E-001 | Five-service stack starts via documented script | `NOT_STARTED` |
| E2E-002 | Governed first-loop smoke PASS | `NOT_STARTED` |
| E2E-003 | Evidence JSON committed under `docs/evidence/` | `NOT_STARTED` |
| E2E-004 | Human-gate path smoke PASS or deferred with reason | `NOT_STARTED` |
| E2E-005 | Real tools remain blocked (conformance test) | `NOT_STARTED` |

---

## Slice 7 — AISTHESIS-RECONSTRUCTABILITY-SPINE-001

| ID | Criterion | Status |
| --- | --- | --- |
| AIS-001 | Live certification `status: PASS` (not fixture-only) | `NOT_STARTED` |
| AIS-002 | `requiredEdges.missing = 0` on live trace | `NOT_STARTED` |
| AIS-003 | `reconstructabilityGrade = complete` | `NOT_STARTED` |
| AIS-004 | Native producers (Allagma/Kanon/Conexus/Metabole) emit edges | `NOT_STARTED` |
| AIS-005 | SDK/build policy unblocks CI and local dev | `NOT_STARTED` |

---

## Slice 8 — METABOLE-DATA-SPINE-HARDENING-001

| ID | Criterion | Status |
| --- | --- | --- |
| MB-001 | Five-service Metabole certification PASS | `NOT_STARTED` |
| MB-002 | SLOD → Kanon peer route smoke PASS | `NOT_STARTED` |
| MB-003 | Status-truth + undeniability gates PASS | `NOT_STARTED` |
| MB-004 | `METABOLE_KANON_BOUNDARY.md` reflects handoff | `NOT_STARTED` |
| MB-005 | ELT/medallion deferrals explicit in DEFERRALS.md | `NOT_STARTED` |

---

## Sign-off

| Role | Name | Date | Signature |
| --- | --- | --- | --- |
| Runtime coordinator | | | |
| Platform mechanics | | | |
| Product owners (×6) | | | |
