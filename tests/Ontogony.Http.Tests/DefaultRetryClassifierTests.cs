using Xunit;

namespace Ontogony.Http.Tests;

/// <summary>
/// Tests for <see cref="DefaultRetryClassifier"/>.
/// </summary>
public class DefaultRetryClassifierTests
{
    [Theory]
    [InlineData(408)]  // Request Timeout
    [InlineData(429)]  // Too Many Requests
    [InlineData(500)]  // Internal Server Error
    [InlineData(502)]  // Bad Gateway
    [InlineData(503)]  // Service Unavailable
    [InlineData(504)]  // Gateway Timeout
    public void IsTransient_WithTransientStatusCode_ReturnsTrue(int statusCode)
    {
        var classifier = new DefaultRetryClassifier();
        
        var isTransient = classifier.IsTransient(new HttpStatusCodeException((System.Net.HttpStatusCode)statusCode, "Test"));
        
        Assert.True(isTransient);
    }

    [Theory]
    [InlineData(200)]  // OK
    [InlineData(201)]  // Created
    [InlineData(400)]  // Bad Request
    [InlineData(401)]  // Unauthorized
    [InlineData(403)]  // Forbidden
    [InlineData(404)]  // Not Found

    public void IsTransient_WithNonTransientStatusCode_ReturnsFalse(int statusCode)
    {
        var classifier = new DefaultRetryClassifier();
        
        var isTransient = classifier.IsTransient(new HttpStatusCodeException((System.Net.HttpStatusCode)statusCode, "Test"));
        
        Assert.False(isTransient);
    }

    [Fact]
    public void IsTransient_WithTaskCanceledException_ReturnsTrue()
    {
        var classifier = new DefaultRetryClassifier();
        var ex = new TaskCanceledException("Timeout");
        
        var isTransient = classifier.IsTransient(ex);
        
        Assert.True(isTransient);
    }

    [Fact]
    public void IsTransient_WithOperationCanceledException_ReturnsTrue()
    {
        var classifier = new DefaultRetryClassifier();
        var ex = new OperationCanceledException("Canceled");
        
        var isTransient = classifier.IsTransient(ex);
        
        Assert.True(isTransient);
    }

    [Fact]
    public void IsTransient_WithHttpRequestException_ReturnsFalse()
    {
        var classifier = new DefaultRetryClassifier();
        var ex = new HttpRequestException("Connection error");
        
        var isTransient = classifier.IsTransient(ex);
        
        Assert.False(isTransient);
    }

    [Fact]
    public void IsTransient_WithArbitraryException_ReturnsFalse()
    {
        var classifier = new DefaultRetryClassifier();
        var ex = new InvalidOperationException("Some error");
        
        var isTransient = classifier.IsTransient(ex);
        
        Assert.False(isTransient);
    }
}
