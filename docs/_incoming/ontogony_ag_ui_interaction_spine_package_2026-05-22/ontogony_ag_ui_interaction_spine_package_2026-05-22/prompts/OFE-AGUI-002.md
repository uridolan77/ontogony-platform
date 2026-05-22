# Implementation Prompt — OFE-AGUI-002

Implement `OFE-AGUI-002` in `uridolan77/ontogony-frontend`.

Scope:

- Add client-side adapters over existing APIs:
  - Allagma run interaction adapter.
  - Kanon decision/human-gate adapter.
  - Conexus model-call adapter.
  - Evidence Spine adapter.
  - Trace correlation adapter.
- Add lookup by run, trace, model call, decision, human gate.

Constraints:

- Use existing API clients.
- Missing downstream data must emit unresolved evidence events.
- Preserve source attempts where available.

Acceptance:

- Known run/model-call/decision/trace/humanGate can generate an interaction session.
- Embedded links open current operator surfaces.
