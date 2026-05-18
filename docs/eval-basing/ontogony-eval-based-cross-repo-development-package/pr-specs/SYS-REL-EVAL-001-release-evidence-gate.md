# PR Spec — SYS-REL-EVAL-001 — Release Evidence Gate

## Owner repo

`allagma-dotnet`

## Goal

Prevent release signoff unless eval evidence exists.

## Add

```text
scripts/validate-eval-release-readiness.ps1
docs/releases/EVAL_RELEASE_EVIDENCE_INDEX.md
docs/templates/EVAL_RELEASE_CHECKLIST.md
```

## Gates

```text
cross-repo eval summary exists
schema validation passes
all critical scenarios pass
no unsafe baseline duplication
route decisions present where required
Kanon topology decisions present where required
observability proof present or explicitly waived
```

## Acceptance

- validator returns non-zero when evidence is missing.
- release docs include exact artifact paths.
