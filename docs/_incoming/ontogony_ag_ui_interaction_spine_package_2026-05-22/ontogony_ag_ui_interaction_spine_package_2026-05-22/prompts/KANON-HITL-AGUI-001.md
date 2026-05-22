# Implementation Prompt — KANON-HITL-AGUI-001

Implement `KANON-HITL-AGUI-001` in `uridolan77/kanon-dotnet`.

Scope:

- Document and test human-gate to interaction-interrupt mapping.
- Define approval/rejection/approval-with-edits response schemas.
- Ensure decision/provenance links are represented as evidence links.

Constraints:

- Kanon remains semantic authority; it does not own Allagma run continuation.
- No Conexus hydration inside Kanon.

Acceptance:

- `humanGateId` maps to pending interrupt.
- approval/rejection maps to resolved interrupt and decision evidence.
- Invalid/stale payload behavior documented.
