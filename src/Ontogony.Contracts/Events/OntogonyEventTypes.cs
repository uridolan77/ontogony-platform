namespace Ontogony.Contracts.Events;

/// <summary>
/// Well-known <see cref="OntogonyEnvelope{TPayload}.EventType"/> string constants (opaque vocabulary; no routing policy).
/// </summary>
public static class OntogonyEventTypes
{
    /// <summary>HTTP request started.</summary>
    public const string HttpRequestStarted = "http.request.started";

    /// <summary>HTTP request completed.</summary>
    public const string HttpRequestCompleted = "http.request.completed";

    /// <summary>HTTP request failed.</summary>
    public const string HttpRequestFailed = "http.request.failed";

    /// <summary>AG-UI message created.</summary>
    public const string AgUiMessageCreated = "agui.message.created";

    /// <summary>AG-UI state updated.</summary>
    public const string AgUiStateUpdated = "agui.state.updated";

    /// <summary>AG-UI user approved.</summary>
    public const string AgUiUserApproved = "agui.user.approved";

    /// <summary>AG-UI user interrupted.</summary>
    public const string AgUiUserInterrupted = "agui.user.interrupted";

    /// <summary>MCP tool invoked.</summary>
    public const string McpToolInvoked = "mcp.tool.invoked";

    /// <summary>MCP tool completed.</summary>
    public const string McpToolCompleted = "mcp.tool.completed";

    /// <summary>MCP resource read.</summary>
    public const string McpResourceRead = "mcp.resource.read";

    /// <summary>A2A task created.</summary>
    public const string A2ATaskCreated = "a2a.task.created";

    /// <summary>A2A task completed.</summary>
    public const string A2ATaskCompleted = "a2a.task.completed";

    /// <summary>A2A artifact generated.</summary>
    public const string A2AArtifactGenerated = "a2a.artifact.generated";

    /// <summary>LLM request created.</summary>
    public const string LlmRequestCreated = "llm.request.created";

    /// <summary>LLM response completed.</summary>
    public const string LlmResponseCompleted = "llm.response.completed";

    /// <summary>LLM provider failed.</summary>
    public const string LlmProviderFailed = "llm.provider.failed";

    /// <summary>Athanor decision detected.</summary>
    public const string DecisionDetected = "athanor.decision.detected";

    /// <summary>Policy evaluated.</summary>
    public const string PolicyEvaluated = "policy.evaluated";

    /// <summary>Agent run started.</summary>
    public const string AgentRunStarted = "agent.run.started";

    /// <summary>Agent run completed.</summary>
    public const string AgentRunCompleted = "agent.run.completed";

    /// <summary>Agent step completed.</summary>
    public const string AgentStepCompleted = "agent.step.completed";

    /// <summary>Cost recorded.</summary>
    public const string CostRecorded = "cost.recorded";

    /// <summary>Provider failed.</summary>
    public const string ProviderFailed = "provider.failed";
}
