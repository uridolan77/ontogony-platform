# Ontogony Operator V1 Build Package

This is a **real development package**, not another audit-only package.

## Goal

Turn the current governed runtime into a coherent first operator product:

> A local operator can run Docker-local Ontogony, open the frontend, understand system posture, start and inspect governed runs, follow Kanon and Conexus evidence, handle human gates, inspect Evidence Spine, and clearly see that real external execution remains blocked.

## Current baseline context

`SYSTEM-ALPHA-006` is the latest locked baseline. Current `main` has useful post-lock improvements that should be validated and included in the next first-version candidate if green:

- Allagma runtime posture.
- Protocol registry.
- Stale incoming-package guard.
- Real-tools block verification.
- Allagma feature connection matrix.
- Conexus evidence flow.
- Frontend health/route/evidence hardening.
- Kanon Connect 001–007.

## Non-goals

- No production readiness.
- No enterprise IAM.
- No real external tool execution.
- No live external side effects.
- No semantic authority outside Kanon.
- No unrelated product/domain features.

## Recommended order

1. `SYS-REAL-TOOLS-BLOCK-VERIFY-001A`
2. `SYS-POSTLOCK-DELTA-REGISTER-001`
3. `SYS-LOCAL-SETTINGS-HYGIENE-001`
4. `SYS-Q006-004-RESTART-PATH-DECISION`
5. `SYS-OPERATOR-HOME-001`
6. `ALLAGMA-OPERATOR-RUNS-001`
7. `SYS-PROTOCOL-RUNTIME-001`
8. `EVIDENCE-SPINE-OPERATOR-001`
9. `CONEXUS-OPERATOR-001`
10. `ALLAGMA-SANDBOX-WORKBENCH-001`
11. `SYSTEM-DEMO-FLOWS-001`
12. `FIRST-VERSION-RC-001`

## Package layout

```text
00-context/              current assessment and boundaries
01-roadmap/              roadmap and dependencies
02-issue-cards/          implementable issue cards
03-prompts/              ready-to-use implementation prompts
04-specs/                UX/runtime/evidence specs
05-evidence-templates/   evidence templates per issue
06-qa/                   validation and Playwright scenario plans
07-checklists/           pre-implementation and RC checklists
08-schemas/              protocol metadata examples
09-demo-flows/           demo guide and demo-flow catalog
10-closeout/             RC closeout template
scripts/                 validation runner template
```
