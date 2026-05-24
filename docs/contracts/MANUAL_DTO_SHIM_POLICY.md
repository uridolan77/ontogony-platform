# Manual DTO shim policy (CONTRACT-DISCIPLINE-001F)

> **Platform contract policy.** Defines when handwritten TypeScript DTOs are permitted in the
> operator frontend and how they must be registered, reviewed, and removed.

**Program:** [`CONTRACT-DISCIPLINE-001`](../_incoming/NEXT_2_CONTRACT.md)  
**Taxonomy:** [`CONTRACT_DISCIPLINE_STANDARD.md`](./CONTRACT_DISCIPLINE_STANDARD.md) — `contractState`  
**Machine register:** `ontogony-frontend/docs/generated/manual-dto-shims.registry.json`  
**Human report:** `ontogony-frontend/docs/generated/MANUAL_DTO_SHIMS.md`

---

## Default rule

**Prefer generated schema types.** Every operator client function should use types from
`src/<service>/api/generated/schema.ts` derived from the committed OpenAPI snapshot.

Handwritten DTOs are **exceptions**, not the default path for new API surfaces.

---

## When a shim is allowed

| `contractState` | Use when | Requirements |
| --- | --- | --- |
| `transitional_shim` | OpenAPI sync is in flight; field or response shape temporarily ahead/behind snapshot | Owner, risk, target removal date or trigger |
| `handwritten_adapter` | Stable operator need; OpenAPI cannot yet express the wire shape cleanly | Owner, risk, documented reason, removal criteria |

A shim is **not** allowed when:

- the route is new and OpenAPI can be updated in the same change
- the shim hides stale snapshot drift (`stale_snapshot` — fix OpenAPI instead)
- the type duplicates a generated schema without normalization need

---

## Required registration fields

Each entry in `manual-dto-shims.registry.json` must include:

```text
service
file
typeName
contractState   (transitional_shim | handwritten_adapter)
owner
risk            (low | medium | high)
reason
targetRemoval
```

Markers in client source that require registration (`manual-dto-shims:check`):

```text
Not yet in committed OpenAPI snapshot
manual shim
OpenAPI components omit
unknown normalization
```

---

## Workflow

1. Implement or change the backend route and update the backend inventory.
2. Sync OpenAPI snapshot (`openapi:sync:<service>`) and regenerate schema.
3. If generated types are still insufficient, add a registry entry **before** merging the client change.
4. Run `npm run manual-dto-shims:sync` to refresh `MANUAL_DTO_SHIMS.md`.
5. Run `npm run manual-dto-shims:check` as part of `contracts:discipline`.

---

## Removal criteria

Remove a shim when **all** of the following hold:

- Committed frontend OpenAPI snapshot documents the route/schema
- Generated `schema.ts` includes the required types
- Client code uses generated types without loss of operator behavior
- Registry entry deleted and `manual-dto-shims:sync` re-run

Transitional shims should not remain open across release candidates without an explicit
owner-approved extension recorded in the registry `targetRemoval` field.

---

## Current posture (2026-05)

As of CONTRACT-DISCIPLINE-001B/001C/001D closeout:

- **Allagma:** 2 transitional start-run shims remain (`StartAgentRunRequestDto`, `StartAgentRunResponseDto`)
- **Conexus:** quota/route-preview shims removed; wire normalization stays internal to client helpers
- **Kanon:** several `handwritten_adapter` entries pending OpenAPI schema documentation for resolve/explain flows

See the generated register for the live list and counts.

---

## Checks

| Command | Role |
| --- | --- |
| `manual-dto-shims:sync` | Regenerate markdown from registry |
| `manual-dto-shims:check` | Fail on unregistered markers or registry drift |
| `contracts:discipline` | Includes shim check in cross-system gate |

---

## Related documents

| Document | Relationship |
| --- | --- |
| [`API_CONTRACT_SOURCE_OF_TRUTH.md`](./API_CONTRACT_SOURCE_OF_TRUTH.md) | OpenAPI/schema precedence over shims |
| [`UI_API_COVERAGE_MATRIX.md`](./UI_API_COVERAGE_MATRIX.md) | Per-service coverage and parity index |
