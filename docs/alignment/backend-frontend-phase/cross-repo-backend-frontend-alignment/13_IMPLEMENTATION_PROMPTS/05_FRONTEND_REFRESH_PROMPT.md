# Prompt — Frontend Snapshot Refresh After Backend Alignment

Repo: uridolan77/ontogony-frontend

Tasks:
1. Refresh OpenAPI snapshots from backend artifacts.
2. Update `docs/openapi/PROVENANCE.md` with backend repo/commit/SHA.
3. Regenerate clients.
4. Run OpenAPI, contract, route, coverage, check, and check:full gates.
5. Replace limitation banners only where backend routes are real.
6. Add UI/E2E for Conexus request search and Allagma operation capabilities if available.

Acceptance:
- No fake backend functionality.
- Evidence/redaction tests remain green.
- Release provenance remains valid.
