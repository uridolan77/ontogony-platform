using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

/// <summary>
/// Publishes envelopes to a transport or in-process bus without defining delivery semantics.
/// </summary>
public interface IEventPublisher
{
    /// <summary>Publishes an envelope to the configured transport.</summary>
    Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}

/// <summary>Handles envelopes of a specific payload type.</summary>
public interface IEventHandler<TPayload>
{
    /// <summary>Handles a single published envelope.</summary>
    Task HandleAsync(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}

/// <summary>Serializes envelopes for bridging or persistence.</summary>
public interface IEventSerializer
{
    /// <summary>Serializes an envelope to JSON text.</summary>
    string Serialize<TPayload>(OntogonyEnvelope<TPayload> envelope);

    /// <summary>Deserializes an envelope from JSON text.</summary>
    OntogonyEnvelope<TPayload> Deserialize<TPayload>(string json);
}
