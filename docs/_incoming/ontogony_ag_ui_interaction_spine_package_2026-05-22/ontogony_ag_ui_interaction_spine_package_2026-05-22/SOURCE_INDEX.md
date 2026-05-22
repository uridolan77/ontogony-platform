# Source Index

This package was prepared from the current architectural context and a targeted review of connected repo files and public AG-UI documentation.

## Repo-grounded files inspected

- `ontogony-platform/docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md`
- `ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts`
- `ontogony-frontend/src/system/correlation/resolveTraceCorrelation.ts`
- `ontogony-ui/package.json`
- `allagma-dotnet/docs/system/allagma-feature-connection.matrix.json`
- `kanon-dotnet/docs/operators/KANON_EVIDENCE_SPINE_HANDOFF.md`
- `conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`

## Public AG-UI references used conceptually

- AG-UI overview: event-based protocol for agent-user interaction.
- AG-UI events: lifecycle, text message, tool call, state, activity, custom, reasoning, and draft/meta event categories.
- AG-UI tools: frontend-defined tools, tool-call lifecycle, human-in-the-loop workflows.
- AG-UI interrupts: interrupt-aware run lifecycle, resume rules, response schema, idempotency, and state-at-interrupt-boundary rules.

## Grounding caveat

This package is a planning and contract package. It does not modify the repos and does not claim that live AG-UI compatibility currently exists in Ontogony.
