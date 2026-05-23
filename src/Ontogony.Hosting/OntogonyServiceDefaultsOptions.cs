namespace Ontogony.Hosting;

/// <summary>
/// Opinionated defaults for ASP.NET Core services (observability, errors, health, JSON).
/// </summary>
public sealed class OntogonyServiceDefaultsOptions
{
    /// <summary>Configuration section path for binding.</summary>
    public const string SectionName = "Ontogony:Hosting:ServiceDefaults";

    /// <summary>Logical service name propagated to observability options.</summary>
    public string ServiceName { get; set; } = "unknown-service";

    /// <summary>Service version string for telemetry.</summary>
    public string ServiceVersion { get; set; } = "0.1.0";

    /// <summary>Short service id for health.v1/ready.v1 payloads (e.g. <c>conexus</c>). Defaults from <see cref="ServiceName"/>.</summary>
    public string? ServiceHealthId { get; set; }

    /// <summary>Operator-facing display name in health.v1.</summary>
    public string? ServiceDisplayName { get; set; }

    /// <summary>Locked system baseline label (e.g. SYSTEM-ALPHA-006).</summary>
    public string? SystemBaseline { get; set; }

    /// <summary>Build git SHA when available.</summary>
    public string? GitSha { get; set; }

    /// <summary>Build timestamp when available.</summary>
    public DateTimeOffset? BuildTimeUtc { get; set; }

    /// <summary>Instance id for multi-instance diagnostics.</summary>
    public string? InstanceId { get; set; }

    /// <summary>When true, registers Ontogony observability services.</summary>
    public bool AddObservability { get; set; } = true;

    /// <summary>When true, registers Ontogony error mapping services.</summary>
    public bool AddErrors { get; set; } = true;

    /// <summary>When true, enables request tracing middleware in <see cref="OntogonyHostingExtensions.UseOntogonyServiceDefaults"/>.</summary>
    public bool UseRequestTracing { get; set; } = true;

    /// <summary>When true, enables exception handling middleware.</summary>
    public bool UseExceptionHandling { get; set; } = true;

    /// <summary>When true, enables service-identity body hash preload middleware.</summary>
    public bool UseServiceIdentityBodyHashPreload { get; set; } = false;

    /// <summary>When true, maps liveness and readiness health endpoints.</summary>
    public bool MapHealthEndpoints { get; set; } = true;

    /// <summary>Liveness probe path.</summary>
    public string HealthPath { get; set; } = "/health";

    /// <summary>Readiness probe path (checks tagged as <c>ready</c>).</summary>
    public string ReadinessPath { get; set; } = "/ready";
}
