# Validation matrix

## Local Aisthesis gates

```powershell
dotnet restore
dotnet build Aisthesis.sln -c Release
dotnet test Aisthesis.sln -c Release --no-build
```

Expected: PASS, 0 failures.

## Fixture smoke

```powershell
./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Fixture -StartApi
```

Expected: PASS, complete, missing required edges = 0.

## RC certification script

```powershell
./scripts/system/run-aisthesis-rc-certification.ps1
```

Expected: PASS or honest partial with explicit gates.

## Five-service certification

```powershell
./scripts/system/run-five-service-live-certification.ps1 -Mode Preflight
./scripts/system/run-five-service-live-certification.ps1 -Mode Fixture
./scripts/system/run-five-service-live-certification.ps1 -Mode LiveOrExplain
```

Expected:

- Preflight: PASS or FAIL with missing repo/config reason.
- Fixture: PASS if Aisthesis fixture path works.
- LiveOrExplain: PASS only for real live proof; otherwise NOT_RUN with exact reason.

## Client coverage truth

Regenerate route inventory/client coverage and prove:

```text
publicRoutes - coveredRoutes - serverOnlyRoutes = empty
```

## Matrix v2 tests

Add tests for:

```text
complete v2 trace => complete
missing v2 edge => diagnostic + suggestedProducerFix
not applicable v2 edge => notApplicableReason present
```

## Auth tests

Add tests for:

```text
producer token cannot write envelope for different producer
mixed-producer batch requires wildcard
edge-only batch requires producerSystem
non-wildcard direct edge write cannot spoof cross-producer edge unless policy allows it
read/evaluate permissions are enforced if implemented
```

## Retention/OTel/IAM

If implemented, add tests. If deferred, close with explicit productionBlocker flag.
