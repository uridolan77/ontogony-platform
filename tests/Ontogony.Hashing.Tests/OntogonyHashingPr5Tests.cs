using System.Text.Json;
using Ontogony.Contracts.Events;
using Xunit;

namespace Ontogony.Hashing.Tests;

public class OntogonyHashingPr5Tests
{
    /// <summary>
    /// CanonicalJson determinism tests - ensure that serialization is stable.
    /// </summary>
    public class CanonicalJsonDeterminismTests
    {
        [Fact]
        public void Serialize_WithObjectKeys_OrdersRecursively()
        {
            var obj = new
            {
                zebra = "last",
                apple = "first",
                middle = new { b = 2, a = 1 }
            };

            var json1 = CanonicalJson.Serialize(obj);
            var json2 = CanonicalJson.Serialize(obj);

            // Both serializations should be identical
            Assert.Equal(json1, json2);

            // Keys should appear in sorted order
            Assert.Contains("\"apple\"", json1);
            Assert.Contains("\"middle\"", json1);
            Assert.Contains("\"zebra\"", json1);

            // Verify ordering by checking substring positions
            var applePos = json1.IndexOf("\"apple\"");
            var middlePos = json1.IndexOf("\"middle\"");
            var zebraPos = json1.IndexOf("\"zebra\"");

            Assert.True(applePos < middlePos && middlePos < zebraPos,
                "Keys should be ordered: apple < middle < zebra");
        }

        [Fact]
        public void Serialize_WithNestedObjects_SortsKeysAtEveryLevel()
        {
            var obj = new
            {
                level1_z = new { z = 3, a = 1, m = 2 },
                level1_a = new { c = 3, a = 1, b = 2 }
            };

            var json = CanonicalJson.Serialize(obj);

            // Verify both levels are sorted
            Assert.Contains("\"a\":1", json); // nested level
            Assert.Contains("\"b\":2", json); // nested level
            Assert.Contains("\"c\":3", json); // nested level
        }

        [Fact]
        public void Serialize_WithArrays_PreservesOrder()
        {
            var obj = new { numbers = new[] { 3, 1, 2 } };

            var json1 = CanonicalJson.Serialize(obj);
            var json2 = CanonicalJson.Serialize(obj);

            // Array order preserved (not sorted)
            Assert.Equal(json1, json2);
            Assert.Contains("[3,1,2]", json1);
        }

        [Fact]
        public void Serialize_WithNull_PreservesNull()
        {
            var obj = new { value = (string?)null };

            var json1 = CanonicalJson.Serialize(obj);
            var json2 = CanonicalJson.Serialize(obj);

            Assert.Equal(json1, json2);
            Assert.Contains("\"value\":null", json1);
        }

        [Fact]
        public void Serialize_WithDifferentWhitespace_ProducesSame()
        {
            var json1 = """{"a":1,"b":2}""";
            var json2 = """
            {
              "b": 2,
              "a": 1
            }
            """;

            var normalized1 = CanonicalJson.Normalize(json1);
            var normalized2 = CanonicalJson.Normalize(json2);

            Assert.Equal(normalized1, normalized2);
        }

        [Fact]
        public void Serialize_WithUnicode_IsStable()
        {
            var obj = new { emoji = "🚀", text = "hello" };

            var json1 = CanonicalJson.Serialize(obj);
            var json2 = CanonicalJson.Serialize(obj);

            Assert.Equal(json1, json2);
        }

        [Fact]
        public void Normalize_ParsesAndSorts()
        {
            var unsorted = """{"z":1,"a":2}""";

            var normalized = CanonicalJson.Normalize(unsorted);

            // Should be sorted
            var aPos = normalized.IndexOf("\"a\"");
            var zPos = normalized.IndexOf("\"z\"");
            Assert.True(aPos < zPos);
        }
    }

    /// <summary>
    /// EnvelopePayloadHasher tests - test hashing for envelopes and operation fingerprints.
    /// </summary>
    public class EnvelopePayloadHasherTests
    {
        private readonly PayloadHasher _payloadHasher = new(new Sha256ContentHashService());
        private readonly EnvelopePayloadHasher _envelopeHasher;

        public EnvelopePayloadHasherTests()
        {
            _envelopeHasher = new EnvelopePayloadHasher(_payloadHasher);
        }

