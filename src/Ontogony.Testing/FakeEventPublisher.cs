using Ontogony.Contracts.Events;
using Ontogony.Messaging;

namespace Ontogony.Testing;

public sealed class FakeEventPublisher : IEventPublisher
{
    private readonly List<object> _published = [];

    public IReadOnlyList<object> Published => _published;

    public Exception? PublishException { get; set; }

    public Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        if (PublishException is not null)
        {
            throw PublishException;
        }

        _published.Add(envelope);
        return Task.CompletedTask;
    }
}