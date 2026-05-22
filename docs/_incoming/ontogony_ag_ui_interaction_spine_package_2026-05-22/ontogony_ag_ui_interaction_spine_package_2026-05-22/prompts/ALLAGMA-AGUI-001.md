# Implementation Prompt — ALLAGMA-AGUI-001

Implement `ALLAGMA-AGUI-001` in `uridolan77/allagma-dotnet`.

Scope:

- Add C# interaction event DTOs or reference platform schema shape.
- Add `IAgentInteractionEventProjector`.
- Project run/events/operations/audit/resume into schema-valid events.
- Add JSONL export or test fixture output.

Constraints:

- Do not replace existing run endpoints.
- Do not add live stream yet unless trivial after projection.
- Do not expose raw secrets or raw model content by default.

Acceptance:

- Projection tests pass for completed, failed, interrupted, and resumed runs.
- JSONL fixture validates against platform schema.
