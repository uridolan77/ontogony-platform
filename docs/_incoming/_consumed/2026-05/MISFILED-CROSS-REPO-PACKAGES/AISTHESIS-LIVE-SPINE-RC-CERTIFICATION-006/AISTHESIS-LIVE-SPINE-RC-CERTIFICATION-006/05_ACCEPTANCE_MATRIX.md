# Acceptance matrix

## Mandatory for RC-certification candidate

| Gate | Required result |
|---|---|
| Aisthesis restore/build/test | PASS, Release |
| Aisthesis test failures | 0 |
| Fixture smoke | PASS |
| Required-edge v1 fixture | complete, missing 0 |
| Required-edge v2 fixture | complete or explicitly partial with reason |
| Bundle fingerprint stability | PASS |
| Aisthesis.Client route coverage | complete, or server-only exceptions documented |
| Evaluation/Krisis durability decision | implemented or explicit accepted deferral |
| Direct edge auth hardening | implemented or explicit accepted blocker/non-blocker decision |
| LES-001 | complete or superseded by complete live trace |
| LES-002 | complete or accepted partial with exact dimension rationale |
| Five-service live certification script | exists, documented, honest PASS/NOT_RUN/FAIL |
| ReleaseMode runbook | exists and config expectations are tested/documented |
| Frontend validation | route-compatible smoke or explicit accepted handoff |
| IAM gate | documented, with production blocker flag |
| Retention/erasure gate | documented, with production blocker flag |
| OTel gate | documented, with production blocker flag |
| Lock decision | evidence-based, no unsupported claim |
| Closeout | complete and synchronized with actual results |

## Mandatory for `lockRequired: true`

| Gate | Required result |
|---|---|
| Aisthesis Release build/test | PASS |
| Producer repo Release gates | PASS or accepted exception with owner/date |
| Five-service live certification | PASS, not NOT_RUN |
| Required-edge v2 | PASS for applicable workflow profile |
| LES-001 / replacement trace | complete |
| LES-002 | complete or accepted partial with no blockers |
| Frontend live backing | validated against running Aisthesis |
| Production IAM | implemented or explicitly out of RC lock scope |
| Retention/erasure | implemented or explicitly out of RC lock scope |
| OTel export | implemented or explicitly out of RC lock scope |
| Deferrals | non-blocking and owner-assigned |

## Closure classifications

### RC-certification candidate

All mandatory gates pass. Live five-service certification either passes or has an accepted environment-only reason, and lock review recommends candidate.

### RC-certification partial

Core Aisthesis gates pass, but live five-service, frontend, IAM, retention, OTel, producer gates, or matrix v2 remain incomplete.

### Blocked

Build/tests/smoke fail, LES-001 regresses, required-edge v1 breaks, or evidence spine cannot reconstruct native producer traces.
