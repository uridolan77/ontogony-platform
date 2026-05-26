using Ontogony.Errors;

namespace Ontogony.Testing.Conformance;

/// <summary>
/// Facade for HTTP error middleware and cross-service error envelope conformance.
/// </summary>
public static class ErrorEnvelopeConformanceKit
{
    /// <summary>Runs middleware error-shape checks (500 + mapped exception).</summary>
    public static async Task RunMiddlewareBaselineAsync()
    {
        await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async().ConfigureAwait(false);
        await ErrorShapeConformanceAssertions.AssertNoUnmappedExceptionMessageLeakAsync().ConfigureAwait(false);
        await ErrorShapeConformanceAssertions.AssertMappedExceptionProducesExpectedShapeAsync(
            new InvalidOperationException("conformance mapped failure"),
            System.Net.HttpStatusCode.UnprocessableEntity,
            "CONFORMANCE_DOMAIN_FAILURE").ConfigureAwait(false);
    }

    /// <summary>Validates a product-native <see cref="CrossServiceErrorEnvelope"/> instance.</summary>
    public static void AssertCrossServiceEnvelope(CrossServiceErrorEnvelope envelope, string expectedSystem)
    {
        CrossServiceErrorEnvelopeConformanceAssertions.AssertValidShape(envelope, expectedSystem);
    }

    /// <summary>Runs standard error envelope checks for consumer adoption tests.</summary>
    public static async Task RunStandardConsumerChecksAsync(CrossServiceErrorEnvelope? nativeEnvelope = null, string? expectedSystem = null)
    {
        await RunMiddlewareBaselineAsync().ConfigureAwait(false);

        if (nativeEnvelope is not null && expectedSystem is not null)
        {
            AssertCrossServiceEnvelope(nativeEnvelope, expectedSystem);
        }
    }
}
