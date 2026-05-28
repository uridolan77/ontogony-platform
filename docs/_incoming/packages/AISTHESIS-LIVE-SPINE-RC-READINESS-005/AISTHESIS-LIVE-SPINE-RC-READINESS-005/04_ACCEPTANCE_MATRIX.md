# Acceptance matrix

## Mandatory for RC-readiness candidate

| Gate | Required result |
|---|---|
| Aisthesis restore/build/test | PASS |
| Aisthesis tests | 0 failures |
| Aisthesis fixture smoke | PASS |
| Required-edge fixture | present 10, missing 0, grade complete |
| Bundle fingerprint stability | test-proven |
| LES-001 rerun or validated recent artifact | PASS / complete / 0 blockers |
| LES-002 | complete OR accepted partial rationale with 0 blockers |
| CI smoke script | exists and is documented |
| ReleaseMode runbook | exists and defines expected config |
| Lock decision doc | exists and includes evidence-based decision |
| Deferrals | explicitly documented |

## Required before `lockRequired: true`

| Gate | Required result |
|---|---|
| Full solution Release build/test for Aisthesis | PASS |
| Full Release gates for producer repos | PASS or explicitly accepted exception |
| CI-compatible five-service smoke | PASS in CI or documented as blocked |
| ReleaseMode smoke | PASS |
| LES-001 | complete |
| LES-002 | complete or justified partial accepted by lock review |
| Frontend trace UI | at least route-compatible or handoff accepted |
| Production IAM | implemented or explicitly not required for lock scope |
| Retention/erasure | implemented or explicitly not required for lock scope |
| OTel export | implemented or explicitly not required for lock scope |

## Closure classifications

### RC-ready candidate

All mandatory gates pass, and lock-decision doc says Aisthesis may proceed to RC review.

### RC-readiness partial

Core Aisthesis gates pass, but live CI / ReleaseMode / LES-002 / cross-repo gates remain incomplete.

### Blocked

Aisthesis tests or fixture smoke fail, or LES-001 no longer produces reconstructable live evidence.
