# AGENT-INTERACTION-LIVE-001 — active workstream

**Status:** in progress  
**Primary repo:** `ontogony-frontend`  
**Roadmap:** [`NEXT.md`](../NEXT.md) Phase 2

## Progress (2026-05-24)

- [x] Live lookup states (`live_loading` / `live_loaded` / `live_partial` / `live_failed`) + banner
- [x] Timeline operator sections with expand/collapse (`agent-interaction-timeline-groups`)
- [x] Provider panel missing-field codes (`not_recorded`, `not_returned_by_api`, `lookup_failed`, …)
- [x] Export bundle `privacy` metadata (`containsRawSecrets`, `redactionApplied`)
- [ ] Per-event Evidence Spine links in timeline cards
- [ ] Export bundle `liveLookupState` field
- [ ] E2E: live lookup failure does not show fixture

## Goal

Turn Agent Interaction into a **live operator workbench**: strict live lookup when `runId` is present, grouped timeline, missing-data discipline, provider/message/tool panels, and export bundle truth.

## Baseline (closed)

`GOVERNED-FAKE-E2E-001` proved live lookup end-to-end:

- [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](../evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md)

## Work items (from package roadmap)

1. Live lookup primary and strict — no silent fixture fallback when `runId` is in URL
2. Explicit source states: `live_loading`, `live_loaded`, `live_partial`, `live_failed`, `fixture_loaded`, `imported_loaded`
3. Timeline event grouping (run lifecycle, Kanon, Conexus, tools, gates, messages, evidence, errors)
4. Event cards with operator labels and missing-data reasons
5. Provider panel field completeness + absence reasons
6. Message stream discipline (visible / redacted / withheld / not recorded)
7. Tool intents and human gates standardized + cross-links
8. Per-event Evidence Spine / service links
9. Export bundle `dataSource`, `isLiveEvidence`, `missing`, `sourceAttempts`, privacy metadata
10. Unit, component, and E2E regression guards

## Related intake (reference only)

- `docs/_incoming/_active/ALLAGMA-AGENT-INTERACTION-001-extracted/` — prior spine contracts (partially implemented)
- Consumed: `GOVERNED_FAKE_E2E_001_Cursor_Package_2026-05-24`

## Out of scope (this sprint)

- Evidence Spine resolver rewrites
- Kanon console polish (`KANON-CONSOLE-POLISH-001`)
- Settings/security UX (`SETTINGS-SECURITY-UX-001`)
