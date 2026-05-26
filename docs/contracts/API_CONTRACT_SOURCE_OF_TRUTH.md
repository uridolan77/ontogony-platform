# API contract source of truth (CONTRACT-DISCIPLINE-001F)

> **Platform contract authority.** Defines the precedence order for API and UI contract artifacts
> across the Ontogony operator system. When artifacts disagree, resolve drift by updating sources
> in this order — never by patching downstream consumers first.

**Program archive:** [`ONTOGONY-CONTRACT-DISCIPLINE-OVER9-001`](../_incoming/_consumed/2026-05/ONTOGONY-CONTRACT-DISCIPLINE-OVER9-001/)  
**Vocabulary:** [`CONTRACT_DISCIPLINE_STANDARD.md`](./CONTRACT_DISCIPLINE_STANDARD.md)  
**Gate:** `npm run contracts:discipline` (frontend) · `scripts/check/check-contract-discipline.ps1` (platform)

---

## Precedence ladder

Resolve contract truth top-down. Lower layers are derived; they must not invent routes or types
that are absent from higher layers without an explicit, registered exception.

| Rank | Artifact | Repo | Role |
| --- | --- | --- | --- |
| 1 | Backend route inventory | `allagma-dotnet`, `conexus-dotnet`, `kanon-dotnet` | Canonical list of exposed HTTP routes, auth, and route class |
| 2 | Backend OpenAPI snapshot | same backend repos | Published contract document for the service |
| 3 | Frontend OpenAPI snapshot | `ontogony-frontend/openapi/*.v0.json` | Committed operator-facing contract mirror |
| 4 | Generated TypeScript schema | `ontogony-frontend/src/*/api/generated/schema.ts` | Typed request/response shapes for the SPA |
| 5 | Service HTTP client | `ontogony-frontend/src/*/api/*Client.ts` | Callable functions used by pages and evidence resolvers |
| 6 | Route-workflow catalog | `ontogony-frontend/src/app/route-workflow-catalog.json` | Operator page ↔ backend route declarations |
| 7 | Generated coverage docs | `ontogony-frontend/docs/generated/*_UI_API_COVERAGE.md` | Human-readable parity reports (derived only) |

**Rule:** If rank 1 exposes a route, ranks 3–6 must either reference it with an explicit
disposition or document why it is intentionally excluded (`backend_only`, `internal_test_only`,
etc.).

---

## Per-service inventory paths

| Service | Inventory | Backend OpenAPI | Frontend snapshot |
| --- | --- | --- | --- |
| Allagma | `allagma-dotnet/docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json` | `allagma-dotnet/docs/api/allagma-openapi-v0.json` | `ontogony-frontend/openapi/allagma.v0.json` |
| Conexus | `conexus-dotnet/docs/generated/CONEXUS_ROUTE_INVENTORY.json` | `conexus-dotnet/openapi/*.snapshot.json` | `ontogony-frontend/openapi/conexus.v0.json` |
| Kanon | `kanon-dotnet/docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json` | `kanon-dotnet/docs/api/kanon-openapi-v1.json` | `ontogony-frontend/openapi/kanon.v0.json` |

Sync commands (frontend):

```text
npm run openapi:sync:allagma
npm run openapi:sync:conexus
npm run openapi:sync:kanon
npm run openapi:check
```

---

## Exceptions that are allowed

These do **not** break source-of-truth order when registered and checked:

| Exception | Register | Check |
| --- | --- | --- |
| Transitional handwritten DTO | `docs/generated/manual-dto-shims.registry.json` | `manual-dto-shims:check` |
| Operator-only OpenAPI expansion (Kanon) | committed frontend snapshot diff | `openapi:check` |
| ServerOnly .NET client vs operator SPA call | coverage doc + operator UI inventory | `kanon:parity-hardening:check` |
| Backend-only route with no UI consumer | inventory `routeClass` + disposition | service `*:route-parity` |

See [`MANUAL_DTO_SHIM_POLICY.md`](./MANUAL_DTO_SHIM_POLICY.md) for shim rules.

---

## Drift detection surfaces

| Check | What it proves |
| --- | --- |
| `contracts:discipline` | Full cross-system bundle (001F) |
| `contracts:service-parity` | Allagma, Conexus, Kanon route + operator UI parity |
| `route-client-drift:check` | Client paths ↔ route-workflow catalog |
| `client-routes:check` | Client call inventory is complete |
| `inventory:check` | Route-workflow catalog ↔ backend inventory |
| `openapi:check` | Frontend snapshots match generation policy |

Summary artifact: `ontogony-frontend/docs/generated/contract-discipline.summary.json`.

---

## Related documents

| Document | Relationship |
| --- | --- |
| [`CONTRACT_DISCIPLINE_STANDARD.md`](./CONTRACT_DISCIPLINE_STANDARD.md) | Taxonomy and classification rules |
| [`UI_API_COVERAGE_MATRIX.md`](./UI_API_COVERAGE_MATRIX.md) | Index of generated service coverage docs |
| [`MANUAL_DTO_SHIM_POLICY.md`](./MANUAL_DTO_SHIM_POLICY.md) | When handwritten types are permitted |
| [`SYSTEM_COMPATIBILITY_GATE.md`](./SYSTEM_COMPATIBILITY_GATE.md) | Six-repo mechanical drift (complementary) |
