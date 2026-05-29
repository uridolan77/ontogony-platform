# Ontogony system test harness

**Owner repo:** [`ontogony-system-test-harness`](https://github.com/uridolan77/ontogony-system-test-harness) (local: `C:\dev\ontogony-system-test-harness`)

**Package ID:** ONTOGONY-SYSTEM-TEST-HARNESS-001

## Purpose

Cross-repo HTTP system tests and evidence bundles that replace manual five-service regression (readiness, governed run, idempotency, human gates, Kanon assistance, Conexus fallback).

Platform does **not** duplicate these tests here — product repos keep unit/integration tests; this harness owns **live stack E2E** only.

## Run (local)

```powershell
cd C:\dev\ontogony-system-test-harness
copy .env.example .env
pwsh ./scripts/run-system-e2e.ps1
```

Stack must be up on ports in [ONTOGONY_RUNTIME_PORT_LOCK_V1](../contracts/ONTOGONY_RUNTIME_PORT_LOCK_V1.md) (or `manifests/system-compatibility.yml` in the harness repo).

## Evidence

- Harness: `docs/generated/MANUAL_REGRESSION_RETIREMENT_REPORT.md` in the harness repo
- Platform closeout: [`docs/evidence/ONTOGONY_SYSTEM_TEST_HARNESS_001_EVIDENCE.md`](../evidence/ONTOGONY_SYSTEM_TEST_HARNESS_001_EVIDENCE.md)

## Intake archive

Implementation source package archived at `docs/_incoming/_consumed/2026-05/ONTOGONY-SYSTEM-TEST-HARNESS-001/`.
