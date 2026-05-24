# UI ↔ API coverage matrix (CONTRACT-DISCIPLINE-001F)

> **Platform contract index.** Maps each operator service to its generated coverage artifacts and
> parity checks. The generated markdown files are derived reports — not sources of truth.

**Program:** [`CONTRACT-DISCIPLINE-001`](../_incoming/NEXT_2_CONTRACT.md)  
**Source-of-truth order:** [`API_CONTRACT_SOURCE_OF_TRUTH.md`](./API_CONTRACT_SOURCE_OF_TRUTH.md)  
**Taxonomy:** [`CONTRACT_DISCIPLINE_STANDARD.md`](./CONTRACT_DISCIPLINE_STANDARD.md)

---

## Service matrix

| Service | Reference level | Generated coverage | Operator UI inventory | Route parity | Operator UI coverage |
| --- | --- | --- | --- | --- | --- |
| **Kanon** | Reference implementation | [`KANON_UI_API_COVERAGE.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/generated/KANON_UI_API_COVERAGE.md) | `KANON_OPERATOR_UI_ROUTE_COVERAGE.json` | `kanon:route-parity` | `kanon:operator-ui-coverage:check` |
| **Allagma** | Parity target (001C) | [`ALLAGMA_UI_API_COVERAGE.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/generated/ALLAGMA_UI_API_COVERAGE.md) | `ALLAGMA_OPERATOR_UI_ROUTE_COVERAGE.json` | `allagma:route-parity` | `allagma:operator-ui-coverage:check` |
| **Conexus** | Parity target (001D) | [`CONEXUS_UI_API_COVERAGE.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/generated/CONEXUS_UI_API_COVERAGE.md) | `CONEXUS_OPERATOR_UI_ROUTE_COVERAGE.json` | `conexus:route-parity` | `conexus:operator-ui-coverage:check` |

All generated paths live under `ontogony-frontend/docs/generated/`.

---

## Cross-service artifacts

| Artifact | Purpose |
| --- | --- |
| `API_CLIENT_ROUTE_USAGE.json` | Every client HTTP call extracted from `*Client.ts` |
| `ROUTE_WORKFLOW_INVENTORY.md` | Merged route-workflow catalog |
| `MANUAL_DTO_SHIMS.md` | Registered handwritten DTOs |
| `contract-discipline.summary.json` | Latest `contracts:discipline` run summary |

---

## Kanon four-layer taxonomy (reference)

Kanon coverage docs use four distinct layers — do not collapse them:

| Layer | Primary artifact |
| --- | --- |
| Backend route | `ONTOLOGY_V0_ROUTE_INVENTORY.json` |
| Generated backend client | `Kanon.Client` (ServerOnly vs operator-callable) |
| Operator frontend route | route-workflow catalog + operator UI coverage |
| Internal-only | smoke, tests, cross-service-only surfaces |

**Important:** `ServerOnly` on `Kanon.Client` means the .NET generated client excludes the route.
It does **not** mean the operator SPA must not call the route. Source-binding routes are the
canonical example (`operator-http` from SPA, `ServerOnly` on .NET client).

---

## Bundled checks

| Command | Scope |
| --- | --- |
| `kanon:parity-hardening:check` | Kanon route parity + operator UI + Domain Switcher smoke artifacts |
| `contracts:service-parity` | All three services' route + operator UI parity |
| `contracts:discipline` | Full cross-system gate (OpenAPI, client routes, shims, parity, inventory, readiness) |

Gate wrapper: `ontogony-platform/scripts/check/check-contract-discipline.ps1`.

---

## Known display caveat (001F follow-up)

Some coverage rows show **Operator UI: Yes** alongside **contract state: `backend_only`**. That
combination is mechanically correct when the route is called via manual/operator HTTP but not
represented in generated frontend schema. Before the gate becomes a required PR check, consider
renaming disposition labels to reduce confusion (`no_generated_frontend_schema`,
`operator_http_manual`, etc.).

---

## Regeneration

```text
cd ontogony-frontend
npm run kanon:operator-ui-coverage:sync
npm run allagma:operator-ui-coverage:sync
npm run conexus:operator-ui-coverage:sync
npm run client-routes:sync
npm run manual-dto-shims:sync
```

Then run `npm run contracts:discipline`.
