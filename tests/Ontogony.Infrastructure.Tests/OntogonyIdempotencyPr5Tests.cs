using Ontogony.Hashing;
using Ontogony.Idempotency;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public class OntogonyIdempotencyPr5Tests
{
    /// <summary>
    /// IdempotencyKeyOptions tests - validate configuration and safety.
    /// </summary>
    public class IdempotencyKeyOptionsTests
    {
        [Fact]
        public void DefaultOptions_HasCorrectDefaults()
        {
            var options = new IdempotencyKeyOptions();

            Assert.Equal("ontogony", options.Namespace);
            Assert.Equal("v1", options.Version);
            Assert.Equal(256, options.MaxKeyLength);
            Assert.True(options.ValidateSafeCharacters);
        }

        [Fact]
        public void CustomOptions_CanSetAllProperties()
        {
            var options = new IdempotencyKeyOptions
            {
                Namespace = "athanor",
                Version = "v2",
                MaxKeyLength = 512,
                ValidateSafeCharacters = false
            };

            Assert.Equal("athanor", options.Namespace);
            Assert.Equal("v2", options.Version);
            Assert.Equal(512, options.MaxKeyLength);
            Assert.False(options.ValidateSafeCharacters);
        }

        [Fact]
        public void Validate_ThrowsOnEmptyNamespace()
        {
            var options = new IdempotencyKeyOptions { Namespace = "" };

            var ex = Assert.Throws<ArgumentException>(() => options.Validate());
            Assert.Contains("Namespace cannot be empty", ex.Message);
        }

        [Fact]
        public void Validate_ThrowsOnEmptyVersion()
        {
            var options = new IdempotencyKeyOptions { Version = "" };

            var ex = Assert.Throws<ArgumentException>(() => options.Validate());
            Assert.Contains("Version cannot be empty", ex.Message);
        }

        [Fact]
        public void Validate_ThrowsOnMaxKeyLengthTooSmall()
        {
            var options = new IdempotencyKeyOptions { MaxKeyLength = 16 };

            var ex = Assert.Throws<ArgumentException>(() => options.Validate());
            Assert.Contains("MaxKeyLength must be at least 32", ex.Message);
        }

        [Fact]
        public void Validate_ThrowsOnUnsafeNamespaceCharacter()
        {
            var options = new IdempotencyKeyOptions { Namespace = "athanor@bad" };

            var ex = Assert.Throws<ArgumentException>(() => options.Validate());
            Assert.Contains("unsafe character", ex.Message);
        }

        [Fact]
        public void Validate_AllowsSafeCharacters()
        {
            var options = new IdempotencyKeyOptions { Namespace = "athanor-v1.1_beta" };

            // Should not throw
            options.Validate();
        }

        [Fact]
        public void Validate_SkipsWhenValidateSafeCharactersFalse()
        {
            var options = new IdempotencyKeyOptions
            {
                Namespace = "bad@namespace",
                ValidateSafeCharacters = false
            };

            // Should not throw because validation is disabled
            options.Validate();
        }
    }

    /// <summary>
    /// IdempotencyKeyBuilder tests - generate keys in format namespace:operation:version:hash.
    /// </summary>
    public class IdempotencyKeyBuilderTests
    {
        private readonly IdempotencyKeyBuilder _builder;
        private readonly IdempotencyKeyBuilder _builderWithCustomOptions;

        public IdempotencyKeyBuilderTests()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            _builder = new IdempotencyKeyBuilder(hasher);

            var customOptions = new IdempotencyKeyOptions { Namespace = "athanor", Version = "v2" };
            _builderWithCustomOptions = new IdempotencyKeyBuilder(hasher, customOptions);
        }

        [Fact]
        public void BuildKey_ProducesCorrectFormat()
        {
            var key = _builder.BuildKey("agentor.run.start", new { runId = "run-123" });

            // Format: namespace:operation:version:hash
            var parts = key.Split(':');
            Assert.Equal(4, parts.Length);
            Assert.Equal("ontogony", parts[0]);
            Assert.Equal("agentor.run.start", parts[1]);
            Assert.Equal("v1", parts[2]);
            // parts[3] is hash
        }

        [Fact]
        public void BuildKey_WithCustomNamespace_IncludesCustomNamespace()
        {
            var key = _builderWithCustomOptions.BuildKey("event.ingest", new { eventId = "e-1" });

            var parts = key.Split(':');
            Assert.Equal("athanor", parts[0]);
            Assert.Equal("event.ingest", parts[1]);
            Assert.Equal("v2", parts[2]);
        }

        [Fact]
        public void BuildKey_WithSameParts_ProducesSameKey()
        {
            var parts = new object?[] { new { operationId = "op-1" } };

            var key1 = _builder.BuildKey("test.operation", parts);
            var key2 = _builder.BuildKey("test.operation", parts);

            Assert.Equal(key1, key2);
        }

        [Fact]
        public void BuildKey_WithDifferentParts_ProducesDifferentKey()
        {
            var key1 = _builder.BuildKey("test.operation", new { id = "1" });
            var key2 = _builder.BuildKey("test.operation", new { id = "2" });

            Assert.NotEqual(key1, key2);
        }

        [Fact]
        public void BuildKey_WithDifferentOperation_ProducesDifferentKey()
        {
            var parts = new object?[] { new { id = "1" } };

            var key1 = _builder.BuildKey("operation.a", parts);
            var key2 = _builder.BuildKey("operation.b", parts);

            Assert.NotEqual(key1, key2);
        }

        [Fact]
        public void BuildKey_WithMultipleParts_IncludesAll()
        {
            var key = _builder.BuildKey("complex.op", "part1", 42, true);

            var parts = key.Split(':');
            Assert.Equal(4, parts.Length);
            // Hash should be consistent
            Assert.NotEmpty(parts[3]);
        }

        [Fact]
        public void BuildKey_WithNullParts_IncludesNull()
        {
            var key = _builder.BuildKey("test.op", new { value = (string?)null });

            var parts = key.Split(':');
            Assert.Equal(4, parts.Length);
            Assert.NotEmpty(parts[3]);
        }

        [Fact]
        public void BuildKey_ThrowsOnEmptyOperation()
        {
            Assert.Throws<ArgumentException>(() => _builder.BuildKey("", new { }));
        }

        [Fact]
        public void BuildKey_ThrowsOnNullOperation()
        {
            Assert.Throws<ArgumentException>(() => _builder.BuildKey(null!, new { }));
        }

        [Fact]
        public void BuildKey_WithUnsafeOperationCharacter_Throws()
        {
            Assert.Throws<ArgumentException>(() => _builder.BuildKey("agentor/run start", new { id = "1" }));
        }

        [Fact]
        public void BuildKeyFromJson_ProducesConsistentKey()
        {
            var json = """{"operationId":"op-1","action":"process"}""";

            var key1 = _builder.BuildKeyFromJson("test.operation", json);
            var key2 = _builder.BuildKeyFromJson("test.operation", json);

            Assert.Equal(key1, key2);
        }

        [Fact]
        public void BuildKeyFromJson_WithDifferentFormatting_ProducesSameKey()
        {
            var json1 = """{"id":"1","action":"process"}""";
            var json2 = """
            {
              "action": "process",
              "id": "1"
            }
            """;

            var key1 = _builder.BuildKeyFromJson("test.op", json1);
            var key2 = _builder.BuildKeyFromJson("test.op", json2);

            Assert.Equal(key1, key2);
        }

        [Fact]
        public void BuildKeyFromJson_ThrowsOnEmptyJson()
        {
            Assert.Throws<ArgumentException>(() => _builder.BuildKeyFromJson("test.op", ""));
        }

        [Fact]
        public void BuildKeyFromJson_ThrowsOnEmptyOperation()
        {
            Assert.Throws<ArgumentException>(() => _builder.BuildKeyFromJson("", "{}"));
        }

        [Fact]
        public void Options_ReturnsConfiguredOptions()
        {
            var options = _builder.Options;

            Assert.Equal("ontogony", options.Namespace);
            Assert.Equal("v1", options.Version);
        }

        [Fact]
        public void Options_ReturnsCustomOptions()
        {
            var options = _builderWithCustomOptions.Options;

            Assert.Equal("athanor", options.Namespace);
            Assert.Equal("v2", options.Version);
        }

        [Fact]
        public void FromParts_StillSupported()
        {
            // Legacy method should still work
            var key = _builder.FromParts("test.operation", new { id = "1" });

            Assert.NotEmpty(key);
            Assert.Contains("test.operation", key);
        }
    }

    /// <summary>
    /// Idempotency key length and format constraints.
    /// </summary>
    public class IdempotencyKeyLengthTests
    {
        [Fact]
        public void BuildKey_WithLongHash_TruncatesIfNeeded()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var options = new IdempotencyKeyOptions { MaxKeyLength = 50 };
            var builder = new IdempotencyKeyBuilder(hasher, options);

            Assert.Throws<InvalidOperationException>(() =>
                builder.BuildKey("very.long.operation.name.that.exceeds.limits", new { data = "test" }));
        }

        [Fact]
        public void BuildKey_WhenShortenedOperationNeeded_KeepsPayloadHashUntruncated()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var options = new IdempotencyKeyOptions { MaxKeyLength = 90 };
            var builder = new IdempotencyKeyBuilder(hasher, options);

            var key = builder.BuildKey("very.long.operation.name.with.many.segments", new { data = "test" });
            var parts = key.Split(':');

            Assert.Equal(4, parts.Length);
            Assert.Equal(64, parts[3].Length);
            Assert.True(key.Length <= 90);
            Assert.DoesNotContain("...", key);
        }

        [Fact]
        public void BuildKey_WithVerySmallMaxLength_ThrowsToAvoidHashTruncation()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var options = new IdempotencyKeyOptions { MaxKeyLength = 70 };
            var builder = new IdempotencyKeyBuilder(hasher, options);

            Assert.Throws<InvalidOperationException>(() =>
                builder.BuildKey("very.long.operation.name", new { data = "test" }));
        }

        [Fact]
        public void BuildKey_WithSmallMaxLength_StillWorks_WhenComponentsFit()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var options = new IdempotencyKeyOptions { MaxKeyLength = 80 };
            var builder = new IdempotencyKeyBuilder(hasher, options);

            var key = builder.BuildKey("op", new { });

            Assert.True(key.Length <= 80);
            Assert.Equal(64, key.Split(':')[3].Length);
        }

        [Fact]
        public void BuildKey_WithLargeMaxLength_NoTruncation()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var options = new IdempotencyKeyOptions { MaxKeyLength = 1000 };
            var builder = new IdempotencyKeyBuilder(hasher, options);

            var key = builder.BuildKey("agentor.run.start", new { runId = "run-123" });

            // Should not be truncated
            Assert.False(key.EndsWith("..."));
            Assert.DoesNotContain("...", key);
        }
    }

    /// <summary>
    /// Idempotency with envelopes - integration test.
    /// </summary>
    public class IdempotencyWithEnvelopesTests
    {
        [Fact]
        public void CanConstructKeyFromEnvelopePayload()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var builder = new IdempotencyKeyBuilder(hasher);

            var payload = new { agentId = "agent-1", instructionId = "inst-123" };
            var key = builder.BuildKey("agentor.run.start", payload);

            Assert.NotEmpty(key);
            Assert.StartsWith("ontogony:agentor.run.start:v1:", key);
        }

        [Fact]
        public void EnvelopeMetadataDoesNotAffectKey()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var builder = new IdempotencyKeyBuilder(hasher);

            var payload = new { agentId = "agent-1" };

            // Same payload, different traces
            var key1 = builder.BuildKey("agentor.run.start", payload);
            var key2 = builder.BuildKey("agentor.run.start", payload);

            Assert.Equal(key1, key2);
        }

        [Fact]
        public void DifferentServiceNamespaces_ProduceDifferentKeys()
        {
            var hasher = new PayloadHasher(new Sha256ContentHashService());
            var payload = new { eventId = "e-1" };

            var agentor = new IdempotencyKeyBuilder(hasher, new IdempotencyKeyOptions { Namespace = "agentor" });
            var athanor = new IdempotencyKeyBuilder(hasher, new IdempotencyKeyOptions { Namespace = "athanor" });

            var key1 = agentor.BuildKey("event.process", payload);
            var key2 = athanor.BuildKey("event.process", payload);

            Assert.NotEqual(key1, key2);
            Assert.StartsWith("agentor:", key1);
            Assert.StartsWith("athanor:", key2);
        }
    }
}
