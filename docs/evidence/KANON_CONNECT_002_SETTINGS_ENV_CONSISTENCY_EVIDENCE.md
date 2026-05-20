# KANON-CONNECT-002 — Kanon settings/env consistency

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (code + unit tests; Docker Playwright re-run optional on operator machine)  
**Canonical ontology version:** `gaming-core@0.1.0`

## Summary

Normalized Docker-local Kanon operator defaults to match ENV-SEED-001 and the gaming-core domain pack. Added settings health card, overview context fields, legacy `gaming-core-v0` migration on read, and compose build args for ontology id/version.

## Canonical decision

| Field | Value |
| --- | --- |
| Ontology id | `gaming-core` |
| Ontology version id | `gaming-core@0.1.0` |
| Legacy id (migrated on read in Local/Docker-local) | `gaming-core-v0` |

## Repos touched

| Repo | Change |
| --- | --- |
| `ontogony-frontend` | `dockerLocalServiceDefaults`, operator settings defaults, normalization, health card, overview context, tests, e2e mocks |
| `ontogony-platform` | `docker-compose.yml` VITE ontology args, `.env.example`, this evidence |

## Key files

| Path | Purpose |
| --- | --- |
| `ontogony-frontend/src/shared/config/dockerLocalServiceDefaults.ts` | `kanonOntologyVersionId`, legacy id list |
| `ontogony-frontend/src/app/settings/operatorSettingsTypes.ts` | Default + `VITE_KANON_ONTOLOGY_VERSION_ID` |
| `ontogony-frontend/src/app/settings/normalizeOperatorSettings.ts` | Legacy version migration |
| `ontogony-frontend/src/kanon/components/KanonSettingsHealthCard.tsx` | Settings page health panel |
| `ontogony-frontend/src/kanon/components/KanonOperatorContextCard.tsx` | Overview shows URL, ontology, version, actor |
| `ontogony-platform/docker/local-working-system/docker-compose.yml` | Build-time ontology env |

## Tests

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/app/settings/validateOperatorSettingsDefaults.test.ts src/app/settings/normalizeOperatorSettings.test.ts src/kanon/components/KanonSettingsHealthCard.test.tsx
```

| Check | Result |
| --- | --- |
| Default ontology version matches seed | PASS |
| Default roles grant domain-pack + provenance read | PASS |
| Legacy `gaming-core-v0` migrates on normalize/read | PASS |
| Kanon settings health card renders | PASS |

## Acceptance

- [x] No mismatch between frontend default ontology version and Docker seed
- [x] Kanon overview shows configured ontology/version and actor roles
- [x] Settings page shows Kanon semantic health card
- [ ] KANON-DEEPEN-014 Playwright re-run on live compose (operator machine)

## Related

- [KANON-CONNECT-001 feature map](./KANON_CONNECT_001_CROSS_REPO_FEATURE_MAP.md)
- Next: **KANON-CONNECT-003** — Allagma semantic authority link audit
