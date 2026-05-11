using System.Text.Json;
using Ontogony.Contracts.Events;
using Ontogony.Contracts.References;
using Xunit;

namespace Ontogony.Messaging.Tests;

public sealed class OntogonyEnvelopeDeterminismTests
{
    [Fact]
    public void Envelope_With_Same_Content_Serializes_Identically()
    {
        var envelope1 = CreateTestEnvelope();
        var envelope2 = CreateTestEnvelope();

        var json1 = JsonSerializer.Serialize(envelope1, new JsonSerializerOptions { WriteIndented = false });
        var json2 = JsonSerializer.Serialize(envelope2, new JsonSerializerOptions { WriteIndented = false });

        Assert.Equal(json1, json2);
    }

    [Fact]
    public void Envelope_With_Different_Whitespace_Produces_Different_Pretty_Json()
    {
        var envelope = CreateTestEnvelope();

        var compact = JsonSerializer.Serialize(envelope, new JsonSerializerOptions { WriteIndented = false });
        var pretty = JsonSerializer.Serialize(envelope, new JsonSerializerOptions { WriteIndented = true });

        Assert.NotEqual(compact, pretty);
        Assert.Contains('\n', pretty);
        Assert.DoesNotContain('\n', compact);
    }

    [Fact]
    public void Envelope_Preserves_Trace_And_Context_On_Deserialize()
    {
        var original = new OntogonyEnvelope<TestPayload>
        {
            EventId = "evt_123",
            EventType = "test.event",
            Source = "test://source",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 30, 0, TimeSpan.Zero),
            TraceId = "trace-abc",
            SpanId = "span-xyz",
            TenantId = "tenant-001",
            WorkspaceId = "workspace-001",
            ProjectId = "project-001",
            ActorId = "actor-001",
            SessionId = "session-001",
            Protocol = "test",
            Payload = new TestPayload("Hello", 42)
        };

        var json = JsonSerializer.Serialize(original);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<TestPayload>>(json)!;

        Assert.Equal(original.EventId, restored.EventId);
        Assert.Equal(original.EventType, restored.EventType);
        Assert.Equal(original.TraceId, restored.TraceId);
        Assert.Equal(original.TenantId, restored.TenantId);
        Assert.Equal(original.SpanId, restored.SpanId);
        Assert.Equal(original.Payload.Text, restored.Payload.Text);
        Assert.Equal(original.Payload.Count, restored.Payload.Count);
    }

    [Fact]
    public void Envelope_With_Metadata_Round_Trips()
    {
        var metadata = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        var envelope = new OntogonyEnvelope<TestPayload>
        {
            EventId = "evt_123",
            EventType = "test.event",
            Source = "test://source",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-abc",
            Protocol = "test",
            Payload = new TestPayload("test", 1),
            Metadata = metadata
        };

        var json = JsonSerializer.Serialize(envelope);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<TestPayload>>(json)!;

        Assert.NotNull(restored.Metadata);
        Assert.Equal(metadata, restored.Metadata);
    }

    [Fact]
    public void Envelope_PayloadHash_Preserved()
    {
        var hash = "sha256_abc123def456";
        var envelope = new OntogonyEnvelope<TestPayload>
        {
            EventId = "evt_123",
            EventType = "test.event",
            Source = "test://source",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-abc",
            Protocol = "test",
            Payload = new TestPayload("test", 1),
            PayloadHash = hash
        };

        var json = JsonSerializer.Serialize(envelope);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<TestPayload>>(json)!;

        Assert.Equal(hash, restored.PayloadHash);
    }

    private static OntogonyEnvelope<TestPayload> CreateTestEnvelope()
    {
        return new OntogonyEnvelope<TestPayload>
        {
            EventId = "evt_123",
            EventType = "test.event",
            Source = "test://source",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            TraceId = "trace-abc",
            Protocol = "test",
            Payload = new TestPayload("Hello", 42)
        };
    }

    private sealed record TestPayload(string Text, int Count);
}

public sealed class EnvelopeRefTypesTests
{
    [Fact]
    public void ActorRef_Serializes_And_Deserializes()
    {
        var actor = new ActorRef("actor-123", "user", "Alice", "tenant-001");
        var envelope = new OntogonyEnvelope<ActorRef>
        {
            EventId = "evt_1",
            EventType = "actor.created",
            Source = "test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-1",
            Protocol = "test",
            Payload = actor
        };

        var json = JsonSerializer.Serialize(envelope);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<ActorRef>>(json)!;

        Assert.Equal("actor-123", restored.Payload.ActorId);
        Assert.Equal("user", restored.Payload.ActorType);
        Assert.Equal("Alice", restored.Payload.DisplayName);
    }

