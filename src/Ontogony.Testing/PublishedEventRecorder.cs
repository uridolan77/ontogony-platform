using Ontogony.Contracts.Events;
using Ontogony.Messaging;

namespace Ontogony.Testing;

/// <summary>
/// Minimal <see cref="IEventPublisher"/> that only appends to an <see cref="InMemoryEventSink"/> (capture-only).
/// Use for tests; it does not dispatch handlers or record observability metrics.
/// </summary>
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