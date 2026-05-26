using System.Net;
using Ontogony.Artifacts;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Http;
using Ontogony.Idempotency;
using Ontogony.Persistence;
using Ontogony.Primitives;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

/// <summary>Self-tests for PR33 conformance kit helpers.</summary>
public sealed class ConformanceKitPr33Tests
{
    // -------------------------------------------------------------------------
    // TracingConformanceAssertions
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Tracing_CanonicalTraceHeader_EchoedInResponse()
    {
        await TracingConformanceAssertions.AssertCanonicalTraceHeaderEchoedAsync("trace-pr33-001");
    }

    [Fact]
    public async Task Tracing_TraceIdGeneratedWhenAbsent()
    {
        await TracingConformanceAssertions.AssertTraceIdGeneratedWhenAbsentAsync();
    }

    [Fact]
    public async Task Tracing_TenantIdPropagatedFromHeader()
    {
        await TracingConformanceAssertions.AssertTenantIdPropagatedAsync("tenant-pr33");
    }

    // -------------------------------------------------------------------------
    // ErrorShapeConformanceAssertions
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Error_UnmappedException_Produces500()
    {
        await ErrorShapeConformanceAssertions.AssertUnmappedExceptionProduces500Async();
    }

    [Fact]
    public async Task Error_UnmappedException_DoesNotLeakInternalMessage()
    {
        await ErrorShapeConformanceAssertions.AssertNoUnmappedExceptionMessageLeakAsync();
    }

    [Fact]
    public async Task Error_MappedException_ProducesExpectedCodeAndStatus()
    {
        await ErrorShapeConformanceAssertions.AssertMappedExceptionProducesExpectedShapeAsync(
            new InvalidOperationException("simulated domain failure"),
            HttpStatusCode.UnprocessableEntity,
            "DOMAIN_FAILURE");
    }

    [Fact]
    public void Error_AssertJsonShape_PassesForWellFormedPayload()
    {
        var json = """{"code":"TEST_ERROR","message":"something went wrong"}""";
        ErrorShapeConformanceAssertions.AssertErrorJsonShape(json, expectedCode: "TEST_ERROR");
    }

    [Fact]
    public void Error_AssertJsonShape_FailsWhenCodeMissing()
    {
        var json = """{"message":"no code here"}""";
        Assert.Throws<InvalidOperationException>(() =>
            ErrorShapeConformanceAssertions.AssertErrorJsonShape(json));
    }

    // -------------------------------------------------------------------------
    // EnvelopeConformanceAssertions
    // -------------------------------------------------------------------------

    private static OntogonyEnvelope<object> BuildConformingEnvelope(bool includeHash = true)
    {
        var payload = new { Name = "pr33" };
        var hasher = new EnvelopePayloadHasher(new PayloadHasher(new Sha256ContentHashService()));

        var envelope = new OntogonyEnvelope<object>
        {
            EventId = Guid.NewGuid().ToString("n"),
            EventType = "ontogony.conformance.test",
            Source = "ontogony://conformance/test",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-env-pr33",
            Protocol = "internal",
            Payload = payload
        };

        if (includeHash)
        {
            // Use with operator to set PayloadHash
            return envelope with { PayloadHash = hasher.ComputeEnvelopePayloadHash(envelope) };
        }

        return envelope;
    }

    [Fact]
    public void Envelope_FullConformance_PassesForValidEnvelope()
    {
        var envelope = BuildConformingEnvelope();
        EnvelopeConformanceAssertions.AssertFullConformance(envelope);
    }

    [Fact]
    public void Envelope_SourceScheme_FailsForNonCanonicalSource()
    {
        var envelope = BuildConformingEnvelope() with { Source = "https://service.example/events" };
        Assert.Throws<InvalidOperationException>(() =>
            EnvelopeConformanceAssertions.AssertSourceFollowsScheme(envelope));
    }

    [Fact]
    public void Envelope_EventTypeNamespaced_FailsForBareType()
    {
        var envelope = BuildConformingEnvelope() with { EventType = "nondotted" };
        Assert.Throws<InvalidOperationException>(() =>
            EnvelopeConformanceAssertions.AssertEventTypeIsNamespaced(envelope));
    }

    [Fact]
    public void Envelope_PayloadHash_FailsOnMismatch()
    {
        var envelope = BuildConformingEnvelope() with { PayloadHash = "wrong-hash" };
        Assert.Throws<InvalidOperationException>(() =>
            EnvelopeConformanceAssertions.AssertPayloadHashMatchesContent(envelope));
    }

    // -------------------------------------------------------------------------
    // HmacConformanceAssertions
    // -------------------------------------------------------------------------

    [Fact]
    public void Hmac_SignatureRoundTrip_Passes()
    {
        HmacConformanceAssertions.AssertSignatureRoundTrip(
            secret: "secret-pr33",
            httpMethod: "POST",
            pathAndQuery: "/api/events?version=1",
            timestamp: "2026-05-12T00:00:00Z",
            nonce: "nonce-pr33-001",
            bodySha256HexLower: "abc123def456abc123def456abc123def456abc123def456abc123def456abc1");
    }

