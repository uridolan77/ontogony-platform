# Implementation Prompt — OFE-AGUI-001

Implement `OFE-AGUI-001` in `uridolan77/ontogony-frontend`.

Scope:

- Add `src/agent-interaction` module.
- Add fixture JSONL adapter and deterministic reducer.
- Add `/system/agent-interaction` route with fixture replay.
- Use `@ontogony/ui/agent` components.
- Add tests for reducer and fixtures.

Constraints:

- Do not require backend changes.
- Do not duplicate Evidence Spine graph logic.
- Keep unknown custom events visible.

Acceptance:

- Workbench replays `sample-run.jsonl` and `sample-human-gate-interrupt.jsonl`.
- Reducer reconstructs messages, status, open interrupts, state, evidence links.
