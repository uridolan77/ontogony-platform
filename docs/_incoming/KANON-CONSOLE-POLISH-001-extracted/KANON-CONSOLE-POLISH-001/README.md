# KANON-CONSOLE-POLISH-001 — Cursor implementation package

Prepared: 2026-05-24

## Purpose

This package turns the recent Ontogony console review into a focused Cursor-ready implementation sprint for **Kanon-facing console polish**.

The goal is not to add new Kanon backend capability. The goal is to make existing Kanon surfaces clearer, safer, more operator-grade, and more faithful to the actual evidence/readiness state.

## Primary target repos

1. `ontogony-frontend` — primary implementation target.
2. `ontogony-ui` — only if shared status, badge, card, empty-state, copy, or layout primitives need to be tightened.
3. `kanon-dotnet` — do not modify in this sprint unless frontend inspection proves a tiny already-expected contract field is impossible to obtain. If that happens, stop and document a separate backend follow-up instead of expanding scope.

## Sprint thesis

Kanon is already the semantic authority layer. The console should make that authority legible:

- Kanon pages must distinguish **authoritative semantic state** from **draft assistance**, **simulated lifecycle analysis**, **fixture/demo evidence**, and **operator-local settings**.
- Every disabled or unavailable action must say why.
- Every partial readiness or unresolved evidence state must show an explicit missing reason.
- Secret-like sample data must be removed from UI defaults.
- Domain-pack lifecycle state must be visually coherent and not confused with generated/test-looking pack versions.

## What this package contains

- `CURSOR_MASTER_PROMPT.md` — single prompt to give Cursor.
- `SOURCE_FINDINGS.md` — issue map extracted from the raw console review.
- `IMPLEMENTATION_PLAN.md` — phased implementation approach.
- `tickets/` — scoped implementation tickets.
- `docs/` — UI contract, status taxonomy, evidence-linking, and copy guidance.
- `examples/` — sample state contracts and copy examples.
- `ACCEPTANCE_CHECKLIST.md` — merge gate.
- `QA_CHECKLIST.md` — manual validation checklist.

## Recommended branch

```bash
git checkout -b feature/KANON-CONSOLE-POLISH-001
```

## Non-goals

- Do not re-architect Evidence Spine.
- Do not add production IAM.
- Do not implement real tool execution.
- Do not change Kanon authority semantics.
- Do not hide warnings merely to make the console look green.
- Do not turn generated/fixture/demo data into release-ready evidence.

## Definition of done

The Kanon console becomes calmer, sharper, and more truthful:

1. Assistance workbench is safe by default and has redaction preview.
2. Domain packs page separates pack inventory, active ontology versions, selected version, lifecycle actions, and simulation outputs.
3. Kanon settings/status surfaces explain health/readiness/role/provenance states without duplicated warnings.
4. Review Queue and Policies partial states explain what is missing and where to go next.
5. Evidence links are consistent, contextual, and clearly labeled.
6. Tests protect the new copy, status semantics, empty states, and dangerous examples.
