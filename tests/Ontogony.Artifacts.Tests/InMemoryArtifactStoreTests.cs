using System.Text;
using Ontogony.Artifacts;
using Xunit;

namespace Ontogony.Artifacts.Tests;

public sealed class InMemoryArtifactStoreTests
{
    [Fact]
    public async Task Put_then_Get_returns_same_bytes_and_metadata()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("hello-artifact");

        var put = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/octet-stream",
            Content = bytes
        });

        Assert.False(put.Existed);
        Assert.Equal(bytes.LongLength, put.Reference.SizeBytes);
        Assert.False(string.IsNullOrWhiteSpace(put.Reference.ArtifactId));
        Assert.False(string.IsNullOrWhiteSpace(put.Reference.ContentHash));

        var got = await store.GetAsync(put.Reference.ArtifactId);
        Assert.Equal(put.Reference, got.Reference);
        Assert.True(bytes.AsSpan().SequenceEqual(got.Bytes.Span));
    }

    [Fact]
    public async Task Put_deduplicates_identical_content_and_scope()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("dedupe-me");

        var first = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            TenantId = "t1",
            Classification = "internal"
        });
        var second = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            TenantId = "t1",
            Classification = "internal"
        });

        Assert.False(first.Existed);
        Assert.True(second.Existed);
        Assert.Equal(first.Reference.ArtifactId, second.Reference.ArtifactId);
        Assert.Equal(first.Reference.ContentHash, second.Reference.ContentHash);
        Assert.Equal(1, store.Count);
    }

    [Fact]
    public async Task Put_does_not_deduplicate_across_tenants()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("same-bytes-different-tenant");

        var t1 = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            TenantId = "tenant-a"
        });
        var t2 = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            TenantId = "tenant-b"
        });

        Assert.NotEqual(t1.Reference.ArtifactId, t2.Reference.ArtifactId);
        Assert.Equal(t1.Reference.ContentHash, t2.Reference.ContentHash);
        Assert.Equal(2, store.Count);
    }

    [Fact]
    public async Task ContentHash_is_deterministic_lowercase_sha256()
    {
        var store = new InMemoryArtifactStore();

        var put = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "text/plain",
            Content = Bytes("abc")
        });

        Assert.Equal(64, put.Reference.ContentHash.Length);
        Assert.Equal(put.Reference.ContentHash, put.Reference.ContentHash.ToLowerInvariant());
        Assert.Equal(
            "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad",
            put.Reference.ContentHash);
    }

    [Fact]
    public async Task SuggestedArtifactId_is_honored_when_unique()
    {
        var store = new InMemoryArtifactStore();
        var put = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = Bytes("{\"k\":1}"),
            SuggestedArtifactId = "user-supplied-id"
        });

        Assert.Equal("user-supplied-id", put.Reference.ArtifactId);
        Assert.True(await store.ExistsAsync("user-supplied-id"));
    }

    [Fact]
    public async Task SuggestedArtifactId_reuse_with_same_content_returns_existed()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("repeatable");

        var first = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            SuggestedArtifactId = "stable-id"
        });
        var second = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            SuggestedArtifactId = "stable-id"
        });

        Assert.False(first.Existed);
        Assert.True(second.Existed);
        Assert.Equal("stable-id", second.Reference.ArtifactId);
    }

    [Fact]
    public async Task SuggestedArtifactId_collision_with_different_content_throws()
    {
        var store = new InMemoryArtifactStore();
        await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = Bytes("payload-a"),
            SuggestedArtifactId = "shared-id"
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() => store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = Bytes("payload-b"),
            SuggestedArtifactId = "shared-id"
        }));
    }

    [Fact]
    public async Task Get_throws_ArtifactNotFoundException_when_missing()
    {
        var store = new InMemoryArtifactStore();
        var ex = await Assert.ThrowsAsync<ArtifactNotFoundException>(() => store.GetAsync("missing"));
        Assert.Equal("missing", ex.ArtifactId);
    }

    [Fact]
    public async Task TryGet_returns_null_when_missing()
    {
        var store = new InMemoryArtifactStore();
        Assert.Null(await store.TryGetAsync("missing"));
    }

    [Fact]
    public async Task GetReference_returns_metadata_without_loading_bytes_buffer()
    {
        var store = new InMemoryArtifactStore();
        var put = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "text/plain",
            Content = Bytes("body")
        });

        var reference = await store.GetReferenceAsync(put.Reference.ArtifactId);
        Assert.NotNull(reference);
        Assert.Equal(put.Reference.ContentHash, reference!.ContentHash);
    }

    [Fact]
    public async Task PutAsync_rejects_missing_media_type()
    {
        var store = new InMemoryArtifactStore();

        await Assert.ThrowsAsync<ArgumentException>(() => store.PutAsync(new ArtifactPutRequest
        {
            MediaType = " ",
            Content = Bytes("x")
        }));
    }

    private static byte[] Bytes(string s) => Encoding.UTF8.GetBytes(s);
}
