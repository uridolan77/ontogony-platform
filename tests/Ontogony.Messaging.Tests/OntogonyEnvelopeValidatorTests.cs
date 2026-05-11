using Ontogony.Contracts.Events;
using Xunit;

namespace Ontogony.Messaging.Tests;

public sealed class OntogonyEnvelopeValidatorTests
{
    private static OntogonyEnvelope<TestPayload> ValidEnvelope() =>
        new()
        {
            EventId = "evt_1",
            EventType = "agentor.run.started",
            Source = "https://tests.local/default",
            OccurredAt = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero),
            TraceId = "trace-1",
            Protocol = ProtocolNames.Agentor,
            SchemaVersion = "1.0",
            Payload = new TestPayload("ok")
        };

    [Fact]
    public void Validate_ValidEnvelope_ReturnsSuccess()
    {
        var v = new DefaultEnvelopeValidator();
        var r = v.Validate(ValidEnvelope());
        Assert.True(r.IsValid);
        Assert.Empty(r.Errors);
    }

    [Fact]
    public void Validate_InvalidEventType_ReturnsError()
    {
        var e = ValidEnvelope() with { EventType = "only.two" };
        var v = new DefaultEnvelopeValidator();
        var r = v.Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.EventType));
    }

    [Fact]
    public void Validate_InvalidSource_ReturnsError()
    {
        var e = ValidEnvelope() with { Source = "not-a-uri" };
        var v = new DefaultEnvelopeValidator();
        var r = v.Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.Source));
    }

    [Fact]
    public void Validate_InvalidPayloadHash_ReturnsError()
    {
        var e = ValidEnvelope() with { PayloadHash = "not-64-hex" };
        var v = new DefaultEnvelopeValidator();
        var r = v.Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.PayloadHash));
    }

    [Fact]
    public void Validate_AllowedProtocols_RejectsUnknown()
    {
        var e = ValidEnvelope() with { Protocol = "custom-proto" };
        var v = new DefaultEnvelopeValidator(new EnvelopeValidatorOptions
        {
            AllowedProtocols = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ProtocolNames.Mcp }
        });
        var r = v.Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Code == "allowed_values");
    }

    private sealed record TestPayload(string Value);
}
