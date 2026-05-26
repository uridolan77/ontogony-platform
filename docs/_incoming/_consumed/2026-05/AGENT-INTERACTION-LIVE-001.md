# AGENT-INTERACTION-LIVE-001 — active workstream

**Status:** Phase 2 **closed** (2026-05-24); **001A stabilization** complete  
**Primary repo:** `ontogony-frontend`  
**Roadmap:** [`NEXT.md`](../NEXT.md) Phase 2

## Phase 2 (closed)

- [x] Live lookup states + strict no-fixture-fallback on `runId` failure
- [x] Timeline operator sections with expand/collapse
- [x] Provider panel missing-field codes
- [x] Export bundle `privacy`, `liveLookupState`, `missing[]`
- [x] Per-event Evidence Spine links (selected event)
- [x] Docker-live E2E (`governed-fake-e2e-docker-live`)

## Phase 001A stabilization (closed)

- [x] `e2e/agent-interaction-live-001a.spec.ts` — human-gate fixture + live_partial SSE failure
- [x] Partial banner lists stream fallback + synthesis warning codes
- [x] Export `liveEvidenceCapture` (`complete` | `partial` | `failed` | `not_applicable`)
- [x] Operator guide: `ontogony-frontend/docs/operators/AGENT_INTERACTION_WORKBENCH.md`

## Next

**`KANON-CONSOLE-POLISH-001`** — see [`NEXT.md`](../NEXT.md).

## Baseline

`GOVERNED-FAKE-E2E-001` — [GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md](../evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md)

## Out of scope (deferred)

- Message availability label polish (`visible` / `redacted` / `hash-only` / …)
- UI density / primary vs debug hierarchy pass
