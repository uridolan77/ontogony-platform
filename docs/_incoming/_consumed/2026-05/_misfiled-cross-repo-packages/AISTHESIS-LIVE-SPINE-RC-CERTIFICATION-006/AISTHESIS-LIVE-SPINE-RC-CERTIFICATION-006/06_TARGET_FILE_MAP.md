# Target file map

## Primary repo: `aisthesis-dotnet`

### Add/update evidence docs

```text
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_CLOSEOUT.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_RELEASE_GATES.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_LIVE_PROOF.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_LOCK_DECISION.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_LES_002_ANALYSIS.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_CLIENT_COVERAGE.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_EDGE_AUTH.md
```

### Add/update contracts

```text
docs/contracts/AISTHESIS_REQUIRED_EDGE_MATRIX_V2.md
docs/contracts/AISTHESIS_LIVE_SPINE_SUMMARY_V3.md
docs/contracts/AISTHESIS_RC_CERTIFICATION_GATE_V1.md
docs/contracts/AISTHESIS_EVALUATION_JOB_V0.md
docs/contracts/AISTHESIS_PRODUCER_EDGE_AUTH_V0.md
docs/contracts/AISTHESIS_RETENTION_ERASURE_CONTRACT_V0.md
docs/contracts/AISTHESIS_OTEL_TRACE_EXPORT_V0.md
```

### Add/update operations docs

```text
docs/operations/AISTHESIS_FIVE_SERVICE_LIVE_CERTIFICATION_RUNBOOK.md
docs/operations/AISTHESIS_RELEASE_MODE_RUNBOOK.md
docs/operations/AISTHESIS_PRODUCTION_IAM_RUNBOOK.md
docs/operations/AISTHESIS_RETENTION_ERASURE_RUNBOOK.md
docs/operations/AISTHESIS_OTEL_TRACE_EXPORT_RUNBOOK.md
```

### Add/update scripts

```text
scripts/system/run-aisthesis-rc-certification.ps1
scripts/system/run-five-service-live-certification.ps1
scripts/system/run-frontend-aisthesis-contract-smoke.ps1
scripts/system/run-producer-emitter-contract-check.ps1
```

### Possible code targets

```text
src/Aisthesis.Api/Endpoints/EvaluationEndpoints.cs
src/Aisthesis.Api/Endpoints/EvidenceEndpoints.cs
src/Aisthesis.Api/Endpoints/EvidenceBatchEndpoints.cs
src/Aisthesis.Application/Auth/ProducerWriteAuthorization.cs
src/Aisthesis.Application/Services/EvidenceBatchIngestionService.cs
src/Aisthesis.Application/Services/EvaluationJobService.cs
src/Aisthesis.Application/Abstractions/IEvaluationJobStore.cs
src/Aisthesis.Infrastructure.Persistence/InMemoryEvaluationJobStore.cs
src/Aisthesis.Infrastructure.Postgres/PostgresEvaluationJobStore.cs
src/Aisthesis.Client/AisthesisClient.cs
tests/Aisthesis.Api.Tests/*Authorization*Tests.cs
tests/Aisthesis.Client.Tests/*Evaluation*Tests.cs
tests/Aisthesis.Krisis.Tests/*EvaluationJob*Tests.cs
```

Do not create these code files blindly. First inspect current repo state and adapt names/locations to existing conventions.

## Platform repo: `ontogony-platform`

```text
docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_PLATFORM_CLOSEOUT.md
docs/evidence/SYSTEM_RC_AISTHESIS_LOCK_REVIEW_006.md
docs/system/ONTOGONY_FIVE_SERVICE_EVIDENCE_CERTIFICATION_MATRIX.md
```

## Frontend repo: `ontogony-frontend`

```text
docs/evidence/ONTOGONY_FRONTEND_AISTHESIS_LIVE_SPINE_006_CLOSEOUT.md
docs/contracts/AISTHESIS_FRONTEND_LIVE_SPINE_CONTRACT_V0.md
```

Only modify frontend implementation if explicitly running this package there.

## Producer repos

Only modify producer repos if the live certification script reveals missing/misnamed native IDs, edges, or evidence types.
