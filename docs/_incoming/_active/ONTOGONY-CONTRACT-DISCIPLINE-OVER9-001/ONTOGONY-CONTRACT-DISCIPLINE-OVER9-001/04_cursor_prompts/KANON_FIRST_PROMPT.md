# Kanon-first prompt for route truth cleanup

Start in `kanon-dotnet`.

Task: implement `KANON-CONTRACT-001` before any other slice.

Steps:

1. Inspect `docs/generated/KANON_COMPATIBILITY_MANIFEST.json`, `docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json`, and `docs/generated/ONTOLOGY_V0_CLIENT_COVERAGE.json`.
2. Treat generated artifacts as authoritative unless the generator itself is wrong.
3. Fix every narrative route/client/server-only count to match generated truth.
4. Add/extend tests that fail on stale route-count literals in `README.md`, `docs/CURRENT_STATE.md`, `docs/KNOWN_LIMITATIONS.md`, and any docs consumed by Allagma.
5. Regenerate route doc fragments, OpenAPI baseline, and compatibility manifest.
6. Write evidence under `docs/evidence/KANON_CONTRACT_001_ROUTE_TRUTH_EVIDENCE.md`.
7. Produce a short handoff note for Allagma to update `SYSTEM_COMPATIBILITY_MATRIX.md`.
