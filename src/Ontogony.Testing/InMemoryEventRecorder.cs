using Ontogony.Contracts.Events;
using Ontogony.Messaging;

namespace Ontogony.Testing;

[Obsolete("Use PublishedEventRecorder for new tests.")]
public sealed class InMemoryEventRecorder : IEventPublisher
{
    private readonly PublishedEventRecorder _inner = new();

    public IReadOnlyList<object> Events => _inner.PublishedEvents;

    public Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        return _inner.PublishAsync(envelope, cancellationToken);
    }
}
