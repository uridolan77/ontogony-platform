namespace Ontogony.Replay.Contracts;

public sealed record ReplaySafetyPosture(
    string ProviderExecutionPolicy = "forbid_real_providers",
    string ToolExecutionPolicy = "forbid_real_tools",
    string RedactionPolicy = "operator_default",
    bool RealProvidersBlocked = true,
    bool RealToolsBlocked = true);
