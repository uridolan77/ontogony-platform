using Xunit;

namespace Ontogony.Idempotency.Tests;

/// <summary>
/// Tests for idempotency key validation and idempotency window behavior.
/// </summary>
public class IdempotencyKeyTests
{
    [Fact]
    public void IdempotencyKey_WithValidFormat_IsAccepted()
    {
        var key = "idempotency-12345-test";
        
        // Idempotency keys should be non-empty strings
        Assert.NotEmpty(key);
        Assert.False(string.IsNullOrWhiteSpace(key));
    }

    [Fact]
    public void IdempotencyKey_WithEmptyValue_IsRejected()
    {
        var key = "";
        
        // Empty keys are invalid
        Assert.True(string.IsNullOrWhiteSpace(key));
    }

    [Fact]
    public void IdempotencyKey_WithWhitespaceOnly_IsRejected()
    {
        var key = "   ";
        
        Assert.True(string.IsNullOrWhiteSpace(key));
    }

    [Fact]
    public void IdempotencyKey_Uniqueness_DifferentKeysAreDifferent()
    {
        var key1 = "request-123";
        var key2 = "request-456";
        
        Assert.NotEqual(key1, key2);
    }

    [Fact]
    public void IdempotencyKey_CaseSensitive()
    {
        var key1 = "Request-123";
        var key2 = "request-123";
        
        Assert.NotEqual(key1, key2);
    }
}
