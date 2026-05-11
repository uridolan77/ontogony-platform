using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

public sealed class InMemoryEventBus : IEventPublisher
{
    private readonly List<object> _handlers = new();
    private readonly object _sync = new();

    public void Register<TPayload>(IEventHandler<TPayload> handler)
    {
        lock (_sync)
        {
            _handlers.Add(handler);
        }
    }

    public async Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        List<IEventHandler<TPayload>> snapshot;
        lock (_sync)
        {
            snapshot = _handlers.OfType<IEventHandler<TPayload>>().ToList();
        }

        foreach (var handler in snapshot)
        {
            await handler.HandleAsync(envelope, cancellationToken);
        }
    }
}
