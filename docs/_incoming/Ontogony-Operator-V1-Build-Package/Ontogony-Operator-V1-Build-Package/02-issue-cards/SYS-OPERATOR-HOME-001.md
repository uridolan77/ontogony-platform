# SYS-OPERATOR-HOME-001 — Unified operator home

**Priority:** P0  
**Repos:** ontogony-frontend, allagma-dotnet, ontogony-platform

## Goal

Create `/system` or `/operator` page showing health, baseline, posture, safety, evidence readiness, and primary actions.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

Operator can understand whether the system is alive, safe, and usable from one page.

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
