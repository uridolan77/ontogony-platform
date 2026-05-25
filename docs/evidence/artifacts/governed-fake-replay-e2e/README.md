# Governed fake replay — committed runtime-lock baselines

## Policy (REPLAY-RUNTIME-PROOF-LOCK-001)

Small **PASS** artifacts under this tree are **committed** so a clean checkout satisfies:

```powershell
cd c:\dev\allagma-dotnet
./scripts/validate-runtime-lock.ps1 -RequireGovernedFakeReplayEvidence
```

Runtime lock pointer (`allagma-dotnet/docs/system/ontogony-runtime.lock.json`):

```text
evidence.governedFakeReplaySummary =
  ontogony-platform/docs/evidence/artifacts/governed-fake-replay-e2e/<timestamp>/governed-fake-replay-summary.json
```

Each baseline directory should include:

| File | Purpose |
| --- | --- |
| `governed-fake-replay-summary.json` | Machine-readable PASS (`ontogony-governed-fake-replay-summary-v1`) |
| `replay-request.json` | Created replay request |
| `replay-result.json` | Orchestration result + service attempts |
| `replay-evidence-bundle.json` | Cross-service evidence bundle |
| `replay-delta.json` | Comparison delta |
| `replay-summary.json` / `replay-summary.md` | Operator-readable summaries |

**Not committed:** ephemeral runs under repo-root `allagma-dotnet/artifacts/governed-fake-replay-e2e/`.

## Current baseline

| Timestamp | Lock baseline | Narrative |
| --- | --- | --- |
| `20260525T081956Z` | Yes | [GOVERNED_FAKE_REPLAY_E2E_001_PASS_20260525T081956Z.md](../../GOVERNED_FAKE_REPLAY_E2E_001_PASS_20260525T081956Z.md) |

## Regenerate locally

Requires Kanon `:5081`, Conexus `:5082`, and Allagma with replay routes (e.g. current build on `:5084`):

```powershell
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-fake-replay-e2e.ps1

# Optional strict Conexus attempt (requires Conexus admin key on Allagma):
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-fake-replay-e2e.ps1 -RequireConexusReplayAttempt
```

After a new PASS, add `<timestamp>/`, update the lock pointer, and refresh this README.
