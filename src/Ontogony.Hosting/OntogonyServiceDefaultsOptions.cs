namespace Ontogony.Hosting;

public sealed class OntogonyServiceDefaultsOptions
{
    public const string SectionName = "Ontogony:Hosting:ServiceDefaults";

    public string ServiceName { get; set; } = "unknown-service";
    public string ServiceVersion { get; set; } = "0.1.0";
    public bool AddObservability { get; set; } = true;
    public bool AddErrors { get; set; } = true;
    public bool UseRequestTracing { get; set; } = true;
    public bool UseExceptionHandling { get; set; } = true;
    public bool UseServiceIdentityBodyHashPreload { get; set; } = false;
    public bool MapHealthEndpoints { get; set; } = true;
    public string HealthPath { get; set; } = "/health";
    public string ReadinessPath { get; set; } = "/ready";
}
