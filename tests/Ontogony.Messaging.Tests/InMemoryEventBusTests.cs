using Ontogony.Contracts.Events;
using Ontogony.Messaging;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Messaging.Tests;

public sealed class InMemoryEventBusTests
{
    [Fact]
    public async Task PublishAsync_Delivers_To_Registered_Handler()
    {
        var bus = new InMemoryEventBus();
        var handler = new RecordingHandler();
        bus.Register(handler);

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("hello"));
        await bus.PublishAsync(envelope);

        Assert.Single(handler.Received);
        Assert.Equal("hello", handler.Received[0].Payload.Value);
    }

    [Fact]
    public async Task InMemoryEventPublisher_CapturesPublishedEnvelope_InSink()
    {
        var sink = new InMemoryEventSink();
        var publisher = new InMemoryEventPublisher(sink);
        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("capture"));

        await publisher.PublishAsync(envelope);

        var captured = sink.ReadAll<Payload>();
        Assert.Single(captured);
        Assert.Equal("capture", captured[0].Payload.Value);
    }

    [Fact]
    public async Task InMemoryEventPublisher_Dispatches_To_Multiple_Handlers()
    {
        var publisher = new InMemoryEventPublisher();
        var handlerA = new RecordingHandler();
        var handlerB = new RecordingHandler();
        publisher.Register(handlerA);
        publisher.Register(handlerB);

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("fanout"));
        await publisher.PublishAsync(envelope);

        Assert.Single(handlerA.Received);
        Assert.Single(handlerB.Received);
        Assert.Equal("fanout", handlerA.Received[0].Payload.Value);
        Assert.Equal("fanout", handlerB.Received[0].Payload.Value);
    }

    [Fact]
    public async Task InMemoryEventPublisher_StopsDispatch_WhenHandlerThrows_AndOptionDisabled()
    {
        var publisher = new InMemoryEventPublisher(options: new EventDispatchOptions
        {
            ContinueOnHandlerException = false
        });
        publisher.Register(new ThrowingHandler());

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("boom"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => publisher.PublishAsync(envelope));
    }

    [Fact]
    public async Task InMemoryEventPublisher_ContinuesDispatch_WhenHandlerThrows_AndOptionEnabled()
    {
        var sink = new InMemoryEventSink();
        var publisher = new InMemoryEventPublisher(sink, new EventDispatchOptions
        {
            ContinueOnHandlerException = true
        });
        var successfulHandler = new RecordingHandler();
        publisher.Register(new ThrowingHandler());
        publisher.Register(successfulHandler);

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("continue"));

        var ex = await Assert.ThrowsAsync<AggregateException>(() => publisher.PublishAsync(envelope));
        Assert.Single(ex.InnerExceptions);
        Assert.Single(successfulHandler.Received);
        Assert.Single(sink.ReadAll<Payload>());
    }

    [Fact]
    public async Task InMemoryEventPublisher_PreservesCorrelationMetadata()
    {
        var sink = new InMemoryEventSink();
        var publisher = new InMemoryEventPublisher(sink);
        var envelope = new OntogonyEnvelope<Payload>
        {
            EventId = "evt_1",
            EventType = "test.event",
            Source = "test://source",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            TraceId = "trace-123",
            TenantId = "tenant-1",
            WorkspaceId = "workspace-1",
            ProjectId = "project-1",
            SessionId = "session-1",
            Protocol = "test",
            Payload = new Payload("meta")
        };

        await publisher.PublishAsync(envelope);

        var captured = sink.ReadAll<Payload>().Single();
        EnvelopeAssertions.AssertCorrelationMetadataEqual(envelope, captured);
    }

    [Fact]
    public async Task InMemoryEventPublisher_ComputesPayloadHash_WhenEnabled()
    {
        var sink = new InMemoryEventSink();
        var publisher = new InMemoryEventPublisher(sink, new EventDispatchOptions
        {
            ComputePayloadHash = true
        });
        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("hash-me"));

        await publisher.PublishAsync(envelope);

        var captured = sink.ReadAll<Payload>().Single();
        EnvelopeAssertions.AssertPayloadHashPresent(captured);
    }

    [Fact]
    public async Task InMemoryEventPublisher_RejectsEnvelopeMissingRequiredFields_WhenValidationEnabled()
    {
        var publisher = new InMemoryEventPublisher(options: new EventDispatchOptions
        {
            ValidateRequiredEnvelopeFields = true
        });
        var invalid = new OntogonyEnvelope<Payload>
        {
            EventId = string.Empty,
            EventType = "test.event",
            Source = "test://source",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            TraceId = "trace-1",
            Protocol = "test",
            Payload = new Payload("invalid")
        };

        await Assert.ThrowsAsync<ArgumentException>(() => publisher.PublishAsync(invalid));
    }

    [Fact]
    public async Task PublishWithResultAsync_IncludesFailure_WhenHandlerThrows()
    {
        var publisher = new InMemoryEventPublisher(options: new EventDispatchOptions
        {
            ContinueOnHandlerException = true,
            RecordObservabilityMetrics = false
        });
        publisher.Register(new ThrowingHandler());

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("boom"));
        var result = await publisher.PublishWithResultAsync(envelope);

        Assert.Single(result.Failures);
        Assert.False(result.HandlerResults[0].Succeeded);
        Assert.Single(publisher.Sink.ReadAll<Payload>());
    }

    [Fact]
    public async Task PublishOnly_DoesNotInvokeHandlers_ButCapturesSink()
    {
        var sink = new InMemoryEventSink();
        var publisher = new InMemoryEventPublisher(sink, new EventDispatchOptions
        {
            OperationMode = EventPublisherOperationMode.PublishOnly,
            RecordObservabilityMetrics = false
        });
        var handler = new RecordingHandler();
        publisher.Register(handler);

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("only"));
        var result = await publisher.PublishWithResultAsync(envelope);

        Assert.Empty(result.HandlerResults);
        Assert.Empty(handler.Received);
        Assert.Single(sink.ReadAll<Payload>());
    }

    [Fact]
    public async Task CaptureOnly_AppendsSink_DoesNotInvokeHandlers()
    {
        var sink = new InMemoryEventSink();
        var publisher = new InMemoryEventPublisher(sink, new EventDispatchOptions
        {
            OperationMode = EventPublisherOperationMode.CaptureOnly,
            RecordObservabilityMetrics = false
        });
        var handler = new RecordingHandler();
        publisher.Register(handler);

        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("cap"));
        var result = await publisher.PublishWithResultAsync(envelope);

        Assert.Empty(result.HandlerResults);
        Assert.Empty(handler.Received);
        Assert.Single(sink.ReadAll<Payload>());
    }

    [Fact]
    public void JsonEventSerializer_RoundTripsEnvelope()
    {
        var serializer = new JsonEventSerializer();
        var envelope = TestEnvelopeFactory.Create("test.event", "test", new Payload("ser"));

        var json = serializer.Serialize(envelope);
        var restored = serializer.Deserialize<Payload>(json);

        Assert.Equal(envelope.EventId, restored.EventId);
        Assert.Equal("ser", restored.Payload.Value);
    }

    private sealed record Payload(string Value);

    private sealed class RecordingHandler : IEventHandler<Payload>
    {
        public List<OntogonyEnvelope<Payload>> Received { get; } = new();

        public Task HandleAsync(OntogonyEnvelope<Payload> envelope, CancellationToken cancellationToken = default)
        {
            Received.Add(envelope);
            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingHandler : IEventHandler<Payload>
    {
        public Task HandleAsync(OntogonyEnvelope<Payload> envelope, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("handler failure");
        }
    }
}
