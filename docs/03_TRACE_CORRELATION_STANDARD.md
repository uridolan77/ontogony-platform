# 03 — Trace Correlation Standard

## Goal

Every event across AG-UI, MCP, A2A, model calls, tool calls, HTTP calls, queues, outbox dispatches, and user approvals must be correlatable.

## Canonical header

```text
X-Ontogony-Trace-Id
```

## Accepted legacy aliases

```text
X-Athanor-Trace-Id
X-Agentor-Trace-Id
X-Conexus-Request-Id
```

## Propagation rules

1. If `X-Ontogony-Trace-Id` exists, use it.
2. Else if a legacy trace/request header exists, adopt it.
3. Else generate a new trace ID.
4. Echo `X-Ontogony-Trace-Id` in every response.
5. During migration, also echo Athanor/Agentor legacy trace headers.
6. Propagate trace context to outgoing HTTP calls.
7. Include the trace ID in every `OntogonyEnvelope`.

## Recommended trace lifecycle

```text
Frontend / AG-UI session
  → Agentor run
  → Conexus model call
  → MCP tool call
  → Athanor event record
  → canonical decision/evidence reconstruction
```

All of the above must share one trace ID.

## Activity span tags

Recommended core tags:

```text
ontogony.trace_id
ontogony.operation_id
service.name
service.version
tenant.id
workspace.id
project.id
actor.id
protocol.name
event.type
```

Protocol-specific tags:

```text
agui.session_id
mcp.tool_name
a2a.task_id
llm.provider
llm.model
llm.input_tokens
llm.output_tokens
llm.cost_usd
```
