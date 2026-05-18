# ENV-UI-001 — ontogony-ui / ontogony-frontend integration readiness

Repos: `ontogony-ui`, `ontogony-frontend`

Goal: verify frontend can reliably consume the UI foundation for operator sanity pages.

Document:

- package/workspace/linking model
- build/check command
- frontend consumption path
- eval dashboard compatibility

Acceptance:

```powershell
# ontogony-ui
npm run check # or build/test equivalent

# ontogony-frontend
npm run check
npx playwright test e2e/allagma-eval-dashboards.spec.ts
```
