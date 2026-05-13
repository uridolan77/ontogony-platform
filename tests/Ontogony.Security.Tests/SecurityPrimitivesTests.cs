using Ontogony.Security;
using Xunit;

namespace Ontogony.Security.Tests;

/// <summary>
/// Tests for Ontogony.Security service identity and replay-protection primitives.
/// </summary>
public class SecurityPrimitivesTests
{
    [Fact]
    public void ExtractBearerToken_WithValidAuthorizationHeader_ReturnsToken()
    {
        var token = OntogonySecurityHeaders.ExtractBearerToken("Bearer abc.def.ghi");

        Assert.Equal("abc.def.ghi", token);
    }

    [Fact]
    public void ExtractBearerToken_WithMalformedAuthorizationHeader_ReturnsNull()
    {
        Assert.Null(OntogonySecurityHeaders.ExtractBearerToken("Basic abc123"));
        Assert.Null(OntogonySecurityHeaders.ExtractBearerToken("Bearer   "));
    }

    [Fact]
    public void ServiceIdentityHmacSignatureHelper_BuildsExpectedCanonicalString()
    {
        var canonical = ServiceIdentityHmacSignatureHelper.BuildCanonical(
            "POST",
            "/v1/messages?tenant=a",
            "1715560000",
            "nonce-123",
            "deadbeef");

        Assert.Equal("POST\n/v1/messages?tenant=a\n1715560000\nnonce-123\ndeadbeef", canonical);
    }

    [Fact]
    public void ServiceIdentityHmacSignatureHelper_ComputeSignatureBase64_IsDeterministic()
    {
        var signature1 = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            "top-secret",
            "GET",
            "/health",
            "1715560000",
            "nonce-1",
            "abc123");

        var signature2 = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            "top-secret",
            "GET",
            "/health",
            "1715560000",
            "nonce-1",
            "abc123");

        Assert.Equal(signature1, signature2);
        Assert.NotEmpty(Convert.FromBase64String(signature1));
    }

    [Fact]
    public void InMemoryNonceReplayStore_RejectsDuplicateNonceWithinRetentionWindow()
    {
        var now = new DateTimeOffset(2026, 05, 13, 12, 00, 00, TimeSpan.Zero);
        var store = new InMemoryNonceReplayStore(
            new InMemoryNonceReplayStoreOptions { NonceRetention = TimeSpan.FromMinutes(5), MaxStoredNonces = 10 },
            () => now);

        var first = store.TryReserveNonce("svc-a", "nonce-1", now);
        var second = store.TryReserveNonce("svc-a", "nonce-1", now.AddMinutes(1));

        Assert.True(first);
        Assert.False(second);
    }

    [Fact]
    public void InMemoryNonceReplayStore_AllowsNonceAfterRetentionEviction()
    {
        var current = new DateTimeOffset(2026, 05, 13, 12, 00, 00, TimeSpan.Zero);
        var store = new InMemoryNonceReplayStore(
            new InMemoryNonceReplayStoreOptions { NonceRetention = TimeSpan.FromMinutes(5), MaxStoredNonces = 10 },
            () => current);

        Assert.True(store.TryReserveNonce("svc-a", "nonce-1", current));

        current = current.AddMinutes(6);

        Assert.True(store.TryReserveNonce("svc-a", "nonce-1", current));
    }

    [Fact]
    public void ServiceIdentityHeaders_ExposeExpectedCanonicalNames()
    {
        Assert.Equal("X-Ontogony-Service-Id", OntogonyServiceIdentityHeaders.ServiceId);
        Assert.Equal("X-Ontogony-Service-Signature", OntogonyServiceIdentityHeaders.Signature);
        Assert.Equal("X-Ontogony-Service-Nonce", OntogonyServiceIdentityHeaders.Nonce);
    }
}
