# Repo Copy Plan

This folder lists suggested destination paths. Do not copy everything blindly. Use it as a map for implementation PRs.

## ontogony-platform

- `schemas/ontogony-agent-interaction-event-v0.schema.json` -> `docs/schemas/ontogony-agent-interaction-event-v0.schema.json`
- `schemas/ontogony-agent-interaction-session-v0.schema.json` -> `docs/schemas/ontogony-agent-interaction-session-v0.schema.json`
- `schemas/agent-interaction-event.matrix.json` -> `docs/system/agent-interaction-event.matrix.json`
- `03_ONTOGONY_AGENT_INTERACTION_SPINE_CONTRACT.md` -> `docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md`

## ontogony-ui

- `contracts/typescript/ontogony-agent-interaction-types.ts` as source reference for `src/components/agent/types.ts`
- `07_UI_PACKAGE_PLAN.md` as PR plan

## ontogony-frontend

- `contracts/typescript/interaction-reducer-sketch.ts` as reducer seed
- `fixtures/events/*.jsonl` into `src/agent-interaction/fixtures/`
- `06_FRONTEND_IMPLEMENTATION_PLAN.md` as PR plan

## allagma-dotnet

- `contracts/csharp/OntogonyAgentInteractionContracts.cs` as DTO sketch
- `08_ALLAGMA_ADAPTER_PLAN.md` as PR plan

## kanon-dotnet

- `09_KANON_HITL_PLAN.md` as PR plan

## conexus-dotnet

- `10_CONEXUS_TRACE_EVENTS_PLAN.md` as PR plan
