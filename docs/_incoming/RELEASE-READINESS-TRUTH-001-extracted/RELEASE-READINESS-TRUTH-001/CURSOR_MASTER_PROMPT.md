# Cursor Master Prompt — RELEASE-READINESS-TRUTH-001

You are working on the Ontogony operator console.

Implement **RELEASE-READINESS-TRUTH-001**: make the System → Release Readiness page honest, evidence-based, and impossible to misread as a real release-candidate certification when it is based on generated/demo artifacts.

## Context

The current release-readiness screen is useful as a route inventory, but its framing is too optimistic.

Observed console behavior:

- The page reports counts such as `Ready 24`, `Partial 8`, `Gap 0`.
- It explicitly says it renders `docs/generated/operator-release-readiness.json` produced by `npm run readiness:sync`.
- It explicitly says there is **no live backend client** and that RC posture is **mechanical, not semantic**.
- Some routes are marked `ready` while their data source is `fixture_only`.
- Some routes are marked `ready` while others are `live_with_fallback`, `unknown`, or partial without enough reason detail.
- The page can visually imply release confidence even though it is only a generated route-level artifact.

## Objective

Refactor the page so that it answers, at a glance:

1. Is this a real release-candidate readiness check or only a generated route coverage artifact?
2. Was live backend validation performed?
3. How fresh is the generated artifact?
4. Which routes are live, live-with-fallback, fixture-only, generated-only, or unknown?
5. Why is each partial route partial?
6. Which rows are blockers for release posture and which are merely documentation/coverage notes?
7. What command or next action should the operator run next?

## Required posture

This is a **truth-hardening** package, not a feature-expansion package.

Do not make the system look more ready than it is. Downgrade optimistic wording. Preserve useful generated route coverage, but label it correctly.

## Scope

Primary:
- `/system/release-readiness` page/component(s).
- Readiness artifact adapter/parser/view model.
- Route readiness table labels and status mapping.
- Summary cards and page header copy.
- Artifact freshness and source disclosure.
- Tests/fixtures around generated artifacts and misleading states.

Secondary:
- Shared status/taxonomy helpers if the release-readiness page already uses shared components.
- Documentation for future backend/live readiness work.

## Non-goals

- Do not implement a production release gate backend.
- Do not invent live readiness data in the frontend.
- Do not make fixture-only data count as release-ready.
- Do not remove the generated artifact page; reframe it accurately.
- Do not broaden into Kanon console polish, Evaluation dashboard polish, Agent Interaction, or Topology unless a tiny shared helper is already used here.
- Do not change route definitions beyond what the page already consumes.

## Implementation posture

Before editing:

1. Locate the route/component for `/system/release-readiness`.
2. Locate the generated artifact file and sync script, likely `docs/generated/operator-release-readiness.json` and `npm run readiness:sync`.
3. Inspect current readiness type definitions, adapters, route rows, data-source values, status badge components, and tests.
4. Compare this package to the real repo state. Discard stale instructions.

During editing:

- Prefer a small view-model layer over broad component rewrites.
- Centralize readiness classification in one helper.
- Keep language operational and blunt.
- Preserve existing styling and UI component conventions.
- Make impossible states visually clear, for example `fixture_only + ready` should become `demo_ready`, `not_release_ready`, or an explicit warning.

After editing:

- Run targeted tests for the page/helper.
- Run typecheck/build/lint where available.
- Regenerate readiness artifact only if the repo's workflow expects it.
- Document backend/live readiness follow-ups separately.

## Definition of done

The release-readiness page must no longer imply real release readiness from a generated artifact alone.

The page should clearly show:

- **Current page mode:** generated route readiness artifact.
- **Release-candidate posture:** not assessed / not release-ready unless live semantic validation exists.
- **Source:** `docs/generated/operator-release-readiness.json` plus generated timestamp.
- **Live backend validation:** absent, unavailable, or separately reported.
- **Artifact freshness:** fresh, stale, unknown, or future-dated.
- **Route rows:** data source, route status, release impact, reason, and next action.
- **Fixture-only rows:** never counted as release-ready.
- **Unknown rows:** treated as gaps or unresolved, not harmless partials.
