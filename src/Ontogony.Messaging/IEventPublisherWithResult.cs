using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

/// <summary>
/// Publishes envelopes and returns structured dispatch diagnostics when the transport supports it.
/// Real brokers typically implement only <see cref="IEventPublisher"/>; use this interface for in-process or diagnostic publishers.
/// </summary>
public interface IEventPublisherWithResult
{
    Task<EventPublishResult> PublishWithResultAsync<TPayload>(
        OntogonyEnvelope<TPayload> envelope,
        CancellationToken cancellationToken = default);
}
