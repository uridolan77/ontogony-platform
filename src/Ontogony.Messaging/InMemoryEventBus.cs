using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

/// <summary>
/// Convenience <see cref="IEventPublisher"/> that owns an <see cref="InMemoryEventPublisher"/> for <b>tests and single-process</b> scenarios.
/// </summary>
public sealed class InMemoryEventBus : IEventPublisher
{
    private readonly InMemoryEventPublisher _publisher;

    /// <summary>Creates a bus with a new in-memory publisher and sink.</summary>
    public InMemoryEventBus()
    {
        _publisher = new InMemoryEventPublisher();
    }

    /// <summary>Creates a bus that wraps an existing in-memory publisher.</summary>
    public InMemoryEventBus(InMemoryEventPublisher publisher)
    {
        _publisher = publisher;
    }

    /// <summary>Capture sink shared with the underlying publisher.</summary>
    public InMemoryEventSink Sink => _publisher.Sink;

    /// <summary>Publishes an envelope and returns structured dispatch diagnostics.</summary>
    public Task<EventPublishResult> PublishWithResultAsync<TPayload>(
        OntogonyEnvelope<TPayload> envelope,
        CancellationToken cancellationToken = default) =>
        _publisher.PublishWithResultAsync(envelope, cancellationToken);

    /// <summary>Registers an in-process handler for a payload type.</summary>
    public void Register<TPayload>(IEventHandler<TPayload> handler)
    {
        _publisher.Register(handler);
    }

    /// <inheritdoc />
    public async Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(envelope, cancellationToken).ConfigureAwait(false);
    }
}
