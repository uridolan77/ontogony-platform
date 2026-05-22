# Implementation Prompt — OUI-AGENT-001

Implement `OUI-AGENT-001` in `uridolan77/ontogony-ui`.

Scope:

- Add product-neutral `src/components/agent` package.
- Add `./agent` package export.
- Implement timeline, run header, message stream, tool-call card, interrupt card, state diff, evidence links, UI intent renderer.
- Add stories and tests.

Constraints:

- No product API DTO imports.
- Components receive view models only.
- Approval controls must be accessible.
- UI intent rendering must be registry-based.

Acceptance:

- `npm run check:exports` passes.
- Storybook covers success, interrupted, missing evidence, redacted payload, and UI intent fallback.
