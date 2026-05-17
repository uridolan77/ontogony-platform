# Prompt — BFA-003/004 Allagma Runtime Contract

Repo: uridolan77/allagma-dotnet

Goal:
Make run operation and replay/evidence support explicit and typed.

Tasks:
1. Audit resume/retry/cancel/replay-trigger/start-run support.
2. Add capability metadata endpoint or health metadata.
3. Implement only supported operations.
4. For unsupported operations, return explicit capability reason.
5. Add typed replay/evidence response schemas.
6. Add event payload contracts.
7. Add OpenAPI and tests.
8. Ensure operations emit trace/run/decision/model-call metadata where relevant.

Validation: `dotnet test`; generated OpenAPI has typed schemas.
