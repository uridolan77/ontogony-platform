using System.ComponentModel.DataAnnotations;

namespace Ontogony.Http;

public sealed class TransportResilienceOptions
{
    [Range(0, 10)]
    public int MaxRetries { get; set; } = 2;

    [Range(10, 60_000)]
    public int BaseDelayMilliseconds { get; set; } = 200;

    [Range(100, 600_000)]
    public int MaxDelayMilliseconds { get; set; } = 5_000;
}
