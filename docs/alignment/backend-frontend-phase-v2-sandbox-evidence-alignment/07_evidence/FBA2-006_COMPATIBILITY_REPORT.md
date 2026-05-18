# FBA2-006 — Cross-service compatibility proof

**Generated:** 2026-05-18  
**Verdict:** PASS (contract + adapter + UI; local-stack smoke optional per P2)

## Scope

Proves backend/frontend alignment for Phase 3 sandbox evidence and operator capability metadata after FBA2-000 through FBA2-005 closeout, including:

- `GET /allagma/v0/runs/{runId}/audit` with optional `sandboxEvidence`
- `GET /allagma/v0/capabilities` for backend-driven sandbox execution boundaries
- Frontend typed adapters, redaction, panels, fixtures, and mocked E2E

## OpenAPI provenance

| Artifact | Path | SHA-256 |
| -------- | ---- | ------- |
| Backend snapshot | `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json` | `b6d6e6e5c8a978f47cba44a62ccefcde2f97f782f2a6837d4b2736b346750964` |
| Frontend synced spec | `ontogony-frontend/openapi/allagma.v0.json` | `330782f362a7eaab9be3a52f6bba12b3cd86eceda45ef7db566c434e32247d63` |

**Commit semantics (P1):**

- `sourceCommit` (`0435a1f…`) — last git commit that changed the backend snapshot file
- `provenanceCommit` (`d4764b8…`) — commit that refreshed the provenance sidecar
- `snapshotSha256` — canonical contract identity (must match across repos)

Frontend provenance: `ontogony-frontend/docs/openapi/ALLAGMA_SNAPSHOT_PROVENANCE.json`

## Backend checks

| Check | Result |
| ----- | ------ |
| `AllagmaOpenApiSnapshotTests` — audit + capabilities paths, sandbox evidence schemas | PASS |
| `AllagmaCapabilitiesApiTests` — authenticated capabilities response | PASS |
| `GetAllagmaCapabilitiesServiceTests` — dev/prod/sandbox flag matrix | PASS |
| Five `Allagma.SandboxExecute*` events in OpenAPI extension | PASS |
| `RealExecutionEnabled=true` rejected at options validation | PASS (existing) |
| `SandboxExecutionEnabled=true` forbidden in production | PASS (existing) |

## Frontend checks

| Check | Result |
| ----- | ------ |
| `allagmaSandboxEvidenceAdapters.test.ts` — audit mapping, capabilities mapper, fallback | PASS |
| `AllagmaSandboxEvidencePanel.test.tsx` — panel rendering | PASS |
| Mocked E2E — run detail / replay workbench sandbox evidence | PASS (existing) |
| Capabilities consumed via `useAllagmaSandboxExecutionCapability` | Implemented |
| Fixture banner for `?sandboxFixture=` query param | PASS (existing) |

## UI surfaces

- **Run detail** — `AllagmaRunSandboxEvidenceSection` → `AllagmaSandboxEvidencePanel`
- **Replay workbench** — same panel with resolved run id
- **Limitations card** — evidence routes from OpenAPI; sandbox boundaries from `/capabilities` with conservative fallback

## Capability metadata (P0)

`GET /allagma/v0/capabilities` returns options-derived truth:

- `realExternalExecution` — blocked unless explicitly enabled (still validator-guarded)
- `localSandboxExecution` — `local_only` when enabled in non-production; `blocked` in production; `disabled` when flag off
- `sandboxEvidence` — `supported: true`, `auditRoute: /allagma/v0/runs/{runId}/audit`

Default dev/test deployment (`SandboxExecutionEnabled=false`) reports local sandbox **disabled**, matching backend state (replacing the prior hardcoded “enabled” bridge).

## Known limitations

| Item | Priority | Notes |
| ---- | -------- | ----- |
| Local-stack smoke E2E | P2 | Mocked E2E remains; optional `start Allagma → seed run → open UI` proof not automated here |
| FBA2-007 release scorecard | Next | Record final commits, CI links, and eval-basing handoff |

## Machine-readable report

See `08_examples/FBA2-006-compatibility-report.json`.

## Next step

**FBA2-007** — Release evidence and scorecard, then eval-basing phase.
