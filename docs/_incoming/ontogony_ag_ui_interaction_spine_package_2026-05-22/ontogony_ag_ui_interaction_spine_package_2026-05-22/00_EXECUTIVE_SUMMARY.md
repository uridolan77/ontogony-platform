# 00 — Executive Summary

## Decision

Build an **Ontogony Agent Interaction Spine**: a cross-service, event-sourced interaction contract compatible with AG-UI concepts but owned by Ontogony’s architecture.

This should not be a CopilotKit-first migration. It should be an Ontogony-first layer with a clean adapter boundary.

## Rationale

Ontogony already has the primitives that most AG-UI-style systems lack:

- Allagma owns run lifecycle, events, audit, resume/retry/cancel/replay.
- Kanon owns semantic decisions, provenance, human gates, semantic graph, and governance judgment.
- Conexus owns model-call evidence, route decisions, provider attempts, usage/cost, and model-call trace links.
- ontogony-frontend already has trace/correlation/evidence-spine resolution logic.
- @ontogony/ui already has a broad shared UI package with system, execution, observability, semantic, operator, chat, and diagnostics slices.

The missing piece is not “more dashboards.” The missing piece is a **live co-execution surface** where the user can observe, steer, approve, edit, retry, and inspect evidence while agentic runs unfold.

## Architectural principle

AG-UI concepts should be translated into Ontogony’s domain rather than imported blindly.

| AG-UI concept | Ontogony equivalent |
| --- | --- |
| Run lifecycle | Allagma run / step / operation lifecycle |
| State snapshot / delta | Interaction session state + evidence graph + run state |
| Tool call | Conexus model invocation, external action, UI-side tool, Kanon-governed action |
| Human interrupt | Kanon human gate / approval pause / policy escalation |
| Generative UI | Typed UI intent rendered through @ontogony/ui, never arbitrary runtime UI execution |
| Messages | User/agent/operator messages, with redaction and evidence links |
| Reasoning events | Sanitized trace/evidence milestones, not private chain-of-thought |
| Custom events | Ontogony domain events: route decision, semantic decision, audit bundle, evidence edge |

## Recommended build strategy

1. **Define contract and schemas.** Publish `ontogony-agent-interaction-event-v0.schema.json`, TypeScript types, C# records, and mapping docs.
2. **Add UI primitives first.** Build reusable agent timeline, approval cards, state diff, tool call, event inspector, and evidence link components in `@ontogony/ui`.
3. **Create frontend client/store.** Add an `agent-interaction` module in `ontogony-frontend` with JSONL/SSE-compatible event ingestion, normalization, replay, and mock fixtures.
4. **Adapt existing APIs before adding new runtime APIs.** Use Allagma run events, Kanon evidence handoff, and Conexus model-call evidence to synthesize interaction events client-side first.
5. **Add backend streams only after UI and schema stabilize.** Allagma should become the first backend producer of canonical interaction events.
6. **Expose AG-UI compatibility later.** Translate between Ontogony internal events and AG-UI event types at a boundary adapter.

## Package recommendation

The next development milestone should be:

```text
ONTOGONY-AGUI-000 — Agent Interaction Spine Baseline
```

It should include:

- Platform schemas and drift validation.
- @ontogony/ui agent components.
- Frontend mock/replay workbench.
- Client-side adapters from existing Allagma/Kanon/Conexus evidence into interaction events.
- Acceptance tests against deterministic fixtures.

Backend streaming is a second milestone, not the first.
