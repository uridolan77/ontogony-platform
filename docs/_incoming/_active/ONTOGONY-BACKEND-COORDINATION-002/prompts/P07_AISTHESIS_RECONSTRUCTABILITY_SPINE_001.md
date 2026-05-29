# P07 — AISTHESIS-RECONSTRUCTABILITY-SPINE-001

Implement slice 7 in `aisthesis-dotnet`. May merge `AISTHESIS-LIVE-FIVE-SERVICE-PASS-009`.

## Read

- `slices/AISTHESIS-RECONSTRUCTABILITY-SPINE-001/README.md`
- `aisthesis-dotnet/docs/runbooks/AISTHESIS_LIVE_FIVE_SERVICE_RUNBOOK.md`
- `aisthesis-dotnet/docs/architecture/AISTHESIS_BOUNDARY.md`

## Task

1. Resolve `global.json` SDK pin vs installed SDKs.
2. Run live five-service certification (not fixture-only):
   ```powershell
   ./scripts/system/run-five-service-live-certification.ps1 -Mode Live
   ```
3. Verify `requiredEdges.missing = 0` and `reconstructabilityGrade = complete`.
4. Archive merged 009 package to `_consumed` if absorbed.
5. Closeout evidence with trace ID reference.

## Do not

- Fake live proof with fixture-only ingestion.
- Move semantic authority into Aisthesis.

## Done when

`AIS-001`–`AIS-005` PASS.
