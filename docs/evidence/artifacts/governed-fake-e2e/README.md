# Governed fake E2E — committed runtime-lock baselines

## Policy (RUNTIME-LOCK-CI-GOVERNED-E2E-001A)

Small **PASS** artifacts under this tree are **committed** so a clean checkout satisfies:

```powershell
cd c:\dev\allagma-dotnet
./scripts/validate-runtime-lock.ps1 -RequireGovernedFakeE2eEvidence
```

The runtime lock pointer is:

```text
ontogony-platform/docs/evidence/artifacts/governed-fake-e2e/<timestamp>/governed-fake-e2e-summary.json
```

Each baseline directory should include:

| File | Purpose |
| --- | --- |
| `governed-fake-e2e-summary.json` | Machine-readable PASS summary (`ontogony-governed-fake-e2e-summary-v1`) |
| `evidence-spine-bundle.json` | Evidence graph acceptance bundle (no secrets) |

**Not committed:** per-run outputs under repo-root `/artifacts/` (local smoke mirrors), Playwright reports, logs.

## Current baseline

| Timestamp | Lock baseline | Narrative |
| --- | --- | --- |
| `20260524T102932Z` | Yes — see `allagma-dotnet` `evidence.governedFakeE2eSummary` | [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](../../GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md) |

When you record a new PASS, add a new `<timestamp>/` folder, update the lock pointer in `allagma-dotnet/docs/system/ontogony-runtime.lock.json`, and keep this README in sync.
