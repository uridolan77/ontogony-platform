using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}

public interface IEventHandler<TPayload>
{
    Task HandleAsync(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}
