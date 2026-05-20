# FIRST-VERSION-RC-001 — First-version release candidate

**Priority:** P0  
**Repos:** allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-platform, ontogony-frontend, ontogony-ui

## Goal

Validate and cut first-version candidate lock after implementation.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

All required gates pass and lock pins current implementation without production claim.

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
