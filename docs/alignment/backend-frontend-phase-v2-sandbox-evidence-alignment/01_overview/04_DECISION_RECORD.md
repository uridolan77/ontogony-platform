# Decision Record

## Decision

After Allagma P3-SB-006, perform a bounded frontend/backend alignment bridge before eval-basing.

## Rationale

Eval-basing will depend on evidence visibility. If the frontend cannot show audit evidence, replay-safe state, side-effect ledger rows, and sandbox execute events, then eval-basing will be detached from operator workflows.

## Consequences

This phase is intentionally narrow. It should not become a broad redesign of Ontogony UI or all backend APIs.

The bridge should finish when:

- Allagma sandbox evidence is visible in frontend.
- The frontend uses typed contracts/adapters.
- Fallback fixtures reflect current backend truth.
- Limitation banners are accurate.
- A compatibility report proves backend/frontend alignment.

## Deferred

Eval scoring, agent topology selection, model quality feedback, full real tool execution, and retry/cancel/replay-trigger unless already supported.
