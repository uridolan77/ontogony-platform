using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.ProtocolIngress;
using Ontogony.ProtocolIngress.Adapters;
using Ontogony.Primitives;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.ProtocolIngress.Tests;

public sealed class GenericJsonProtocolAdapterTests
{
    private readonly GenericJsonProtocolAdapter _adapter;
    private readonly FakeClock _clock;

    public GenericJsonProtocolAdapterTests()
    {
        _clock = new FakeClock();
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        var validator = new DefaultEnvelopeValidator();
        _adapter = new GenericJsonProtocolAdapter(payloadHasher, idGenerator, _clock, validator);
    }

    [Fact]
    public void Normalize_WithValidJsonPayload_ReturnsSuccess()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123",
            "data": { "userId": "user-456" }
        }
        """;
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope);
        Assert.Equal("generic-json.ingress.normalized", result.Envelope.EventType);
        Assert.Equal("user.created", result.Envelope.Payload.RawEventType);
        Assert.Equal("generic-json://user-service", result.Envelope.Source);
        Assert.Equal("trace-123", result.Envelope.TraceId);
        Assert.Equal("generic-json", result.Envelope.Protocol);
    }

    [Fact]
    public void Normalize_WithMissingEventType_ReturnsFailure()
    {
        // Arrange
        var rawJson = """
        {
            "source": "user-service",
            "traceId": "trace-123"
        }
        """;
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "eventType");
    }

    [Fact]
    public void Normalize_WithMissingTraceIdAndRequirePolicy_ReturnsFailure()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "data": { }
        }
        """;
        var context = new ProtocolIngressContext 
        { 
            IdGenerationPolicy = TraceIdGenerationPolicy.RequireProvided 
        };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "TraceId");
    }

    [Fact]
    public void Normalize_WithMissingTraceIdAndGeneratePolicy_GeneratesTraceId()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service"
        }
        """;
        var context = new ProtocolIngressContext 
        { 
            IdGenerationPolicy = TraceIdGenerationPolicy.GenerateIfMissing 
        };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope?.TraceId);
        Assert.NotEmpty(result.Envelope.TraceId);
    }

    [Fact]
    public void Normalize_WithEmptyRawTraceId_CascadesToContextTraceId()
    {
        // Arrange: Empty raw trace ID should not be treated as "found"
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": ""
        }
        """;
        var contextTraceId = "trace-from-context";
        var context = new ProtocolIngressContext 
        { 
            TraceId = contextTraceId,
            IdGenerationPolicy = TraceIdGenerationPolicy.RequireProvided
        };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(contextTraceId, result.Envelope?.TraceId);
    }

    [Fact]
    public void Normalize_PreservesPayloadHash()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123",
            "data": { "userId": "user-456" }
        }
        """;
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope?.PayloadHash);
        Assert.NotEmpty(result.Envelope.PayloadHash);
        Assert.Equal(result.Envelope.Payload.CanonicalPayloadHash, result.Envelope.PayloadHash);
    }

    [Fact]
    public void Normalize_WithInvalidJson_ReturnsFailure()
    {
        // Arrange
        var rawJson = "{ invalid json }";
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "rawJson");
    }

    [Fact]
    public void Normalize_AppliesContextMetadata()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123"
        }
        """;
        var context = new ProtocolIngressContext 
        { 
            TraceId = "trace-123",
            Metadata = new ProtocolIngressContextMetadata
            {
                TenantId = "tenant-123",
                ActorId = "actor-456"
            }
        };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("tenant-123", result.Envelope?.TenantId);
        Assert.Equal("actor-456", result.Envelope?.ActorId);
    }

    [Fact]
    public void Normalize_UsesFakeClock_ForDeterministicTimestamp()
    {
        // Arrange
        var testTime = DateTimeOffset.UtcNow.AddDays(-1);
        _clock.Advance(testTime - _clock.UtcNow);

        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123"
        }
        """;
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(testTime, result.Envelope?.OccurredAt);
    }

    [Fact]
    public void Normalize_NormalizesSourceToAbsoluteUri()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123"
        }
        """;
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("generic-json://user-service", result.Envelope?.Source);
    }

    [Fact]
    public void Normalize_WithAlreadyNormalizedSource_DoesNotDoublePrefix()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "generic-json://user-service",
            "traceId": "trace-123"
        }
        """;

        // Act
        var result = _adapter.Normalize(rawJson, new ProtocolIngressContext());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("generic-json://user-service", result.Envelope?.Source);
    }

    [Fact]
    public void Normalize_WithContextOccurredAt_UsesContextTime()
    {
        // Arrange
        var expected = new DateTimeOffset(2026, 5, 12, 10, 0, 0, TimeSpan.Zero);
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123"
        }
        """;
        var context = new ProtocolIngressContext
        {
            OccurredAt = expected
        };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Envelope?.OccurredAt);
    }

    [Fact]
    public void Normalize_WithWhitespaceTraceEverywhere_AndRequireProvided_Fails()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "   "
        }
        """;
        var context = new ProtocolIngressContext
        {
            TraceId = "\t",
            IdGenerationPolicy = TraceIdGenerationPolicy.RequireProvided
        };

        // Act
        var result = _adapter.Normalize(rawJson, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "TraceId");
    }

    [Fact]
    public void Normalize_RawPayloadSerialization_DoesNotIncludeParsedObject()
    {
        // Arrange
        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123"
        }
        """;

        // Act
        var result = _adapter.Normalize(rawJson, new ProtocolIngressContext());

        // Assert
        Assert.True(result.IsSuccess);
        var serializedPayload = System.Text.Json.JsonSerializer.Serialize(result.Envelope!.Payload);
        Assert.DoesNotContain("ParsedObject", serializedPayload, StringComparison.Ordinal);
    }
}

