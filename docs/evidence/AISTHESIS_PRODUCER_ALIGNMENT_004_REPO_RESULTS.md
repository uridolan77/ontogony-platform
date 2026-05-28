# AISTHESIS producer alignment 004 — repo results

**Date:** 2026-05-28

| Repo | Build (changed projects) | Tests | Notes |
|---|---|---|---|
| `kanon-dotnet` | `Kanon.Application`, `Kanon.Infrastructure` — **PASS** | Prior `Kanon.Tests` not re-run (API file lock) | Native IDs on HTTP payload; `TryEmitEdgeAsync`; policy_evaluation type |
| `conexus-dotnet` | `Conexus.Application`, `Conexus.Infrastructure`, `Conexus.Infrastructure.Tests` — **PASS** | 111 passed (`Conexus.Infrastructure.Tests`) | `providerAttemptId`, route evidence IDs, `routed_to` edge |
| `metabole-dotnet` | `Metabole.Application`, `Metabole.Infrastructure`, `Metabole.Infrastructure.Tests` — **PASS** | 34 passed | `pipelineRunId`, `mappingCandidateId`, `artifactId`; pipeline/profile/mapping/artifact edges |
| `allagma-dotnet` | `Allagma.Application` — **PASS** | 13 passed (`FullyQualifiedName~Aisthesis`) | Cross-producer links; `KanonPlanningResult.SemanticPlanId` |
| `aisthesis-dotnet` | Fixture smoke only | 3 passed (`RequiredEdge*`) | Fixture ingest unchanged; smoke **PASS** |

## Commands used

```powershell
# Kanon
dotnet build src\Kanon.Application\Kanon.Application.csproj -c Release
dotnet build src\Kanon.Infrastructure\Kanon.Infrastructure.csproj -c Release

# Conexus
dotnet build src\Conexus.Application\Conexus.Application.csproj -c Release
dotnet test tests\Conexus.Infrastructure.Tests\Conexus.Infrastructure.Tests.csproj -c Release

# Metabole
dotnet test tests\Metabole.Infrastructure.Tests\Metabole.Infrastructure.Tests.csproj -c Release

# Allagma
dotnet build src\Allagma.Application\Allagma.Application.csproj -c Release
dotnet test tests\Allagma.Tests\Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~Aisthesis"

# Aisthesis fixture
./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Fixture -StartApi
```

Full solution builds were blocked locally by running API processes locking `bin\Release` outputs (Kanon/Allagma/Metabole on ports 5081–5085).
