# Quality Gates

## Gate A — Local developer gate

A developer should run this before pushing a meaningful service change:

```text
static checks
unit tests
service integration smoke
readiness check
```

Pass criteria:

- No architecture boundary violations.
- No broken unit/domain tests.
- Changed service starts.
- Health/readiness behave correctly.

## Gate B — Pull request gate

CI should run:

```text
static checks
unit tests
service integration tests
contract tests against changed service
frontend build if API contracts changed
```

Pass criteria:

- No contract drift without explicit approval.
- Error envelopes remain stable.
- Auth behavior remains stable.
- No new undocumented endpoints.

## Gate C — System regression gate

Run before merging integration-heavy changes:

```text
Kanon + Conexus + Allagma + Postgres
Allagma governed run E2E
human gate E2E
idempotency E2E
Kanon assistance E2E
Conexus fallback E2E
trace/correlation E2E
```

Pass criteria:

- Core E2E flows pass.
- Evidence bundle produced.
- No duplicate side effects on retry.
- Run/events/decisions correlate across services.

## Gate D — Release candidate gate

Run before release/demo:

```text
full system E2E
UI E2E
restart survival
migration smoke
load smoke
security negative matrix
observability assertions
```

Pass criteria:

- Critical and high severity tests pass.
- Medium severity failures have explicit waiver.
- Evidence bundle stored under release version.
- Manual test checklist reduced to exploratory/product review only.

## Failure severity

| Severity | Meaning | Merge policy |
|---|---|---|
| Critical | data corruption, unsafe side effect, auth bypass, broken core E2E | block |
| High | major service flow broken, idempotency broken, restart data loss | block |
| Medium | non-core route regression, degraded UX, missing telemetry | require owner sign-off |
| Low | docs/test naming/minor diagnostic weakness | may merge with issue |