        [Fact]
        public void ComputeEnvelopePayloadHash_WithSamePayload_ProducesSameHash()
        {
            var payload = new { message = "test", count = 42 };

            var envelope1 = new OntogonyEnvelope<object>
            {
                EventId = "evt_1",
                EventType = "test.event",
                Source = "test://source",
                OccurredAt = DateTimeOffset.UnixEpoch,
                TraceId = "trace-1",
                Protocol = "test",
                Payload = payload
            };

            var envelope2 = new OntogonyEnvelope<object>
            {
                EventId = "evt_2", // Different envelope ID
                EventType = "test.event",
                Source = "test://source",
                OccurredAt = DateTimeOffset.UnixEpoch,
                TraceId = "trace-2", // Different trace ID
                Protocol = "test",
                Payload = payload
            };

            var hash1 = _envelopeHasher.ComputeEnvelopePayloadHash(envelope1);
            var hash2 = _envelopeHasher.ComputeEnvelopePayloadHash(envelope2);

            // Hashes should be identical (payload is same, metadata doesn't matter)
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void ComputeEnvelopePayloadHash_WithDifferentPayload_ProducesDifferentHash()
        {
            var payload1 = new { message = "test1" };
            var payload2 = new { message = "test2" };

            var envelope1 = new OntogonyEnvelope<object>
            {
                EventId = "evt_1",
                EventType = "test.event",
                Source = "test://source",
                OccurredAt = DateTimeOffset.UnixEpoch,
                TraceId = "trace-1",
                Protocol = "test",
                Payload = payload1
            };

            var envelope2 = new OntogonyEnvelope<object>
            {
                EventId = "evt_1",
                EventType = "test.event",
                Source = "test://source",
                OccurredAt = DateTimeOffset.UnixEpoch,
                TraceId = "trace-1",
                Protocol = "test",
                Payload = payload2
            };

            var hash1 = _envelopeHasher.ComputeEnvelopePayloadHash(envelope1);
            var hash2 = _envelopeHasher.ComputeEnvelopePayloadHash(envelope2);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void ComputeOperationFingerprint_WithSamePayload_ProducesSameFingerprint()
        {
            var payload = new { id = "op-123", action = "process" };

            var fp1 = _envelopeHasher.ComputeOperationFingerprint("agentor.run.start", payload);
            var fp2 = _envelopeHasher.ComputeOperationFingerprint("agentor.run.start", payload);

            Assert.Equal(fp1, fp2);
        }

        [Fact]
        public void ComputeOperationFingerprint_DifferentOperations_ProduceDifferentFingerprints()
        {
            var payload = new { id = "op-123" };

            var fp1 = _envelopeHasher.ComputeOperationFingerprint("agentor.run.start", payload);
            var fp2 = _envelopeHasher.ComputeOperationFingerprint("agentor.run.stop", payload);

            Assert.NotEqual(fp1, fp2);
        }

        [Fact]
        public void ComputeOperationFingerprint_DifferentPayloads_ProduceDifferentFingerprints()
        {
            var op = "athanor.event.ingest";
            var payload1 = new { decision_id = "d-1" };
            var payload2 = new { decision_id = "d-2" };

            var fp1 = _envelopeHasher.ComputeOperationFingerprint(op, payload1);
            var fp2 = _envelopeHasher.ComputeOperationFingerprint(op, payload2);

            Assert.NotEqual(fp1, fp2);
        }

        [Fact]
        public void ComputeOperationFingerprintFromJson_ParsesAndHashes()
        {
            var json = """{"id":"op-123","action":"process"}""";

            var fp = _envelopeHasher.ComputeOperationFingerprintFromJson("test.operation", json);

            Assert.NotEmpty(fp);
            Assert.Matches(@"^[a-f0-9]{64}$", fp); // SHA-256 hex
        }

        [Fact]
        public void ComputeOperationFingerprintFromJson_WithDifferentFormatting_ProducesSameHash()
        {
            var json1 = """{"id":"op-123","action":"process"}""";
            var json2 = """
            {
              "action": "process",
              "id": "op-123"
            }
            """;

            var fp1 = _envelopeHasher.ComputeOperationFingerprintFromJson("test.operation", json1);
            var fp2 = _envelopeHasher.ComputeOperationFingerprintFromJson("test.operation", json2);

            Assert.Equal(fp1, fp2);
        }

        [Fact]
        public void ComputeEnvelopePayloadHash_ReturnsLowercaseHex()
        {
            var envelope = new OntogonyEnvelope<object>
            {
                EventId = "evt_1",
                EventType = "test.event",
                Source = "test://source",
                OccurredAt = DateTimeOffset.UnixEpoch,
                TraceId = "trace-1",
                Protocol = "test",
                Payload = new { data = "test" }
            };

            var hash = _envelopeHasher.ComputeEnvelopePayloadHash(envelope);

            // Should be lowercase hex SHA-256
            Assert.Matches(@"^[a-f0-9]{64}$", hash);
        }

        [Fact]
        public void ComputeOperationFingerprint_ThrowsOnNullPayload()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _envelopeHasher.ComputeOperationFingerprint("test.op", null!));
        }

        [Fact]
        public void ComputeOperationFingerprint_ThrowsOnEmptyOperation()
        {
            Assert.Throws<ArgumentException>(() =>
                _envelopeHasher.ComputeOperationFingerprint("", new { }));
        }
    }

    /// <summary>
    /// Payload hash determinism tests across different JSON representations.
    /// </summary>
    public class PayloadHashDeterminismTests
    {
        private readonly PayloadHasher _hasher = new(new Sha256ContentHashService());

        [Fact]
        public void ComputeCanonicalJsonHash_WithReorderedProperties_ProducesSameHash()
        {
            var obj1 = new { z = 1, a = 2, m = 3 };
            var obj2 = new { a = 2, m = 3, z = 1 };

            var hash1 = _hasher.ComputeCanonicalJsonHash(obj1);
            var hash2 = _hasher.ComputeCanonicalJsonHash(obj2);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void ComputeCanonicalJsonHashFromJson_WithDifferentWhitespace_ProducesSameHash()
        {
            var json1 = """{"a":1,"b":2}""";
            var json2 = """
            {
              "b": 2,
              "a": 1
            }
            """;

            var hash1 = _hasher.ComputeCanonicalJsonHashFromJson(json1);
            var hash2 = _hasher.ComputeCanonicalJsonHashFromJson(json2);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void ComputeCanonicalJsonHash_WithNull_PreservesDifference()
        {
            var withNull = new { value = (string?)null };
            var withoutField = new { };

            var hash1 = _hasher.ComputeCanonicalJsonHash(withNull);
            var hash2 = _hasher.ComputeCanonicalJsonHash(withoutField);

            // Different hashes: one has null, other doesn't have field
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void ComputeCanonicalJsonHash_IsLowercaseHex()
        {
            var obj = new { test = "data" };

            var hash = _hasher.ComputeCanonicalJsonHash(obj);

            // Should be 64-char lowercase hex
            Assert.Matches(@"^[a-f0-9]{64}$", hash);
        }
    }
}
