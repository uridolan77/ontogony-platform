# Cross-repo validation plan

## Aisthesis baseline

```powershell
dotnet restore
dotnet build Aisthesis.sln -c Release
dotnet test Aisthesis.sln -c Release --no-build
./scripts/run-aisthesis-live-spine-smoke.ps1 -StartApi
./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Fixture -StartApi
```

## Producer repo gate

In each producer repo:

```powershell
dotnet restore
dotnet build *.sln -c Release
dotnet test *.sln -c Release --no-build
./scripts/system/run-<producer>-aisthesis-producer-smoke.ps1
```

## Live proof gate

After all producer smokes pass:

```powershell
cd C:\dev\aisthesis-dotnet
./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Live
```

PASS requires:

```json
{
  "mode": "Live",
  "status": "PASS",
  "requiredEdges": { "present": 10, "missing": 0 },
  "reconstructabilityGrade": "complete"
}
```
