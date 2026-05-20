# EVIDENCE-SPINE-008 — E2E browser and Docker verification evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (mocked Playwright); Docker-local checklist scripted  
**Statement:** Browser e2e proves paste-any-ID resolution for run, evaluation, model call, and decision identifiers, partial missing-edge honesty, and redacted export bundles. Docker-local stale-bundle risk is controlled via `verify-frontend-browser-provenance.ps1 -Build`.

## Delivered

| Artifact | Path |
| --- | --- |
| Playwright spec | `ontogony-frontend/e2e/evidence-spine-workbench.spec.ts` |
| Mock scenarios | `evidence-spine`, `evidence-spine-partial` in `e2e/helpers/mockServices.ts` |
| Conexus mocks | `conexusModelCallDetail`, `conexusRouteDecisionDetail` in `e2e/helpers/mockData.ts` |
| Docker checklist script | `docker/local-working-system/scripts/run-evidence-spine-docker-local-verification.ps1` |
| Frontend evidence index | `ontogony-frontend/docs/evidence/EVIDENCE_SPINE_008_E2E_BROWSER_VERIFICATION_EVIDENCE.md` |

## Playwright coverage

| Case | Identifier | Assertion |
| --- | --- | --- |
| Run lookup | `run-123` | Allagma run, Kanon decision, Conexus model call nodes |
| Eval lookup | `eval-correlation-001` | Evaluation + subject run nodes |
| Model call lookup | `chatcmpl-123` | Conexus model call + trace nodes |
| Decision lookup | `decision-plan-001` | Kanon decision + Allagma run nodes |
| Partial graph | run without planning decision | Missing links panel |
| Export bundle | run id | Schema id in panel + preview JSON |

## Validation

```powershell
cd C:\dev\ontogony-frontend
npm run typecheck
npm test -- src/evidence-spine
npm run test:e2e -- e2e/evidence-spine-workbench.spec.ts

cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-evidence-spine-docker-local-verification.ps1 -Build
```

## Docker-local provenance

Uses [DOCKER-LOCAL-VERIFY-001](DOCKER_LOCAL_VERIFY_001_FRONTEND_BROWSER_PROVENANCE_EVIDENCE.md): rebuild frontend image from git HEAD before manual browser checks.

## Next

- **EVIDENCE-SPINE-009** — package closeout
