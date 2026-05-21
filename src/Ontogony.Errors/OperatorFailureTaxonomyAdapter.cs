namespace Ontogony.Errors;

/// <summary>Maps neutral <see cref="CrossServiceErrorEnvelope"/> values to operator taxonomy (SYS-TIGHT-006).</summary>
public static class OperatorFailureTaxonomyAdapter
{
    /// <summary>Maps a cross-service envelope to an operator taxonomy view.</summary>
    public static OperatorFailureView FromCrossServiceEnvelope(CrossServiceErrorEnvelope envelope) =>
        Build(
            envelope.Code,
            envelope.Message,
            envelope.System,
            envelope.Retryable,
            envelope.Stage,
            envelope.DownstreamSystem,
            envelope.TraceId,
            envelope.CorrelationId);

    /// <summary>Maps an Ontogony <see cref="ApiError"/> into an operator taxonomy view for a given system.</summary>
    public static OperatorFailureView FromApiError(
        ApiError apiError,
        string system,
        string? downstreamSystem = null,
        bool? retryable = null,
        string? stage = null,
        string? correlationId = null) =>
        FromCrossServiceEnvelope(
            CrossServiceErrorEnvelopeExtensions.FromApiError(
                apiError,
                system,
                stage,
                downstreamSystem,
                correlationId,
                retryable));

    private static OperatorFailureView Build(
        string code,
        string message,
        string system,
        bool? retryable,
        string? stage,
        string? downstreamSystem,
        string? traceId,
        string? correlationId)
    {
        var taxonomy = ResolveTaxonomy(code, retryable, stage, downstreamSystem);
        return new OperatorFailureView(
            taxonomy,
            TitleFor(taxonomy),
            string.IsNullOrWhiteSpace(message) ? TitleFor(taxonomy) : message,
            code,
            system,
            retryable,
            stage,
            downstreamSystem,
            traceId,
            correlationId,
            RecommendedActionsFor(taxonomy, downstreamSystem, retryable));
    }

