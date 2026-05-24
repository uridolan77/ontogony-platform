# Runtime lock — governed fake E2E (RUNTIME-LOCK-CI-GOVERNED-E2E-001)

## Purpose

Promote **GOVERNED-FAKE-E2E-001** from ad-hoc local proof to a **controlled, reproducible artifact** that runtime-lock and release planning can reference — without claiming production certification.

## Staged rollout

| Stage | Status | What |
| --- | --- | --- |
| 1 | Done | Manual local smoke (`run-governed-fake-e2e.ps1`) |
| 2 | **This sprint** | One local command + optional manual GitHub Actions workflow |
| 3 | Partial | Lock pointer + `-RequireGovernedFakeE2eEvidence` validator flag |
| 4 | Deferred | Required PR integration gate |

## One local command

From `ontogony-platform` (Docker-local stack running for Playwright):

```powershell
pwsh -File scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1
```

Backend only (faster):

```powershell
pwsh -File scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1 -SkipPlaywright
```

Wait for compose health first:

```powershell
pwsh -File scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1 -WaitDockerStack
```

## Canonical artifacts (per run)

Written under `artifacts/governed-fake-e2e/<timestamp>/` (Allagma) and mirrored to `ontogony-platform/artifacts/runtime-lock-governed-fake-e2e/<timestamp>/`:

| File | Role |
| --- | --- |
| `governed-fake-e2e-summary.json` | Machine-readable PASS summary (`ontogony-governed-fake-e2e-summary-v1`) |
| `governed-fake-e2e-output.log` | Transcript log |
| `evidence-spine-bundle.json` | Evidence graph acceptance bundle |
| `governed-fake-e2e-result.json` | Legacy detailed result (compat) |
| `playwright-report/` | Optional Docker-live UI proof |

## Runtime lock pointer

`allagma-dotnet/docs/system/ontogony-runtime.lock.json`:

```json
"evidence": {
  "governedFakeE2eSummary": "ontogony-platform/docs/evidence/artifacts/governed-fake-e2e/20260524T102932Z/governed-fake-e2e-summary.json"
}
```

Validate (optional gate):

```powershell
cd c:\dev\allagma-dotnet
./scripts/validate-runtime-lock.ps1 -RequireGovernedFakeE2eEvidence
./scripts/check-governed-fake-e2e-summary.ps1
```

## Manual CI

GitHub Actions: **Governed fake E2E (runtime lock)** (`governed-fake-e2e-runtime-lock.yml`) — `workflow_dispatch` only; runs backend smoke, uploads artifacts. Does **not** run Docker-live Playwright (keep that local until Stage 4).

## Relationship to Release Readiness page

The operator console **Release readiness artifact** page does **not** auto-attach live validation yet. A future rule may count a route as RC-eligible only when a fresh `governed-fake-e2e-summary.json` exists — that wiring belongs to a later sprint, not this one.

## Prior closure

- [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](../evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md)
- [RELEASE_READINESS_TRUTH.md](https://github.com/uridolan77/ontogony-frontend/blob/main/docs/operators/RELEASE_READINESS_TRUTH.md) (frontend)
