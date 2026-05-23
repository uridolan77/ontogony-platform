namespace Ontogony.Hosting.Contracts;

/// <summary>Single readiness check in ready.v1.</summary>
public sealed record ReadinessCheckV1(
    string Id,
    string Label,
    string Status,
    string Severity,
    string? Detail = null,
    string? System = null,
    string? DataSource = null,
    IReadOnlyDictionary<string, string>? Links = null,
    DateTimeOffset? LastCheckedAtUtc = null);
