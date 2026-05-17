# Frontend Update Plan After Backend Work

For each backend service:

```bash
npm run openapi:gen
npm run openapi:check
npm run contracts:audit
npm run routes:check
npm run e2e:coverage:check
npm run check
npm run check:full
```

## Limitation banner policy

A limitation banner may be removed only when:

1. Backend route exists.
2. Route is in OpenAPI.
3. Frontend generated client sees it.
4. Wrapper/client path passes contract audit.
5. UI has success/error/empty/unauthorized/degraded states.
6. E2E covers happy and failure path.
7. Docs identify backend commit.

## Evidence export policy

Any new backend evidence response must be redacted either at backend source or in frontend builder before panel rendering, with `safeDisplayJson` defense-in-depth.
