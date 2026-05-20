# KANON-CONNECT-LOCK-001 — Runtime baseline promotion

**Date:** 2026-05-20  
**Issue:** KANON-CONNECT-LOCK-001 (Alpha-005 delta package)  
**Runtime baseline:** **SYSTEM-ALPHA-006** — [`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json)  
**Canonical closeout:** [`allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_ALPHA_006_CLOSEOUT.md)

**Verdict:** **COMPLETE** — KANON-CONNECT 001–007 promoted from parallel evidence into governed runtime baseline criteria.  
**Non-claims:** Not production readiness. Real external tool execution remains blocked.

## Promotion decision

| Class | Items | Lock implication |
| --- | --- | --- |
| **Baseline blockers** (must PASS at cut) | 006 route parity, 007 Docker cross-service smoke | Required for `SYSTEM-ALPHA-005` promotion; retained under **ALPHA-006** via lock `evidence.kanonDeepeningBrowserQa` |
| **Companion frontend evidence** | 002 settings/env, 003 Allagma links, 004 Conexus assistance, 005 Evidence Spine semantic graph | Documented in baseline; re-run on operator machine when FE/Kanon pins change |
| **Informational / inventory** | 001 cross-repo feature map | Authoritative gap inventory; does not gate lock validators |

Kanon connect work does **not** add new Kanon product capability. It proves cross-repo wiring for deepening v2 surfaces already shipped.

## KANON-CONNECT 001–007 status

| Id | Theme | Verdict | Baseline role | Evidence |
| --- | --- | --- | --- | --- |
| 001 | Cross-repo feature map | **PASS** | Informational inventory | [KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md](./KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md) |
| 002 | Settings / env consistency | **PASS** | Companion (FE + compose) | [KANON_CONNECT_002_SETTINGS_ENV_CONSISTENCY_EVIDENCE.md](./KANON_CONNECT_002_SETTINGS_ENV_CONSISTENCY_EVIDENCE.md) |
| 003 | Allagma semantic link audit | **PASS** | Companion (FE links) | [KANON_CONNECT_003_ALLAGMA_SEMANTIC_LINK_AUDIT_EVIDENCE.md](./KANON_CONNECT_003_ALLAGMA_SEMANTIC_LINK_AUDIT_EVIDENCE.md) |
| 004 | Conexus assistance observability | **PASS** | Companion (FE + Kanon tests) | [KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md](./KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md) |
| 005 | Evidence Spine semantic graph | **PASS** | Companion (FE spine) | [KANON_CONNECT_005_EVIDENCE_SPINE_SEMANTIC_GRAPH_EVIDENCE.md](./KANON_CONNECT_005_EVIDENCE_SPINE_SEMANTIC_GRAPH_EVIDENCE.md) |
| 006 | Route / OpenAPI / catalog parity | **PASS** | **Baseline blocker** | [KANON_CONNECT_006_ROUTE_PARITY_EVIDENCE.md](./KANON_CONNECT_006_ROUTE_PARITY_EVIDENCE.md) |
| 007 | Docker cross-service smoke | **PASS** (7/7) | **Baseline blocker** | [KANON_CONNECT_007_DOCKER_CROSS_SERVICE_SMOKE_EVIDENCE.md](./KANON_CONNECT_007_DOCKER_CROSS_SERVICE_SMOKE_EVIDENCE.md) |

## Acceptance highlights (required by issue card)

### Route parity (006)

| Check | Status |
| --- | --- |
| 59 `/ontology/v0` signatures aligned across Kanon inventory, OpenAPI, FE catalog | **PASS** |
| `npm run kanon:route-parity:check` in `ontogony-frontend` `npm run check` | **PASS** |

```powershell
cd C:\dev\ontogony-frontend
npm run kanon:route-parity:check
```

### Docker cross-service smoke (007)

| Check | Status |
| --- | --- |
| Compose rebuild `kanon-api`, `allagma-api`, `conexus-api`, `ontogony-frontend` | **PASS** |
| ENV-SEED-001 bootstrap | **PASS** |
| Playwright `kanon-connect-007` | **PASS — 7/7** |

```powershell
cd C:\dev\ontogony-frontend
npm run docker:smoke:kanon-connect-007
```

Lock references 007 via `ontogony-runtime.lock.json` → `evidence.kanonDeepeningBrowserQa`.

### Evidence Spine semantic graph (005)

| Check | Status |
| --- | --- |
| `GET /ontology/v0/semantic-graph` imported in `resolveEvidenceSpineDeepGraphs` | **PASS** |
| Adapter + export bundle unit tests | **PASS** |
| Covered in 007 Playwright step 5 (Evidence Spine + Kanon graph) | **PASS** |

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/kanon/adapters/kanonSemanticGraphEvidenceAdapters.test.ts src/evidence-spine/resolveEvidenceSpineDeepGraphs.test.ts
```

## Baseline timeline

| Cut | Kanon connect role |
| --- | --- |
| **SYSTEM-ALPHA-005** | First runtime promotion after 007 PASS (cohesion + observability + 7/7 smoke) |
| **SYSTEM-ALPHA-006** | Connect evidence **retained**; quarantine hygiene (Q-006-001..003) does not invalidate 001–007 |

## Operator revalidation (moving-main)

When Kanon or frontend pins change after ALPHA-006:

```powershell
cd C:\dev\ontogony-frontend
npm run kanon:route-parity:check
npm run docker:smoke:kanon-connect-007
```

Per-repo unit gates: see [allagma-dotnet/docs/evidence/README.md](../../../allagma-dotnet/docs/evidence/README.md) (SYS-E2E-REVALIDATE-006 matrix).

## Related

- Allagma feature matrix audit: [SYS_CONNECT_MATRIX_AUDIT_001_EVIDENCE.md](../../../allagma-dotnet/docs/evidence/SYS_CONNECT_MATRIX_AUDIT_001_EVIDENCE.md)
- Kanon deepening sequence: [KANON_DEEPEN_SEQUENCE_STATUS.md](./KANON_DEEPEN_SEQUENCE_STATUS.md)
- Issue card: `docs/_incoming/Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20/issue-cards/KANON-CONNECT-LOCK-001.md`
