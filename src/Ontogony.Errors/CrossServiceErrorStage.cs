namespace Ontogony.Errors;

/// <summary>Neutral stage labels for cross-service error envelopes (string-extensible).</summary>
public static class CrossServiceErrorStage
{
    /// <summary>Inbound request handling.</summary>
    public const string Request = "request";

    /// <summary>Outbound downstream integration.</summary>
    public const string Downstream = "downstream";

    /// <summary>Semantic planning or compile phase.</summary>
    public const string Planning = "planning";

    /// <summary>Policy evaluation or human gate.</summary>
    public const string Policy = "policy";

    /// <summary>Model gateway completion.</summary>
    public const string Model = "model";

    /// <summary>Persistence or storage.</summary>
    public const string Persistence = "persistence";

    /// <summary>Readiness or startup checks.</summary>
    public const string Readiness = "readiness";

    /// <summary>Idempotency coordination.</summary>
    public const string Idempotency = "idempotency";
}
