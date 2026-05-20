# SYS-PROTOCOL-RUNTIME-001 — Evidence-first runtime protocol identity

**Priority:** P1  
**Repos:** allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend, ontogony-platform

## Goal

Add lightweight protocol metadata to evidence-producing cross-service acts.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

Operator/Evidence Spine can see protocol id, authority, side-effect level where available.

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
