# KANON-CONNECT-003 — Allagma semantic authority link audit

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (unit + catalog contract tests; Docker Playwright optional on operator machine)  
**Statement:** Deterministic tests prove Allagma run/gate surfaces link to Kanon when ids exist and show honest unavailable state otherwise. No mutation authority added.

## Summary

Expanded `buildAllagmaSemanticAuthorityLinkSlots` coverage for all eight semantic link kinds, added UI contract tests for the links card and human-gate context card, documented cross-service links in `route-workflow-catalog.json`, and added Docker-local Playwright proof for run detail → Kanon planning decision → semantic graph panel.

## Repos touched

| Repo | Change |
| --- | --- |
| `ontogony-frontend` | Slot tests, card tests, catalog `crossServiceLinks`, e2e `kanon-connect-003-allagma-kanon-links.spec.ts`, strengthened 014 test 10 |
| `ontogony-platform` | This evidence file |

## Link matrix (Allagma → Kanon)

| Slot | Target when available | Unavailable when |
| --- | --- | --- |
| Ontology version | `/kanon/ontologies?versionId=` | Missing on run GET |
| Domain pack | `/kanon/domain-packs` | No `domainPackId` on metadata |
| Planning decision | `/kanon/decisions?decisionId=` | Before planning completes |
| Action evaluation | `/kanon/decisions?decisionId=` | Not on run GET (events/audit) |
| Policy / gate explain | `/kanon/policies?…` | Missing ontology + gate/decision context |
| Topology authorization | `/kanon/decisions?decisionId=` | Not on audit topology |
| Resolved decision | `/kanon/decisions?decisionId=` | Use trace/planning links |
| Trace decisions | `/kanon/decisions?mode=trace` | No trace id |
| Human gate | `/allagma/gates` (+ run filter) | No gate on pause events |

`/allagma/gates` also links via `HumanGateContextCard` to `/kanon/policies` and `/kanon/decisions` when gate context is resolved.

## Tests

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/allagma/adapters/buildAllagmaSemanticAuthorityLinkSlots.test.ts src/allagma/components/AllagmaSemanticAuthorityLinksCard.test.tsx src/allagma/components/HumanGateContextCard.test.tsx src/app/route-workflow-catalog.cross-service-links.test.ts
```

| Check | Result |
| --- | --- |
| Semantic authority slot matrix (14 cases) | PASS |
| Links card: no fabricated Open in Kanon | PASS |
| Human gate context Kanon hrefs | PASS |
| Route catalog cross-service contract | PASS |

### Docker-local Playwright (optional)

```powershell
cd C:\dev\ontogony-frontend
npx playwright test e2e/kanon-connect-003-allagma-kanon-links.spec.ts --config=playwright.docker-local.config.ts
```

Requires ENV-SEED-001 baseline run with `planningDecisionId` on compose stack.

## Acceptance

- [x] Allagma run/gate/evidence surfaces have deterministic Kanon link tests
- [x] Missing ids show unavailable state; card does not render broken links
- [x] Route workflow catalog documents `/allagma/runs/:runId` → `/kanon/decisions` and `/allagma/gates` → `/kanon/policies`
- [x] No mutation authority added
- [ ] Live Playwright on compose (operator machine)

## Related

- [KANON-CONNECT-001](./KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md)
- [KANON-CONNECT-002](./KANON_CONNECT_002_SETTINGS_ENV_CONSISTENCY_EVIDENCE.md)
- [KANON-DEEPEN-005](./KANON_DEEPEN_005_CROSS_SERVICE_SEMANTIC_LINKS_EVIDENCE.md)
- [KANON-CONNECT-004](./KANON_CONNECT_004_CONEXUS_ASSISTANCE_OBSERVABILITY_EVIDENCE.md) — Conexus assistance observability proof
- Next: **KANON-CONNECT-005** — Evidence Spine imports Kanon semantic graph
