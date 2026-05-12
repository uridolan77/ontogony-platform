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

    [Fact]
    public void Current_WithHmac_BodyExceedsMaxSignedBytes_ReturnsNull()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            MaxSignedBodyBytes = 4,
            AllowUnsignedEmptyBody = false,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var body = "12345";
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var nonce = Guid.NewGuid().ToString("n");
        var pathAndQuery = "/events";

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = new PathString("/events");
        context.Request.Body = new MemoryStream(bodyBytes);
        context.Request.ContentLength = bodyBytes.Length;
        context.Request.Headers[options.ServiceIdHeaderName] = "svc";
        context.Request.Headers[options.ServiceTimestampHeaderName] = ts;
        context.Request.Headers[options.ServiceNonceHeaderName] = nonce;
        context.Request.Headers[options.ServiceBodyHashHeaderName] = bodyHash;
        context.Request.Headers[options.SignatureHeaderName] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
                "unit-test-secret", "POST", pathAndQuery, ts, nonce, bodyHash);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_MissingBodyHash_OnPost_ReturnsNull()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            AllowUnsignedEmptyBody = false,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var body = "{}";
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var nonce = Guid.NewGuid().ToString("n");
        var pathAndQuery = "/events";
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = new PathString("/events");
        context.Request.Body = new MemoryStream(bodyBytes);
        context.Request.ContentLength = bodyBytes.Length;
        context.Request.Headers[options.ServiceIdHeaderName] = "svc";
        context.Request.Headers[options.ServiceTimestampHeaderName] = ts;
        context.Request.Headers[options.ServiceNonceHeaderName] = nonce;
        context.Request.Headers[options.SignatureHeaderName] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
                "unit-test-secret", "POST", pathAndQuery, ts, nonce, bodyHash);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_GetWithoutBodyHash_AllowUnsignedEmpty_Succeeds()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            AllowUnsignedEmptyBody = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var emptyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(ReadOnlySpan<byte>.Empty);
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var nonce = Guid.NewGuid().ToString("n");
        var pathAndQuery = "/events";

        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = new PathString("/events");
        context.Request.Body = Stream.Null;
        context.Request.Headers[options.ServiceIdHeaderName] = "svc";
        context.Request.Headers[options.ServiceTimestampHeaderName] = ts;
        context.Request.Headers[options.ServiceNonceHeaderName] = nonce;
        context.Request.Headers[options.SignatureHeaderName] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
                "unit-test-secret", "GET", pathAndQuery, ts, nonce, emptyHash);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.NotNull(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_RequirePreloadedBodyHash_WithoutPreload_ReturnsNull()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = true,
            RequirePreloadedBodyHashForHmacBodies = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var context = BuildSignedContext(options, "svc", "unit-test-secret", body: "{}");
        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.Null(accessor.Current);
    }

    [Fact]
    public void Current_WithHmac_RequirePreloadedBodyHash_WithPrecomputed_Succeeds()
    {
        var nonces = new InMemoryNonceReplayStore();
        var options = new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            RequireNonce = true,
            RequirePreloadedBodyHashForHmacBodies = true,
            ServiceSecrets = { ["svc"] = "unit-test-secret" }
        };

        var body = "{}";
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var bodyHash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var nonce = Guid.NewGuid().ToString("n");
        var pathAndQuery = "/events";

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = new PathString("/events");
        context.Request.Body = new MemoryStream(bodyBytes);
        context.Request.ContentLength = bodyBytes.Length;
        context.Request.Headers[options.ServiceIdHeaderName] = "svc";
        context.Request.Headers[options.ServiceTimestampHeaderName] = ts;
        context.Request.Headers[options.ServiceNonceHeaderName] = nonce;
        context.Request.Headers[options.ServiceBodyHashHeaderName] = bodyHash;
        context.Request.Headers[options.SignatureHeaderName] =
            ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
                "unit-test-secret", "POST", pathAndQuery, ts, nonce, bodyHash);
        context.Items[ServiceIdentityBodyHashContext.HttpContextItemKey] =
            new ServiceIdentityBodyHashContext.Precomputed(TooLarge: false, HexLower: bodyHash);

        _contextAccessor.HttpContext = context;

        var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options, nonceReplayStore: nonces);
        Assert.NotNull(accessor.Current);
    }

    [Fact]
    public void InMemoryNonceReplayStore_SameNonceAfterRetention_AllowsReuse()
    {
        var t = new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset Clock() => t;
        var store = new InMemoryNonceReplayStore(
            new InMemoryNonceReplayStoreOptions { NonceRetention = TimeSpan.FromMinutes(1) },
            Clock);

        Assert.True(store.TryReserveNonce("svc", "n1", t));
        Assert.False(store.TryReserveNonce("svc", "n1", t));

        t = t.AddMinutes(2);
        Assert.True(store.TryReserveNonce("svc", "n1", t));
    }

    [Fact]
    public void InMemoryNonceReplayStore_MaxStoredNonces_EvictsOldest()
    {
        var t0 = DateTimeOffset.Parse("2026-05-01T00:00:00Z", CultureInfo.InvariantCulture);
        var store = new InMemoryNonceReplayStore(new InMemoryNonceReplayStoreOptions
        {
            MaxStoredNonces = 3,
            NonceRetention = TimeSpan.FromDays(30)
        });

        Assert.True(store.TryReserveNonce("svc", "a", t0));
        Assert.True(store.TryReserveNonce("svc", "b", t0.AddMinutes(1)));
        Assert.True(store.TryReserveNonce("svc", "c", t0.AddMinutes(2)));
        Assert.True(store.TryReserveNonce("svc", "d", t0.AddMinutes(3)));
        Assert.True(store.TryReserveNonce("svc", "a", t0.AddMinutes(4)));
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
