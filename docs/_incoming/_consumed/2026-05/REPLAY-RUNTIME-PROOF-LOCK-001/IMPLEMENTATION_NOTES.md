# REPLAY-RUNTIME-PROOF-LOCK-001 — Runtime proof closeout

## Delivered

1. **Live PASS smoke** on local stack (Kanon 5081, Conexus 5082, Allagma replay on 5084).
2. **Committed platform baseline:** `docs/evidence/artifacts/governed-fake-replay-e2e/20260525T081956Z/`
3. **Runtime lock pointer:** `allagma-dotnet/docs/system/ontogony-runtime.lock.json` → `evidence.governedFakeReplaySummary`
4. **Platform wrapper:** `scripts/smoke/run-runtime-lock-governed-fake-replay-e2e.ps1` (fixed absolute output paths + default Allagma `:5084`)
5. **Lock validation:** `-RequireGovernedFakeReplayEvidence` on `validate-runtime-lock.ps1`
6. **CI:** `.github/workflows/governed-fake-replay-runtime-lock.yml` (manual `workflow_dispatch`)
7. **Narrative:** `docs/evidence/GOVERNED_FAKE_REPLAY_E2E_001_PASS_20260525T081956Z.md`

## Allagma smoke fix

`run-governed-fake-replay-e2e.ps1` accepts absolute `-OutputDirectory` (platform evidence mirror).

## Verify on clean checkout

```powershell
cd c:\dev\allagma-dotnet
./scripts/validate-runtime-lock.ps1 -RequireGovernedFakeReplayEvidence
```

## Regenerate

```powershell
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-fake-replay-e2e.ps1
# Update lock timestamp + README + evidence markdown after promotion.
```

## Note on Conexus attempt

Baseline PASS allows `conexus: skipped` when Allagma lacks `Conexus__AdminApiKey`. Use `-RequireConexusReplayAttempt` for strict proof.
