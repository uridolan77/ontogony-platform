# Repo task — ontogony-frontend

Frontend owns operator proof for system cohesion.

## Required work

Add or update:

- `docs/system/SYSTEM_COHESION_OPERATOR_CONSOLE.md`

The doc should identify how an operator inspects:

1. Allagma run id.
2. Kanon planning/action/human-gate/assistance decisions.
3. Conexus model call id and route decision id.
4. Evidence Spine graph from trace/correlation/run/decision/modelCall/review item roots.
5. Replay/runtime evidence if available.
6. Contract discipline / route coverage outputs.

## Optional implementation

If the current operator console lacks an obvious SYSTEM-COH entrypoint, add a small System Cohesion status panel that links existing pages. Avoid a large UI redesign.

## Tests

Add docs tests or fixture tests proving the operator evidence fixture includes cross-service identifiers.

## Done when

The closeout report can say not only “the backend produced evidence” but also “the operator can inspect it.”
