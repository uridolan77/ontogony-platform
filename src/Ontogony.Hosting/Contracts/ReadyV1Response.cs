namespace Ontogony.Hosting.Contracts;

/// <summary>Wire contract for <c>GET /ready</c> (ready.v1).</summary>
public sealed record ReadyV1Response(
    string SchemaVersion,
    string Status,
    string Service,
    DateTimeOffset CheckedAtUtc,
    IReadOnlyList<ReadinessCheckV1> Checks,
    string? Summary = null,
    IReadOnlyList<string>? Warnings = null,
    IReadOnlyList<string>? Failures = null);