public sealed class CloudEventsProtocolAdapterTests
{
    private readonly CloudEventsProtocolAdapter _adapter;
    private readonly FakeClock _clock;

    public CloudEventsProtocolAdapterTests()
    {
        _clock = new FakeClock();
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        var validator = new DefaultEnvelopeValidator();
        _adapter = new CloudEventsProtocolAdapter(payloadHasher, idGenerator, _clock, validator);
    }

    [Fact]
    public void Normalize_WithValidCloudEvent_ReturnsSuccess()
    {
        // Arrange
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "https://user-service",
            Time = DateTimeOffset.UtcNow.ToString("O"),
            Data = new { userId = "user-456" }
        };
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope);
        Assert.Equal("event-123", result.Envelope.EventId);
        Assert.Equal("cloudevents.ingress.normalized", result.Envelope.EventType);
        Assert.Equal("com.example.user.created", result.Envelope.Payload.RawEventType);
        Assert.Equal("cloudevents", result.Envelope.Protocol);
    }

    [Fact]
    public void Normalize_WithMissingId_ReturnsFailure()
    {
        // Arrange
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "",
            Type = "com.example.user.created",
            Source = "https://user-service"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "Id");
    }

    [Fact]
    public void Normalize_ExtractsTraceIdFromExtensions()
    {
        // Arrange
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "https://user-service",
            Extensions = new Dictionary<string, object> { ["traceid"] = "trace-from-extension" }
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("trace-from-extension", result.Envelope?.TraceId);
    }

    [Fact]
    public void Normalize_NormalizesSourceToAbsoluteUri()
    {
        // Arrange
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "user-service",
            Time = DateTimeOffset.UtcNow.ToString("O")
        };
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("cloudevents://user-service", result.Envelope?.Source);
    }

    [Fact]
    public void Normalize_WithInvalidTime_UsesContextOccurredAtFallback()
    {
        // Arrange
        var expected = new DateTimeOffset(2026, 5, 12, 11, 0, 0, TimeSpan.Zero);
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "user-service",
            Time = "not-a-date",
            Extensions = new Dictionary<string, object> { ["traceId"] = "trace-123" }
        };
        var context = new ProtocolIngressContext { OccurredAt = expected };

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Envelope?.OccurredAt);
    }

    [Fact]
    public void Normalize_ExtractsTraceIdFromJsonElementExtension()
    {
        // Arrange
        var extensionDoc = System.Text.Json.JsonDocument.Parse("\"trace-from-json-element\"");
        var cloudEvent = new CloudEventEnvelope
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "https://user-service",
            Extensions = new Dictionary<string, object>
            {
                ["traceId"] = extensionDoc.RootElement.Clone()
            }
        };

        // Act
        var result = _adapter.Normalize(cloudEvent, new ProtocolIngressContext());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("trace-from-json-element", result.Envelope?.TraceId);
    }
}

public sealed class McpProtocolAdapterTests
{
    private readonly McpProtocolAdapter _adapter;
    private readonly FakeClock _clock;

    public McpProtocolAdapterTests()
    {
        _clock = new FakeClock();
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        var validator = new DefaultEnvelopeValidator();
        _adapter = new McpProtocolAdapter(payloadHasher, idGenerator, _clock, validator);
    }

