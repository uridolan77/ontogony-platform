# CONEXUS-OPERATOR-001 — Conexus operator capability completion

**Priority:** P1  
**Repos:** conexus-dotnet, ontogony-frontend, ontogony-platform

## Goal

Wire route preview, quota, model-call evidence, usage/cost, and Evidence Spine links.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

Operator can inspect model gateway behavior without real provider side effects.

## Required evidence

Create an evidence file with:

```text
Issue:
Repos changed:
Files changed:
Tests run:
Docker/browser validation:
Known limitations:
Safety statement:
Verdict:
```

## Non-goals

- No production-readiness claim.
- No real external tool execution.
- No enterprise IAM.
- No semantic authority outside Kanon.
