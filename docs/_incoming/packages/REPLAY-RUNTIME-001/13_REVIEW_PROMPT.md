# 13 — Review prompt

Review the implementation of `REPLAY-RUNTIME-001` against the current codebase.

## Scope

Repos:

- `C:\dev\ontogony-platform`
- `C:\devllagma-dotnet`
- `C:\dev\kanon-dotnet`
- `C:\dev\conexus-dotnet`
- `C:\dev\ontogony-frontend`
- `C:\dev\ontogony-ui`

## Review questions

1. Did the implementation preserve service boundaries?
2. Does every replay path declare one of the approved modes?
3. Can exact replay be selected only when exactness is proven?
4. Are real providers blocked by default?
5. Are real external tools blocked by default?
6. Does Allagma own replay records/orchestration without owning Kanon or Conexus semantics?
7. Does Kanon remain the authority for semantic replay/provenance claims?
8. Does Conexus remain the authority for model-call, route-decision, provider-attempt replay/dry-run claims?
9. Are replay result bundles connected to Evidence Spine rather than a parallel graph?
10. Are replay artifacts exportable and redacted?
11. Does the frontend use canonical UI primitives?
12. Are raw/source attempts collapsed by default?
13. Are route inventory, OpenAPI, generated clients, route workflow catalog, API client usage, parity checks, and contract discipline updated for every route/DTO change?
14. Do existing governed fake E2E, Evidence Spine, Agent Interaction, runtime lock, and release-readiness truth features still work?
15. Is there at least one concrete governed fake replay path?

## Required evidence

The review should cite:

- changed files;
- tests run;
- smoke artifacts generated;
- replay export sample;
- Evidence Spine link sample;
- contract discipline output;
- any deferred items.

## Failure conditions

Request changes if:

- replay silently calls a real provider;
- replay silently executes a real tool;
- exact replay is exposed without proof;
- UI adds a dense raw replay page;
- route/API changes bypass contract discipline;
- Evidence Spine is duplicated;
- semantic decisions move out of Kanon;
- provider routing moves out of Conexus.