    [Fact]
    public void SubjectRef_Preserves_Metadata()
    {
        var subject = new SubjectRef("item", "item-456", "Important Item", "source-xyz");
        var envelope = new OntogonyEnvelope<SubjectRef>
        {
            EventId = "evt_2",
            EventType = "item.updated",
            Source = "test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-2",
            Protocol = "test",
            Payload = subject
        };

        var json = JsonSerializer.Serialize(envelope);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<SubjectRef>>(json)!;

        Assert.Equal("item-456", restored.Payload.Id);
        Assert.Equal("Important Item", restored.Payload.DisplayName);
    }

    [Fact]
    public void ArtifactRef_Captures_Content_Hash()
    {
        var artifact = new ArtifactRef(
            "artifact-789",
            "file",
            "model-output.json",
            "sha256_xyz",
            "application/json");

        var envelope = new OntogonyEnvelope<ArtifactRef>
        {
            EventId = "evt_3",
            EventType = "artifact.created",
            Source = "test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-3",
            Protocol = "test",
            Payload = artifact
        };

        var json = JsonSerializer.Serialize(envelope);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<ArtifactRef>>(json)!;

        Assert.Equal("sha256_xyz", restored.Payload.ContentHash);
        Assert.Equal("application/json", restored.Payload.MimeType);
    }

    [Fact]
    public void TraceRef_Captures_Span_Hierarchy()
    {
        var trace = new TraceRef("trace-abc", "span-1", "span-parent", "1");
        var envelope = new OntogonyEnvelope<TraceRef>
        {
            EventId = "evt_4",
            EventType = "trace.recorded",
            Source = "test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-abc",
            Protocol = "test",
            Payload = trace
        };

        var json = JsonSerializer.Serialize(envelope);
        var restored = JsonSerializer.Deserialize<OntogonyEnvelope<TraceRef>>(json)!;

        Assert.Equal("span-1", restored.Payload.SpanId);
        Assert.Equal("span-parent", restored.Payload.ParentSpanId);
    }
}

public sealed class EventMetadataTests
{
    [Fact]
    public void EventMetadata_ToFlatDictionary_Includes_All_Fields()
    {
        var metadata = new OntogonyEventMetadata(
            Subject: "test-subject",
            DetailLevel: "verbose",
            SchemaVersion: "2.0",
            CustomProperties: new Dictionary<string, string> { { "custom", "value" } });

        var flat = metadata.ToFlatDictionary();

        Assert.Equal("test-subject", flat["subject"]);
        Assert.Equal("verbose", flat["detailLevel"]);
        Assert.Equal("2.0", flat["schemaVersion"]);
        Assert.Equal("value", flat["custom"]);
    }

    [Fact]
    public void EventMetadata_FromFlatDictionary_Restores_Fields()
    {
        var flat = new Dictionary<string, string>
        {
            { "subject", "test-subject" },
            { "detailLevel", "verbose" },
            { "custom1", "customValue1" },
            { "custom2", "customValue2" }
        };

        var metadata = OntogonyEventMetadata.FromFlatDictionary(flat);

        Assert.Equal("test-subject", metadata.Subject);
        Assert.Equal("verbose", metadata.DetailLevel);
        Assert.NotNull(metadata.CustomProperties);
        Assert.Equal("customValue1", metadata.CustomProperties["custom1"]);
    }

    [Fact]
    public void EventMetadata_RoundTrip_Preserves_Content()
    {
        var original = new OntogonyEventMetadata(
            Subject: "my-subject",
            DetailLevel: "debug",
            SchemaVersion: "1.5",
            CustomProperties: new Dictionary<string, string>
            {
                { "key1", "val1" },
                { "key2", "val2" }
            });

        var flat = original.ToFlatDictionary();
        var restored = OntogonyEventMetadata.FromFlatDictionary(flat);

        Assert.Equal(original.Subject, restored.Subject);
        Assert.Equal(original.DetailLevel, restored.DetailLevel);
        Assert.Equal(original.SchemaVersion, restored.SchemaVersion);
        Assert.NotNull(restored.CustomProperties);
        Assert.Equal(original.CustomProperties!["key1"], restored.CustomProperties["key1"]);
    }
}