    private static string ResolveTaxonomy(
        string code,
        bool? retryable,
        string? stage,
        string? downstreamSystem)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return OperatorFailureTaxonomyKind.Unknown;
        }

        var normalized = code.Trim();

        if (Matches(normalized, CrossServiceErrorCodes.AuthMissing)
            || normalized.Contains("unauthorized", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.AuthFailed;
        }

        if (Matches(normalized, CrossServiceErrorCodes.AuthForbidden)
            || normalized.Contains("policy_denied", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("human_gate", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains(".forbidden", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.Forbidden;
        }

        if (Matches(normalized, CrossServiceErrorCodes.ValidationFailed)
            || string.Equals(normalized, "ValidationFailed", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.ValidationFailed;
        }

        if (normalized.Contains("not_found", StringComparison.OrdinalIgnoreCase)
            || string.Equals(normalized, "NotFound", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.NotFound;
        }

        if (Matches(normalized, CrossServiceErrorCodes.IdempotencyConflict)
            || Matches(normalized, CrossServiceErrorCodes.IdempotencyInProgress)
            || normalized.Contains("idempotency", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.IdempotencyConflict;
        }

        if (Matches(normalized, CrossServiceErrorCodes.Timeout))
        {
            return OperatorFailureTaxonomyKind.Timeout;
        }

        if (normalized.Contains("quota", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("insufficient_quota", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.QuotaExceeded;
        }

        if (Matches(normalized, CrossServiceErrorCodes.DownstreamUnavailable)
            || normalized.Contains("plan_unavailable", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("provider_unavailable", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("topology_unavailable", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("readiness.unavailable", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.DownstreamUnavailable;
        }

        if (Matches(normalized, CrossServiceErrorCodes.DownstreamFailure)
            || string.Equals(stage, CrossServiceErrorStage.Model, StringComparison.OrdinalIgnoreCase)
            || string.Equals(downstreamSystem, "conexus", StringComparison.OrdinalIgnoreCase))
        {
            return retryable == true
                ? OperatorFailureTaxonomyKind.ProviderFailedRetryable
                : OperatorFailureTaxonomyKind.ProviderFailedTerminal;
        }

        if (normalized.Contains("conflict", StringComparison.OrdinalIgnoreCase)
            || string.Equals(normalized, "Conflict", StringComparison.OrdinalIgnoreCase))
        {
            return OperatorFailureTaxonomyKind.Conflict;
        }

        if (retryable == true
            && (normalized.Contains("unavailable", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("gateway", StringComparison.OrdinalIgnoreCase)))
        {
            return OperatorFailureTaxonomyKind.ProviderFailedRetryable;
        }

        return OperatorFailureTaxonomyKind.Unknown;
    }

    private static bool Matches(string code, string expected) =>
        string.Equals(code, expected, StringComparison.OrdinalIgnoreCase);

    private static string TitleFor(string taxonomy) =>
        taxonomy switch
        {
            OperatorFailureTaxonomyKind.AuthFailed => "Authentication failed",
            OperatorFailureTaxonomyKind.Forbidden => "Action not permitted",
            OperatorFailureTaxonomyKind.ValidationFailed => "Request validation failed",
            OperatorFailureTaxonomyKind.NotFound => "Resource not found",
            OperatorFailureTaxonomyKind.Conflict => "Conflict",
            OperatorFailureTaxonomyKind.IdempotencyConflict => "Idempotency conflict",
            OperatorFailureTaxonomyKind.DownstreamUnavailable => "Downstream service unavailable",
            OperatorFailureTaxonomyKind.ProviderFailedRetryable => "Provider error (retryable)",
            OperatorFailureTaxonomyKind.ProviderFailedTerminal => "Provider error",
            OperatorFailureTaxonomyKind.QuotaExceeded => "Quota exceeded",
            OperatorFailureTaxonomyKind.Timeout => "Request timed out",
            _ => "Unexpected failure",
        };

    private static IReadOnlyList<string> RecommendedActionsFor(
        string taxonomy,
        string? downstreamSystem,
        bool? retryable)
    {
        var downstream = string.IsNullOrWhiteSpace(downstreamSystem)
            ? "the downstream service"
            : downstreamSystem.Trim();

        return taxonomy switch
        {
            OperatorFailureTaxonomyKind.AuthFailed =>
            [
                "Verify operator settings: service token, project API key, and base URLs.",
                "Confirm the target environment matches this console configuration.",
            ],
            OperatorFailureTaxonomyKind.Forbidden =>
            [
                "Confirm the actor or topology authorization allows this action.",
                "Open Kanon policy or human-gate evidence if the run was gated.",
            ],
            OperatorFailureTaxonomyKind.ValidationFailed =>
            [
                "Review request inputs against the API contract or OpenAPI schema.",
                "Fix validation errors before retrying the same payload.",
            ],
            OperatorFailureTaxonomyKind.NotFound =>
            [
                "Confirm the identifier exists in the target environment.",
                "Use the evidence spine to check for stale or cross-environment ids.",
            ],
            OperatorFailureTaxonomyKind.Conflict =>
            [
                "Refresh run or resource state before retrying.",
                "Use a new idempotency key if the payload changed.",
            ],
            OperatorFailureTaxonomyKind.IdempotencyConflict =>
            [
                "Reuse the same idempotency key only when the payload is unchanged.",
                "Wait for an in-progress reservation to complete, or use a new key.",
            ],
            OperatorFailureTaxonomyKind.DownstreamUnavailable =>
            [
                $"Check {downstream} health and connectivity from this environment.",
                "Retry after the dependency recovers; capture trace id in support notes.",
            ],
            OperatorFailureTaxonomyKind.ProviderFailedRetryable =>
            [
                $"Inspect Conexus model-call evidence and route decisions for {downstream}.",
                retryable == true
                    ? "Safe to retry with the same inputs when quota and policy allow."
                    : "Retry only after confirming the root cause is transient.",
            ],
            OperatorFailureTaxonomyKind.ProviderFailedTerminal =>
            [
                $"Inspect Conexus model-call evidence for {downstream}.",
                "Change model alias, inputs, or policy before retrying.",
            ],
            OperatorFailureTaxonomyKind.QuotaExceeded =>
            [
                "Review Conexus governance quota before additional model calls.",
                "Reduce parallelism or wait for the quota window to reset.",
            ],
            OperatorFailureTaxonomyKind.Timeout =>
            [
                "Retry when dependencies are healthy; consider smaller requests or shorter paths.",
                "Capture trace and correlation ids for latency investigation.",
            ],
            _ =>
            [
                "Capture trace and correlation ids from the failure banner.",
                "Open the evidence spine or run audit journey for cross-service context.",
            ],
        };
    }
}