    [Fact]
    public void Normalize_WithValidMcpEvent_ReturnsSuccess()
    {
        // Arrange
        var mcpEvent = new McpEvent
        {
            EventId = "event-123",
            EventType = "tool.called",
            Source = "mcp-agent",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(mcpEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope);
        Assert.Equal("mcp", result.Envelope.Protocol);
        Assert.Equal("mcp.ingress.normalized", result.Envelope.EventType);
        Assert.Equal("tool.called", result.Envelope.Payload.RawEventType);
    }

    [Fact]
    public void Normalize_GeneratesEventIdIfMissing()
    {
        // Arrange
        var mcpEvent = new McpEvent
        {
            EventType = "tool.called",
            Source = "mcp-agent",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(mcpEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope?.EventId);
        Assert.NotEmpty(result.Envelope.EventId);
    }

    [Fact]
    public void Normalize_NormalizesSourceToAbsoluteUri()
    {
        // Arrange
        var mcpEvent = new McpEvent
        {
            EventId = "event-123",
            EventType = "tool.called",
            Source = "mcp-agent",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(mcpEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("mcp://mcp-agent", result.Envelope?.Source);
    }
}

public sealed class A2aProtocolAdapterTests
{
    private readonly A2aProtocolAdapter _adapter;
    private readonly FakeClock _clock;

    public A2aProtocolAdapterTests()
    {
        _clock = new FakeClock();
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        var validator = new DefaultEnvelopeValidator();
        _adapter = new A2aProtocolAdapter(payloadHasher, idGenerator, _clock, validator);
    }

    [Fact]
    public void Normalize_WithValidA2aEvent_ReturnsSuccess()
    {
        // Arrange
        var a2aEvent = new A2aEvent
        {
            MessageId = "msg-123",
            MessageType = "request",
            SenderId = "agent-1",
            ReceiverId = "agent-2",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(a2aEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope);
        Assert.Equal("a2a", result.Envelope.Protocol);
        Assert.Equal("a2a.ingress.normalized", result.Envelope.EventType);
        Assert.Equal("request", result.Envelope.Payload.RawEventType);
        Assert.Equal("a2a://agent-1", result.Envelope.Source);
        Assert.Equal("agent-1", result.Envelope.ActorId);
    }

    [Fact]
    public void Normalize_WithMissingReceiverId_ReturnsFailure()
    {
        // Arrange
        var a2aEvent = new A2aEvent
        {
            MessageType = "request",
            SenderId = "agent-1",
            ReceiverId = "",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(a2aEvent, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "ReceiverId");
    }

    [Fact]
    public void Normalize_NormalizesSourceToAbsoluteUri()
    {
        // Arrange
        var a2aEvent = new A2aEvent
        {
            MessageId = "msg-123",
            MessageType = "request",
            SenderId = "agent-1",
            ReceiverId = "agent-2",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(a2aEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("a2a://agent-1", result.Envelope?.Source);
    }
}

public sealed class AgUiProtocolAdapterTests
{
    private readonly AgUiProtocolAdapter _adapter;
    private readonly FakeClock _clock;

    public AgUiProtocolAdapterTests()
    {
        _clock = new FakeClock();
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        var validator = new DefaultEnvelopeValidator();
        _adapter = new AgUiProtocolAdapter(payloadHasher, idGenerator, _clock, validator);
    }

    [Fact]
    public void Normalize_WithValidAgUiEvent_ReturnsSuccess()
    {
        // Arrange
        var agUiEvent = new AgUiEvent
        {
            Action = "click.button",
            SessionId = "session-123",
            UserId = "user-456",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(agUiEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope);
        Assert.Equal("ag-ui", result.Envelope.Protocol);
        Assert.Equal("ag-ui.ingress.normalized", result.Envelope.EventType);
        Assert.Equal("click.button", result.Envelope.Payload.RawEventType);
        Assert.StartsWith("ag-ui://", result.Envelope.Source);
    }

    [Fact]
    public void Normalize_GeneratesEventIdIfMissing()
    {
        // Arrange
        var agUiEvent = new AgUiEvent
        {
            Action = "click.button",
            SessionId = "session-123",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(agUiEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope?.EventId);
    }

    [Fact]
    public void Normalize_NormalizesSourceWithSessionContext()
    {
        // Arrange
        var agUiEvent = new AgUiEvent
        {
            Action = "click.button",
            SessionId = "session-123",
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(agUiEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("ag-ui://session/session-123", result.Envelope?.Source);
    }
}

/// <summary>
/// Tests for deterministic hash generation across normalizations.
/// </summary>
public sealed class PayloadHashDeterminismTests
{
    [Fact]
    public void GenericJsonAdapter_ProducesDeterministicHash()
    {
        // Arrange
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        var validator = new DefaultEnvelopeValidator();
        var clock = new FakeClock();
        var adapter = new GenericJsonProtocolAdapter(payloadHasher, idGenerator, clock, validator);

        var rawJson = """
        {
            "eventType": "user.created",
            "source": "user-service",
            "traceId": "trace-123",
            "data": { "userId": "user-456" }
        }
        """;
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result1 = adapter.Normalize(rawJson, context);
        var result2 = adapter.Normalize(rawJson, context);

        // Assert
        Assert.True(result1.IsSuccess);
        Assert.True(result2.IsSuccess);
        Assert.Equal(result1.Envelope?.PayloadHash, result2.Envelope?.PayloadHash);
    }
}
