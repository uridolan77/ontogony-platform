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
        _adapter = new GenericJsonProtocolAdapter(payloadHasher, idGenerator);
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
        Assert.Equal("user.created", result.Envelope.EventType);
        Assert.Equal("user-service", result.Envelope.Source);
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
        Assert.Equal(result.Envelope.Payload.PayloadHash, result.Envelope.PayloadHash);
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
}

public sealed class CloudEventsProtocolAdapterTests
{
    private readonly CloudEventsProtocolAdapter _adapter;

    public CloudEventsProtocolAdapterTests()
    {
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        _adapter = new CloudEventsProtocolAdapter(payloadHasher, idGenerator);
    }

    [Fact]
    public void Normalize_WithValidCloudEvent_ReturnsSuccess()
    {
        // Arrange
        var cloudEvent = new CloudEvent
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "https://user-service",
            Time = DateTimeOffset.UtcNow,
            Data = new { userId = "user-456" }
        };
        var context = new ProtocolIngressContext { TraceId = "trace-123" };

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Envelope);
        Assert.Equal("event-123", result.Envelope.EventId);
        Assert.Equal("com.example.user.created", result.Envelope.EventType);
        Assert.Equal("https://user-service", result.Envelope.Source);
        Assert.Equal("cloudevents", result.Envelope.Protocol);
    }

    [Fact]
    public void Normalize_WithMissingId_ReturnsFailure()
    {
        // Arrange
        var cloudEvent = new CloudEvent
        {
            Id = "",  // Empty string to test required field
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
        var cloudEvent = new CloudEvent
        {
            Id = "event-123",
            Type = "com.example.user.created",
            Source = "https://user-service",
            Extensions = new Dictionary<string, object?> { { "traceid", "trace-from-extension" } }
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(cloudEvent, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("trace-from-extension", result.Envelope?.TraceId);
    }
}

public sealed class McpProtocolAdapterTests
{
    private readonly McpProtocolAdapter _adapter;

    public McpProtocolAdapterTests()
    {
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        _adapter = new McpProtocolAdapter(payloadHasher, idGenerator);
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
        Assert.Equal("tool.called", result.Envelope.EventType);
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
}

public sealed class A2aProtocolAdapterTests
{
    private readonly A2aProtocolAdapter _adapter;

    public A2aProtocolAdapterTests()
    {
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        _adapter = new A2aProtocolAdapter(payloadHasher, idGenerator);
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
        Assert.Equal("agent-1", result.Envelope.Source);
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
            ReceiverId = "",  // Empty to test required field validation
            TraceId = "trace-123"
        };
        var context = new ProtocolIngressContext();

        // Act
        var result = _adapter.Normalize(a2aEvent, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Field == "ReceiverId");
    }
}

public sealed class AgUiProtocolAdapterTests
{
    private readonly AgUiProtocolAdapter _adapter;

    public AgUiProtocolAdapterTests()
    {
        var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
        var idGenerator = new FakeIdGenerator();
        _adapter = new AgUiProtocolAdapter(payloadHasher, idGenerator);
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
        Assert.Equal("click.button", result.Envelope.EventType);
        Assert.Equal("ag-ui", result.Envelope.Source);
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
        var adapter = new GenericJsonProtocolAdapter(payloadHasher, idGenerator);

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
