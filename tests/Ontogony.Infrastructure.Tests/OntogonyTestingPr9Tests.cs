using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Ontogony.Errors;
using Ontogony.Observability;
using Ontogony.Security;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyTestingPr9Tests
{
    [Fact]
    public void FakeClock_AdvancesDeterministically()
    {
        var initial = new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);
        var clock = new FakeClock(initial);

        clock.Advance(TimeSpan.FromMinutes(5));

        Assert.Equal(initial.AddMinutes(5), clock.UtcNow);
    }

    [Fact]
    public void FakeIdGenerator_UsesQueuedGuidThenDeterministicSequence()
    {
        var fake = new FakeIdGenerator();
        var queued = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        fake.Enqueue(queued);

        var first = fake.NewGuid();
        var second = fake.NewGuid();

        Assert.Equal(queued, first);
        Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000001"), second);
        Assert.Equal("evt_00000000000000000000000000000002", fake.NewId("evt"));
    }

    [Fact]
    public void FakeCurrentActorAccessor_ExposesConfiguredActor()
    {
        var actor = new CurrentActor("actor-1", OntogonyActorTypes.Human, [OntogonyRoleNames.HumanOperator]);
        var accessor = new FakeCurrentActorAccessor { Current = actor };

        Assert.NotNull(accessor.Current);
        Assert.Equal("actor-1", accessor.Current!.ActorId);
    }

    [Fact]
    public async Task StubHttpMessageHandler_SupportsScriptedResponsesAndExceptions()
    {
        var handler = new StubHttpMessageHandler();
        handler.EnqueueResponse(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
        handler.EnqueueException(new HttpRequestException("flaky"));

        using var client = new HttpClient(handler);

        var first = await client.GetAsync("https://example.test/first");
        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync("https://example.test/second"));

        Assert.Equal(HttpStatusCode.TooManyRequests, first.StatusCode);
        Assert.Equal(2, handler.CallCount);
    }

    [Fact]
    public async Task RecordingHttpMessageHandler_CapturesOutgoingRequests()
    {
        var handler = new RecordingHttpMessageHandler((request, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = new StringContent("ok", Encoding.UTF8, "text/plain")
            };
            return Task.FromResult(response);
        });

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/resource");
        request.Headers.TryAddWithoutValidation("X-Test", "1");

        await client.SendAsync(request);

        Assert.Single(handler.Requests);
        Assert.True(handler.Requests[0].Headers.Contains("X-Test"));
    }

    [Fact]
    public void TestCorrelationScope_PushesAndRestoresContext()
    {
        Assert.Null(OntogonyCorrelationContext.Current);

        using (new TestCorrelationScope("trace-pr9", tenantId: "tenant-1"))
        {
            Assert.NotNull(OntogonyCorrelationContext.Current);
            Assert.Equal("trace-pr9", OntogonyCorrelationContext.Current!.TraceId);
            Assert.Equal("tenant-1", OntogonyCorrelationContext.Current!.TenantId);
        }

        Assert.Null(OntogonyCorrelationContext.Current);
    }

    [Fact]
    public void EnvelopeFixtureBuilder_BuildsEnvelopeWithOverrides()
    {
        var envelope = new EnvelopeFixtureBuilder<Payload>()
            .WithEventId("evt-123")
            .WithEventType("agent.run.started")
            .WithProtocol("mcp")
            .WithTraceId("trace-abc")
            .WithMetadata("k1", "v1")
            .WithPayload(new Payload("value"))
            .Build();

        Assert.Equal("evt-123", envelope.EventId);
        Assert.Equal("agent.run.started", envelope.EventType);
        Assert.Equal("mcp", envelope.Protocol);
        Assert.Equal("trace-abc", envelope.TraceId);
        Assert.Equal("v1", envelope.Metadata["k1"]);
    }

    [Fact]
    public void ApiErrorAssertions_ValidateCodeMessageAndTraceId()
    {
        var error = new ApiError("ValidationFailed", "The request is invalid.", "trace-1");

        ApiErrorAssertions.AssertCode(error, "ValidationFailed");
        ApiErrorAssertions.AssertMessageContains(error, "invalid");
        ApiErrorAssertions.AssertTraceId(error, "trace-1");
    }

    [Fact]
    public void CanonicalJsonAssertions_CheckEquivalentAndDifferentPayloads()
    {
        CanonicalJsonAssertions.AssertEquivalentJson(
            "{\"b\":2,\"a\":1}",
            "{\"a\":1,\"b\":2}");

        Assert.Throws<InvalidOperationException>(() =>
            CanonicalJsonAssertions.AssertDifferentJson("{\"a\":1}", "{\"a\":1}"));
    }

    [Fact]
    public async Task MiddlewareTestHarness_CreatesRunsAndAssertsHttpContext()
    {
        var context = MiddlewareTestHarness.CreateHttpContext("POST", "/test");

        await MiddlewareTestHarness.RunAsync(async ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status202Accepted;
            ctx.Response.Headers["X-Result"] = "ok";
            await ctx.Response.WriteAsync("{\"status\":\"accepted\"}");
        }, context);

        Assert.Equal(StatusCodes.Status202Accepted, context.Response.StatusCode);
        MiddlewareTestHarness.AssertResponseHeader(context, "X-Result", "ok");
        MiddlewareTestHarness.AssertResponseJsonContains(context, "status");
    }

    private sealed record Payload(string Value);
}
