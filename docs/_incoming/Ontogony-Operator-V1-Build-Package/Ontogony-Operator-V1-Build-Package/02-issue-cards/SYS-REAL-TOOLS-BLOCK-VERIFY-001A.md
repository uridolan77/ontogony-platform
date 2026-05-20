# SYS-REAL-TOOLS-BLOCK-VERIFY-001A — Allagma docs-only CI safety gate

**Priority:** P0  
**Repos:** allagma-dotnet

## Goal

Add docs-only CI job so docs PRs run `scripts/validate-real-tools-block.ps1`.

## Implementation outline

1. Inspect current repo state and avoid duplicating existing work.
2. Implement the smallest coherent version that improves the operator product.
3. Add focused tests.
4. Update route/workflow catalogs where frontend routes are touched.
5. Add an evidence file.
6. Preserve all safety boundaries.

## Acceptance

Allagma docs-only PRs cannot overclaim real external execution.

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
