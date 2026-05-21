# FIRST-VERSION-RC-001 Closeout

**Date:** 2026-05-21  
**Candidate baseline:** **OPERATOR-V1-001** (runtime floor: **SYSTEM-ALPHA-006**)  
**Verdict:** PASS

## Locked commits

| Repo | SHA |
| --- | --- |
| `allagma-dotnet` | `7c94c57ae2b008ae0b7b871ea71fd33c5c97c67d` |
| `kanon-dotnet` | `ab12715406480f3eaed715c27efe20dc37216816` |
| `conexus-dotnet` | `9ec193082439438d2aba36bb3c446242ac03105f` |
| `ontogony-platform` | `18717e2e257a14ae6de4128089129ce333173ec6` |
| `ontogony-frontend` | `4faf005e31fd7987c5e883203d69af8b366c9da1` |
| `ontogony-ui` | `6f357d9caa752c1e4a93ccd2f9d0c49e46ae7cb4` |

Lock file: [`docs/system/ontogony-operator-v1.lock.json`](../system/ontogony-operator-v1.lock.json)

## Operator capabilities

| Capability | Status | Evidence |
| --- | --- | --- |
| Operator home | PASS | [SYS_OPERATOR_HOME_001](../../../ontogony-frontend/docs/evidence/SYS_OPERATOR_HOME_001_EVIDENCE.md) |
| Guided run workbench | PASS | [ALLAGMA_OPERATOR_RUNS_001](../../../ontogony-frontend/docs/evidence/ALLAGMA_OPERATOR_RUNS_001_EVIDENCE.md) |
| Human gates | PASS | demo flows |
| Evidence Spine | PASS | [EVIDENCE_SPINE_OPERATOR_001](../../../ontogony-frontend/docs/evidence/EVIDENCE_SPINE_OPERATOR_001_EVIDENCE.md) |
| Conexus operator | PASS | [CONEXUS_OPERATOR_001](../evidence/CONEXUS_OPERATOR_001_EVIDENCE.md) |
| Sandbox workbench | PASS | [ALLAGMA_SANDBOX_WORKBENCH_001](../evidence/ALLAGMA_SANDBOX_WORKBENCH_001_EVIDENCE.md) |
| Demo flows | PASS | [SYSTEM_DEMO_FLOWS_001](../evidence/SYSTEM_DEMO_FLOWS_001_EVIDENCE.md) |

## Validation

| Gate | Result | Artifact |
| --- | --- | --- |
| Platform tests | PASS | `Ontogony.Platform.sln` |
| Allagma tests | PASS | `Allagma.sln` |
| Kanon tests | PASS | `kanon-dotnet` |
| Conexus tests | PASS | `conexus-dotnet` |
| Frontend check | PASS | `npm run check` |
| UI check | PASS | `ontogony-ui` `npm run check` |
| Demo Playwright | PASS | `test:e2e:demo-flows` 8/8 |
| Docker cohesion | PASS | ALPHA-006 `artifacts/system-e2e/...` |
| Restart | PASS | ALPHA-006 restart evidence |
| Observability | PASS | ALPHA-006 observability summary |
| Evidence Spine live | PASS | prior 009A evidence |
| Protocol / stale / real-tools | PASS | platform validators |

## Safety

- [x] Real external execution remains blocked.
- [x] No production-readiness claim.
- [x] No enterprise IAM claim.
- [x] No semantic authority moved out of Kanon.

## Release statement

```text
OPERATOR-V1-001 is PASS for the first Docker-local operator product scope on SYSTEM-ALPHA-006.
It is not a production-readiness claim.
Real external tool execution remains blocked.
```
