# Source Findings — RELEASE-READINESS-TRUTH-001

This package is based on the raw Ontogony console review provided by the operator.

## Release-readiness page findings

The console shows a System → Release Readiness page with the following content pattern:

```text
Fixture / demo only: Renders docs/generated/operator-release-readiness.json produced by npm run readiness:sync. No live backend client; RC posture is mechanical, not semantic.

Release readiness
Per-route operator readiness scorecard (FE-9-007). Regenerate with npm run readiness:sync.

Ready 24
Partial 8
Gap 0
```

It then lists routes and areas, for example:

```text
/                              system   live_with_fallback   ready
/system                        system   live_with_fallback   partial
/system/topology               system   live_with_fallback   ready
/system/release-readiness      system   fixture_only         ready
/settings                      system   live                 partial
/system/evidence-spine         system   live                 ready
/system/agent-interaction      system   live_with_fallback   partial
...
/allagma/runs/:runId/audit     allagma  unknown              partial
```

## Core problems

### 1. Generated artifact is framed as release readiness

The page admits that it renders a generated JSON artifact and has no live backend client, but the dominant visual language is still `Ready / Partial / Gap`.

This creates a false release-confidence signal.

### 2. `fixture_only` route can be `ready`

The route `/system/release-readiness` is shown as `fixture_only` and `ready`. That may be true as a demo rendering route, but it is not true as release readiness.

### 3. `Gap 0` is misleading

If there is no live backend client and RC posture is mechanical, then `Gap 0` can only mean `Gap 0 in the generated route artifact`, not `Gap 0 for release`.

### 4. Partial routes lack reasons

Rows like `/settings`, `/system`, `/system/agent-interaction`, `/kanon/assistance`, `/kanon/review-queue`, `/kanon/policies`, `/allagma/audit`, and `/allagma/runs/:runId/audit` are partial, but the table does not tell the operator what exactly is missing.

### 5. Data source status needs stronger semantics

The values `live`, `live_with_fallback`, `fixture_only`, and `unknown` are useful but under-enforced.

Recommended semantics:

```text
live              = values came from backend/client call
live_with_fallback = backend attempted, fallback filled missing/failed pieces
fixture_only      = static/demo/generated fixture, not live evidence
unknown           = route/source not classified; should be treated as unresolved
```

### 6. Artifact freshness is visible but not interpreted

The artifact shows a generation timestamp, but the page should say whether that artifact is fresh, stale, unknown, or future-dated relative to local time/build time/service health checks.

### 7. RC posture needs a separate status

The page should distinguish:

- route inventory status
- operator page rendering status
- backend live validation status
- release-candidate posture

These are currently collapsed.

## Related system findings

From settings/topology console text:

- Services are healthy but health payloads have format warnings.
- No version metadata is returned by health endpoints.
- Trace bridge is ready to test but not proven.
- Conexus may be healthy while strict readiness is not ready.

These reinforce the need to prevent a generated route artifact from looking like release readiness.

## Implementation principle

Every green badge must be backed by the kind of evidence it claims.

If evidence is generated-only, say generated-only. If no live validation ran, do not imply release readiness.
