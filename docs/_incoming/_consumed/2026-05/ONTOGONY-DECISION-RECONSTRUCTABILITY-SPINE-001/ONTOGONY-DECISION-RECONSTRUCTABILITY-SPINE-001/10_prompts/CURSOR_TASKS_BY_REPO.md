# Cursor Tasks by Repo

## Task 1 — Kanon

```text
Implement the Kanon core of ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001.

Start by inspecting existing Kanon Evidence Spine, decision record lifecycle, route inventory, contracts, and test conventions. Then add the minimal contracts and deterministic classifier for DecisionEvent/ReconstructabilityReport/MissingEvidenceDiagnostic. Add golden fixtures and unit tests for F/P/S/O classification, strict score, PASS/WARN/FAIL governance, and safe reasoning surrogate policy. Prefer existing namespaces and docs conventions. Regenerate route inventory/OpenAPI if routes are added. Update docs with actual implemented routes/classes.
```

## Task 2 — Allagma

```text
Implement the Allagma fragment side of ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001.

Inspect current run lifecycle, operations endpoint, replay runtime, human gates, actor context, evidence spine links, and tests. Add or extend projections so run operations and human gates expose reconstructability fragments: actor, authorization, output action, state-before/after or explicit not-applicable. Add tests for approved gate, denied gate, non-mutating operation, and mutating operation missing state. Do not add duplicate routes if existing run/operation details can be extended.
```

## Task 3 — Conexus

```text
Implement the Conexus fragment side of ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001.

Inspect current model-call, route-decision, provider routing, guardrail/fallback, admin auth, and tracing contracts. Add fragment/projection fields for route decisions and model calls: input/output hashes, selected provider/model, routing policy/default marker, safe route explanation/reason code, guardrail/fallback status, tool action metadata, trace/correlation ids. Add tests proving no hidden chain-of-thought is stored and that blocked/fallback route decisions produce reconstructable evidence.
```

## Task 4 — Frontend/UI

```text
Implement the operator console UI for ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001.

Inspect existing Evidence Spine graph/detail panels, Allagma run pages, Kanon decision pages, Conexus model call pages, and @ontogony/ui component patterns. Add a Decision Reconstruction panel/drawer with header, PASS/WARN/FAIL, strict score, F/P/S/O property table, missing-evidence diagnostics, linked fragments, post-condition state summary, and safe-reasoning notice. Keep list views compact. Mark fixture data clearly. Add tests for full, partial, blocking, opaque, and fixture reports.
```

## Task 5 — Platform/docs

```text
Update ontogony-platform documentation for ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001.

Add protocol docs for DecisionEvent, ReconstructabilityReport, F/P/S/O semantics, critical action blocking rules, local demo flow, and cross-repo acceptance checklist. If existing validation scripts exist, add an optional local reconstructability validation script. Keep docs aligned with actual routes and DTOs after backend work is complete.
```
