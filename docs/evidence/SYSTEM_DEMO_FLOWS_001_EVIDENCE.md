# SYSTEM-DEMO-FLOWS-001 evidence

**Date:** 2026-05-21  
**Repos:** `ontogony-platform`, `ontogony-frontend`  
**Verdict:** PASS

## Summary

Delivered operator V1 demo package: [`OPERATOR_V1_DEMO_GUIDE.md`](../operators/OPERATOR_V1_DEMO_GUIDE.md), [`operator-demo-flow-catalog.json`](../operators/operator-demo-flow-catalog.json), Docker-local demo id export scripts, and mocked Playwright coverage in `ontogony-frontend`.

## Files changed

| Repo | Files |
| --- | --- |
| `ontogony-platform` | `docs/operators/OPERATOR_V1_DEMO_GUIDE.md`, `docs/operators/operator-demo-flow-catalog.json`, `docs/operators/README.md`, `docker/local-working-system/scripts/run-operator-v1-demo-prep.ps1`, `docker/local-working-system/scripts/write-operator-v1-demo-ids.ps1`, `docs/evidence/SYSTEM_DEMO_FLOWS_001_EVIDENCE.md` |
| `ontogony-frontend` | `e2e/operator-v1-demo-flows.spec.ts`, mock helpers, `package.json`, `docs/evidence/SYSTEM_DEMO_FLOWS_001_EVIDENCE.md`, `docs/operators/FRONTEND_DOCKER_LOCAL_CONTRACT.md`, `e2e/coverage-catalog.json` |

## Validation

| Gate | Command | Result |
| --- | --- | --- |
| Playwright demo flows | `cd ontogony-frontend && npm run test:e2e:demo-flows` | See frontend evidence |
| Demo prep (optional) | `docker/local-working-system/scripts/run-operator-v1-demo-prep.ps1` | Requires APIs on `:5081`–`:5083` |

## Operator behavior

Eight catalog flows map to routes and seed fields documented in the demo guide. `operator-v1-demo-ids.json` is generated from `env-seed-001-report.json` for live walkthroughs.

## Safety statement

- Real external tool execution remains blocked.
- Model assistance remains draft-only/non-authoritative where applicable.
- No production-readiness claim is made.

## Known limitations

- Demo prep does not start compose; see `docker/local-working-system/README.md`.
- Live Conexus assistance can be slow; CI uses mocks for `kanon-assistance`.

## Next step

Index in evidence README; proceed to `FIRST-VERSION-RC-001` when remaining Operator V1 cards are done.
