# FBA2-007 — Release evidence and eval-basing handoff

**Generated:** 2026-05-18  
**Bridge verdict:** COMPLETE  
**Eval-basing readiness:** READY

This document closes the `backend-frontend-phase-v2-sandbox-evidence-alignment` bridge. No further FBA2 product code is required before starting eval-basing.

## Phase scorecard

| Phase | Description | Status |
| ----- | ----------- | ------ |
| FBA2-000 | Register v2 alignment package | **DONE** |
| FBA2-001 | Allagma Phase 3 RC contract closeout | **DONE** |
| FBA2-002 | Allagma OpenAPI / provenance refresh | **DONE** |
| FBA2-003 | Frontend sandbox evidence adapter | **DONE** |
| FBA2-004 | Frontend audit / replay UI rendering | **DONE** |
| FBA2-005 | Fixtures, fallbacks, backend capabilities | **DONE** |
| FBA2-006 | Cross-service compatibility proof | **DONE** |
| FBA2-007 | Release evidence (this document) | **DONE** |

## Release commits

Recorded at scorecard generation (`git rev-parse HEAD`):

| Repo | Role | Commit | Notes |
| ---- | ---- | ------ | ----- |
| `allagma-dotnet` | Backend contract + capabilities | `8cab40bbcd2abe744b81ba29214d56fc10b23e74` | `GET /allagma/v0/capabilities`, OpenAPI snapshot, provenance |
| `ontogony-frontend` | Operator UI + client | `955cc868672d6577aad48aedf57963875ac790ca` | Adapters, panels, hooks, synced OpenAPI |
| `ontogony-platform` | Alignment evidence | `6d76d323a15832f7c7f33f25afa63f505e582787` | FBA2-006/007 evidence (prior); extend on commit of this file |

**OpenAPI snapshot identity** (canonical; may differ from release HEAD when provenance `sourceCommit` is the last commit that touched the snapshot file):

| Artifact | SHA-256 |
| -------- | ------- |
| Backend `docs/api/allagma-openapi-v1.snapshot.json` | `b6d6e6e5c8a978f47cba44a62ccefcde2f97f782f2a6837d4b2736b346750964` |
| Frontend `openapi/allagma.v0.json` (synced) | `330782f362a7eaab9be3a52f6bba12b3cd86eceda45ef7db566c434e32247d63` |

Provenance sidecars:

- Backend: `allagma-dotnet/docs/api/allagma-openapi-v1.provenance.json` (`sourceCommit`, `provenanceCommit`, `snapshotSha256`)
- Frontend: `ontogony-frontend/docs/openapi/ALLAGMA_SNAPSHOT_PROVENANCE.json` (`backendSourceCommit`, `backendProvenanceCommit`, `backendSnapshotSha256`)

## Test evidence

### Allagma (backend)

```powershell
cd allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "FullyQualifiedName~Capabilities|FullyQualifiedName~OpenApi"
```

**Result (2026-05-18):** 8 passed, 0 failed

Covers:

- OpenAPI drift: `/allagma/v0/capabilities`, audit `sandboxEvidence`, five sandbox execute events, provenance `sourceCommit` / `provenanceCommit`
- `AllagmaCapabilitiesApiTests` — 401 without bearer; authenticated shape
- `GetAllagmaCapabilitiesServiceTests` — dev disabled/enabled, production block

Additional Phase 3 RC evidence: `allagma-dotnet/docs/evidence/PHASE3_MINIMAL_SANDBOX_RC_EVIDENCE.md`

### Frontend (operator UI)

```bash
cd ontogony-frontend
npx vitest run src/allagma/adapters/allagmaSandboxEvidenceAdapters.test.ts src/allagma/components/AllagmaSandboxEvidencePanel.test.tsx
npm run openapi:check
```

**Result (2026-05-18):** 17 unit tests passed; `openapi:check` passed

Mocked E2E (sandbox evidence panels):

```bash
npx playwright test e2e/allagma-sandbox-evidence.spec.ts
```

Spec: `e2e/allagma-sandbox-evidence.spec.ts` — run detail ledger/timeline/boundaries, legacy empty bundle, replay workbench, limitations card.

