using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ontogony.Artifacts;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Xunit;

namespace Ontogony.Artifacts.Tests;

public sealed class ArtifactContractsTests
{
    private static readonly DateTimeOffset FixedInstant = new(2026, 5, 12, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void ArtifactRef_CanonicalJson_is_deterministic()
    {
        var a = CreateSampleRef();
        var b = CreateSampleRef();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);

        var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var roundTrip = JsonSerializer.Deserialize<ArtifactRef>(ja, opts)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void ArtifactPutResult_is_serialization_friendly()
    {
        var result = new ArtifactPutResult(CreateSampleRef(), Existed: true);
        var json = CanonicalJson.Serialize(result);
        Assert.Contains("\"existed\":true", json, StringComparison.Ordinal);
        Assert.Contains("\"reference\":", json, StringComparison.Ordinal);
    }

    [Fact]
    public void OntogonyEnvelope_of_ArtifactRef_validates_with_DefaultEnvelopeValidator()
    {
        var inner = CreateSampleRef();
        var envelope = new OntogonyEnvelope<ArtifactRef>
        {
            EventId = "art-event-1",
            EventType = "ontogony.artifact.recorded",
            Source = "https://emitter.example/artifacts",
            OccurredAt = FixedInstant,
            TraceId = "trace-abc",
            Protocol = ProtocolNames.GenericJson,
            Payload = inner
        };

        var validator = new DefaultEnvelopeValidator();
        var result = validator.Validate(envelope);
        Assert.True(
            result.IsValid,
            result.IsValid ? string.Empty : string.Join("; ", result.Errors.Select(e => $"{e.Field}:{e.Message}")));
    }

    [Fact]
    public void Public_contract_surface_avoids_banned_semantic_tokens()
    {
        var asm = typeof(ArtifactRef).Assembly;
        var rx = new Regex(
            "(BestModel|RouteDecision|Canon|Belief|Authority|\\bPlan\\b|Retention|Provider)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        const BindingFlags memberFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        foreach (var t in asm.GetExportedTypes())
        {
            Assert.False(rx.IsMatch(t.Name), $"Type {t.FullName}");

            if (t.IsEnum)
            {
                foreach (var name in Enum.GetNames(t))
                {
                    Assert.False(rx.IsMatch(name), $"Enum member {t.FullName}.{name}");
                }

                continue;
            }

            foreach (var p in t.GetProperties(memberFlags))
            {
                if (p.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                Assert.False(rx.IsMatch(p.Name), $"Property {t.FullName}.{p.Name}");
            }

            foreach (var e in t.GetEvents(memberFlags))
            {
                Assert.False(rx.IsMatch(e.Name), $"Event {t.FullName}.{e.Name}");
            }

            foreach (var m in t.GetMethods(memberFlags))
            {
                if (m.IsSpecialName)
                {
                    continue;
                }

                Assert.False(rx.IsMatch(m.Name), $"Method {t.FullName}.{m.Name}");
            }

            foreach (var f in t.GetFields(memberFlags))
            {
                Assert.False(rx.IsMatch(f.Name), $"Field {t.FullName}.{f.Name}");
            }

            foreach (var nt in t.GetNestedTypes(memberFlags))
            {
                Assert.False(rx.IsMatch(nt.Name), $"Nested type {t.FullName}+{nt.Name}");
            }
        }
    }

    [Fact]
    public void Public_surface_has_no_cloud_provider_types()
    {
        var asm = typeof(ArtifactRef).Assembly;
        var banned = new[] { "S3", "Azure", "Gcs", "Aws", "Blob", "Bucket" };

        foreach (var t in asm.GetExportedTypes())
        {
            foreach (var token in banned)
            {
                Assert.DoesNotContain(token, t.Name, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    private static ArtifactRef CreateSampleRef() => new()
    {
        ArtifactId = "artifact-1",
        ContentHash = "abc123",
        MediaType = "application/json",
        SizeBytes = 42,
        ContentEncoding = "identity",
        StorageTier = "inline",
        Classification = "internal",
        Uri = null,
        TenantId = "t1",
        WorkspaceId = null,
        ProjectId = null,
        CreatedAt = FixedInstant
    };

    internal static byte[] Bytes(string s) => Encoding.UTF8.GetBytes(s);
}
