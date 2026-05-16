# Cross-service error envelope (SYS-COH-004)

## Summary

Adds neutral `CrossServiceErrorEnvelope`, `CrossServiceErrorCodes`, and `CrossServiceErrorStage` to `Ontogony.Errors` for cross-repo HTTP clients and downstream failure mapping.

## Consumers

- `allagma-dotnet` — maps Kanon/Conexus failures to stable codes (Phase B).
- `kanon-dotnet` — continues `ApiError` middleware shape; may adopt envelope on selected routes later (Phase C).
- `conexus-dotnet` — OpenAI-shaped errors remain; clients map into envelope at consumer boundary.

## Breaking changes

None. Additive contract only.
