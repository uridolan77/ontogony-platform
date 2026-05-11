# Event Type Conventions

Use:

```text
protocol.entity.verb
```

Examples:

```text
agui.message.created
agui.state.updated
agui.user.approved
mcp.tool.invoked
mcp.tool.completed
a2a.task.created
a2a.task.completed
llm.request.created
llm.response.completed
policy.evaluated
athanor.decision.detected
```

Rules:

1. Use lowercase.
2. Use dots as separators.
3. Keep protocol prefix explicit.
4. Past-tense verbs for facts already observed.
5. Imperative command events should be separate and explicit, e.g. `mcp.tool.invoke_requested`.
