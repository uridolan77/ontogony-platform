# 04 — Backend Plan: Allagma

## Goal

Allagma must become the causal spine for the governed fake run.

## Required behavior

When starting a governed fake run, Allagma must:

1. create or receive `traceId`;
2. create or receive `correlationId`;
3. store both on the run or in auditable event metadata;
4. call Kanon using the same trace/correlation context;
5. persist returned Kanon planning decision id;
6. call Conexus using the same trace/correlation context;
7. persist returned Conexus model call id;
8. expose all these IDs via run detail, run audit, run events, or a dedicated evidence endpoint.

## Candidate code areas

Check these areas first:

```text
allagma-dotnet/src/Allagma.Contracts/RunContracts.cs
allagma-dotnet/src/Allagma.Domain/AgentRun.cs
allagma-dotnet/src/Allagma.Application/StartAgentRunService.cs
allagma-dotnet/src/Allagma.Application/RetryAgentRunService.cs
allagma-dotnet/src/Allagma.Application/GamingCoreSummarizePlayerRiskCompileIntentMapper.cs
allagma-dotnet/src/Allagma.Application/*Kanon*
allagma-dotnet/src/Allagma.Application/*Conexus*
allagma-dotnet/src/Allagma.Infrastructure/Persistence/EfAgentRunRepository.cs
allagma-dotnet/tests/Allagma.Tests/
```

## Model purpose discipline

Allagma should not send a concrete model name such as `gpt-4o-mini` as the domain default.

Expected:

```text
modelPurpose: summarize-player-risk
Conexus alias: risk-summary-v0
Provider/model: resolved by Conexus
```

## Events to record

At minimum, emit or persist evidence events like:

```json
{
  "eventType": "kanon.planning_decision.recorded",
  "decisionId": "decision_...",
  "traceId": "...",
  "correlationId": "..."
}
```

```json
{
  "eventType": "conexus.model_call.completed",
  "modelCallId": "chatcmpl-...",
  "routeDecisionId": "rd-...",
  "traceId": "...",
  "correlationId": "..."
}
```

## Acceptance

- `GET /allagma/v0/runs/{runId}` exposes or links enough information to find planning decision and model call.
- `GET /allagma/v0/runs/{runId}/events` exposes trace/correlation and downstream ids.
- Evidence Spine can start from run id and discover Kanon and Conexus nodes without fixture data.
