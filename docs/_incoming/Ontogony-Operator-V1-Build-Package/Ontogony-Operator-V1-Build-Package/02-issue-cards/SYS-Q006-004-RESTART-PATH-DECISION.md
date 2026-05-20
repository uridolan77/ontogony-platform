# SYS-Q006-004-RESTART-PATH-DECISION — Restart path ambiguity decision

**Priority:** P0  
**Repos:** allagma-dotnet, ontogony-platform

## Goal

Fix or retire the legacy restart script conflict with compose ports.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

First-version docs have one clear canonical restart path.

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
