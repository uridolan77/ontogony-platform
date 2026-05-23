# GOVERNED-FAKE-E2E-001 — Cursor Master Prompt

You are working in the Ontogony local alpha system across these repos:

- `ontogony-platform`
- `allagma-dotnet`
- `kanon-dotnet`
- `conexus-dotnet`
- `ontogony-frontend`
- `ontogony-ui`

## Mission

Implement **GOVERNED-FAKE-E2E-001**: prove one live, non-fixture, governed fake-provider run end-to-end.

The result must demonstrate that the local operator console can start or select a real governed run and resolve its evidence chain across:

```text
Allagma run
  -> Kanon planning / semantic decision
  -> Conexus fake model call
  -> Conexus route decision
  -> Conexus provider attempt
  -> shared trace id
  -> shared correlation id
  -> exported Evidence Spine bundle
```

This is a **truth-hardening** package, not a feature fantasy package. Do not overstate readiness. Do not count fixture replay as live proof. Do not enable real external tools. Do not enable real OpenAI calls. This sprint proves the governed local fake path only.

## Current symptoms to address

The current console can show a direct Conexus fake chat and reconstruct the Conexus side of the evidence graph, but the graph is partial:

- direct Conexus chat does not naturally have a Kanon decision;
- route decision id can appear in model-call evidence but fail detail resolution;
- Agent Interaction defaults to fixture replay;
- cross-service trace bridge is described as "ready to test" rather than proven;
- replay/evidence pages often start with no identifiers or unresolved links;
- partial graphs do not always explain missing links precisely;
- fixture/demo IDs can look too similar to live evidence.

This package focuses on creating one **live governed fake run** that should legitimately include the Kanon and Conexus links.

## Hard constraints

1. **No real external execution.**
2. **No real OpenAI/provider calls.**
3. **Use fake Conexus provider only.**
4. **Use existing alpha-local ontology path, normally `gaming-core@0.1.0`.**
5. **Do not convert demo fixture data into "evidence."**
6. **Every green/resolved claim must be backed by live API data.**
7. **Every missing edge must have a reason code.**
8. **Keep changes narrow enough to merge safely.**

## Expected live flow

```text
1. Operator starts governed fake run from Allagma or uses latest completed governed fake run.
2. Allagma creates run with ontologyVersionId = gaming-core@0.1.0.
3. Allagma creates/propagates traceId and correlationId.
4. Allagma calls Kanon for semantic planning / decision.
5. Kanon records decision with the same traceId/correlationId.
6. Allagma calls Conexus using model purpose -> alias resolution.
7. Conexus routes alias to fake / fake.chat.
8. Conexus records model call, route decision, provider attempt, usage.
9. Allagma stores returned modelCallId and relevant evidence IDs.
10. Evidence Spine resolves the full graph from runId, traceId, correlationId, decisionId, modelCallId, or routeDecisionId.
```

## Implementation order

1. Read `01_CURRENT_DIAGNOSIS.md`.
2. Implement backend identifier propagation first.
3. Fix Conexus route decision resolution if still broken.
4. Add Evidence Spine reason/applicability rules.
5. Add latest-live-run/live-governed-fake shortcuts.
6. Add integration and frontend regression tests.
7. Run manual QA from `10_MANUAL_QA_RUNBOOK.md`.
8. Complete `11_ACCEPTANCE_CRITERIA.md`.

## Definition of done

A local operator can start a governed fake run and then open Evidence Spine. The graph contains real live nodes, not fixture nodes:

- `allagma.run`
- `platform.trace`
- `platform.correlation`
- `kanon.decision`
- `conexus.modelCall`
- `conexus.routeDecision`
- `conexus.providerAttempt`

The bundle exports without unresolved route-decision detail, and Kanon decision is resolved when the flow is governed.
