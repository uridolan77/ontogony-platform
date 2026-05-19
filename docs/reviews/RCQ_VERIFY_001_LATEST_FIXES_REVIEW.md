# RCQ-VERIFY-001 — Latest fixes repo-quality verification

**Date:** 2026-05-20  
**Reviewer:** automated RCQ pass (code + evidence + tests; browser not re-run in this pass)  
**Scope:** Operator polish, CORS, sidebar, health probes, Kanon auth clarity, header/theme, observability guidance (commits through `ontogony-frontend@022c223`, `ontogony-ui@d238c18`, backend CORS fixes on main).

## Executive summary

| Verdict | Area |
|--------|------|
| **PASS — correct layer** | CORS in each API repo; shell/sidebar in `ontogony-ui`; product health/nav/pages in `ontogony-frontend`; runbooks/evidence in `ontogony-platform` |
| **PASS — minimal & durable** | Targeted files; no broad rewrites; health contracts are frontend-local (appropriate for operator-only probes) |
| **PARTIAL — evidence vs browser** | FE-003A code is on `main` but **Docker frontend image may still be stale** if rebuild failed; acceptance checkboxes in platform evidence remain unchecked |
| **PARTIAL — tests** | Good unit coverage for health presentation, CORS integration in backends; **no e2e** asserting absence of `/health/live` 404s or theme label |
| **FIXED in review** | Platform 003A evidence had **Conexus/Allagma host ports swapped**; rebuild instructions now reference CA-aware orchestrator |

The “remaining issues” list in the RCQ prompt largely describes **pre-003A browser behavior**. Source on `main` addresses them; operators must **rebuild the frontend image** (or run `npm run dev`) and **refresh operator settings** (for `defaultActorRoles`) to see the fixes.

---

## 1. Repo placement matrix

| Fix | Correct repo? | Actual location | Notes |
|-----|---------------|-----------------|-------|
| Docker-local CORS | Yes | `kanon-dotnet`, `allagma-dotnet`, `conexus-dotnet` | Dev-only CORS options + `CorsIntegrationTests` per service |
| Kanon actor headers on ontology routes | Yes | `kanon-dotnet` | CORS allows `X-Ontogony-*` headers; auth still Kanon domain |
| Sidebar groups + scroll + utility section | Yes | `ontogony-ui` (`Sidebar`, `NavList`, `AppShell`) | Product wires `navItems` + `utilityNavItems` in `ontogony-frontend` |
| Health probe contracts | Yes | `ontogony-frontend` | `serviceHealthContracts.ts` — not a shared platform package (operator-console-only) |
| `/health` non-JSON → live | Yes | `ontogony-frontend` `healthClient.ts` | Backend unchanged; ASP.NET often returns plain text |
| Conexus `/ready` 503 vs down | Yes | `ontogony-frontend` `serviceHealthPresentation.ts` | Liveness from `/health` + probes; readiness separate |
| Kanon domain-pack 403 clarity | **Acceptable dev** | `ontogony-frontend` only | Sends `X-Ontogony-Roles` from settings; **no Kanon seed change** |
| Theme label | Yes | `ontogony-frontend` `ShellThemeLabel.tsx` | Uses `@ontogony/ui/theme`; not hardcoded in shell |
| Command search placeholder | Yes | `ontogony-frontend` `OntogonyShell.tsx` | Product-owned slot content |
| Observability empty-state guidance | Yes | `ontogony-frontend` | `ConexusObservabilityGuidanceCard` — documents API limitation honestly |
| Evaluation sub-nav tabs | Yes | `ontogony-frontend` | `AllagmaEvaluationsNavTabs` |
| Run detail / trace / eval presentation (002) | Yes | `ontogony-frontend` | Adapters + tests; Dialog close in `ontogony-ui` |
| Evidence / compose runbooks | Yes | `ontogony-platform` + per-repo `docs/evidence/` | Platform mirrors cross-repo acceptance |

### Questionable (not wrong, but note for production)

- **Kanon roles via frontend settings (Option A):** Correct for `DevelopmentTrustedHeaders` + Docker-local. **Not** a substitute for production IAM; production should not rely on browser-sent `Admin` role.
- **Health contracts only in frontend:** Durable for the console; if other clients need the same matrix, consider `ontogony-platform/docs/operators/` canonical table (platform 003A evidence now has it; ports corrected).

### Not placed in backend (intentionally)

- Non-JSON `/health` parsing — frontend tolerance, not an API contract change.
- Command Center “degraded” wording — pure operator UX.

---

## 2. Minimality and durability

