# Regression test filter map

Use actual test names from the repos. This file is a planning map; Cursor must update it with real filters.

## Allagma

Suggested filters:

```powershell
dotnet test Allagma.sln -c Release --filter FullyQualifiedName~System
dotnet test Allagma.sln -c Release --filter FullyQualifiedName~Idempotency
dotnet test Allagma.sln -c Release --filter FullyQualifiedName~Conexus
dotnet test Allagma.sln -c Release --filter FullyQualifiedName~Kanon
dotnet test Allagma.sln -c Release --filter FullyQualifiedName~RealTool
```

## Kanon

```powershell
dotnet test Kanon.sln -c Release --filter FullyQualifiedName~EvidenceSpine
dotnet test Kanon.sln -c Release --filter FullyQualifiedName~ConexusAssistance
dotnet test Kanon.sln -c Release --filter FullyQualifiedName~HumanGate
dotnet test Kanon.sln -c Release --filter FullyQualifiedName~Decision
```

## Conexus

```powershell
dotnet test Conexus.sln -c Release --filter FullyQualifiedName~ProviderFallback
dotnet test Conexus.sln -c Release --filter FullyQualifiedName~ModelCall
dotnet test Conexus.sln -c Release --filter FullyQualifiedName~Idempotency
```

## Frontend

```powershell
npm test -- src/evidence-spine
npm run contracts:discipline
```

Update this file to exact commands during implementation.
