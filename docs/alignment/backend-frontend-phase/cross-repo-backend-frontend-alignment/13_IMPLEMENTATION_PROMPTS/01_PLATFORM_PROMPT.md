# Prompt — BFA-001 Platform Shared Contract Baseline

Repo: uridolan77/ontogony-platform

Implement shared cross-service contract primitives:
1. Add/finalize constants for trace/correlation/run/decision/model-call/request/human-gate/replay IDs.
2. Add shared metadata envelope/helper.
3. Add error envelope helper for minimal APIs.
4. Add service metadata helper for health/OpenAPI.
5. Add tests for header propagation and envelope serialization.
6. Update docs/changelog.

Validation: `dotnet test`.
