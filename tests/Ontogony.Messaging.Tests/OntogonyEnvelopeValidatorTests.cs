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
        Assert.Contains(r.Errors, x => x.Message.Contains("absolute URI", StringComparison.Ordinal));
    }

    [Fact]
    public void Validate_Source_CustomSchemeAbsolute_ReturnsSuccess()
    {
        var e = ValidEnvelope() with { Source = "mcp://ingestion/session" };
        Assert.True(new DefaultEnvelopeValidator().Validate(e).IsValid);
    }

    [Fact]
    public void Validate_Source_RelativePath_ReturnsError()
    {
        var e = ValidEnvelope() with { Source = "/events/stream" };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Message.Contains("absolute URI", StringComparison.Ordinal));
    }

    [Fact]
    public void Validate_Source_HttpsUriWithPath_ReturnsSuccess()
    {
        var e = ValidEnvelope() with { Source = "https://example.com/events" };
        Assert.True(new DefaultEnvelopeValidator().Validate(e).IsValid);
    }

    [Fact]
    public void Validate_Source_FileUri_ReturnsError()
    {
        var e = ValidEnvelope() with { Source = "file:///tmp/x" };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.Source));
    }

    [Fact]
    public void Validate_Source_UrnAbsolute_ReturnsSuccess()
    {
        var e = ValidEnvelope() with { Source = "urn:ontogony:test" };
        Assert.True(new DefaultEnvelopeValidator().Validate(e).IsValid);
    }

    [Fact]
    public void Validate_StrictRecorderProtocols_AllowsAgUiMcpA2a()
    {
        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ProtocolNames.AgUi,
            ProtocolNames.Mcp,
            ProtocolNames.A2A
        };
        var v = new DefaultEnvelopeValidator(new EnvelopeValidatorOptions { AllowedProtocols = allowed });
        foreach (var proto in new[] { ProtocolNames.AgUi, ProtocolNames.Mcp, ProtocolNames.A2A })
        {
            var e = ValidEnvelope() with { Protocol = proto };
            Assert.True(v.Validate(e).IsValid, proto);
        }
    }

    [Fact]
    public void Validate_MissingEventId_ReturnsError()
    {
        var e = ValidEnvelope() with { EventId = "" };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.EventId));
    }

    [Fact]
    public void Validate_DefaultOccurredAt_ReturnsError()
    {
        var e = ValidEnvelope() with { OccurredAt = default };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.OccurredAt));
    }

    [Fact]
    public void Validate_MissingTraceId_ReturnsError()
    {
        var e = ValidEnvelope() with { TraceId = "" };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.TraceId));
    }

    [Fact]
    public void Validate_InvalidTraceIdCharacter_ReturnsError()
    {
        var e = ValidEnvelope() with { TraceId = "trace with spaces" };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.TraceId));
    }

    [Fact]
    public void Validate_MissingProtocol_ReturnsError()
    {
        var e = ValidEnvelope() with { Protocol = "" };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.Protocol));
    }

    [Fact]
    public void Validate_BlankSchemaVersion_ReturnsError()
    {
        var e = ValidEnvelope() with { SchemaVersion = "   " };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.SchemaVersion));
    }

    [Fact]
    public void Validate_ReferencePayloadNull_ReturnsError()
    {
        var e = ValidEnvelope() with { Payload = null! };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.Payload));
    }

    [Fact]
    public void Validate_EventIdTooLong_ReturnsError()
    {
        var e = ValidEnvelope() with { EventId = new string('a', 300) };
        var r = new DefaultEnvelopeValidator().Validate(e);
        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, x => x.Field == nameof(OntogonyEnvelope<TestPayload>.EventId));
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
