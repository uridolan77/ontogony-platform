# Scope and boundary

## In scope

- Producer-native envelope emission.
- Native ID completeness.
- Cross-producer and internal evidence edge emission.
- Idempotent batch emission.
- Producer-token compatible Aisthesis writes.
- Repo-local unit/integration tests.
- Producer smoke scripts.
- Closeout evidence.

## Out of scope

- Aisthesis semantic inference.
- Aisthesis workflow orchestration.
- Aisthesis model routing.
- Aisthesis data transformation.
- Production IAM implementation.
- Retention/erasure destructive APIs.
- Frontend implementation unless explicitly requested.

## Ownership

| Concern | Owner |
|---|---|
| Run lifecycle | Allagma |
| Semantic plan / decision / policy | Kanon |
| Route / model call / provider attempt | Conexus |
| Pipeline / profile / mapping / artifact | Metabole |
| Evidence graph / reconstructability | Aisthesis |
