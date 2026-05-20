# KANON-CONNECT-007 — Full Kanon cross-service Docker smoke

**Recorded at (UTC):** 2026-05-20  
**Verdict:** See run output below (automated gate)  
**Statement:** Docker-local compose rebuild of `kanon-api`, `allagma-api`, `conexus-api`, and `ontogony-frontend`, ENV-SEED-001 bootstrap, and Playwright cross-service browser smoke for Kanon connect acceptance. Runtime lock not updated (SYSTEM-ALPHA cut is separate).

## Orchestration

```powershell
cd C:\dev\ontogony-frontend
npm run docker:smoke:kanon-connect-007
```

Or stepwise:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build kanon-api conexus-api allagma-api ontogony-frontend
docker compose up -d kanon-api conexus-api allagma-api ontogony-frontend
..\..\scripts\wait-local-working-system.ps1
..\..\scripts\seed-and-verify-local-working-system.ps1

cd C:\dev\ontogony-frontend
$env:ENV_SEED_001_REPORT = "C:\dev\ontogony-platform\docker\local-working-system\artifacts\env-seed-001-report.json"
npm run test:e2e:docker-live:kanon-connect-007
```

Report artifact: `docker/local-working-system/artifacts/kanon-connect-007-smoke-report.json`

## Browser smoke coverage

| Scenario | Spec step |
| --- | --- |
| Kanon overview | 1 |
| Kanon all workbenches (`/kanon/*`) | 1 |
| Allagma run → Kanon decision + semantic graph | 2 |
| Allagma gate → Kanon policy explanation | 3 (full link when `WaitingForHumanGate` run exists; otherwise gates page load) |
| Kanon assistance → Conexus model call | 4 (live invoke + observability link, or seed model-call fallback) |
| Evidence Spine Kanon decision + semantic graph | 5 |
| Evidence Spine Allagma run + Kanon nodes | 6 |
| Evidence Spine Conexus model call + correlation | 7 |

Playwright: `ontogony-frontend/e2e/kanon-connect-007-docker-local-cross-service-smoke.spec.ts`

Related suites (still in `playwright.docker-local.config.ts`): `kanon-deepen-014`, `kanon-connect-003`, `evidence-spine-docker-live`.

## SDK / image drift (CONNECT-007 task 1)

Compose builds use `mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim` inside Dockerfiles; host `global.json` uses `9.0.100` with `rollForward: latestFeature` in `allagma-dotnet` and `conexus-dotnet`. Rebuild is performed in-container so host SDK `9.0.203` vs prior `9.0.314` pin mismatch does not block image build.

## Acceptance

- [ ] Full compose rebuild succeeds (`docker compose build` for four services)
- [ ] ENV-SEED-001 passes
- [ ] `kanon-connect-007` Playwright suite passes
- [x] No `ontogony-runtime.lock.json` update in this slice

## Related

- [KANON-CONNECT-006](./KANON_CONNECT_006_ROUTE_PARITY_EVIDENCE.md)
- [KANON-DEEPEN-014](./KANON_DEEPEN_014_BROWSER_QA_BASELINE_CANDIDATE_EVIDENCE.md)
- Next: **SYSTEM-ALPHA-004/005-CUT** — runtime lock promotion
