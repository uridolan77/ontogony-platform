using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http;
using Ontogony.Security;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ServiceIdentityHmacTests
{
    private readonly HttpContextAccessor _contextAccessor = new();

    [Fact]
    public void Current_WithHmac_ValidRequest_Succeeds()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var context = BuildSignedContext(options, "svc", "unit-test-secret", body: "{}");
        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        var actor = accessor.Current;

        Assert.NotNull(actor);
        Assert.Equal("svc", actor!.ActorId);
    }

    [Fact]
    public void Current_WithHmac_InvalidSignature_ReturnsNull()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var context = BuildSignedContext(options, "svc", "unit-test-secret", body: "{}");
        context.Request.Headers[OntogonyServiceIdentityHeaders.Signature] = "AAAA";

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_StaleTimestamp_ReturnsNull()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            MaxTimestampSkew = TimeSpan.FromMinutes(5),
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var stale = DateTimeOffset.UtcNow.AddHours(-2).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var context = BuildSignedContext(options, "svc", "unit-test-secret", body: "{}", timestampUnix: stale);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_MissingNonceWhenRequired_ReturnsNull()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var body = "{}";
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var pathAndQuery = "/events";

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = new PathString("/events");
        context.Request.Body = new MemoryStream(bodyBytes);
        context.Request.ContentLength = bodyBytes.Length;
        context.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "svc";
        context.Request.Headers[OntogonyServiceIdentityHeaders.Timestamp] = ts;
        context.Request.Headers[OntogonyServiceIdentityHeaders.BodyHash] = bodyHash;
        context.Request.Headers[OntogonyServiceIdentityHeaders.Signature] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
                "unit-test-secret", "POST", pathAndQuery, ts, string.Empty, bodyHash);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_ReplayedNonce_ReturnsNull_OnSecondEvaluation()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var context = BuildSignedContext(options, "svc", "unit-test-secret", body: "{}");
        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.NotNull(accessor.Current);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_NonceNotRequired_SucceedsWithoutReplayStore()
    {
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = false,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var body = "{}";
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var pathAndQuery = "/events";

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = new PathString("/events");
        context.Request.Body = new MemoryStream(bodyBytes);
        context.Request.ContentLength = bodyBytes.Length;
        context.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "svc";
        context.Request.Headers[OntogonyServiceIdentityHeaders.Timestamp] = ts;
        context.Request.Headers[OntogonyServiceIdentityHeaders.BodyHash] = bodyHash;
        context.Request.Headers[OntogonyServiceIdentityHeaders.Signature] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
                "unit-test-secret", "POST", pathAndQuery, ts, string.Empty, bodyHash);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: null);
        Assert.NotNull(accessor.Current);
    }

    private static DefaultHttpContext BuildSignedContext(
        ServiceIdentityOptions options,
        string serviceId,
        string secret,
        string body,
        string? timestampUnix = null)
    {
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);
        var ts = timestampUnix ?? DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var nonce = Guid.NewGuid().ToString("n");
        var pathAndQuery = "/events";

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = new PathString("/events");
        context.Request.Body = new MemoryStream(bodyBytes);
        context.Request.ContentLength = bodyBytes.Length;
        context.Request.Headers[options.ServiceIdHeaderName] = serviceId;
        context.Request.Headers[options.ServiceTimestampHeaderName] = ts;
        context.Request.Headers[options.ServiceNonceHeaderName] = nonce;
        context.Request.Headers[options.ServiceBodyHashHeaderName] = bodyHash;
        context.Request.Headers[options.SignatureHeaderName] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(secret, "POST", pathAndQuery, ts, nonce, bodyHash);

        return context;
    }
}
