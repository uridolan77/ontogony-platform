# 06 — Frontend Replay Workbench plan

## UI principle

Do not create another dense console page. Replay is a task-oriented workflow connected to Evidence Spine and Agent Interaction.

## Proposed module

Add:

```text
src/replay/
  api/
  components/
  hooks/
  adapters/
  types.ts
  replayModeLabels.ts
  replayEvidenceLinks.ts
```

The module should be small and contract-driven.

## Route strategy

Add one route only if needed:

```text
/system/replay
```

This route is a focused Replay Workbench, not a dashboard. It should be accessible from:

- Evidence Spine page actions;
- Allagma Run Detail;
- Allagma Audit Journey;
- Agent Interaction graph/timeline;
- Kanon decision/provenance links;
- Conexus model-call evidence links.

## Workbench layout

Use canonical primitives from `ontogony-ui`:

- `OperatorPageFrame`
- `OperatorSignalSummary`
- `OperatorDisclosure`
- `OperatorDataTable`
- `ConfirmDialog`
- `DestructiveConfirmDialog` only if a future risky/destructive replay mode is introduced.

### 1. Root input/search

Component:

```text
ReplayRootLookup.tsx
```

Behavior:

- Accept paste of run/trace/correlation/model-call/decision/provider-attempt/bundle identifiers.
- Reuse Evidence Spine parser/resolver where possible.
- Support manual target kind override.
- Show ambiguity clearly.

### 2. Eligibility summary

Component:

```text
ReplayEligibilitySummary.tsx
```

Show:

- target kind;
- owner service;
- evidence coverage;
- allowed modes;
- unavailable modes and reasons;
- safety posture;
- recommended mode.

Use `OperatorSignalSummary`; do not use unbounded badge rows.

### 3. Replay mode selector

Component:

```text
ReplayModeSelector.tsx
```

Only show selectable modes returned by backend eligibility. Disable unavailable modes with reason. Default to safest useful mode.

### 4. Preview panel

Component:

```text
ReplayPreviewPanel.tsx
```

Show:

- what will be read;
- what will be simulated;
- what will be skipped;
- which services will be contacted;
- provider/tool execution policy;
- expected artifacts.

Advanced source attempts are collapsed by default using `OperatorDisclosure`.

### 5. Run replay action

Component:

```text
ReplayRunAction.tsx
```

- Uses `ConfirmDialog` for deterministic simulation or dry-run.
- Evidence-only/reconstructed can run without scary confirmation, but still shows safety posture.
- Real provider/tool execution is not enabled in this package.

### 6. Replay timeline/result

Component:

```text
ReplayResultTimeline.tsx
```

Show service attempts in order:

- Allagma local reconstruction;
- Kanon replay bundle/provenance attempt;
- Conexus route/model-call attempt;
- export/delta generation.

### 7. Delta comparison

Component:

```text
ReplayDeltaComparison.tsx
```

Use canonical table/list patterns. Keep raw JSON collapsed.

### 8. Evidence Spine links

Component:

```text
ReplayEvidenceLinksPanel.tsx
```

Show:

- original root link;
- replay result link;
- replay bundle export;
- original/replay delta links;
- source attempts.

Reuse `EvidenceSpineOpenLink` and existing evidence row link helpers.

### 9. Export bundle

Component:

```text
ReplayExportPanel.tsx
```

Either reuse or extend `EvidenceSpineExportPanel`. Avoid a duplicate export UX.

## Frontend API/client work

- Add generated schemas after backend OpenAPI updates.
- Add product-level client wrapper under `src/replay/api`.
- Add DTO adapters so UI components do not consume raw backend DTOs directly.
- Update route-workflow catalog.
- Update `API_CLIENT_ROUTE_USAGE.json`.
- Add manual DTO shim registry entry only if generation cannot cover a transitional route.

## Tests

- Unit: eligibility summary mode labeling.
- Unit: mode selector disables unavailable modes with reasons.
- Unit: preview correctly shows provider/tool execution blocked.
- Unit: delta comparison renders matched/diverged/skipped.
- Integration: Evidence Spine node action opens Replay Workbench with prefilled target.
- E2E optional: governed fake replay flow from Evidence Spine to replay result export.

## Explicit UI non-goals

- No second Evidence Spine page.
- No raw JSON wall by default.
- No duplicate warning banners.
- No unbounded badge rows.
- No page-local layout wrappers unless named `temporary_bridge` and documented for deletion.
