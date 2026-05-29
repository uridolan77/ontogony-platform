# Aisthesis five-service live certification runbook

## Prerequisites

- Aisthesis running on 5084.
- Allagma running on 5083.
- Kanon running on 5081.
- Conexus running on 5082.
- Metabole running on 5085.
- Producer tokens configured.
- Aisthesis Postgres mode preferred.
- Live workflow trigger configured.

## Commands

```powershell
./scripts/system/run-five-service-live-certification.ps1 -Mode Preflight
./scripts/system/run-five-service-live-certification.ps1 -Mode Fixture
./scripts/system/run-five-service-live-certification.ps1 -Mode LiveOrExplain
```

## PASS criteria

See `contracts/AISTHESIS_LIVE_SPINE_SUMMARY_V3.md`.
