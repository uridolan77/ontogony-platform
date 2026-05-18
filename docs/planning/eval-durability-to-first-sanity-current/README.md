# Ontogony Eval Durability → First Full Sanity Current Package

This package replaces the older incoming package as the current working plan.

The old package at:

```text
ontogony-platform/docs/_incoming/ontogony-eval-hardening-to-full-sanity-package.zip
```

was useful, but it predates the latest completed state:

```text
7.  EVAL-FIX-001
8.  CX-ROUTE-EVIDENCE-001
9.  AGM-EVAL-001
10. SYS-EVAL-001
11. SYS-OBS-EVAL-001
```

This updated package starts from that completed baseline and makes the next implementation target:

```text
EVAL-DUR-001 — durable eval/baseline persistence
```

with one optional cleanup gate before it:

```text
EVAL-CLEAN-001 — final eval-sequence cleanup before durable persistence
```

## Recommended next sequence

```text
0. EVAL-CLEAN-001        — optional but recommended cleanup/readiness pass
1. EVAL-DUR-001          — durable eval/baseline persistence
2. EVAL-CI-001           — CI eval suite and regression gates
3. EVAL-QUALITY-001      — richer scoring and calibrated judge scaffolding
4. EVAL-DATA-001         — scenario dataset management
5. FE-EVAL-002           — richer eval dashboards and trend views
6. BE-POLISH-001         — backend readiness polish
7. FE-POLISH-001         — frontend readiness polish
8. ALIGN-EVAL-001        — backend/frontend eval alignment refresh
9. SYS-FULL-SANITY-001   — first full cross-repo sanity test
10. RC-FIRST-SANITY-001  — first sanity milestone closeout
```

## Critical rule

Do not expand agentic complexity before durability and CI gates. The current task is to make the eval loop durable, repeatable, and operator-readable.
