# Implementation Plan — RELEASE-READINESS-TRUTH-001

## Phase 0 — Inspect current state

Locate:

- `/system/release-readiness` route/page/component.
- Generated artifact source, likely `docs/generated/operator-release-readiness.json`.
- `npm run readiness:sync` script.
- Existing route readiness types.
- Existing status/data-source badge components.
- Tests for release readiness or system pages.

Produce a short internal note before editing:

```text
Current component:
Current adapter/helper:
Current generated artifact shape:
Current tests:
Shared components used:
```

## Phase 1 — Add a release-readiness view model

Create or update a small adapter that maps raw generated artifact rows into a truthful view model.

Suggested model:

```ts
export type ReadinessDataSource =
  | 'live'
  | 'live_with_fallback'
  | 'fixture_only'
  | 'generated_only'
  | 'unknown';

export type RouteArtifactStatus = 'ready' | 'partial' | 'gap' | 'unknown';

export type ReleaseImpact =
  | 'release_ready_candidate'
  | 'needs_live_validation'
  | 'not_release_ready'
  | 'demo_only'
  | 'unresolved';

export interface ReleaseReadinessRouteViewModel {
  route: string;
  area: string;
  dataSource: ReadinessDataSource;
  artifactStatus: RouteArtifactStatus;
  releaseImpact: ReleaseImpact;
  reason: string;
  nextAction: string;
  isBlocking: boolean;
}
```

Do not force these exact names if the repo has existing conventions. Preserve local naming.

## Phase 2 — Separate generated route coverage from RC posture

Change the page header to distinguish:

1. **Generated route artifact**: available/stale/missing.
2. **Live backend validation**: not run / unavailable / partial / passed.
3. **Release-candidate posture**: not assessed / not ready / candidate / passed.

Recommended current default for the raw observed state:

```text
Release-candidate posture: Not assessed
Reason: This page is currently generated from route metadata. No live semantic/backend validation is attached to this scorecard.
```

## Phase 3 — Rework summary cards

Replace or qualify `Ready / Partial / Gap` cards.

Better cards:

```text
Generated routes
Ready in artifact
Needs review
Fixture/demo only
Unknown source
Live validation
RC posture
```

If the existing UI must keep Ready/Partial/Gap, prefix or subtitle them:

```text
Artifact ready
Artifact partial
Artifact gaps
```

Never let `Ready 24` stand alone.

## Phase 4 — Enforce data-source semantics

Rules:

```text
fixture_only + ready => demo_only, not release-ready
unknown + any status => unresolved, blocking or needs review
live_with_fallback + ready => needs review unless fallback is explicitly non-critical
live + ready => eligible for release-ready candidate, but only if live validation exists
```

Implement this in one helper and cover with tests.

## Phase 5 — Add route-level reasons and next actions

For every route row, show:

- route
- area
- data source
- artifact status
- release impact
- reason
- next action

Examples:

```text
/system/release-readiness
Data source: fixture_only
Artifact status: ready
Release impact: demo_only
Reason: Page renders generated artifact only; no live release validation.
Next action: Add live validation or keep this route excluded from RC posture.
```

```text
/allagma/runs/:runId/audit
Data source: unknown
Artifact status: partial
Release impact: unresolved
Reason: Data source classification is missing.
Next action: Classify as live, fallback, fixture, generated, or unsupported.
```

## Phase 6 — Add artifact freshness

Compute freshness from artifact generated timestamp.

Suggested categories:

```text
fresh       <= 24 hours old
aging       > 24 hours and <= 7 days
stale       > 7 days
future      generatedAt is in the future
unknown     no timestamp or invalid timestamp
```

Use repo conventions if a time helper already exists.

Show:

```text
Generated: 2026-05-23T17:18:00.362Z
Freshness: fresh / stale / unknown
Command: npm run readiness:sync
```

## Phase 7 — Tests and fixtures

Add tests for:

- `fixture_only + ready` is not release-ready.
- `unknown` is unresolved.
- `live_with_fallback + ready` is not silently release-ready.
- generated artifact summary labels cards as artifact counts.
- stale/future/invalid timestamps.
- partial rows include reasons and next actions.

## Phase 8 — Documentation

Add or update a small docs note:

```text
docs/operators/RELEASE_READINESS_TRUTH.md
```

Content:

- generated route artifact purpose
- difference between artifact readiness and release-candidate readiness
- how to regenerate artifact
- what live validation would require later
- why fixture-only rows do not count as release-ready

Only add docs where the repo normally stores operator docs.
