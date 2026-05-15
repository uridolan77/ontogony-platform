namespace Ontogony.Observability;

/// <summary>
/// Tag keys for outbound integration metrics emitted by <see cref="IIntegrationOperationMeter"/>.
/// </summary>
public static class IntegrationMetricDimensions
{
    /// <summary>Calling service name.</summary>
    public const string SourceService = "source_service";

    /// <summary>Downstream service name.</summary>
    public const string TargetService = "target_service";

    /// <summary>Logical operation name (for example HTTP method or RPC name).</summary>
    public const string Operation = "operation";

    /// <summary>Outcome status (<c>success</c> or <c>failure</c>).</summary>
    public const string Status = "status";

    /// <summary>Mechanical error code when the call failed.</summary>
    public const string ErrorCode = "error_code";

    /// <summary>HTTP status code when the integration call used HTTP.</summary>
    public const string HttpStatus = "http_status";
}
