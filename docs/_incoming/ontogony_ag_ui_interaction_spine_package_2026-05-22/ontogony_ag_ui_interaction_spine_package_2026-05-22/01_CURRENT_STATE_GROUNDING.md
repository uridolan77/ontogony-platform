# 01 — Current State Grounding

This package is grounded in the current repo shape observed on 2026-05-22. The key conclusion is that Ontogony already has a substantial evidence/correlation substrate. The AG-UI work should therefore be additive.

## Existing cross-service evidence contract

`ontogony-platform/docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md` defines the current cross-service contract index. Its purpose statement says operators should paste one identifier and get a single governed execution graph across Allagma, Kanon, and Conexus. It explicitly defines authority boundaries:

- Allagma owns run lifecycle, audit bundle, eval/baseline ids on runs.
- Kanon owns decision records, provenance, semantic graph, human-gate decisions.
- Conexus owns model-call evidence, route decisions, quota/usage.
- Frontend owns `resolveEvidenceSpine` and workbench UI.

The same contract states that v1 implementation is client-side in the operator console rather than a backend unified resolve API.

## Existing frontend resolver

`ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts` already imports and coordinates:

- Allagma run, evaluation, baseline, run events.
- Conexus model-call list/detail, execution-run lookup, route-decision detail.
- Kanon decision and source-binding routes.
- `extractHumanGateIdFromEvents`.
- `resolveTraceCorrelation`.
- evidence graph helpers and source-attempt tracking.

This is the strongest reason not to build a separate AG-UI product island. The new interaction spine should wrap and reuse the existing resolver outputs.

## Existing trace/correlation resolver

`ontogony-frontend/src/system/correlation/resolveTraceCorrelation.ts` already resolves across:

- Allagma run by run id or trace id.
- Conexus execution run by model call id.
- Kanon decision by trace or id.
- Allagma run events for human gate extraction.
- correlation links into UI items from `@ontogony/ui/system`.

This file gives the interaction spine a ready-made identity-resolution layer.

## Existing Allagma runtime surface

`allagma-dotnet/docs/system/allagma-feature-connection.matrix.json` lists direct HTTP endpoints for:

- list/start/get runs
- list run events
- get run audit
- get run operations
- resume/retry/cancel/replay
- list/write run evaluations
- capabilities and runtime posture

It also documents Kanon typed calls for plan compilation, topology evaluation, action evaluation, and human-gate check, plus Conexus typed calls for chat completions. This is exactly the runtime substrate needed for an AG-UI-like event bridge.

## Existing Kanon evidence handoff

`kanon-dotnet/docs/operators/KANON_EVIDENCE_SPINE_HANDOFF.md` maps Kanon evidence objects to routes:

- decision records
- provenance
- replay bundles
- semantic graph anchors
- domain-pack evolution
- Conexus assistance review
- topology policy
- human gates

It also defines cross-service caller obligations for propagating `X-Kanon-Decision-Id`, Allagma topology decision ids, trace ids, and correlation ids. This becomes the semantic-authority layer for interaction interrupts.

## Existing Conexus model-call evidence flow

`conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md` defines a deterministic operator flow:

1. Create model call via `/v1/chat/completions`.
2. Read project-scoped model-call evidence.
3. Read admin model-call detail.
4. Read evidence links.
5. Read evidence bundle.
6. Read route-decision detail.

It also defines ID consistency between `modelCallId`, `routeDecisionId`, `traceId`, `correlationId`, and `requestId`. This should map into `MODEL_CALL_*`, `ROUTE_DECISION_*`, and evidence-link interaction events.

## Existing @ontogony/ui package capacity

`@ontogony/ui` already exports many subpaths: `./system`, `./execution`, `./observability`, `./semantic`, `./operator`, `./diagnostics`, `./chat`, `./dialogs`, `./status`, `./layout`, and more. The new agentic UI work should add a focused `./agent` subpath rather than overloading existing slices.

## Current gap

The current system can resolve and inspect evidence **after or around** execution. It does not yet expose a canonical, live, user-facing interaction event contract that lets UI and agent runtime share:

- run lifecycle
- step progress
- message streaming
- tool/model calls
- state snapshots and deltas
- human interrupts
- approval/resume payloads
- evidence links
- UI intents
- replayable fixtures

That is the gap this package addresses.
