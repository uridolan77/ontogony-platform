using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

/// <summary>
/// Convenience <see cref="IEventPublisher"/> that owns an <see cref="InMemoryEventPublisher"/> for <b>tests and single-process</b> scenarios.
/// </summary>
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

    public Task<EventPublishResult> PublishWithResultAsync<TPayload>(
        OntogonyEnvelope<TPayload> envelope,
        CancellationToken cancellationToken = default) =>
        _publisher.PublishWithResultAsync(envelope, cancellationToken);

    public void Register<TPayload>(IEventHandler<TPayload> handler)
    {
        _publisher.Register(handler);
    }

    public async Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(envelope, cancellationToken).ConfigureAwait(false);
    }
}
