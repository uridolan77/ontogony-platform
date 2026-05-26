namespace Ontogony.Replay.Contracts;

/// <summary>Replay target identity and ownership metadata.</summary>
public sealed record ReplayTarget(
    string Kind,
    string Identifier,
    string OwnerService,
    bool Root = true,
    string? DisplayIdentifier = null);
