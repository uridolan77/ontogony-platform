using Ontogony.Hashing;

namespace Ontogony.Testing;

public static class CanonicalJsonAssertions
{
    public static void AssertEquivalentJson(string leftJson, string rightJson)
    {
        var left = CanonicalJson.Normalize(leftJson);
        var right = CanonicalJson.Normalize(rightJson);
        if (!string.Equals(left, right, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("JSON payloads are not canonically equivalent.");
        }
    }

    public static void AssertDifferentJson(string leftJson, string rightJson)
    {
        var left = CanonicalJson.Normalize(leftJson);
        var right = CanonicalJson.Normalize(rightJson);
        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Expected canonical JSON payloads to differ.");
        }
    }
}