| Check | Assessment |
|-------|------------|
| Targeted vs rewrite | **Pass** — 003A touched ~28 frontend files; UI shell nav earlier (`dd59669`, `d238c18`) scoped to layout components |
| Frontend workaround for backend bug | **Pass** — CORS fixed in APIs; frontend only classifies browser-blocked vs down |
| Duplicated logic | **Minor** — `deriveServiceHealthPresentation` + `healthClient` + probes are layered but testable; no duplicate CORS |
| Hardcoded local-only | **Pass with caveat** — `defaultActorRoles: "Admin"` in `defaultOperatorSettings` is dev-oriented; stored in operator settings, not compile-time env |
| `readServiceHealth` left `requestJson` path | **Pass** — dedicated `fetch` for `/health` with `*/*` accept (appropriate) |

---

## 3. Evidence drift

| Document | Claim | Code / runtime |
|----------|-------|----------------|
| `FE_OPERATOR_POLISH_003A_*` (frontend) | Fixes listed | **Matches** `main` |
| `FE_OPERATOR_POLISH_003A_*` (platform) | Acceptance checklist | **Unchecked** — honest; browser verify pending |
| `FE_OPERATOR_POLISH_003A_*` (platform) | Conexus **5083** / Allagma **5082** | **Wrong** — corrected in this RCQ (5082 Conexus, 5083 Allagma) |
| `FE_OPERATOR_POLISH_002` | Command center unreachable only when all `down` | **Matches** `SystemOverviewPage.tsx` |
| `FE_OPERATOR_POLISH_002` | No backend changes | **Still true** for 002; 003A same |
| Prior session “Docker rebuild OK” | Implied | **Overclaimed if** bare `docker compose build` ran without CA — UI stage can fail; container keeps old image |
| `Command search later` / `Theme: dark` | Removed in evidence narrative | **Absent from source** — only mentioned as past problem in evidence |

**Operator localStorage:** Existing browsers may lack `defaultActorRoles` until settings are saved once; `operatorSettingsStorage.ts` deep-merges `allagma` defaults — **documented gap**, not a code bug.

---

## 4. Test coverage vs real bugs

| Bug / behavior | Tests present? | Gap |
|----------------|----------------|-----|
| CORS preflight (Kanon actor headers) | `Kanon.Tests/CorsIntegrationTests` (4) | Allagma + Conexus similar suites |
| Health: no Kanon `/live` probe | `serviceEndpointProbes.test.ts` | Good |
| Health: plain `/health` 200 | `healthClient.test.ts` | Good |
| Health: strict readiness presentation | `serviceHealthPresentation.test.ts` | Good |
| `useSystemHealth` unreachable = `down` only | `useSystemHealth.test.tsx` | Good |
| Run detail presentation | `runDetailPresentation.test.ts` | Good |
| Trace enrichment | `enrichTraceCorrelationView.test.ts` | Good |
| Cross-service links | `CrossServiceLinksCard.test.tsx` | Good |
| Theme label | `ShellThemeLabel.test.tsx` | Does not assert DOM theme class sync |
| Nav groups / utility | `nav.test.ts` | Structure only; no visual regression |
| Kanon 403 role card | `kanonActorAuthorization.test.ts` | No page-level integration test |
| **Browser: no 404 noise** | — | **Missing** — needs manual network tab or e2e |
| **Docker image contains 003A** | — | **Missing** — needs successful frontend image build |

**Validation run (this RCQ):**

```text
ontogony-frontend: npm run typecheck — PASS
ontogony-frontend: vitest (9 files, 26 tests) — PASS
kanon-dotnet: CorsIntegrationTests — PASS (4)
```

---

## 5. Docker rebuild vs browser result

| Item | Status |
|------|--------|
| Compose service | `ontogony-frontend` build context `ontogony-frontend` + `ontogony_ui` additional context |
| Dockerfile | Multi-stage; `npm ci --include=dev` in UI + FE stages; `EXTRA_CA_CERT_BASE64` supported |
| Known failure mode | `npm ci` / `tsc not found` without CA (`PMQA002_002`) |
| Recommended command | `start-local-working-system.ps1 -Build` (auto CA injection) |
| Risk | **Stale SPA** at `:5175` if build failed but container “Running” on old image |

**Conclusion:** Code review **does not prove** browser shows 003A fixes until a **successful** frontend image build and hard refresh. Dev server from `ontogony-frontend` on `main` is an acceptable alternative for verification.

---

## Per-repo findings

### ontogony-platform

| | |
|-|-|
| **Correct** | Evidence docs, compose port docs, seed scripts, CORS follow-up docs |
| **Questionable** | — |
| **Over-broad** | — |
| **Missing tests** | N/A (docs/scripts) |
| **Evidence drift** | 003A port table swapped (fixed); rebuild path understated CA need (fixed) |
| **Follow-up** | Link this review from `docker/local-working-system/README.md` FE polish section (optional) |

