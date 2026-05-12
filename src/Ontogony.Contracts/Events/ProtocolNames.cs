namespace Ontogony.Contracts.Events;

/// <summary>
/// Opaque <see cref="OntogonyEnvelope{TPayload}.Protocol"/> discriminator strings (no registry enforcement here).
/// </summary>
public static class ProtocolNames
{
    /// <summary>AG-UI protocol.</summary>
    public const string AgUi = "ag-ui";

    /// <summary>Generic JSON payloads.</summary>
    public const string GenericJson = "generic-json";

    /// <summary>Model Context Protocol.</summary>
    public const string Mcp = "mcp";

    /// <summary>Agent-to-agent protocol.</summary>
    public const string A2A = "a2a";

    /// <summary>OpenTelemetry.</summary>
    public const string OpenTelemetry = "otel";

    /// <summary>CloudEvents.</summary>
    public const string CloudEvents = "cloudevents";

    /// <summary>Conexus gateway.</summary>
    public const string Conexus = "conexus";

    /// <summary>Agentor product.</summary>
    public const string Agentor = "agentor";

    /// <summary>Athanor product.</summary>
    public const string Athanor = "athanor";

    /// <summary>Vercel AI SDK.</summary>
    public const string VercelAiSdk = "vercel-ai-sdk";

    /// <summary>OpenAI Agents SDK.</summary>
    public const string OpenAiAgents = "openai-agents";

    /// <summary>LangGraph.</summary>
    public const string LangGraph = "langgraph";
}