## Required proof checklist

| Proof | Evidence |
| ----- | -------- |
| Allagma P3-SB-006 closeout | `PHASE3_MINIMAL_SANDBOX_RC_EVIDENCE.md`, readiness validator |
| Sandbox evidence rendered | `AllagmaSandboxEvidencePanel`, fixtures + mocked E2E |
| Sandbox execute timeline (5 events) | Adapter tests + `allagma-sandbox-evidence.spec.ts` |
| Real external execution blocked | Capabilities endpoint + panel `allagma-sandbox-real-external-blocked` |
| Local sandbox boundary from backend | `GET /allagma/v0/capabilities` → `useAllagmaSandboxExecutionCapability` |
| No raw marker content exposed | Panel raw-content note; adapter redaction tests |
| Limitation banners accurate | `AllagmaRunOperationsLimitationsCard` + OpenAPI-derived evidence routes |
| OpenAPI contract gate | `npm run openapi:check` |
| Cross-repo compatibility | `07_evidence/FBA2-006_COMPATIBILITY_REPORT.md` |

## Capabilities endpoint proof

**Route:** `GET /allagma/v0/capabilities`  
**Schema:** `ontogony-allagma-capabilities-v1`

Default non-production deployment (`SandboxExecutionEnabled=false`):

- `realExternalExecution`: `enabled=false`, `status=blocked`
- `localSandboxExecution`: `enabled=false`, `status=disabled`
- `sandboxEvidence`: `supported=true`, `auditRoute=/allagma/v0/runs/{runId}/audit`

Production: `localSandboxExecution` reports `blocked` even if flag set (validator also fails startup).

**Hardening note:** Capability service includes a branch for `RealExecutionEnabled=true` → `enabled`, but `AllagmaToolsOptionsValidator` rejects that at startup. Acceptable for this bridge; future real-executor phase should force capability output to `blocked` regardless.

## Sandbox evidence UI proof

| Surface | Component | Data source |
| ------- | --------- | ----------- |
| Run detail | `AllagmaRunSandboxEvidenceSection` | Live audit bundle or `?sandboxFixture=` |
| Replay workbench | Same panel | Resolved run id |
| Limitations | `AllagmaRunOperationsLimitationsCard` | OpenAPI + capabilities hook |

Fixture scenarios (demo only, banner shown): `fullExecute`, `replaySkipped`, `blockedExecute`, `failedExecute`, `noEvidence`.

## Known limitations (accepted)

| Item | Priority | Status |
| ---- | -------- | ------ |
| Local-stack smoke E2E (start Allagma → seed run → open UI) | P2 | Optional; mocked E2E is the gate |
| Capability `RealExecutionEnabled=true` reporting branch | Hardening | Validator blocks; align endpoint copy in future phase |
| Kanon / Conexus FBA2-specific changes | — | Not required for this bridge |

## Eval-basing readiness declaration

The following are **available for eval-basing** to consume without re-opening the sandbox evidence bridge:

1. Stable Allagma audit bundle contract with optional `sandboxEvidence`
2. Committed OpenAPI snapshot + split provenance (`sourceCommit` / `provenanceCommit` / `snapshotSha256`)
3. Frontend typed adapters, redaction, and operator panels
4. Backend-driven sandbox capability metadata (`/allagma/v0/capabilities`)
5. Compatibility report (FBA2-006) and this release scorecard (FBA2-007)

**Next phase:** eval-basing per `10_next_eval_basing/01_HANDOFF_TO_EVAL_BASING.md` (EVAL-001 through EVAL-005).

## Related artifacts

| Document | Path |
| -------- | ---- |
| Compatibility report | `07_evidence/FBA2-006_COMPATIBILITY_REPORT.md` |
| Machine-readable scorecard | `08_examples/FBA2-007-release-scorecard.json` |
| Machine-readable compatibility | `08_examples/FBA2-006-compatibility-report.json` |
| Eval-basing handoff | `10_next_eval_basing/01_HANDOFF_TO_EVAL_BASING.md` |
