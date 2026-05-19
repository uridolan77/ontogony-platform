# 10 — PR Prompts

## PFH-000 — Package setup

```text
We are starting PFH-000 — Product feature hardening package setup.

Repo:
- uridolan77/ontogony-platform

Goal:
Unpack and register the product feature hardening package:
docs/product-hardening/eval-alignment-frontend-depth/

Boundary:
Docs/package setup only. No runtime source changes. No workflow changes. Not production readiness.

Tasks:
- Copy the zip to docs/_incoming/
- Unpack to docs/product-hardening/eval-alignment-frontend-depth/
- Add docs/evidence/PRODUCT_FEATURE_HARDENING_PACKAGE_EVIDENCE.md
- Update docs/releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md with a pointer if appropriate

Validation:
- File inventory present
- Manifest parses as JSON
- No runtime changes
- Not production readiness

Suggested commit:
docs(product): add eval alignment frontend hardening package
```

## PFH-001 — Current-state audit

```text
We are starting PFH-001 — Product hardening current-state audit.

Repos:
- ontogony-platform
- allagma-dotnet
- kanon-dotnet
- conexus-dotnet
- ontogony-frontend
- ontogony-ui

Goal:
Audit current eval, alignment, and frontend product surfaces before implementation.

Boundary:
Audit/docs only unless a broken doc pointer must be fixed. Not production readiness.

Deliver:
- updated gap matrices
- route/OpenAPI/client inventory
- frontend pages/hooks/adapters/test inventory
- product gap priority list
- evidence doc

Suggested commit:
docs(product): audit eval alignment frontend hardening state
```

## EVAL-PRODUCT-001 — Eval query/list contract

```text
Goal:
Define and implement or formalize the eval query/list contract used by the eval dashboard.

Boundary:
Contract-first. No fake global list. If backend route is absent, either add it or document limitation explicitly.

Validation:
Backend tests, OpenAPI check, generated client/adapters, frontend tests, evidence.
```

## ALIGN-PRODUCT-001 — Contract matrix refresh

```text
Goal:
Refresh the backend/frontend contract matrix for eval, run detail, replay, topology, trace/correlation, and comparison surfaces.

Boundary:
Alignment first. Runtime changes only if required to fix a direct contract mismatch.
```

## FE-PRODUCT-001 — Eval dashboard v2

```text
Goal:
Improve eval dashboard product depth after EVAL-PRODUCT-001 and ALIGN-PRODUCT-001.

Boundary:
No invented backend semantics. Keep fixture/live/degraded clarity.
```
