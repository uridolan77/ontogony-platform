namespace Ontogony.Hosting.Contracts;

/// <summary>Wire contract for <c>GET /health</c> (health.v1).</summary>
public sealed record HealthV1Response(
    string SchemaVersion,
    string Status,
    string Service,
    string ServiceDisplayName,
    string Version,
    string? Baseline,
    string? GitSha,
    DateTimeOffset? BuildTimeUtc,
    string Environment,
    string? InstanceId,
    DateTimeOffset CheckedAtUtc,
    IReadOnlyDictionary<string, string>? Links = null,
    IReadOnlyList<string>? Warnings = null);
