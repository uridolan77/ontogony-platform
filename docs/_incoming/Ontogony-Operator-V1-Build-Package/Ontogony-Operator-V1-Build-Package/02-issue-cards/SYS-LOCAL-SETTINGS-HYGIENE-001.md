# SYS-LOCAL-SETTINGS-HYGIENE-001 — Tracked local settings hygiene

**Priority:** P0  
**Repos:** allagma-dotnet

## Goal

Review `.claude/settings.local.json` and keep/remove/template intentionally.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

No accidental local-only settings enter the RC baseline.

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
