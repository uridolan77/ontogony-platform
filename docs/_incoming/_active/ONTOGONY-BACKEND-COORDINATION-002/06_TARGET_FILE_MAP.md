# Target file map — ONTOGONY-BACKEND-COORDINATION-002

Canonical paths to create or update per slice. **Update in place** when file exists.

---

## Parent package (ontogony-platform)

| Path | Slice | Action |
| --- | --- | --- |
| `docs/_incoming/_active/ONTOGONY-BACKEND-COORDINATION-002/` | Parent | Active → consumed on closeout |
| `docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md` | 3 | Create or promote |
| `docs/contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md` | 4 | Create or promote |
| `docs/schemas/ontogony-cross-service-error-envelope-v1.schema.json` | 3 | Create/update |
| `docs/schemas/ontogony-context-propagation-v1.schema.json` | 4 | Create/update |
| `docs/evidence/ONTOGONY_BACKEND_COORDINATION_002_CLOSEOUT.md` | Parent | Create on closeout |
| `scripts/validate-cross-service-error-envelope.ps1` | 3 | Extend |

---

## allagma-dotnet

| Path | Slice | Action |
| --- | --- | --- |
| `docs/system/SYSTEM_COMPATIBILITY_MATRIX.md` | 2 | Refresh |
| `docs/system/ontogony-runtime.lock.json` | 2 | Bump if needed |
| `docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` | 3 | Complete |
| `docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md` | 4 | Refresh |
| `docs/system/BACKEND_REPO_DOCS_INDEX.md` | 1 | Refresh |
| `docs/system/BACKEND_REPO_BOUNDARY_MATRIX.md` | Parent | Update scores |
| `docs/system/conexus-model-alias-manifest.snapshot.json` | 5 | Refresh |
| `docs/testing/SYSTEM_E2E_SCENARIOS.md` | 6 | Extend |
| `docs/evidence/SYSTEM_COMPATIBILITY_MATRIX_001_CLOSEOUT.md` | 2 | Create |
| `docs/evidence/SHARED_ERROR_CONTRACT_001_CLOSEOUT.md` | 3 | Create |
| `docs/evidence/BACKEND_SYSTEM_E2E_001_*.json` | 6 | Evidence artifact |
| `scripts/validate-runtime-lock.ps1` | 2 | Extend |
| `scripts/smoke-first-system.ps1` | 6 | Verify five-service |
| `scripts/lib/system-cohesion-e2e.ps1` | 4, 6 | Correlation + E2E |

---

## conexus-dotnet

| Path | Slice | Action |
| --- | --- | --- |
| `docs/integrations/SYSTEM_COHESION_CONEXUS_ALIGNMENT.md` | 2–4 | Update |
| `docs/contracts/CONEXUS_MODEL_ALIAS_CONSUMER_CONTRACT_V0.md` | 5 | Create |
| `docs/generated/conexus-model-alias-manifest.json` | 5 | Refresh |
| `docs/evidence/SHARED_ERROR_CONTRACT_001_CLOSEOUT.md` | 3 | Create |
| `docs/evidence/ALLAGMA_CONEXUS_MODEL_ALIAS_001_CLOSEOUT.md` | 5 | Create |
| `src/Conexus.Api/` middleware error mapping | 3 | Align envelope |

---

## kanon-dotnet

| Path | Slice | Action |
| --- | --- | --- |
| `docs/integrations/SYSTEM_COHESION_KANON_ALIGNMENT.md` | 2–4 | Update |
| `docs/integrations/ERROR_CONTRACTS.md` | 3 | Cross-ref platform contract |
| `docs/generated/KANON_COMPATIBILITY_MANIFEST.json` | 2 | Refresh |
| `docs/evidence/SHARED_ERROR_CONTRACT_001_CLOSEOUT.md` | 3 | Create |
| `src/Kanon.Api/` middleware + endpoint error docs | 3 | Align |

---

## metabole-dotnet

| Path | Slice | Action |
| --- | --- | --- |
| `docs/system/METABOLE_LIVE_STACK_CERTIFICATION_MATRIX.md` | 8 | Refresh |
| `docs/system/METABOLE_KANON_BOUNDARY.md` | 8 | Handoff clarity |
| `docs/generated/METABOLE_COMPATIBILITY_MANIFEST.json` | 2 | Refresh |
| `docs/evidence/METABOLE_DATA_SPINE_HARDENING_001_CLOSEOUT.md` | 8 | Create |
| `scripts/smoke/run-metabole-five-service-certification.ps1` | 8 | PASS evidence |

---

## aisthesis-dotnet

| Path | Slice | Action |
| --- | --- | --- |
| `docs/system/` (new) | 7 | Optional matrices |
| `global.json` | 7 | SDK alignment |
| `docs/evidence/AISTHESIS_RECONSTRUCTABILITY_SPINE_001_CLOSEOUT.md` | 7 | Create |
| `docs/runbooks/AISTHESIS_LIVE_FIVE_SERVICE_RUNBOOK.md` | 7 | Update |
| `scripts/system/run-five-service-live-certification.ps1` | 7 | Live PASS |

---

## Per-repo docs order (slice 1)

| Path | Action |
| --- | --- |
| `README.md` | Status link to cohesion sprint |
| `docs/README.md` | Full index sections |
| `docs/DEFERRALS.md` | Sprint deferrals appended |
| `docs/status/*_CLEANUP_ORGANIZATION_STATUS.md` | Superseded-by banner → coordination-002 |

---

## Evidence naming convention

```text
docs/evidence/<SLICE-ID>_CLOSEOUT.md
docs/evidence/<SLICE-ID>_EVIDENCE.json
docs/evidence/<SLICE-ID>_VALIDATION_LOG.txt
```