### ontogony-frontend

| | |
|-|-|
| **Correct** | Health contracts, presentation, Kanon client headers, shell slots, observability guidance, eval tabs, 002 adapters |
| **Questionable** | `defaultActorRoles: "Admin"` default — OK for local, document for prod |
| **Over-broad** | None significant |
| **Missing tests** | E2e for health probe network noise; integration for DomainPacksPage + 403 card |
| **Evidence drift** | None in source; platform copy had port typo |
| **Follow-up** | After rebuild, run manual checklist in `FE_OPERATOR_POLISH_003A_*`; consider `inspect-docker-local-operator-frontend.ps1` update if it asserts old health paths |

### ontogony-ui

| | |
|-|-|
| **Correct** | `AppShell`/`Sidebar` utility nav, scroll regions, `NavList` groups, Dialog primitives (002) |
| **Questionable** | — |
| **Over-broad** | — |
| **Missing tests** | Layout stories exist; no automated test for utility section visibility |
| **Evidence drift** | None found |
| **Follow-up** | Ensure frontend Docker build pins UI context that includes `d238c18`+ |

### kanon-dotnet

| | |
|-|-|
| **Correct** | `KanonCorsOptions`, development CORS middleware, actor header trust mode |
| **Questionable** | No Docker seed granting `local-operator` roles — intentional if frontend sends roles |
| **Over-broad** | — |
| **Missing tests** | Domain-pack 403 with missing roles (API unit exists in `DomainPackManagementApiTests`) |
| **Evidence drift** | — |
| **Follow-up** | Optional: document trusted-header role model in `docs/operators/` for console operators |

### allagma-dotnet

| | |
|-|-|
| **Correct** | CORS for dev; evaluations routes used by console |
| **Questionable** | — |
| **Over-broad** | — |
| **Missing tests** | CORS tests present |
| **Follow-up** | — |

### conexus-dotnet

| | |
|-|-|
| **Correct** | `/health/live`, `/live`, `/ready` as documented; CORS tests |
| **Questionable** | — |
| **Over-broad** | — |
| **Missing tests** | Readiness 503 behavior covered in API integration tests |
| **Follow-up** | — |

---

## Follow-up fixes (priority)

| Priority | Item | Owner | Effort |
|----------|------|-------|--------|
| P0 | Successful `ontogony-frontend` Docker build + browser verify 003A checklist | Operator | Small |
| P1 | Save operator settings once (or reset) so `defaultActorRoles` merges | Operator | Trivial |
| P2 | E2e: Command Center refresh does not request Kanon/Allagma `/live` | frontend | Small |
| P3 | Production stance doc: do not use header-injected `Admin` outside dev | kanon + platform docs | Small |
| P4 | Optional Kanon seed: map `local-operator` → Auditor for docker-only | kanon | Medium — only if product wants backend-owned defaults |

**Implemented in this RCQ (docs only):**

- `docs/evidence/FE_OPERATOR_POLISH_003A_HEALTH_AND_HEADER_CLEANUP_EVIDENCE.md` — port table + rebuild/CA note

---

## Answers to review questions (short)

1. **Correct repo?** Yes, with health/Kanon-role UX correctly in frontend and CORS in backends.  
2. **Minimal but durable?** Yes; Kanon roles in headers is dev-appropriate, not production IAM.  
3. **Evidence overclaim?** Platform 003A ports were wrong; rebuild success was assumed too easily. Frontend evidence matches code.  
4. **Tests cover real bug?** Unit-level yes; browser/e2e gap for 404 noise and stale image.  
5. **Docker proves browser?** **Not without a successful rebuild** — use CA-aware `start-local-working-system.ps1 -Build` or local `npm run dev`.

---

## Commits referenced

| Repo | Commit | Subject |
|------|--------|---------|
| ontogony-frontend | `022c223` | fix(ui): align operator health probes and header controls |
| ontogony-frontend | `613b6f7` | fix(ui): polish operator evidence and status clarity |
| ontogony-frontend | `ebc3101` | fix(ui): clarify local health status and improve operator navigation |
| ontogony-ui | `d238c18` | fix(ui): improve shell navigation and diagnostics modal primitives |
| ontogony-ui | `dd59669` | feat(ui): collapsible operator nav and clearer health cards |
| kanon-dotnet | `82a70b1` | fix(kanon): allow Ontogony actor headers for Docker-local CORS |
| allagma-dotnet | `37035fa` | fix(allagma): allow Docker-local frontend CORS in development |
| conexus-dotnet | `76cf727` | fix(conexus): allow Docker-local frontend CORS in development |
| ontogony-platform | `2a7459a` | docs(env): record operator health and header cleanup evidence |
