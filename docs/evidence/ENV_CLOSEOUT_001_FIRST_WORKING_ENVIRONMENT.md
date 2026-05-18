# ENV-CLOSEOUT-001 — First working local environment closeout

**Recorded at (UTC):** 2026-05-19  
**Verdict:** PASS  
**Statement:** This is first working local environment, not production readiness.

## Scope

Close the script-based local operator sanity program in `ontogony-platform`: release closeout docs, scorecard, known limitations, next steps (ENV-DOCKER-LOCAL). Documentation and validation only — no runtime source, workflows, or secrets.

## Delivered

```text
docs/releases/FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md
docs/releases/FIRST_WORKING_ENVIRONMENT_SCORECARD.md
docs/releases/FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md
docs/releases/FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md
docs/evidence/ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md
docs/environments/README.md (updated)
```

## Repository SHAs (closeout)

| Repo | SHA |
| --- | --- |
| ontogony-platform | `a9e317f8c9d238fc92aa4c7f602d6c4d4b72fca2` |
| allagma-dotnet | `d39a98d76308b5c6337e2407d0d2f4f27d01c237` |
| kanon-dotnet | `b4e1d34123534fcdd29dbfcb9aa294ab57ad9791` |
| conexus-dotnet | `b5d5ae04f314965c4d49f9d6210b5fbf70318198` |
| ontogony-frontend | `fb5a640aed124a793f67b6d19560cd7f04dfa154` |
| ontogony-ui | `10f8a02665c17390ea60836afee2d12e3097a2d5` |

## Commands run

```powershell
cd C:\dev\allagma-dotnet
.\scripts\env\check-local-operator-sanity.ps1 -DevRoot C:\dev -SkipHealthCheck
.\scripts\env\validate-guided-main-flow.ps1 -AllowSkippedFrontend
```

## Results

| Check | Result |
| --- | --- |
| Six repos under `C:\dev\` | **PASS** |
| `validate-guided-main-flow.ps1` on existing full-sanity report | **PASS** (`artifacts/full-sanity/full-sanity-report.json`) |
| Release closeout quartet | **PASS** (this PR) |

## Evidence chain (prior ENV PRs)

| PR | Evidence |
| --- | --- |
| ENV-SETUP-001 | `docs/evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md` |
| ENV-SETUP-002 | `allagma-dotnet/docs/evidence/ENV_SETUP_002_STACK_SCRIPTS_EVIDENCE.md` |
| ENV-RUN-001 | `allagma-dotnet/docs/evidence/ENV_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md` |
| ENV-FE-001 | `ontogony-frontend/docs/evidence/ENV_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md` |
| ENV-PG-001 | `allagma-dotnet/docs/evidence/ENV_PG_001_POSTGRES_DURABLE_EVIDENCE.md` |
| ENV-UI-001 | `ontogony-frontend/docs/evidence/ENV_UI_001_INTEGRATION_READINESS.md` |

## Latest machine report IDs (ENV-PG-001 run)

| Role | ID |
| --- | --- |
| Baseline run | `run_605fbc0654af469785f07c13ced0cf25` |
| Subject run | `run_ef16456834ee4bb3997ebb2912406b0b` |
| Subject topology auth | `decision_3af24ca0a94749f6bb03eb997b2ff6b0` |
| Baseline eval | `eval_aca6d1b39d6c4368b3ab0676249a9e60` |
| Subject eval | `eval_200bbfe3c76744a89c82740a902eb1f3` |
| Baseline comparison | `cmp_3518f4a7041e494491e16911d300cc86` |

## Ports

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma | `http://localhost:5083` |

## Frontend walkthrough

- Automated: OpenAPI check, adapter tests, Playwright eval dashboards (ENV-FE-001) — **PASS**
- Manual: `ontogony-frontend/docs/development/LOCAL_OPERATOR_WALKTHROUGH.md` — operator responsibility
- Runner: `frontend.status=skipped` accepted with `-AllowSkippedFrontend` on guided validator

## Safety

| Check | Status |
| --- | --- |
| No secrets committed | **yes** |
| Real external execution | **disabled** |
| Production readiness claimed | **no** |

## Known limitations

See [FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md](../releases/FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md). Script-based stack only; Docker phase not started.

## Next step

**ENV-DOCKER-LOCAL** — begin `ENV-DOCKER-001` per `allagma-dotnet/docs/environments/working-local-environment-complete-package/07_PR_SEQUENCE.md`.
