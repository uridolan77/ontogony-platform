namespace Ontogony.Contracts.References;

/// <summary>
/// Protocol-neutral entity reference (opaque type/id; no storage or graph semantics).
/// </summary>
/// <param name="Type">Opaque entity type name.</param>
/// <param name="Id">Entity identifier.</param>
/// <param name="Label">Optional human label.</param>
/// <param name="Version">Optional opaque version or etag.</param>
public sealed record EntityRef(
    string Type,
    string Id,
    string? Label = null,
    string? Version = null);