public sealed class CloudEventsConversionTests
{
    [Fact]
    public void Envelope_ToCloudEvent_PreservesCore_Fields()
    {
        var envelope = new OntogonyEnvelope<TestData>
        {
            EventId = "evt_abc123",
            EventType = "mcp.tool.invoked",
            Source = "mcp://runtime",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 30, 0, TimeSpan.Zero),
            TraceId = "trace-xyz",
            Protocol = "mcp",
            Payload = new TestData("input")
        };

        var cloudEvent = envelope.ToCloudEvent();

        Assert.Equal("evt_abc123", cloudEvent.Id);
        Assert.Equal("mcp.tool.invoked", cloudEvent.Type);
        Assert.Equal("mcp://runtime", cloudEvent.Source);
        Assert.Equal("2026-05-11T10:30:00.0000000+00:00", cloudEvent.Time);
    }

    [Fact]
    public void Envelope_ToCloudEvent_Includes_Extensions()
    {
        var envelope = new OntogonyEnvelope<TestData>
        {
            EventId = "evt_1",
            EventType = "test.event",
            Source = "test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-abc",
            SpanId = "span-1",
            TenantId = "tenant-1",
            WorkspaceId = "workspace-1",
            ProjectId = "project-1",
            ActorId = "actor-1",
            SessionId = "session-1",
            Protocol = "test",
            PayloadHash = "sha256_xyz",
            Payload = new TestData("test")
        };

        var cloudEvent = envelope.ToCloudEvent();

        Assert.NotNull(cloudEvent.Extensions);
        Assert.Equal("trace-abc", cloudEvent.Extensions!["traceId"]);
        Assert.Equal("span-1", cloudEvent.Extensions!["spanId"]);
        Assert.Equal("tenant-1", cloudEvent.Extensions!["tenantId"]);
        Assert.Equal("workspace-1", cloudEvent.Extensions!["workspaceId"]);
        Assert.Equal("project-1", cloudEvent.Extensions!["projectId"]);
        Assert.Equal("actor-1", cloudEvent.Extensions!["actorId"]);
        Assert.Equal("session-1", cloudEvent.Extensions!["sessionId"]);
        Assert.Equal("test", cloudEvent.Extensions!["protocol"]);
        Assert.Equal("sha256_xyz", cloudEvent.Extensions!["payloadHash"]);
    }

    [Fact]
    public void CloudEvent_ToOntogonyEnvelope_Restores_Metadata()
    {
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "evt_1",
            Type = "test.event",
            Source = "test://source",
            Time = "2026-05-11T10:00:00Z",
            Data = new TestData("test"),
            Extensions = new Dictionary<string, object>
            {
                { "traceId", "trace-abc" },
                { "tenantId", "tenant-1" },
                { "projectId", "project-1" }
            }
        };

        var envelope = cloudEvent.ToOntogonyEnvelope<TestData>();

        Assert.Equal("evt_1", envelope.EventId);
        Assert.Equal("test.event", envelope.EventType);
        Assert.Equal("trace-abc", envelope.TraceId);
        Assert.Equal("tenant-1", envelope.TenantId);
        Assert.Equal("project-1", envelope.ProjectId);
    }

    [Fact]
    public void Envelope_ToCloudEvent_RoundTrip_Preserves_Payload()
    {
        var original = new OntogonyEnvelope<TestData>
        {
            EventId = "evt_xyz",
            EventType = "test.event",
            Source = "test",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            TraceId = "trace-1",
            Protocol = "test",
            Payload = new TestData("original content")
        };

        var cloudEvent = original.ToCloudEvent();
        var restored = cloudEvent.ToOntogonyEnvelope<TestData>();

        Assert.Equal(original.EventId, restored.EventId);
        Assert.Equal(original.EventType, restored.EventType);
        Assert.Equal(original.TraceId, restored.TraceId);
        Assert.Equal(original.Payload.Content, restored.Payload.Content);
    }

    [Fact]
    public void CloudEventJson_Serializes_Cleanly()
    {
        var envelope = new OntogonyEnvelope<TestData>
        {
            EventId = "evt_1",
            EventType = "test.event",
            Source = "test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-1",
            Protocol = "test",
            Payload = new TestData("test")
        };

        var json = envelope.ToCloudEventJson();

        Assert.Contains("\"specversion\"", json);
        Assert.Contains("\"id\"", json);
        Assert.Contains("\"type\"", json);
        Assert.Contains("\"source\"", json);
    }

    private sealed record TestData(string Content);
}
