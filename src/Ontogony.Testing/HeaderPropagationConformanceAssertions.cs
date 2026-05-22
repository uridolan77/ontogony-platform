using System.Net.Http.Headers;
using Ontogony.Contracts.Events;
using Ontogony.Http;
using Ontogony.Observability;

namespace Ontogony.Testing;

/// <summary>
/// Reusable assertions that prove a service wires Ontogony outbound header propagation correctly.
/// </summary>
public static class HeaderPropagationConformanceAssertions
{
    /// <summary>
    /// Verifies platform header constants still match the frozen PLATFORM-9-003 contract.
    /// </summary>
    public static void AssertFrozenHeaderConstantsAlign()
    {
        if (!OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyEventHeaders.TraceParent, StringComparer.Ordinal)
            || !OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyIntegrationHeaders.LegacyCorrelationId, StringComparer.Ordinal)
            || !OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyIntegrationHeaders.ActorId, StringComparer.Ordinal)
            || !OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyIntegrationHeaders.ActorType, StringComparer.Ordinal)
            || !OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyIntegrationHeaders.ActorRoles, StringComparer.Ordinal)
            || !OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyIntegrationHeaders.IdempotencyKey, StringComparer.Ordinal)
            || !OntogonyPropagationHeaderContract.FrozenRequired.Contains(OntogonyPropagationHeaderContract.AllagmaRunId, StringComparer.Ordinal))
        {
            throw new InvalidOperationException("Frozen propagation contract drifted from platform header constants.");
        }
    }

    /// <summary>
    /// Sends one outbound request through <see cref="IntegrationHeadersDelegatingHandler"/> and
    /// asserts frozen headers were propagated from correlation/integration context.
    /// </summary>
    public static async Task AssertIntegrationHandlerPropagatesScenarioAsync(
        PropagationHeaderScenario scenario,
        IEnumerable<IOutboundActorPropagator>? propagators = null)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        var recorder = new RecordingHttpMessageHandler();
        var handler = new IntegrationHeadersDelegatingHandler(propagators);
        handler.InnerHandler = recorder;

        using var correlationScope = OntogonyCorrelationContext.Push(new CorrelationState(
            scenario.TraceId ?? "trace-prop-001",
            scenario.CorrelationId ?? "corr-prop-001",
            ActorId: scenario.ActorId,
            TraceParent: scenario.TraceParent));

        var additional = scenario.AllagmaRunId is null
            ? null
            : new Dictionary<string, string> { [OntogonyPropagationHeaderContract.AllagmaRunId] = scenario.AllagmaRunId };

        using var integrationScope = OntogonyIntegrationContext.Push(new IntegrationOutboundState(
            IdempotencyKey: scenario.IdempotencyKey,
            ActorId: scenario.ActorId,
            ActorType: scenario.ActorType,
            ActorRoles: scenario.ActorRoles is null
                ? null
                : scenario.ActorRoles.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries),
            AdditionalHeaders: additional));

        using var client = new HttpClient(handler);
        await client.GetAsync("https://conformance.test/outbound");

        var request = recorder.Requests.SingleOrDefault()
            ?? throw new InvalidOperationException("Expected exactly one recorded outbound request.");

        AssertHeaderValue(request.Headers, OntogonyEventHeaders.TraceParent, scenario.TraceParent);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.LegacyCorrelationId, scenario.CorrelationId);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.ActorId, scenario.ActorId);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.ActorType, scenario.ActorType);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.ActorRoles, scenario.ActorRoles);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.IdempotencyKey, scenario.IdempotencyKey);
        AssertHeaderValue(request.Headers, OntogonyPropagationHeaderContract.AllagmaRunId, scenario.AllagmaRunId);

        if (!string.IsNullOrWhiteSpace(scenario.TraceId))
        {
            AssertHeaderValue(request.Headers, OntogonyEventHeaders.TraceId, scenario.TraceId);
        }
    }

    /// <summary>
    /// Asserts a captured outbound request includes all frozen headers that have expected values in <paramref name="scenario"/>.
    /// </summary>
    public static void AssertFrozenHeadersOnRequest(HttpRequestMessage request, PropagationHeaderScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(scenario);

        AssertHeaderValue(request.Headers, OntogonyEventHeaders.TraceParent, scenario.TraceParent);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.LegacyCorrelationId, scenario.CorrelationId);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.ActorId, scenario.ActorId);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.ActorType, scenario.ActorType);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.ActorRoles, scenario.ActorRoles);
        AssertHeaderValue(request.Headers, OntogonyIntegrationHeaders.IdempotencyKey, scenario.IdempotencyKey);
        AssertHeaderValue(request.Headers, OntogonyPropagationHeaderContract.AllagmaRunId, scenario.AllagmaRunId);
    }

    /// <summary>
    /// Returns the first header value for <paramref name="headerName"/> or null when absent.
    /// </summary>
    public static string? ReadHeader(HttpRequestHeaders headers, string headerName)
    {
        return headers.TryGetValues(headerName, out var values) ? values.SingleOrDefault() : null;
    }

    private static void AssertHeaderValue(HttpRequestHeaders headers, string headerName, string? expected)
    {
        if (string.IsNullOrWhiteSpace(expected))
        {
            return;
        }

        var actual = ReadHeader(headers, headerName);
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected outbound header '{headerName}' to be '{expected}', but was '{actual ?? "(missing)"}'.");
        }
    }

}
