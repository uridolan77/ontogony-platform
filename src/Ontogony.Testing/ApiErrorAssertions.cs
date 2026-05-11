using Ontogony.Errors;

namespace Ontogony.Testing;

public static class ApiErrorAssertions
{
    public static void AssertCode(ApiError error, string expectedCode)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (!string.Equals(error.Code, expectedCode, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected error code '{expectedCode}' but got '{error.Code}'.");
        }
    }

    public static void AssertMessageContains(ApiError error, string expectedFragment)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (string.IsNullOrWhiteSpace(expectedFragment))
        {
            throw new ArgumentException("expectedFragment cannot be empty.", nameof(expectedFragment));
        }

        if (error.Message?.Contains(expectedFragment, StringComparison.OrdinalIgnoreCase) != true)
        {
            throw new InvalidOperationException($"Expected error message to contain '{expectedFragment}'.");
        }
    }

    public static void AssertTraceId(ApiError error, string expectedTraceId)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (!string.Equals(error.TraceId, expectedTraceId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected trace id '{expectedTraceId}' but got '{error.TraceId}'.");
        }
    }
}