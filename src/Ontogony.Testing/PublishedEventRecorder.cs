using Ontogony.Contracts.Events;
using Ontogony.Messaging;

namespace Ontogony.Testing;

public sealed class PublishedEventRecorder : IEventPublisher
{
    private readonly InMemoryEventSink _sink = new();

    public InMemoryEventSink Sink => _sink;

    public IReadOnlyList<object> PublishedEvents => _sink.ReadAll();

    public Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        _sink.Append(envelope);
        return Task.CompletedTask;
    }
}