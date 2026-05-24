namespace Ontogony.Replay.Contracts;

public sealed record ReplayEvidenceReference(
    string Kind,
    string Identifier,
    string? Uri = null,
    string? Fingerprint = null);
