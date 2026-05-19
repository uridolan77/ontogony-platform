# RP-003 — Allagma real provider guided flow evidence (platform)

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — cross-repo guided flow script + compose wiring; real failure classified

**Boundary:** Platform records RP-003 closeout. **No platform runtime code beyond compose env passthrough.** **Not production readiness.**

## Sibling deliverables

| Repo | Item |
| --- | --- |
| `allagma-dotnet` | `scripts/env/run-guided-main-flow-real-provider.ps1`, evidence, report JSON |
| `ontogony-platform` | `docker/local-working-system/docker-compose.yml` — Conexus RP env passthrough |
| `conexus-dotnet` | RP-002 gate (prerequisite) |

## Acceptance mapping

| Criterion | Result |
| --- | --- |
| Allagma completes real flow **or** failure classified | **PASS** — external `provider_transport_error` / Allagma 500 classified |
| Run / eval / trace / Conexus evidence (fake path) | **PASS** on fake baseline |
| Kanon topology unchanged | **PASS** (`single_workflow` baseline) |
| Evidence export | **Script includes** eval write + export on successful real run; skipped when transport fails |
| Fake-provider regression | **PASS** |
| No secrets | **PASS** |

## Operator entry

```powershell
cd C:\dev\allagma-dotnet
.\scripts\env\run-guided-main-flow-real-provider.ps1 -AttemptRealProviderCall -UseDockerConexusRestart
```

Policy: [`docs/operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](../operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md)

## Addendum — RP-003A (2026-05-19)

RP-003A closed the live-completion gap: Docker runtime CA trust + successful real OpenAI path. See [`RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md`](./RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md).

## Next step

**`RP-004`** — frontend operator visibility (`prompts/RP-004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY.md`).

## Required statement

```text
RP-003 proves Allagma guided flow integration with Conexus real-provider local mode.
Fake provider remains the default after kill switch.
Not production readiness.
```
