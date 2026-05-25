# Add or change an API route

> **Audience:** backend developer, frontend developer  
> **Applies to:** owning API repo + `ontogony-frontend`  
> **Source of truth:** generated route inventories, OpenAPI snapshots  
> **Last verified:** 2026-05-25

## 1. Choose the owning repo

| Prefix / area | Owner |
| --- | --- |
| `/ontology/v0` | Kanon |
| `/v1/chat`, admin gateway | Conexus |
| `/allagma/v0` | Allagma |
| Mechanical shared types | Ontogony.Platform (no product routes) |

## 2. Backend implementation

- Add minimal API / endpoint in `*.Api` composition layer only.
- Application rules in `*.Application`; no provider SDKs in wrong repos.
- Unit + integration tests in owning test project.
- Error shape: `Ontogony.Errors` middleware patterns.

## 3. Regenerate backend evidence

| Repo | Typical commands |
| --- | --- |
| Allagma | `node ./scripts/patch-allagma-replay-openapi-snapshot.mjs`, `update-allagma-openapi-provenance.ps1` |
| Kanon | OpenAPI drift tests / snapshot update per `docs/TESTING.md` |
| Conexus | OpenAPI snapshot scripts per repo |

Commit updated `docs/generated/*_ROUTE_INVENTORY.json` when inventory scripts exist.

## 4. Frontend sync

```powershell
cd C:\dev\ontogony-frontend
npm run openapi:sync:allagma   # or :conexus / Kanon sync script
npm run openapi:gen
npm run client-routes:sync
npm run contracts:discipline
npm run typecheck
```

## 5. Route-workflow catalog

If the route is operator-facing, add workflow metadata in `route-workflow-catalog.json` (generated usage report under `docs/generated/`).

## 6. Cross-service registry (if needed)

- Evidence Spine matrix: `ontogony-platform/docs/system/system-evidence-spine-resolution.matrix.json`
- Protocol registry: `docs/system/system-protocol-registry.json`
- Replay contract: `docs/contracts/REPLAY_RUNTIME_CONTRACT.md`

## 7. Runtime lock / smoke

If the route participates in governed proofs, re-run relevant smoke and consider evidence pointer updates in `ontogony-runtime.lock.json`.

## 8. Documentation

- Link from this learning guide — do not fork a second route table.
- Migration note if breaking: `docs/migrations/<date>-<topic>.md`

## References

- [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md)
- [12_ADD_A_FRONTEND_PAGE.md](./12_ADD_A_FRONTEND_PAGE.md)
