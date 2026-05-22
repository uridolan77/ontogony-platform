# Migration: Agent interaction spine v0 (PLAT-AGUI-000)

**Date:** 2026-05-22  
**Type:** Additive contract (no breaking HTTP changes)

## Summary

Ontogony publishes `ontogony-agent-interaction-v0`: JSON Schemas, event-family matrix, JSONL fixtures, and drift gates for a cross-service **agent interaction event log**. This complements the existing Evidence Spine graph contract; it does not replace it.

## Repos affected

| Repo | Action required |
| --- | --- |
| **ontogony-platform** | Consume published schemas and validators (this PR). |
| **ontogony-ui** | `OUI-AGENT-001` — add `./agent` view-model components. |
| **ontogony-frontend** | `OFE-AGUI-001` / `002` — replay workbench and client-side adapters. |
| **allagma-dotnet** | `ALLAGMA-AGUI-001` — optional projection to interaction events. |
| **kanon-dotnet** | `KANON-HITL-AGUI-001` — human-gate interrupt mapping docs/tests. |
| **conexus-dotnet** | `CONEXUS-AGUI-001` — model-call event projection docs/tests. |

## Consumer notes

- Validate emitted events against `docs/schemas/ontogony-agent-interaction-event-v0.schema.json`.
- Use `docs/system/agent-interaction-event.matrix.json` for service ownership and source routes.
- Do not embed raw prompts, completions, secrets, or hidden reasoning in event payloads by default.
- Prefer admin Conexus model-call routes for operator-depth evidence (see matrix `operatorPreferredSources`).

## Validation

```powershell
cd ontogony-platform
.\scripts\validate-agent-interaction-spine.ps1
dotnet test tests/Ontogony.Infrastructure.Tests --filter "FullyQualifiedName~AgentInteraction"
```
