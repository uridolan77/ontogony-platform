using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http;
using Ontogony.Security;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyServiceIdentitySigningHandlerTests
{
    [Fact]
    public async Task SendAsync_SetsCanonicalSigningHeaders()
    {
        var clock = new DateTimeOffset(2026, 5, 12, 10, 30, 0, TimeSpan.Zero);
        const string nonce = "nonce-123";
        var terminal = new CaptureHandler();
        var handler = new OntogonyServiceIdentitySigningHandler(
        serviceId: "svc-a",
        secret: "secret-a",
        keyId: "k-current",
        utcNow: () => clock,
        nonceFactory: () => nonce)
        {
            InnerHandler = terminal
        };

        var invoker = new HttpMessageInvoker(handler);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://example.internal/events?x=1")
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };

        _ = await invoker.SendAsync(request, CancellationToken.None);

        Assert.NotNull(terminal.CapturedRequest);
        var captured = terminal.CapturedRequest!;
        AssertHeader(captured, OntogonyServiceIdentityHeaders.ServiceId, "svc-a");
        AssertHeader(captured, OntogonyServiceIdentityHeaders.KeyId, "k-current");

        var timestamp = clock.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        AssertHeader(captured, OntogonyServiceIdentityHeaders.Timestamp, timestamp);
        AssertHeader(captured, OntogonyServiceIdentityHeaders.Nonce, nonce);

        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(Encoding.UTF8.GetBytes("{}"));
        AssertHeader(captured, OntogonyServiceIdentityHeaders.BodyHash, bodyHash);

        var expectedSignature = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            "secret-a",
            "POST",
            "/events?x=1",
            timestamp,
            nonce,
            bodyHash);
        AssertHeader(captured, OntogonyServiceIdentityHeaders.Signature, expectedSignature);
    }

    [Fact]
    public async Task SendAsync_BodyHash_And_Signature_Verify_ServerSide()
    {
        var clock = new DateTimeOffset(2026, 5, 12, 10, 30, 0, TimeSpan.Zero);
        var terminal = new CaptureHandler();
        var handler = new OntogonyServiceIdentitySigningHandler(
            serviceId: "svc-a",
            secret: "secret-a",
            keyId: "k-current",
            utcNow: () => clock,
            nonceFactory: () => "nonce-abc")
        {
            InnerHandler = terminal
        };

        var invoker = new HttpMessageInvoker(handler);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://example.internal/events")
        {
            Content = new StringContent("{\"n\":1}", Encoding.UTF8, "application/json")
        };

        _ = await invoker.SendAsync(request, CancellationToken.None);
        var outbound = terminal.CapturedRequest!;

        var body = await outbound.Content!.ReadAsStringAsync();
        var bodyBytes = Encoding.UTF8.GetBytes(body);

        var serverContext = new DefaultHttpContext();
        serverContext.Request.Method = outbound.Method.Method;
        serverContext.Request.Path = new PathString(outbound.RequestUri!.AbsolutePath);
        serverContext.Request.QueryString = new QueryString(outbound.RequestUri.Query);
        serverContext.Request.Body = new MemoryStream(bodyBytes);
        serverContext.Request.ContentLength = bodyBytes.Length;

        foreach (var header in outbound.Headers)
        {
            serverContext.Request.Headers[header.Key] = header.Value.ToArray();
        }

        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = true,
            RequireKeyIdForHmacSignature = true
        };

        var resolver = new FixedSigningSecretResolver();
        var accessor = new ServiceIdentityCurrentActorAccessor(
            new HttpContextAccessor { HttpContext = serverContext },
            options,
            nonceReplayStore: new InMemoryNonceReplayStore(),
            clock: new FixedClock(clock),
            signingSecretResolver: resolver);

        Assert.NotNull(accessor.Current);
    }

    private static void AssertHeader(HttpRequestMessage request, string name, string expected)
    {
        Assert.True(request.Headers.TryGetValues(name, out var values));
        Assert.Equal(expected, Assert.Single(values));
    }

    private sealed class CaptureHandler : HttpMessageHandler
    {
        public HttpRequestMessage? CapturedRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CapturedRequest = request;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }

    private sealed class FixedSigningSecretResolver : IServiceSigningSecretResolver
    {
        public ServiceSigningSecretSet ResolveSecrets(string serviceId, string? keyId)
        {
            if (!string.Equals(serviceId, "svc-a", StringComparison.Ordinal))
                return ServiceSigningSecretSet.Empty(serviceId, keyId);

            var secrets = new[]
            {
                new ServiceSigningSecret("k-current", "secret-a", IsCurrent: true),
                new ServiceSigningSecret("k-prev", "secret-prev", IsCurrent: false)
            };

            var selected = string.IsNullOrWhiteSpace(keyId)
                ? secrets[0]
                : secrets.FirstOrDefault(s => string.Equals(s.KeyId, keyId, StringComparison.Ordinal));

            return new ServiceSigningSecretSet(serviceId, keyId, selected, secrets);
        }
    }

    private sealed class FixedClock : Ontogony.Primitives.IClock
    {
        private readonly DateTimeOffset _utcNow;

        public FixedClock(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public DateTimeOffset UtcNow => _utcNow;
    }
}
