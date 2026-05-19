# RP-004 — Frontend Real Provider Operator Visibility Prompt

```text
We are starting RP-004 — Frontend real-provider operator visibility.

Repos:
- uridolan77/ontogony-frontend
- uridolan77/ontogony-platform
- uridolan77/ontogony-ui only if shared primitives need minor updates

Goal:
Make real-provider mode visible and honest in operator UI without adding unsafe trigger behavior.

Boundary:
- Frontend/operator visibility only.
- No UI for entering provider secrets.
- No hidden real-provider trigger.
- No production readiness.
- No secrets.

Tasks:
1. Inspect eval dashboard, run detail, eval detail/export, Conexus observability links, and settings/health surfaces.
2. Add/refine provider-mode indicators:
   - fake/local
   - real-provider local validation
   - unavailable
   - unauthorized/missing key
   - rate-limited/provider error
3. Ensure real-provider run evidence is visible via journey links.
4. Preserve fixture/live/degraded distinctions.
5. Add tests for state mapping and degraded states.

Deliver:
- frontend docs/evidence/RP_004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY_EVIDENCE.md
- platform docs/evidence/RP_004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY_EVIDENCE.md

Acceptance:
- operator can tell fake vs real-provider local validation
- provider errors are visible
- no secret-entry UI
- fixture/live/degraded clarity preserved
- not production readiness

Suggested commits:
- feat(ui): RP-004 show real provider validation state
- docs(product): RP-004 record frontend real provider visibility
```
