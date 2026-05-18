# ENV-SEED-001 — deterministic seed/bootstrap evidence

**Recorded at (UTC):** 2026-05-18T23:04:08Z  
**Verdict:** PASS  
**Statement:** This package supports the first working local environment and first Dockerized local working system. It is not production readiness.

## Scope

`ontogony-platform` only:

- deterministic seed/bootstrap runtime script in compose tree
- API-based verification for Conexus fake route, Kanon topology authorization behavior, and Allagma evaluation write/list/comparison flows
- docs/evidence updates for Docker local working system sequence

No compose orchestration, no raw schema SQL seed rows, and no production hardening.

## Delivered

```text
docker/local-working-system/scripts/seed-and-verify-local-working-system.ps1
docker/local-working-system/artifacts/env-seed-001-report.json
docker/local-working-system/README.md
docs/environments/docker-local-working-system/04_DATABASES_AND_SEEDS.md
docs/environments/docker-local-working-system/08_TROUBLESHOOTING.md
docs/environments/docker-local-working-system/09_KNOWN_LIMITATIONS.md
docs/environments/docker-local-working-system/README.md
docs/environments/README.md
docs/releases/FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md
docs/evidence/ENV_SEED_001_DETERMINISTIC_BOOTSTRAP_EVIDENCE.md
```

## Command run

```powershell
cd C:\dev\ontogony-platform

powershell -NoProfile -ExecutionPolicy Bypass `
  -File .\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

## Results

| Check | Result |
| --- | --- |
| Script process exit code | **0** |
| PowerShell runtime errors (`CommandNotFoundException`, parser, escaping) | **none observed** |
| Conexus dev bootstrap API | **PASS** (`projectId=dev-project`, alias `gpt-4o-mini`, provider `fake`) |
| Kanon topology behavior via Allagma runs | **PASS** (baseline `single_workflow` null auth decision id; subject `centralized_orchestrator` non-empty decision id) |
| Conexus route evidence | **PASS** (baseline + subject `routeDecisionId` non-empty and fetchable via admin route-decision API) |
| Allagma evaluation persistence APIs | **PASS** (write/list baseline+subject evals; baseline comparison create/fetch) |
| JSON report artifact | **PASS** (`docker/local-working-system/artifacts/env-seed-001-report.json`) |

Console completion line:

```text
ENV-SEED-001 seed + verify PASS.
```

## Key runtime IDs (from report)

- baseline run: `run_6b6a354c5f8241c98a9287e999348114`
- subject run: `run_6e9ee925296e475d8d069a8034bd9b20`
- subject topology authorization decision: `decision_bb3fa6f7d87745b4956662de4bfbfaf9`
- baseline route decision: `rd-0HNLKV6BPTQGT-00000001`
- subject route decision: `rd-0HNLKV6BPTQGT-00000002`
- baseline comparison: `cmp_25c34eb67aef48bbabc7e48be123160c`

## Safety

| Check | Status |
| --- | --- |
| Fake/local Conexus provider | **yes** (`providerKey=fake`) |
| Real external execution enabled | **no** (unchanged) |
| Real provider keys committed | **no** |
| Production readiness claim | **no** |

## Known limitations

- ENV-COMPOSE-001 remains next for `docker-compose.yml` + `.env.example`.
- Seed script verifies API-level persistence behavior; restart-survival proof remains in guided Docker run/closeout phases.
- This evidence validates **host-local API** behavior against currently running services; Docker compose networking/container DNS proof remains pending ENV-COMPOSE-001 + ENV-DOCKER-RUN-001.
- Runtime report file is local under `docker/local-working-system/artifacts/` and remains untracked via `.gitignore`.

## Sequence status resolution

- `ENV-DB-001`: complete, with evidence at `docs/evidence/ENV_DB_001_POSTGRES_BOOTSTRAP_EVIDENCE.md`.
- `ENV-SEED-001`: complete for host-local deterministic seed verification.
- `ENV-COMPOSE-001`: next.

## Next step

**ENV-COMPOSE-001** — Docker Compose orchestration in `ontogony-platform/docker/local-working-system/`.
