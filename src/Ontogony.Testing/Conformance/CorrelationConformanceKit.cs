namespace Ontogony.Testing.Conformance;

/// <summary>
/// Facade for correlation and header propagation conformance (PLATFORM-9-003).
/// </summary>
public static class CorrelationConformanceKit
{
    /// <summary>Verifies frozen header constants still match the platform contract.</summary>
    public static void AssertFrozenContractAlignment()
    {
        HeaderPropagationConformanceAssertions.AssertFrozenHeaderConstantsAlign();
    }

    /// <summary>Verifies request tracing middleware generates and echoes trace ids.</summary>
    public static Task AssertTracingMiddlewareBaselineAsync()
    {
        return TracingConformanceAssertions.AssertTraceIdGeneratedWhenAbsentAsync();
    }

    /// <summary>Runs standard correlation checks suitable for consumer adoption tests.</summary>
    public static async Task RunStandardConsumerChecksAsync()
    {
        AssertFrozenContractAlignment();
        await AssertTracingMiddlewareBaselineAsync().ConfigureAwait(false);
    }
}
