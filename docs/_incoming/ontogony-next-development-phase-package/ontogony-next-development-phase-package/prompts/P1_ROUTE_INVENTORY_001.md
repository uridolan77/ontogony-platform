# P1 — ROUTE-INVENTORY-001

## Objective

Prevent route/security drift across backend services.

## Scope

Generate/test inventories for:

- Conexus `/admin/**`
- Kanon `/ontology/v0/**`
- Allagma `/allagma/v0/**`

## Inventory fields

- Service
- Method
- Path
- Handler/module
- Auth mode
- Required scope/role/actor
- Route classification
- Response contract
- Error envelope
- Evidence/audit behavior
- Production exposure note

## Acceptance

- Inventory generated and committed.
- Tests fail when routes are unclassified.
- Admin routes cannot land without security classification.
