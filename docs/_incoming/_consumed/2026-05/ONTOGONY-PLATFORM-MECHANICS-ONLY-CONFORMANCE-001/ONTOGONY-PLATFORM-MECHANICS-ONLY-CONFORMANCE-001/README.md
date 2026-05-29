# ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001

Comprehensive dev package for `ontogony-platform`.

## Purpose

Harden Ontogony.Platform around three linked goals:

1. **Keep Platform mechanical-only** — every new Platform proposal must pass the reuse test:
   `Can this be reused by Conexus, Kanon, Allagma, Metabole, and Aisthesis without importing product meaning?`
2. **Strengthen consumer conformance** — provide runnable harnesses that each product repo can execute.
3. **Stabilize cross-service mechanical schemas** — error envelope, correlation/header context, evidence references,
   idempotency state, replay contract, and actor context.

## Package posture

This package is intentionally not a runtime-feature package. It must not move semantic authority, model routing,
workflow orchestration, data transformation, or evidence interpretation into Platform.

```text
Platform  = neutral mechanics
Conexus   = model access
Kanon     = semantic authority
Allagma   = governed execution
Metabole  = data transformation spine
Aisthesis = evidence / reconstructability spine
```

## Start here

1. Read `00_UNPACK_PROMPT.md`.
2. Execute `04_MASTER_IMPLEMENTATION_SEQUENCE.md`.
3. Track acceptance in `05_ACCEPTANCE_MATRIX.md`.
4. Use scripts under `scripts/` as Cursor/dev-agent implementation targets.
5. Close with `10_CLOSEOUT_TEMPLATE.md`.

## Non-claims

- No production readiness claim.
- No runtime lock authority migration to Platform.
- No product semantics in Platform.
- No provider SDKs, ontology logic, business approval rules, agent plans, domain workflow logic, or UI.
