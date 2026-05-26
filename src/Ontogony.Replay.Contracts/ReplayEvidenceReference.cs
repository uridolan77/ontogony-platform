namespace Ontogony.Replay.Contracts;

/// <summary>Reference to replay evidence payload or location.</summary>
public sealed record ReplayEvidenceReference(
    string Kind,
    string Identifier,
    string? Uri = null,
    string? Fingerprint = null);
