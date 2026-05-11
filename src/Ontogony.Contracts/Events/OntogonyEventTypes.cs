namespace Ontogony.Contracts.Events;

public static class OntogonyEventTypes
{
    public const string HttpRequestStarted = "http.request.started";
    public const string HttpRequestCompleted = "http.request.completed";
    public const string HttpRequestFailed = "http.request.failed";

    public const string AgUiMessageCreated = "agui.message.created";
    public const string AgUiStateUpdated = "agui.state.updated";
    public const string AgUiUserApproved = "agui.user.approved";
    public const string AgUiUserInterrupted = "agui.user.interrupted";

    public const string McpToolInvoked = "mcp.tool.invoked";
    public const string McpToolCompleted = "mcp.tool.completed";
    public const string McpResourceRead = "mcp.resource.read";

    public const string A2ATaskCreated = "a2a.task.created";
    public const string A2ATaskCompleted = "a2a.task.completed";
    public const string A2AArtifactGenerated = "a2a.artifact.generated";

    public const string LlmRequestCreated = "llm.request.created";
    public const string LlmResponseCompleted = "llm.response.completed";
    public const string LlmProviderFailed = "llm.provider.failed";

    public const string DecisionDetected = "athanor.decision.detected";
    public const string PolicyEvaluated = "policy.evaluated";

    public const string AgentRunStarted = "agent.run.started";
    public const string AgentRunCompleted = "agent.run.completed";
    public const string AgentStepCompleted = "agent.step.completed";

    public const string CostRecorded = "cost.recorded";
    public const string ProviderFailed = "provider.failed";
}
