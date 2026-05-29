# Ontogony Platform — cleanup organization status

**Date:** 2026-05-29  
**One-page summary**

> **Cleanup record (BACKEND-REPO-CLEANUP-ORGANIZATION-001).** **Current status:** see [CURRENT_STATE.md](../CURRENT_STATE.md). **Active sprint:** [ONTOGONY-BACKEND-COORDINATION-002](../_incoming/_active/ONTOGONY-BACKEND-COORDINATION-002/) — slice 1 [BACKEND-REPO-DOCS-ORDER-002](../_incoming/_active/ONTOGONY-BACKEND-COORDINATION-002/slices/BACKEND-REPO-DOCS-ORDER-002/) **in progress**.

---

## What is now clean

- `docs/_incoming/` conforms to policy (`_active`, `_consumed`, `README.md` only).
- Misfiled Aisthesis RC package trees archived with `CONSUMED.md`.
- Cleanup audit, status, evidence, and deferrals documented.
- `README.md` points to docs hub and cleanup status.

---

## What is still messy

- `PublicApi.Tests` snapshot drift (pre-existing).
- Empty `docs/generated/` placeholder.
- Large `_consumed/2026-05/` archive (35+ packages) — navigable via manifest but heavy.
- `_donors/` quarantine still contains historical conexus/agentor reference code.

---

## Next recommended cleanup PR / package

**CROSS-REPO-IDENTITY-CORRELATION-001** (coordination slice 4). **SHARED-ERROR-CONTRACT-001** closed 2026-05-29 — evidence: [`docs/evidence/SHARED_ERROR_CONTRACT_001_CLOSEOUT.md`](../evidence/SHARED_ERROR_CONTRACT_001_CLOSEOUT.md).

---

## Readiness scores (1–10)

| Dimension | Score | Notes |
| --- | ---: | --- |
| Code organization | **9** | 27 packages, clear tiers, architecture tests |
| Docs organization | **8** | Strong spine; status folder now present |
| Test discoverability | **9** | 1:1 package tests + CI scripts documented |
| Architecture clarity | **9** | AGENTS.md + ProductSemanticLeakageTests |
| Cross-repo cohesion | **7** | SystemCompatibility gate exists; E2E gaps remain |
| Operational readiness | **7** | Docker-local stack; alpha versioning |

**Overall:** **8.2 / 10**
