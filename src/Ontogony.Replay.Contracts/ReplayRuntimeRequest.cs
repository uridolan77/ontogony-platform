namespace Ontogony.Replay.Contracts;

/// <summary>Replay runtime request boundary record.</summary>
public sealed record ReplayRuntimeRequest(
    string ReplayId,
    string RootIdentifier,
    string RootKind,
    string RequestedMode,
    DateTimeOffset CreatedAt,
    ReplayRequestedBy? RequestedBy = null,
    string ProviderExecutionPolicy = "forbid_real_providers",
    string ToolExecutionPolicy = "forbid_real_tools",
    string RedactionPolicy = "operator_default");

/// <summary>Actor that requested a replay run.</summary>
public sealed record ReplayRequestedBy(
    string ActorId,
    IReadOnlyList<string>? Roles = null);
