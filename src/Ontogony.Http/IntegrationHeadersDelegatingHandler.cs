using System.Diagnostics;
using System.Net.Http.Headers;
using Ontogony.Contracts.Events;
using Ontogony.Observability;

namespace Ontogony.Http;

/// <summary>
/// Propagates Ontogony correlation, actor, and idempotency headers on outbound <see cref="HttpClient"/> calls.
/// </summary>
public class IntegrationHeadersDelegatingHandler : DelegatingHandler
{
    private readonly IReadOnlyList<IOutboundActorPropagator> _propagators;

    /// <summary>Creates the handler with optional actor propagators from DI.</summary>
    public IntegrationHeadersDelegatingHandler(IEnumerable<IOutboundActorPropagator>? propagators = null)
    {
        _propagators = propagators?.ToArray() ?? Array.Empty<IOutboundActorPropagator>();
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlation = OntogonyCorrelationContext.Current;
        IntegrationHeaderPropagation.Apply(
            request.Headers,
            correlation,
            OntogonyIntegrationContext.Current,
            _propagators);

        if (correlation is null)
        {
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceParent, Activity.Current?.Id);
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceState, Activity.Current?.TraceStateString);
        }

        return base.SendAsync(request, cancellationToken);
    }

    private static void AddIfMissing(HttpRequestHeaders headers, string name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || headers.Contains(name))
        {
            return;
        }

        headers.TryAddWithoutValidation(name, value);
    }
}

/// <summary>
/// Back-compat name for <see cref="IntegrationHeadersDelegatingHandler"/>.
/// </summary>
public sealed class CorrelationHeadersDelegatingHandler : IntegrationHeadersDelegatingHandler
{
    /// <inheritdoc />
    public CorrelationHeadersDelegatingHandler(IEnumerable<IOutboundActorPropagator>? propagators = null)
        : base(propagators)
    {
    }
}
