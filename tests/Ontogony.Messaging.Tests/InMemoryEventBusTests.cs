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
}
