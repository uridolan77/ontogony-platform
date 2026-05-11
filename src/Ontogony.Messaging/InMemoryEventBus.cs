using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

public sealed class InMemoryEventBus : IEventPublisher
{
    private readonly InMemoryEventPublisher _publisher;

    public InMemoryEventBus()
    {
        _publisher = new InMemoryEventPublisher();
    }

    public InMemoryEventBus(InMemoryEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public InMemoryEventSink Sink => _publisher.Sink;

    public void Register<TPayload>(IEventHandler<TPayload> handler)
    {
        _publisher.Register(handler);
    }

    public async Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(envelope, cancellationToken).ConfigureAwait(false);
    }
}
