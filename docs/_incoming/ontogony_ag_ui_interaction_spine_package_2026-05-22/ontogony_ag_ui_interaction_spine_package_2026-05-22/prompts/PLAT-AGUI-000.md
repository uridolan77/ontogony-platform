# Implementation Prompt — PLAT-AGUI-000

Review this package and implement `PLAT-AGUI-000` in `uridolan77/ontogony-platform`.

Scope:

- Add `docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md`.
- Add JSON schemas for event and session v0.
- Add `docs/system/agent-interaction-event.matrix.json`.
- Add validation scripts for schemas and package fixtures.
- Add evidence doc proving validation.

Constraints:

- Do not change runtime services.
- Do not replace Evidence Spine.
- Explicitly document redaction and hidden reasoning rules.
- Keep schema v0 additive.

Acceptance:

- Fixture JSONL events validate.
- Matrix covers Allagma/Kanon/Conexus/frontend/UI/platform ownership.
- Contract links to existing Evidence Spine contract and service handoff docs.
