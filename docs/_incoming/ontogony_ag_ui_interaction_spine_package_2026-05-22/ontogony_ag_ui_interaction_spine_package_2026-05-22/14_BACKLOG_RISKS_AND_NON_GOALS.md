# 14 — Backlog, Risks, and Non-Goals

## Backlog

- Live SSE stream from Allagma.
- WebSocket bidirectional transport if SSE + POST actions become insufficient.
- External AG-UI client demo.
- CopilotKit integration proof-of-concept.
- AI-stack mock server inspired by AIMock concepts.
- Repo/docs intelligence layer exposed as an interaction tool.
- Multi-agent/sub-agent visualization.
- User steering events beyond interrupt/resume.
- UI intent registry expanded beyond simple trusted components.

## Risks

### Event contract overreach

Risk: The first version tries to model everything and blocks implementation.  
Mitigation: Start with fixture replay and client-side synthesized events. Keep `v0` additive.

### Backend stream too early

Risk: Streaming endpoint ships before frontend reducer and UI model are stable.  
Mitigation: Build JSONL + synthesized adapters first.

### Conflating evidence with interaction

Risk: The new work duplicates Evidence Spine.  
Mitigation: Evidence Spine remains graph authority; Agent Interaction Spine references it and renders it temporally.

### Exposing hidden reasoning

Risk: “reasoning events” become private chain-of-thought leakage.  
Mitigation: Only expose sanitized milestones, evidence attempts, decision reasons, and provenance.

### Arbitrary generative UI execution

Risk: Agent-proposed UI becomes unsafe/unmaintainable.  
Mitigation: Registry-only UI intents with strict schemas.

### Product-specific UI leakage into @ontogony/ui

Risk: Shared UI package imports frontend/product DTOs.  
Mitigation: @ontogony/ui receives view models only.

## Non-goals

- Replacing Evidence Spine.
- Replacing Allagma run endpoints.
- Replacing Kanon human-gate/provenance contracts.
- Replacing Conexus model-call evidence routes.
- Full production security hardening.
- Raw prompt/completion export.
- Hidden reasoning export.
- Arbitrary runtime component execution.
- Immediate dependency on a .NET AG-UI SDK.
- Immediate CopilotKit migration.
