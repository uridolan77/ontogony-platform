# EVIDENCE-SPINE-OPERATOR-001 — Protocol-aware Evidence Spine graph

**Priority:** P1  
**Repos:** ontogony-frontend, allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-platform

## Goal

Upgrade Evidence Spine as the main explanation surface for runs/decisions/model-calls.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

Given a run id, operator sees Allagma/Kanon/Conexus graph with completeness and source attempts.

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
