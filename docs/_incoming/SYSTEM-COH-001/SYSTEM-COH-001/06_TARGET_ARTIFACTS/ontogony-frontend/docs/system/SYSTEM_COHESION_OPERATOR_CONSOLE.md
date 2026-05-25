# SYSTEM-COH-001 — Operator console visibility

## Purpose

The system is not truly cohesive unless an operator can inspect the cross-service evidence.

## Required operator paths

An operator should be able to start from any of these identifiers:

- Allagma run id
- trace id
- correlation id
- Kanon decision id
- Kanon review item id
- canonical fact id
- semantic quality snapshot id
- Conexus model call id
- Conexus route decision id

and inspect the related graph in Evidence Spine / operator console.

## Required visible nodes

Where data exists, graphs should show:

- Allagma run
- Allagma events/audit/replay evidence
- Kanon decision
- Kanon provenance/lifecycle
- Kanon semantic graph
- canonical fact / fact audit
- operator review item
- Conexus model call
- Conexus route/fallback/provider attempt

## Acceptance

At least one fixture or contract test should demonstrate a system cohesion graph with:

```text
Allagma run → Kanon decision → Conexus model call
```

and one enriched Kanon graph with:

```text
review item → fact → audit → source/resolution decisions → semantic graph
```

If live Playwright proof is deferred, mark `evidence_spine_operator_visibility` as `DEFERRED_WITH_REASON` in the acceptance matrix.
