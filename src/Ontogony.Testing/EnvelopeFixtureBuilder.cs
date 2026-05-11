using Ontogony.Contracts.Events;

namespace Ontogony.Testing;

public sealed class EnvelopeFixtureBuilder<TPayload>
{
    private string _eventId = "evt_fixture_0001";
    private string _eventType = "agentor.run.started";
    private string _source = "test://fixture";
    private DateTimeOffset _occurredAt = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string _traceId = "trace_fixture";
    private string? _tenantId;
    private string? _workspaceId;
    private string? _projectId;
    private string? _actorId;
    private string? _sessionId;
    private string _protocol = ProtocolNames.Agentor;
    private string _schemaVersion = "1.0";
    private string? _payloadHash;
    private readonly Dictionary<string, string> _metadata = new(StringComparer.Ordinal);
    private TPayload? _payload;

    public EnvelopeFixtureBuilder<TPayload> WithEventId(string eventId) { _eventId = eventId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithEventType(string eventType) { _eventType = eventType; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithSource(string source) { _source = source; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithOccurredAt(DateTimeOffset occurredAt) { _occurredAt = occurredAt; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithTraceId(string traceId) { _traceId = traceId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithTenantId(string tenantId) { _tenantId = tenantId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithWorkspaceId(string workspaceId) { _workspaceId = workspaceId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithProjectId(string projectId) { _projectId = projectId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithActorId(string actorId) { _actorId = actorId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithSessionId(string sessionId) { _sessionId = sessionId; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithProtocol(string protocol) { _protocol = protocol; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithSchemaVersion(string schemaVersion) { _schemaVersion = schemaVersion; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithPayloadHash(string payloadHash) { _payloadHash = payloadHash; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithMetadata(string key, string value) { _metadata[key] = value; return this; }
    public EnvelopeFixtureBuilder<TPayload> WithPayload(TPayload payload) { _payload = payload; return this; }

    public OntogonyEnvelope<TPayload> Build()
    {
        if (_payload is null)
        {
            throw new InvalidOperationException("Payload must be provided before building envelope fixture.");
        }

        return new OntogonyEnvelope<TPayload>
        {
            EventId = _eventId,
            EventType = _eventType,
            Source = _source,
            OccurredAt = _occurredAt,
            TraceId = _traceId,
            TenantId = _tenantId,
            WorkspaceId = _workspaceId,
            ProjectId = _projectId,
            ActorId = _actorId,
            SessionId = _sessionId,
            Protocol = _protocol,
            SchemaVersion = _schemaVersion,
            Payload = _payload,
            PayloadHash = _payloadHash,
            Metadata = new Dictionary<string, string>(_metadata, StringComparer.Ordinal)
        };
    }
}