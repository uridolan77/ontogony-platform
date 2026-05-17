# OpenAPI and Provenance Plan

## Target artifact

Every backend service publishes:

```json
{
  "service": "conexus",
  "repo": "uridolan77/conexus-dotnet",
  "commit": "sha",
  "serviceVersion": "0.1.0",
  "openapiPath": "openapi/conexus.v0.json",
  "openapiSha256": "64 hex",
  "generatedAt": "ISO date",
  "generator": "Swashbuckle|NSwag|custom"
}
```

## Backend tasks

1. Add deterministic OpenAPI export command.
2. Add CI step to generate OpenAPI.
3. Upload OpenAPI JSON and provenance JSON as artifacts.
4. Fail CI if generated OpenAPI differs from committed snapshot, if snapshots are committed.
5. Use consistent artifact names:
   - `conexus-openapi-v0`
   - `kanon-openapi-v0`
   - `allagma-openapi-v0`

## Frontend tasks

1. Refresh `openapi/*.json` from backend artifacts.
2. Update `docs/openapi/PROVENANCE.md`.
3. Record backend repo+commit+SHA.
4. Run OpenAPI, contract, check, and check:full gates.

## Acceptance

A frontend release can answer which backend commit generated each OpenAPI snapshot and which frontend build consumed it.
