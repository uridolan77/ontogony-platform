# Source package integration map

This package absorbs `AISTHESIS-LIVE-SPINE-RC-READINESS-005` rather than replacing it blindly.

| RC-readiness-005 file/theme | Integrated here as |
|---|---|
| `00_UNPACK_PROMPT.md` | `00_UNPACK_PROMPT.md`, expanded to RC certification |
| `01_PACKAGE_MANIFEST.md` | `01_PACKAGE_MANIFEST.md`, with source/review integration metadata |
| `02_CURRENT_STATE_BASELINE.md` | `02_INTEGRATED_BASELINE.md` |
| `03_SCOPE_AND_BOUNDARY.md` | `04_SCOPE_AND_BOUNDARY.md`, boundary preserved |
| `04_ACCEPTANCE_MATRIX.md` | `05_ACCEPTANCE_MATRIX.md`, expanded to certification gates |
| `05_TARGET_FILE_MAP.md` | `06_TARGET_FILE_MAP.md`, expanded with code/doc/script targets |
| `06_IMPLEMENTATION_PLAN.md` | `07_IMPLEMENTATION_PLAN.md`, expanded into 14 slices |
| `07_VALIDATION_MATRIX.md` | `08_VALIDATION_MATRIX.md` |
| `08_RELEASE_MODE_GATE.md` | `13_RELEASE_MODE_AND_LOCK_GATE.md` |
| `09_CI_FIVE_SERVICE_SMOKE.md` | `09_LIVE_FIVE_SERVICE_CERTIFICATION.md` + scripts |
| `10_LES_002_COMPLETION_PLAN.md` | `10_LES_001_LES_002_PROOF_DISCIPLINE.md` |
| `11_FRONTEND_CONTRACT_HANDOFF.md` | `17_FRONTEND_LIVE_SPINE_VALIDATION.md` |
| `12_PRODUCTION_IAM_GATE.md` | `14_PRODUCTION_IAM_AND_EDGE_AUTH_GATE.md` |
| `13_RETENTION_ERASURE_GATE.md` | `15_RETENTION_ERASURE_IMPLEMENTATION_GATE.md` |
| `14_OTEL_TRACE_EXPORT_GATE.md` | `16_OTEL_TRACE_EXPORT_GATE.md` |
| `15_LOCK_DECISION_FRAMEWORK.md` | `13_RELEASE_MODE_AND_LOCK_GATE.md` + closeout template |
| `16_RISK_REGISTER.md` | `19_RISK_REGISTER.md` |
| `17_ROLLBACK_PLAN.md` | `20_ROLLBACK_PLAN.md` |
| `18_CLOSEOUT_TEMPLATE.md` | `22_CLOSEOUT_TEMPLATE.md` |
| `19_CURSOR_EXECUTION_RULES.md` | `21_CURSOR_EXECUTION_RULES.md` |
| contracts | preserved and versioned upward where needed |
| scripts | preserved conceptually, upgraded to certification scripts |
| repo prompts | expanded for all producer/frontend/platform paths |

## Main additions beyond 005

1. Matrix v2, not just v1/v2 summary docs.
2. Durable evaluation/Krisis job plan.
3. Client coverage completion for evaluation routes.
4. Direct edge authorization hardening.
5. Real live five-service certification harness.
6. Frontend contract smoke, not just handoff.
7. Producer-specific v2 recheck prompts.
8. Platform lock review prompt.
