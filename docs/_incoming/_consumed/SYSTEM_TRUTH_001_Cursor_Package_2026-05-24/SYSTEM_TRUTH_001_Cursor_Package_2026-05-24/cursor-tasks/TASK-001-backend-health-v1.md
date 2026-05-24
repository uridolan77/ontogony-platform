# TASK-001 — Backend Health V1

## Goal

Make `/health` return `health.v1` for Conexus, Kanon, and Allagma.

## Steps

1. Identify existing health endpoint mapping in each backend.
2. Add or reuse a DTO matching `contracts/health.v1.schema.json`.
3. Populate:
   - schemaVersion
   - status
   - service
   - serviceDisplayName
   - version
   - baseline
   - gitSha if available
   - buildTimeUtc if available
   - environment
   - checkedAtUtc
4. Keep liveness cheap. Do not perform expensive dependency checks here.
5. Add tests.

## Acceptance

- `/health` returns JSON.
- Frontend no longer reports generic payload format warning for compliant services.
