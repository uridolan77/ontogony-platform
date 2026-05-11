namespace Ontogony.Contracts.References;

/// <summary>
/// Reference to the subject of an event (the primary entity the event describes).
/// </summary>
public sealed record SubjectRef(
    string Type,
    string Id,
    string? DisplayName = null,
    string? Source = null);
