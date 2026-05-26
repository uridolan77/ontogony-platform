# REPLAY-RUNTIME-FULL-INTEGRATION-001E — Governed fake replay evidence + runtime lock

## Allagma (REPLAY-RUNTIME-005 base)

- `scripts/smoke/run-governed-fake-replay-e2e.ps1`
- `scripts/lib/write-governed-fake-replay-artifacts.ps1` — `ontogony-governed-fake-replay-summary-v1`
- `scripts/check-governed-fake-replay-summary.ps1`

## Runtime lock

- `scripts/validate-runtime-lock.ps1` — `-RequireGovernedFakeReplayEvidence` validates `evidence.governedFakeReplaySummary`.
- `docs/system/ONTOGONY_RUNTIME_LOCK.md` — documents `governedFakeReplaySummary` key.

Set lock evidence after a PASS run (example):

```json
"governedFakeReplaySummary": "ontogony-platform/docs/evidence/artifacts/governed-fake-replay-e2e/<timestamp>/governed-fake-replay-summary.json"
```

## Platform wrappers

| Script | Purpose |
| --- | --- |
| `scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1 -IncludeReplay` | Full governed-fake proof + replay smoke + platform mirror |
| `scripts/smoke/run-runtime-lock-governed-fake-replay-e2e.ps1` | Replay-only smoke → `docs/evidence/artifacts/governed-fake-replay-e2e/<timestamp>/` |

## Verify

```powershell
# Requires Kanon + Conexus + Allagma on 5081-5083
powershell -NoProfile -File c:\dev\allagma-dotnet\scripts\smoke\run-governed-fake-replay-e2e.ps1

powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-fake-replay-e2e.ps1

powershell -NoProfile -File c:\dev\allagma-dotnet\scripts\validate-runtime-lock.ps1 -RequireGovernedFakeReplayEvidence
```
