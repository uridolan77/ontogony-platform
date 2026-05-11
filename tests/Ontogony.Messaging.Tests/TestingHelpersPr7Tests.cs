using Ontogony.Contracts.Events;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Messaging.Tests;

public sealed class TestingHelpersPr7Tests
{
    [Fact]
    public async Task PublishedEventRecorder_CapturesPublishedEvents()
    {
        var recorder = new PublishedEventRecorder();
        var envelope = TestEnvelopeFactory.Create("agentor.run.started", ProtocolNames.Agentor, new Payload("rec"));

        await recorder.PublishAsync(envelope);

        Assert.Single(recorder.PublishedEvents);
    }

    [Fact]
    public async Task FakeEventPublisher_CanBeConfiguredToThrow()
    {
        var publisher = new FakeEventPublisher
        {
            PublishException = new InvalidOperationException("boom")
        };
        var envelope = TestEnvelopeFactory.Create("agentor.run.started", ProtocolNames.Agentor, new Payload("x"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => publisher.PublishAsync(envelope));
    }

    [Fact]
    public void EnvelopeAssertions_ValidateRequiredFields()
    {
        var envelope = TestEnvelopeFactory.Create("agentor.run.started", ProtocolNames.Agentor, new Payload("ok"));

        EnvelopeAssertions.AssertHasRequiredFields(envelope);
    }

    [Fact]
    public void EnvelopeAssertions_ThrowsWhenPayloadHashMissing()
    {
        var envelope = TestEnvelopeFactory.Create("agentor.run.started", ProtocolNames.Agentor, new Payload("hash"));

        Assert.Throws<InvalidOperationException>(() => EnvelopeAssertions.AssertPayloadHashPresent(envelope));
    }

    private sealed record Payload(string Value);
}
