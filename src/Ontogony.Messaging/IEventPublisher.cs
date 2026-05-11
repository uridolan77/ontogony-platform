using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

/// <summary>
/// Publishes envelopes to a transport or in-process bus without defining delivery semantics.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}

/// <summary>Handles envelopes of a specific payload type.</summary>
public interface IEventHandler<TPayload>
{
    Task HandleAsync(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}

/// <summary>Serializes envelopes for bridging or persistence.</summary>
public interface IEventSerializer
{
    string Serialize<TPayload>(OntogonyEnvelope<TPayload> envelope);

    OntogonyEnvelope<TPayload> Deserialize<TPayload>(string json);
}
