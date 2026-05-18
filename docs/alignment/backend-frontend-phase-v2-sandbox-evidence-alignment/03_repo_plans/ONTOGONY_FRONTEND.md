# Repo Plan — ontogony-frontend

## Objective
Consume and render Allagma Phase 3 sandbox evidence without regressing the operator-grade frontend.

## PRs

### FE-FBA2-001 — Allagma contract refresh
Refresh OpenAPI snapshot, regenerate client if applicable, update provenance docs, add type-level tests.

### FE-FBA2-002 — Sandbox evidence adapter
Add adapter from backend audit bundle to UI model. It must tolerate missing `SandboxEvidence`, map ledger rows, normalize marker path, classify replay-safe status, classify event statuses, and redact unsafe fields.

### FE-FBA2-003 — Audit/replay UI card
Add Sandbox Evidence section to Allagma run audit/replay detail.

### FE-FBA2-004 — Sandbox execute timeline
Render five sandbox execute events in existing run event timeline.

### FE-FBA2-005 — Capability-driven banners
Replace hardcoded/ambiguous Allagma sandbox limitation banners with backend capability truth.

### FE-FBA2-006 — Fixtures and E2E
Add fixture audit bundle with sandbox evidence, fixture timeline with all five events, route test/e2e smoke, and evidence docs.
