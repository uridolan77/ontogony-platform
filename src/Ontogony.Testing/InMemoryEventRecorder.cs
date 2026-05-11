using Ontogony.Contracts.Events;
using Ontogony.Messaging;

namespace Ontogony.Testing;

public sealed class InMemoryEventRecorder : IEventPublisher
{
    private readonly List<object> _events = new();

    public IReadOnlyList<object> Events => _events;

    public Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        _events.Add(envelope);
        return Task.CompletedTask;
    }
}
