# Implementation plan — KANON-CONSOLE-POLISH-001

## Phase 0 — Inventory actual code

Search the frontend for the visible Kanon text and locate the current components, hooks, adapters, types, and tests.

Recommended search terms:

```text
Conexus assistance workbench
Draft-only model assistance
Domain packs
Lifecycle history
Operator actor context
Domain-pack read
Provenance read
Review Queue
Policies
Force-redact fields
Evidence spine
```

Deliverable: a short internal note in the PR summary naming the files actually changed.

## Phase 1 — Shared Kanon status and copy primitives

Create or reuse small primitives for:

- `KanonAuthorityNotice`
- `DraftOnlyAssistanceNotice`
- `UnavailableActionReason`
- `EvidenceLinkGroup`
- `PartialStateReason`
- `LifecycleStatusBadge`

Do not over-generalize. Prefer boring components with clear props.

## Phase 2 — Assistance workbench safety polish

Work items:

1. Replace secret-like default sample context.
2. Split allowed fields from force-redacted fields in copy and examples.
3. Add redaction preview.
4. Improve result state with decision/evidence links.
5. Add tests.

Safe default sample:

```json
{
  "summary": "Operator review context for a non-secret semantic change.",
  "sourceType": "domain-pack",
  "changeIntent": "review-notes"
}
```

Suggested default fields:

```text
Allowed fields: summary,sourceType,changeIntent
Force-redact fields: none
```

If the UI requires a force-redact example, use a non-default expandable example with a neutral name such as `internalNote`, not `apiKey`, `token`, `password`, or `secret`.

## Phase 3 — Domain-pack lifecycle polish

Work items:

1. Restructure summary cards.
2. Separate pack inventory and active ontology versions.
3. Improve selected pack panel.
4. Inline unavailable action reasons.
5. Improve lifecycle timeline labels.
6. Add tests.

Proposed page sections:

```text
1. Operator actor context
2. Domain-pack inventory
3. Selected pack version
4. Lifecycle actions
5. Diff / impact / migration simulations
6. Lifecycle timeline
7. Evidence / decision links
```

Domain-pack inventory should distinguish:

```text
Packs on disk
Persisted lifecycle versions
Active ontology versions
Generated/test-looking versions, if detected
```

Detection of generated/test-looking names must be defensive. If no backend metadata exists, label as `name pattern warning`, not definitive test state.

## Phase 4 — Settings semantic health polish

Work items:

1. Deduplicate local credential warnings.
2. Replace `unknown source` where storage mode is known.
3. Add role-preset guidance.
4. Keep Kanon semantic settings health compact and actionable.

Suggested role preset copy:

```text
Local admin: Admin — mutation-capable local operator.
Read-only semantic authority: Auditor, ProvenanceReader — inspect packs, provenance, decisions, and evidence without lifecycle mutation.
System service: System — service-to-service automation where explicitly configured.
```

## Phase 5 — Review Queue / Policies partial-state reasons

Work items:

1. Find current partial/empty state rendering.
2. Add reason codes.
3. Render next action.
4. Add tests.

Reason codes should include:

```text
no_items
not_configured
insufficient_role
backend_route_missing
generated_client_missing
fixture_only
live_fallback
not_in_scope
unknown
```

## Phase 6 — Evidence-link consistency

Work items:

1. Find all Kanon evidence/decision/provenance links.
2. Create consistent labeling convention.
3. Add accessible copy labels and copy success feedback.
4. Ensure links pass the right identifier kind into Evidence Spine.

## Phase 7 — Tests and docs

Required commands depend on repo setup. Use existing commands. Likely examples:

```bash
npm test -- src/kanon
npm test -- src/settings
npm run test
npm run lint
npm run typecheck
```

Do not invent commands in the PR summary. Report the actual commands run.
