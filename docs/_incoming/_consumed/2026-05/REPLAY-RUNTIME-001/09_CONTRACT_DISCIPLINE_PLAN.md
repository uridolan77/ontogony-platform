# 09 — Contract discipline plan

## Rule

Every replay route or DTO change must pass the existing contract discipline loop.

## Required update checklist

For every backend route change:

1. Update backend route inventory.
2. Update backend OpenAPI snapshot.
3. Update backend API docs/fragments.
4. Update backend route/auth/usage coverage docs if present.
5. Update tests that enforce route inventory and OpenAPI parity.
6. Update frontend OpenAPI snapshot.
7. Regenerate TypeScript schemas/client.
8. Add product-level frontend client wrapper.
9. Update route-workflow catalog.
10. Update `API_CLIENT_ROUTE_USAGE.json`.
11. Update manual DTO shim registry if generation is not yet possible.
12. Update service route parity checks.
13. Run `contracts:discipline`.

## Platform

Update:

- `docs/contracts/CONTRACT_DISCIPLINE_STANDARD.md` if replay introduces a new mandatory artifact class.
- `scripts/check/check-contract-discipline.ps1` only if the rule set changes.
- `docs/system/system-protocol-registry.json` with replay runtime protocol version.
- System compatibility gate if replay protocol becomes release-blocking.

## Allagma

Update:

- route inventory docs/generated artifacts;
- OpenAPI snapshot if present;
- API docs;
- contract snapshots for replay request/result;
- client-generated artifacts if Allagma client exists;
- system route/test matrices.

## Kanon

Update:

- `docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`
- `docs/api/kanon-openapi-v1.json`
- generated route fragments;
- `ONTOLOGY_V0_CLIENT_COVERAGE.json`
- `ONTOLOGY_V0_OPERATOR_UI_COVERAGE.json`
- route/auth matrix if replay routes are role-gated.

## Conexus

Update:

- `docs/generated/CONEXUS_ROUTE_INVENTORY.json`
- `openapi/conexus-admin-v0.snapshot.json`
- admin contract snapshots;
- route catalog tests;
- operator UI coverage;
- Conexus Evidence Spine contract if replay adds metadata.

## Frontend

Update:

- service snapshots under `openapi/` or existing generated contract location;
- generated TypeScript client/schema;
- `src/replay/api` wrappers;
- `scripts/lib/route-workflow-inventory.mjs` generated output;
- `API_CLIENT_ROUTE_USAGE.json`;
- route workflow catalog entries:
  - Replay Workbench;
  - Evidence Spine -> Replay;
  - Allagma Run Detail -> Replay;
  - Kanon Decision -> Replay;
  - Conexus Model Call -> Replay.

## Prohibited shortcuts

- Do not call fetch manually from React components when a generated/product client exists.
- Do not ship route changes with stale OpenAPI snapshots.
- Do not add manual DTOs without registry entry and deletion plan.
- Do not mark replay routes as frontend-covered unless a real route/workflow uses them.
- Do not weaken existing contract discipline checks to pass replay work.
