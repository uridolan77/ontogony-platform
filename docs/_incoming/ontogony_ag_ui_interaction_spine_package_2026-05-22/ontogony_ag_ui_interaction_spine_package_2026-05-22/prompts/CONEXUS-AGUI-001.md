# Implementation Prompt — CONEXUS-AGUI-001

Implement `CONEXUS-AGUI-001` in `uridolan77/conexus-dotnet`.

Scope:

- Map existing model-call evidence flow into interaction event projection.
- Produce JSONL fixture from fake provider call.
- Link route decision, provider attempts, usage/cost, evidence bundle.

Constraints:

- No raw prompt/completion export by default.
- Do not add mutation routes.
- Do not make Conexus semantic authority.

Acceptance:

- Fake provider model call emits valid model-call events.
- Route decision, trace id, correlation id, and model call ids align.
