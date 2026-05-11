using System.ComponentModel.DataAnnotations;

namespace Ontogony.Http;

public sealed class HttpIntegrationOptions
{
    public IntegrationAdapterMode Mode { get; set; } = IntegrationAdapterMode.Disabled;

    public string? BaseUrl { get; set; }

    [Range(1, 600)]
    public int TimeoutSeconds { get; set; } = 30;

    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
}
