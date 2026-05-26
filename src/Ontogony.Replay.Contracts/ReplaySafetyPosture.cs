namespace Ontogony.Replay.Contracts;

/// <summary>Replay safety posture for provider, tool, and redaction policies.</summary>
public sealed record ReplaySafetyPosture(
    string ProviderExecutionPolicy = "forbid_real_providers",
    string ToolExecutionPolicy = "forbid_real_tools",
    string RedactionPolicy = "operator_default",
    bool RealProvidersBlocked = true,
    bool RealToolsBlocked = true);
