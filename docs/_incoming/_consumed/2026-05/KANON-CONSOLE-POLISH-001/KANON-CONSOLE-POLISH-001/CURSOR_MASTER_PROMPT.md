# Cursor master prompt — KANON-CONSOLE-POLISH-001

You are working in the Ontogony frontend/operator-console codebase. Implement **KANON-CONSOLE-POLISH-001** as a focused frontend polish sprint for Kanon-facing console surfaces.

## Mission

Make the Kanon console pages more truthful, safer, and more operator-grade without expanding backend capability.

Treat Kanon as semantic authority. The UI must distinguish:

- authoritative semantic state;
- draft-only model assistance;
- simulated domain-pack analysis;
- local browser operator settings;
- fixture/demo/generated artifacts;
- unresolved or partial evidence.

## Scope

Primary repo: `ontogony-frontend`.
Optional shared UI repo: `ontogony-ui`, only for reusable primitives that are clearly shared.
Avoid backend changes. If inspection reveals a missing backend contract, document it as a follow-up instead of implementing backend expansion.

## First step: inspect actual code

Before editing, inspect the current implementation and tests for:

- Kanon assistance page / Conexus assistance workbench.
- Kanon domain packs page.
- Kanon review queue page.
- Kanon policies page.
- Kanon semantic settings health on Settings page.
- shared status/badge/readiness components.
- Evidence Spine link helper components.
- existing test framework and page tests.

Likely paths may include, but are not guaranteed to be exactly:

```text
src/kanon/**
src/settings/**
src/system/**
src/evidence-spine/**
src/components/**
src/lib/**
```

Do not assume names. Search by visible copy from the console review: `Conexus assistance workbench`, `Domain packs`, `Operator actor context`, `Lifecycle history`, `Force-redact fields`, `health payload format warning`, `review queue`, `policies`.

## Implementation priorities

### 1. Kanon assistance safety polish

- Remove default sample context containing secret-like values, especially `apiKey`, `secret-live-key`, or any string that teaches users to paste secrets.
- Do not include secret-like fields in default `Allowed fields` examples.
- Add or improve redaction preview before submit.
- Clearly label all assistance as `draft_only`, `non-authoritative`, and `requires governed human review before acceptance`.
- After a request succeeds, show the Kanon decision link and evidence/provenance link when available.

### 2. Domain-pack lifecycle polish

- Separate these concepts visually:
  - packs on disk;
  - persisted pack versions;
  - active ontology versions;
  - selected pack/version;
  - lifecycle timeline;
  - simulation outputs.
- Avoid implying that generated/test-looking versions are normal user-facing active packs.
- Add filter or grouping for test/generated packs if such metadata exists; otherwise display a clear warning when pack names look generated.
- Disable unavailable lifecycle actions inline and show the reason next to the button or in accessible tooltip/help text.
- Make lifecycle vocabulary coherent: `draft`, `validated`, `reviewed`, `accepted`, `loaded`, `active`, `deprecated` where supported by real data. Do not invent backend states; map unknown states explicitly.

### 3. Kanon settings/status polish

- Reduce repeated credential/local-storage warnings.
- Replace `unknown source` for stored credentials with a clear source enum if the data exists: `session`, `local`, `default`, `env`, `not_set`, or `unknown` only as a true fallback.
- Clarify actor-role presets: local admin vs read-only semantic authority.
- Keep warning truth: if `/health` returns a non-JSON or non-contract payload, keep the warning, but explain it once and link to diagnostics.

### 4. Review Queue and Policies partial states

- Partial pages must show precise reasons: missing backend route, no items, insufficient role, not configured, fixture-only fallback, generated client missing, or not yet implemented.
- Empty states must guide the operator to the next action.
- Do not render a vague `partial` status without a reason.

### 5. Evidence links and decision provenance

- Use one consistent component/pattern for Kanon decision links, provenance links, replay-bundle links, and Evidence Spine links.
- Link text must say what will be opened: `Open planning decision`, `Open pack load decision`, `Open Evidence Spine for decision`, etc.
- Copy buttons should have accessible labels and success feedback.

## Tests required

Add or update tests covering:

- Assistance default sample contains no secret-like fields or values.
- Assistance redaction preview shows forced-redacted fields before submit.
- Domain-pack unavailable actions are disabled and include reason text.
- Domain-pack summary does not conflate disk packs with active ontology versions.
- Partial Review Queue / Policies states include reason text.
- Credential source labels do not show `unknown source` when a storage mode is known.
- Evidence links have contextual accessible names.

Use existing test conventions. Do not add a new test framework.

## Acceptance criteria

The console should look less “demo optimistic” and more like a semantic-authority operator workbench. No green status should be achieved by hiding missing evidence. Every unresolved/partial/unavailable state must explain itself.

## Final output expected from Cursor

When done, produce:

1. Summary of changed files.
2. Screens/pages touched.
3. Tests added/updated and results.
4. Any backend follow-ups discovered but not implemented.
5. Remaining known limitations.
