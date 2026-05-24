namespace Ontogony.Replay.Contracts;

public sealed record ReplayTarget(
    string Kind,
    string Identifier,
    string OwnerService,
    bool Root = true,
    string? DisplayIdentifier = null);
