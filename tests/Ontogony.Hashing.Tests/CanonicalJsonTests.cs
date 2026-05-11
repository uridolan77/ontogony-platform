using Ontogony.Hashing;
using Xunit;

namespace Ontogony.Hashing.Tests;

public sealed class CanonicalJsonTests
{
    [Fact]
    public void Normalize_Sorts_Object_Keys_Recursively()
    {
        var a = CanonicalJson.Normalize("{\"b\":2,\"a\":{\"d\":4,\"c\":3}}");
        var b = CanonicalJson.Normalize("{\"a\":{\"c\":3,\"d\":4},\"b\":2}");

        Assert.Equal(b, a);
    }

    [Fact]
    public void PayloadHasher_Gives_Same_Hash_For_Equivalent_Json()
    {
        var hasher = new PayloadHasher(new Sha256ContentHashService());
        var a = hasher.ComputeCanonicalJsonHashFromJson("{\"b\":2,\"a\":1}");
        var b = hasher.ComputeCanonicalJsonHashFromJson("{\"a\":1,\"b\":2}");

        Assert.Equal(a, b);
    }
}
