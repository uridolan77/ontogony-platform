using System.Text.Json;
using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

public sealed class JsonEventSerializer : IEventSerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public JsonEventSerializer(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _jsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public string Serialize<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        return JsonSerializer.Serialize(envelope, _jsonSerializerOptions);
    }

    public OntogonyEnvelope<TPayload> Deserialize<TPayload>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("json cannot be null or empty.", nameof(json));
        }

        return JsonSerializer.Deserialize<OntogonyEnvelope<TPayload>>(json, _jsonSerializerOptions)
            ?? throw new InvalidOperationException("Unable to deserialize event envelope.");
    }
}