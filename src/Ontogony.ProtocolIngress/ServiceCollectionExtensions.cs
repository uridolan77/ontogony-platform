using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.ProtocolIngress.Adapters;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers mechanical protocol ingress adapters and their required shared dependencies.
    /// </summary>
    public static IServiceCollection AddOntogonyProtocolIngress(this IServiceCollection services)
    {
        services.TryAddSingleton<IContentHashService, Sha256ContentHashService>();
        services.TryAddSingleton<PayloadHasher>();
        services.TryAddSingleton<IIdGenerator, GuidIdGenerator>();
        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton<IEnvelopeValidator, DefaultEnvelopeValidator>();

        services.TryAddSingleton<GenericJsonProtocolAdapter>();
        services.TryAddSingleton<CloudEventsProtocolAdapter>();
        services.TryAddSingleton<McpProtocolAdapter>();
        services.TryAddSingleton<A2aProtocolAdapter>();
        services.TryAddSingleton<AgUiProtocolAdapter>();

        services.TryAddSingleton<IProtocolIngressAdapter<string>>(sp => sp.GetRequiredService<GenericJsonProtocolAdapter>());
        services.TryAddSingleton<IProtocolIngressAdapter<CloudEventEnvelope>>(sp => sp.GetRequiredService<CloudEventsProtocolAdapter>());
        services.TryAddSingleton<IProtocolIngressAdapter<McpEvent>>(sp => sp.GetRequiredService<McpProtocolAdapter>());
        services.TryAddSingleton<IProtocolIngressAdapter<A2aEvent>>(sp => sp.GetRequiredService<A2aProtocolAdapter>());
        services.TryAddSingleton<IProtocolIngressAdapter<AgUiEvent>>(sp => sp.GetRequiredService<AgUiProtocolAdapter>());

        return services;
    }
}
