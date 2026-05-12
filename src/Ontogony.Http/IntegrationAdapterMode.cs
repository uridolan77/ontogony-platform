namespace Ontogony.Http;

/// <summary>
/// Selects how an integration HTTP client behaves at runtime.
/// </summary>
public enum IntegrationAdapterMode
{
    /// <summary>No outbound HTTP (throws or short-circuits depending on host wiring).</summary>
    Disabled = 0,

    /// <summary>Test double / stub mode (host-defined).</summary>
    Fake = 1,

    /// <summary>Live HTTP calls.</summary>
    Http = 2
}
