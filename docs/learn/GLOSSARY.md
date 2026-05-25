# Glossary

> **Audience:** all  
> **Applies to:** cross-repo Ontogony  
> **Source of truth:** [`../operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](../operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) (operator terms); this page is the learning-path index  
> **Last verified:** 2026-05-25

## Core services

| Term | Definition |
| --- | --- |
| **Kanon** | Semantic authority: ontology, decisions, provenance, action policy |
| **Allagma** | Governed execution: runs, events, tool intents, human gates, replay orchestration |
| **Conexus** | Model gateway: aliases, providers, routing, usage |
| **Ontogony.Platform** | Shared mechanics packages (`Ontogony.*`) |
| **Operator console** | `ontogony-frontend` + `ontogony-ui` |

## Identifiers

| Term | Definition |
| --- | --- |
| **runId** | Allagma governed run primary key |
| **traceId** | End-to-end correlation across services |
| **correlationId** | Secondary correlation / business context |
| **planningDecisionId** | Kanon decision used to plan a run |
| **modelCallId** | Conexus chat completion id (`chatcmpl-…`) |
| **routeDecisionId** | Conexus routing decision record |
| **replayId** | Cross-service replay orchestration record |

## Execution

| Term | Definition |
| --- | --- |
| **ToolIntent** | Proposed consequential action awaiting Kanon evaluation |
| **human_gate** | Kanon outcome requiring operator approval before proceed |
| **model purpose** | Allagma-declared reason for a model invocation |
| **model alias** | Conexus-stable logical model name |
| **provider route** | Resolved provider endpoint + policy |
| **MAF** | Microsoft Agent Framework adapter (isolated in Allagma composition) |

## Evidence and proof

| Term | Definition |
| --- | --- |
| **Evidence Spine** | Console graph resolver across Allagma/Kanon/Conexus |
| **Agent Interaction** | Run-scoped event stream workbench |
| **Governed fake E2E** | Local proof smoke across three services |
| **Runtime lock** | `ontogony-runtime.lock.json` reproducibility pin |
| **System Truth** | Console release readiness / connectivity surface |

## Replay

| Term | Definition |
| --- | --- |
| **evidence_only** | Replay mode that rebuilds manifests without re-executing providers/tools |
| **clientIdempotencyKey** | Stable key for replay request dedup on run events |
| **RunReplayRequested** | Allagma event when manifest replay is recorded |

## UI

| Term | Definition |
| --- | --- |
| **OperatorPageFrame** | Canonical page shell (`@ontogony/ui`) |
| **route-workflow catalog** | Frontend map of routes to operator workflows |
| **contracts:discipline** | Frontend gate for OpenAPI/route parity |

## Full operator glossary

[`../operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](../operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) — liveness vs readiness, decision IDs, fake provider, etc.
