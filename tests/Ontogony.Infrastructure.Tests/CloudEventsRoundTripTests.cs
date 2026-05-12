using Ontogony.Contracts.Events;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class CloudEventsRoundTripTests
{
    private readonly FakeClock _clock = new();

    [Fact]
    public void CloudEvent_RoundTrip_WithMinimalEnvelope_PreservesRequiredFields()
    {
        // Arrange
        var originalEnvelope = new OntogonyEnvelope<object>
        {
            EventId = "evt:roundtrip:001",
            EventType = "system.test.minimal",
            Source = "https://test.ontogony.dev",
            OccurredAt = _clock.UtcNow,
            TraceId = "trace:minimal:001",
            Protocol = "mcp",
            SchemaVersion = "1.0",
            Payload = new { message = "roundtrip test" }
        };

        // Act
        var cloudEvent = originalEnvelope.ToCloudEvent();
        var restoredEnvelope = cloudEvent.ToOntogonyEnvelope<object>();

        // Assert - Required fields preserved
        Assert.Equal(originalEnvelope.EventId, restoredEnvelope.EventId);
        Assert.Equal(originalEnvelope.EventType, restoredEnvelope.EventType);
        Assert.Equal(originalEnvelope.Source, restoredEnvelope.Source);
        Assert.Equal(originalEnvelope.OccurredAt, restoredEnvelope.OccurredAt);
        Assert.Equal(originalEnvelope.TraceId, restoredEnvelope.TraceId);
        Assert.Equal(originalEnvelope.Protocol, restoredEnvelope.Protocol);
        Assert.Equal(originalEnvelope.SchemaVersion, restoredEnvelope.SchemaVersion);
    }

    [Fact]
    public void CloudEvent_RoundTrip_WithFullEnvelope_PreservesAllOptionalFields()
    {
        // Arrange
        var metadata = new Dictionary<string, string> { { "key1", "value1" } };
        var originalEnvelope = new OntogonyEnvelope<object>
        {
            EventId = "evt:roundtrip:002",
            EventType = "system.test.full",
            Source = "https://test.ontogony.dev",
            OccurredAt = _clock.UtcNow,
            TraceId = "trace:full:001",
            SpanId = "span:001",
            ParentSpanId = "span:parent:001",
            TenantId = "tenant:001",
            WorkspaceId = "workspace:001",
            ProjectId = "project:001",
            ActorId = "actor:001",
            SessionId = "session:001",
            Protocol = "a2a",
            SchemaVersion = "1.0",
            PayloadHash = "abc123def456",
            Metadata = metadata,
            Payload = new { data = "full envelope" }
        };

        // Act
        var cloudEvent = originalEnvelope.ToCloudEvent();
        var restoredEnvelope = cloudEvent.ToOntogonyEnvelope<object>();

        // Assert - All fields preserved
        Assert.Equal(originalEnvelope.EventId, restoredEnvelope.EventId);
        Assert.Equal(originalEnvelope.EventType, restoredEnvelope.EventType);
        Assert.Equal(originalEnvelope.Source, restoredEnvelope.Source);
        Assert.Equal(originalEnvelope.OccurredAt, restoredEnvelope.OccurredAt);
        Assert.Equal(originalEnvelope.TraceId, restoredEnvelope.TraceId);
        Assert.Equal(originalEnvelope.SpanId, restoredEnvelope.SpanId);
        Assert.Equal(originalEnvelope.ParentSpanId, restoredEnvelope.ParentSpanId);
        Assert.Equal(originalEnvelope.TenantId, restoredEnvelope.TenantId);
        Assert.Equal(originalEnvelope.WorkspaceId, restoredEnvelope.WorkspaceId);
        Assert.Equal(originalEnvelope.ProjectId, restoredEnvelope.ProjectId);
        Assert.Equal(originalEnvelope.ActorId, restoredEnvelope.ActorId);
        Assert.Equal(originalEnvelope.SessionId, restoredEnvelope.SessionId);
        Assert.Equal(originalEnvelope.Protocol, restoredEnvelope.Protocol);
        Assert.Equal(originalEnvelope.SchemaVersion, restoredEnvelope.SchemaVersion);
        Assert.Equal(originalEnvelope.PayloadHash, restoredEnvelope.PayloadHash);
    }

    [Fact]
    public void CloudEvent_ToCloudEvent_ContainsCorrectSpecVersion()
    {
        // Arrange
        var envelope = new OntogonyEnvelope<object>
        {
            EventId = "evt:ce:001",
            EventType = "system.test.ce",
            Source = "https://test.ontogony.dev",
            OccurredAt = _clock.UtcNow,
            TraceId = "trace:ce:001",
            Protocol = "cloudevents",
            SchemaVersion = "1.0",
            Payload = new { }
        };

        // Act
        var cloudEvent = envelope.ToCloudEvent();

        // Assert
        Assert.Equal("1.0", cloudEvent.SpecVersion);
    }

    [Fact]
    public void CloudEvent_Extensions_StoreAllOptionalEnvelopeFields()
    {
        // Arrange
        var envelope = new OntogonyEnvelope<object>
        {
            EventId = "evt:ext:001",
            EventType = "system.test.ext",
            Source = "https://test.ontogony.dev",
            OccurredAt = _clock.UtcNow,
            TraceId = "trace:ext:001",
            SpanId = "span:ext:001",
            Protocol = "mcp",
            SchemaVersion = "2.0",
            PayloadHash = "hash123",
            Payload = new { }
        };

        // Act
        var cloudEvent = envelope.ToCloudEvent();

        // Assert - Extensions contain optional fields
        Assert.NotNull(cloudEvent.Extensions);
        Assert.True(cloudEvent.Extensions.ContainsKey("traceId"));
        Assert.True(cloudEvent.Extensions.ContainsKey("spanId"));
        Assert.True(cloudEvent.Extensions.ContainsKey("protocol"));
        Assert.True(cloudEvent.Extensions.ContainsKey("schemaVersion"));
        Assert.True(cloudEvent.Extensions.ContainsKey("payloadHash"));
        
        Assert.Equal("trace:ext:001", cloudEvent.Extensions["traceId"]);
        Assert.Equal("span:ext:001", cloudEvent.Extensions["spanId"]);
        Assert.Equal("mcp", cloudEvent.Extensions["protocol"]);
        Assert.Equal("2.0", cloudEvent.Extensions["schemaVersion"]);
        Assert.Equal("hash123", cloudEvent.Extensions["payloadHash"]);
    }
}
