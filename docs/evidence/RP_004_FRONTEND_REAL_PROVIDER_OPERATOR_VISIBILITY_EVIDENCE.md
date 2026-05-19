# RP-004 — Frontend real provider operator visibility evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — frontend operator visibility for fake vs real-provider local validation

**Boundary:** Tracking and acceptance for RP-004. **No platform runtime changes.** **No secrets.** **Not production readiness.**

## Cross-repo deliverables

| Repo | Artifact |
| --- | --- |
| `ontogony-frontend` | Provider validation state resolver, banners, Conexus posture card, surface integrations |
| `ontogony-frontend` | [`RP_004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY_EVIDENCE.md`](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/evidence/RP_004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY_EVIDENCE.md) |
| `ontogony-platform` | This evidence record |

## Acceptance checklist (RP-004)

| Item | Result |
| --- | --- |
| Frontend shows fake vs real-provider validation state | **PASS** |
| Provider errors are visible | **PASS** |
| No UI secret entry | **PASS** |
| Fixture/live/degraded clarity preserved | **PASS** |

## Operator surfaces

Operators can read provider posture on:

- Allagma evaluations dashboard (live)
- Allagma run and evaluation detail (route metrics)
- Conexus observability and routing
- Operator settings (Conexus section)

Policy reference: [`REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](../operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md).

## Prerequisites satisfied

| Prior item | Status |
| --- | --- |
| RP-003 Allagma guided flow | **DONE** |
| RP-002 Conexus real-provider local mode | **DONE** |
| RP-001 secret/budget gates | **DONE** |

## Next step

**`RP-005`** — real-provider manual QA execution (`prompts/RP-005_REAL_PROVIDER_MANUAL_QA_EXECUTION.md`).

## Required statement

```text
RP-004 makes real-provider local validation visible in the operator frontend without enabling secrets in the browser.
Fake provider remains the documented default. No CI real-provider calls. Not production readiness.
```