    [Fact]
    public void Hmac_CanonicalStringFormat_HasFiveParts()
    {
        HmacConformanceAssertions.AssertCanonicalStringFormat(
            "GET", "/path", "2026-01-01T00:00:00Z", "nonce1",
            "0000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void Hmac_BodyHashFormat_IsLowercase64Chars()
    {
        var body = System.Text.Encoding.UTF8.GetBytes("{\"x\":1}");
        HmacConformanceAssertions.AssertBodyHashFormat(body);
    }

    [Fact]
    public void Hmac_TamperedSignature_IsRejected()
    {
        HmacConformanceAssertions.AssertTamperedSignatureIsRejected(
            secret: "secret-pr33",
            httpMethod: "POST",
            pathAndQuery: "/api/events",
            timestamp: "2026-05-12T00:00:00Z",
            nonce: "nonce-pr33",
            bodySha256HexLower: "abc123def456abc123def456abc123def456abc123def456abc123def456abc1");
    }

    // -------------------------------------------------------------------------
    // OutboxConformanceHarness
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Outbox_WriteThenRead_FindsMessage()
    {
        var store = new InMemoryOutboxStore();
        var msg = OutboxConformanceHarness.BuildMessage("msg-pr33-1");
        await OutboxConformanceHarness.AssertWriteThenReadAsync(store, store, msg);
    }

    [Fact]
    public async Task Outbox_MarkDispatched_RemovesFromQueue()
    {
        var store = new InMemoryOutboxStore();
        var msg = OutboxConformanceHarness.BuildMessage("msg-pr33-2");
        await OutboxConformanceHarness.AssertMarkDispatchedRemovesFromQueueAsync(store, store, store, msg);
    }

    [Fact]
    public async Task Outbox_DuplicateWrite_Throws()
    {
        var store = new InMemoryOutboxStore();
        var msg = OutboxConformanceHarness.BuildMessage("msg-pr33-dup");
        await OutboxConformanceHarness.AssertDuplicateWriteThrowsAsync(store, msg);
    }

    [Fact]
    public async Task Outbox_MarkFailed_IsIdempotent()
    {
        var store = new InMemoryOutboxStore();
        var msg = OutboxConformanceHarness.BuildMessage("msg-pr33-fail");
        await OutboxConformanceHarness.AssertMarkFailedIsIdempotentAsync(store, store, msg);
    }

    // -------------------------------------------------------------------------
    // HttpResilienceConformanceHarness
    // -------------------------------------------------------------------------

    [Fact]
    public async Task HttpResilience_NoRetryOnSuccess()
    {
        var stub = new StubHttpMessageHandler();
        var clock = new FakeClock();
        var client = HttpResilienceConformanceHarness.BuildResilientClient(
            stub,
            opts => { opts.MaxRetries = 2; },
            clock);

        await HttpResilienceConformanceHarness.AssertNoRetryOnSuccessAsync(client, stub);
    }

    [Fact]
    public async Task HttpResilience_RetriesOnTransientFailure()
    {
        var stub = new StubHttpMessageHandler();
        var clock = new FakeClock();
        var client = HttpResilienceConformanceHarness.BuildResilientClient(
            stub,
            opts => { opts.MaxRetries = 2; },
            clock);

        // 2 retries = 3 total attempts
        await HttpResilienceConformanceHarness.AssertRetriesOnTransientFailureAsync(client, stub, expectedTotalAttempts: 3);
    }

    [Fact]
    public async Task HttpResilience_CircuitOpensAfterThreshold()
    {
        var stub = new StubHttpMessageHandler();
        await HttpResilienceConformanceHarness.AssertCircuitOpensAfterThresholdAsync(stub, failuresToOpen: 3);
    }

    [Fact]
    public async Task HttpResilience_RespectsRetryAfterHeader()
    {
        var stub = new StubHttpMessageHandler();
        await HttpResilienceConformanceHarness.AssertRespectsRetryAfterHeaderAsync(
            stub,
            retryAfter: TimeSpan.FromMilliseconds(40));
    }

    [Fact]
    public async Task HttpResilience_TotalTimeoutLimitsAttempts()
    {
        var stub = new StubHttpMessageHandler();
        await HttpResilienceConformanceHarness.AssertTotalTimeoutLimitsAttemptsAsync(
            stub,
            totalTimeout: TimeSpan.FromMilliseconds(90),
            maxExpectedAttempts: 4);
    }

    // -------------------------------------------------------------------------
    // IdempotencyLedgerConformanceHarness
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Idempotency_TryBeginIsExclusive()
    {
        var ledger = new InMemoryIdempotencyLedger();
        await IdempotencyLedgerConformanceHarness.AssertTryBeginIsExclusiveAsync(ledger, "key-pr33-idem-1");
    }

    [Fact]
    public async Task Idempotency_LifecycleTransitions()
    {
        var ledger = new InMemoryIdempotencyLedger();
        await IdempotencyLedgerConformanceHarness.AssertLifecycleTransitionsAsync(ledger, "key-pr33-idem-2");
    }

    // -------------------------------------------------------------------------
    // ArtifactStoreConformanceHarness
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Artifact_PutGetAndDedupe()
    {
        var store = new InMemoryArtifactStore();
        await ArtifactStoreConformanceHarness.AssertPutGetAndDedupeAsync(store);
    }

    [Fact]
    public async Task Artifact_ExistsAndTryGet()
    {
        var store = new InMemoryArtifactStore();
        await ArtifactStoreConformanceHarness.AssertExistsAndTryGetAsync(store);
    }
}
