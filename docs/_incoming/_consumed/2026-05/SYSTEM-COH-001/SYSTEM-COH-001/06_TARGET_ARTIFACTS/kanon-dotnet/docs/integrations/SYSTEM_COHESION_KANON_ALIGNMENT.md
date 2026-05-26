# SYSTEM-COH-001 — Kanon alignment

## Kanon role

Kanon is the semantic authority for the Ontogony governed runtime.

Kanon participates in SYSTEM-COH-001 through:

- semantic plan compilation;
- action/policy evaluation;
- human gate check/resolve;
- decision records and provenance;
- semantic graph and Evidence Spine handoff;
- canonical fact and audit evidence;
- Conexus assistance as advisory `draft_only` evidence;
- trace/entity decision discovery.

## Runtime-owned vs operator-owned routes

Allagma runtime should consume only client-supported governed-loop routes. Operator/server-only routes remain out of runtime dependency unless explicitly approved.

## Assistance boundary

Conexus assistance remains advisory. Kanon records the request/outcome as decision evidence. Authority changes require Kanon-side accept/reject/review flow.

## SYSTEM-COH scenarios

| Scenario | Kanon obligation |
|---|---|
| governed_run_complete | compile plan / policy decisions available |
| human_gate_pause_resume | check/resolve gate state and decision provenance |
| kanon_conexus_assistance | call Conexus through redacted advisory path and record draft-only decision |
| correlation_chain | persist trace/correlation/run ids on decision records where applicable |
| evidence_spine_operator_visibility | expose handoff routes for decision, semantic graph, facts, audits, review items |
| error compatibility | document stable errors or transitional shapes consumed by Allagma |

## Acceptance

This doc should be covered by a docs/contract test if Kanon has equivalent handoff tests.
