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

    [Fact]
    public async Task Same_bytes_with_different_ContentEncoding_do_not_dedupe()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("payload");

        var identity = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            ContentEncoding = "identity"
        });
        var gzip = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            ContentEncoding = "gzip"
        });

        Assert.False(identity.Existed);
        Assert.False(gzip.Existed);
        Assert.NotEqual(identity.Reference.ArtifactId, gzip.Reference.ArtifactId);
        Assert.Equal(identity.Reference.ContentHash, gzip.Reference.ContentHash);
        Assert.Equal(2, store.Count);
    }

    [Fact]
    public async Task StorageTier_and_Uri_are_hint_metadata_and_do_not_break_dedupe()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("hint-payload");

        var first = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            StorageTier = "inline",
            Uri = "ontogony://artifacts/first"
        });
        var second = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/json",
            Content = bytes,
            StorageTier = "remote",
            Uri = "ontogony://artifacts/second"
        });

        Assert.False(first.Existed);
        Assert.True(second.Existed);
        Assert.Equal(first.Reference.ArtifactId, second.Reference.ArtifactId);
        Assert.Equal(first.Reference.StorageTier, second.Reference.StorageTier);
        Assert.Equal(first.Reference.Uri, second.Reference.Uri);
        Assert.Equal(1, store.Count);
    }

    [Fact]
    public async Task Stream_put_computes_same_hash_as_buffer_put()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("stream-equivalence");

        var bufferPut = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/octet-stream",
            Content = bytes,
            TenantId = "tenant-buffer"
        });

        using var stream = new MemoryStream(bytes);
        var streamPut = await store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "application/octet-stream",
            ContentStream = stream,
            TenantId = "tenant-stream"
        });

        Assert.Equal(bufferPut.Reference.ContentHash, streamPut.Reference.ContentHash);
        Assert.Equal(bufferPut.Reference.SizeBytes, streamPut.Reference.SizeBytes);
    }

    [Fact]
    public async Task Stream_put_then_get_roundtrips_bytes()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("stream-roundtrip");

        using var stream = new MemoryStream(bytes);
        var put = await store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "text/plain",
            ContentStream = stream
        });

        var got = await store.GetAsync(put.Reference.ArtifactId);
        Assert.True(bytes.AsSpan().SequenceEqual(got.Bytes.Span));
    }

    [Fact]
    public async Task Stream_put_dedupes_same_bytes_within_scope()
    {
        var store = new InMemoryArtifactStore();
        var bytes = Bytes("stream-dedupe");

        using var s1 = new MemoryStream(bytes);
        var first = await store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "application/json",
            ContentStream = s1,
            TenantId = "t1"
        });

        using var s2 = new MemoryStream(bytes);
        var second = await store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "application/json",
            ContentStream = s2,
            TenantId = "t1"
        });

        Assert.False(first.Existed);
        Assert.True(second.Existed);
        Assert.Equal(first.Reference.ArtifactId, second.Reference.ArtifactId);
    }

    [Fact]
    public async Task Stream_put_rejects_when_ExpectedSizeBytes_mismatches()
    {
        var store = new InMemoryArtifactStore();
        using var stream = new MemoryStream(Bytes("abcd"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "text/plain",
            ContentStream = stream,
            ExpectedSizeBytes = 99
        }));
    }

    [Fact]
    public async Task Stream_put_rejects_when_ExpectedContentHash_mismatches()
    {
        var store = new InMemoryArtifactStore();
        using var stream = new MemoryStream(Bytes("payload"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "text/plain",
            ContentStream = stream,
            ExpectedContentHash = "0000000000000000000000000000000000000000000000000000000000000000"
        }));
    }

    [Fact]
    public async Task Stream_put_accepts_matching_ExpectedContentHash()
    {
        var store = new InMemoryArtifactStore();
        const string knownSha256OfAbc = "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad";
        using var stream = new MemoryStream(Bytes("abc"));

        var put = await store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = "text/plain",
            ContentStream = stream,
            ExpectedSizeBytes = 3,
            ExpectedContentHash = knownSha256OfAbc
        });

        Assert.Equal(knownSha256OfAbc, put.Reference.ContentHash);
    }

    [Fact]
    public async Task Stream_put_rejects_missing_media_type()
    {
        var store = new InMemoryArtifactStore();
        using var stream = new MemoryStream(Bytes("x"));

        await Assert.ThrowsAsync<ArgumentException>(() => store.PutAsync(new ArtifactStreamPutRequest
        {
            MediaType = " ",
            ContentStream = stream
        }));
    }

    [Fact]
    public async Task Returned_bytes_are_defensive_copies_and_cannot_mutate_store()
    {
        var store = new InMemoryArtifactStore();
        var original = Bytes("immutable");
        var put = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/octet-stream",
            Content = original
        });

        var firstGet = await store.GetAsync(put.Reference.ArtifactId);

        if (System.Runtime.InteropServices.MemoryMarshal.TryGetArray(firstGet.Bytes, out var segment)
            && segment.Array is { } array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = 0xFF;
            }
        }

        var secondGet = await store.GetAsync(put.Reference.ArtifactId);
        Assert.True(original.AsSpan().SequenceEqual(secondGet.Bytes.Span));
    }

    [Fact]
    public async Task Caller_mutating_input_buffer_after_put_does_not_change_stored_bytes()
    {
        var store = new InMemoryArtifactStore();
        var input = Bytes("snapshot");
        var snapshot = (byte[])input.Clone();

        var put = await store.PutAsync(new ArtifactPutRequest
        {
            MediaType = "application/octet-stream",
            Content = input
        });

        for (var i = 0; i < input.Length; i++)
        {
            input[i] = 0;
        }

        var got = await store.GetAsync(put.Reference.ArtifactId);
        Assert.True(snapshot.AsSpan().SequenceEqual(got.Bytes.Span));
    }

    private static byte[] Bytes(string s) => Encoding.UTF8.GetBytes(s);
}
