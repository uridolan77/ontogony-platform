namespace Ontogony.Replay.Contracts;

public sealed record ReplayEnvironmentSnapshot(
    string ServiceName,
    string ServiceVersion,
    string? Environment = null,
    string? CommitSha = null,
    string? Framework = null,
    IReadOnlyDictionary<string, string>? PackageVersions = null,
    IReadOnlyDictionary<string, string>? ConfigurationFingerprints = null);
