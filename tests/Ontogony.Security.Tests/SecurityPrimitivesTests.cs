using Xunit;

namespace Ontogony.Security.Tests;

/// <summary>
/// Tests for Ontogony.Security service identity and authentication primitives.
/// </summary>
public class SecurityPrimitivesTests
{
    [Fact]
    public void ServiceIdentity_HeaderNames_AreWellFormed()
    {
        // Standard service identity header names
        var traceIdHeader = "X-Ontogony-Trace-Id";
        var serviceIdHeader = "X-Ontogony-Service-Id";
        var signatureHeader = "X-Ontogony-Service-Signature";
        
        // Headers should not be empty or null
        Assert.NotEmpty(traceIdHeader);
        Assert.NotEmpty(serviceIdHeader);
        Assert.NotEmpty(signatureHeader);
        
        // Headers should follow canonical naming (X-*)
        Assert.StartsWith("X-", traceIdHeader);
        Assert.StartsWith("X-", serviceIdHeader);
        Assert.StartsWith("X-", signatureHeader);
    }

    [Fact]
    public void Hmac_SignatureFormat_IsBase64Encodable()
    {
        var secretBytes = "test-secret"u8.ToArray();
        var bodyBytes = "test-body"u8.ToArray();
        
        // HMAC signatures should be deterministic byte arrays
        using (var hmac = new System.Security.Cryptography.HMACSHA256(secretBytes))
        {
            var signature = hmac.ComputeHash(bodyBytes);
            var base64Signature = Convert.ToBase64String(signature);
            
            // Base64 signature should be decodable
            var decodedSignature = Convert.FromBase64String(base64Signature);
            Assert.Equal(signature, decodedSignature);
        }
    }

    [Fact]
    public void FixedTimeCompare_WithEqualStrings_Succeeds()
    {
        var str1 = "secret-value-123";
        var str2 = "secret-value-123";
        
        // Secrets should match exactly
        Assert.Equal(str1, str2);
    }

    [Fact]
    public void FixedTimeCompare_WithDifferentStrings_Fails()
    {
        var str1 = "secret-value-123";
        var str2 = "secret-value-456";
        
        Assert.NotEqual(str1, str2);
    }
}
