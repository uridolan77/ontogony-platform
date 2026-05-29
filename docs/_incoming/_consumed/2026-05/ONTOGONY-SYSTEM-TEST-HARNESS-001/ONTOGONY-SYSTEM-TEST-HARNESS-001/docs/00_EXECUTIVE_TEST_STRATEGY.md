# Executive Testing Strategy

## Goal

Create a comprehensive test harness capable of replacing at least **50% of repeatable manual regression testing** across Ontogony.

This means automating the checks humans currently repeat after every backend/frontend change:

- Does each service start?
- Are health/readiness routes honest?
- Are protected routes actually protected?
- Do the major flows still work?
- Does a failed dependency create the right error?
- Are idempotent requests safe?
- Do run/event/decision records survive restart?
- Does the UI cover the backend capabilities?
- Are traces and correlation IDs available for debugging?

It does **not** mean replacing exploratory testing, product judgment, UX taste, architecture review, or philosophical/modeling judgment.

## Test pyramid for Ontogony

Ontogony needs a modified pyramid:

```text
                        Exploratory + human review
                     UI journey tests / console coverage
              Cross-service E2E / restart / idempotency / replay
          Service API integration / contract / auth / error envelope
      Unit tests / domain tests / fixtures / deterministic policy tests
 Static checks / architecture boundaries / dependency restrictions / lint
```

The critical layer is the middle: **service API + cross-service E2E**, because Ontogony's risk lives in seams, not only inside classes.

## Manual testing replacement target

| Manual activity | Automation replacement target | Harness mechanism |
|---|---:|---|
| Rechecking health/readiness after local stack start | 95% | readiness tests + scripts |
| Rechecking core API happy paths | 80% | service integration tests |
| Rechecking auth failures | 80% | negative auth matrix |
| Rechecking Allagma run lifecycle | 75% | system E2E tests |
| Rechecking Kanon policy/human gate behavior | 70% | authority tests + E2E gates |
| Rechecking Conexus completion/fallback/idempotency | 70% | gateway tests + fake providers |
| Rechecking restart survival | 60% | restart E2E scripts |
| Rechecking UI page presence/API calls | 60% | Playwright coverage tests |
| Rechecking observability | 50% | trace/metric/log assertions |
| Exploratory/UX/product judgment | 0-25% | automation can assist, not replace |

## Core principle

Every significant service flow should produce an evidence bundle:

```text
request payload
response payload
status code
trace/correlation IDs
service events / decisions / run state
assertion summary
timestamps
```

Evidence bundles are what make automated testing credible enough to replace manual checks.
