# Ontogony five-service evidence certification matrix

System reference for RC certification ownership across backend repos.

## Service roles

| Service | Evidence producer role | Certification owner |
|---|---|---|
| Allagma | Run, tool intent/execution, side effects | allagma-dotnet |
| Kanon | Semantic plan/decision, human gates, canonical facts | kanon-dotnet |
| Conexus | Model calls, routing, fallback/error, usage | conexus-dotnet |
| Metabole | Pipeline, profile, mapping, review artifacts | metabole-dotnet |
| Aisthesis | Ingestion, reconstructability, evaluation, certification | aisthesis-dotnet |

## Matrix versions

| Matrix | Scope | Evaluator |
|---|---|---|
| v1 | 10 minimum cross-service edges | `RequiredEdgeEvaluator` |
| v2 | v1 + profile-aware operational edges | `RequiredEdgeEvaluatorV2` |

## Certification scripts (Aisthesis)

```text
scripts/system/run-aisthesis-rc-certification.ps1
scripts/system/run-five-service-live-certification.ps1
scripts/system/run-producer-emitter-contract-check.ps1
scripts/system/run-frontend-aisthesis-contract-smoke.ps1
```

## Honest status line (2026-05-29)

```text
Fixture certification: PASS
Live five-service certification: NOT_RUN
System RC lock: false
```
