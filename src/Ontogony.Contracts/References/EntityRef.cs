namespace Ontogony.Contracts.References;

public sealed record EntityRef(
    string Type,
    string Id,
    string? Label = null,
    string? Version = null);
