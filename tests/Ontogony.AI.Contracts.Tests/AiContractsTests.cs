using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ontogony.AI.Contracts;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Xunit;

namespace Ontogony.AI.Contracts.Tests;

public sealed class AiContractsTests
{
    private static readonly DateTimeOffset FixedInstant = new(2026, 5, 12, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void LlmRequestEnvelope_CanonicalJson_is_deterministic()
    {
        var a = CreateSampleRequest();
        var b = CreateSampleRequest();

        var ja = CanonicalJson.Serialize(a);
        var jb = CanonicalJson.Serialize(b);

        Assert.Equal(ja, jb);
        var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var roundTrip = JsonSerializer.Deserialize<LlmRequestEnvelope>(ja, opts)!;
        Assert.Equal(ja, CanonicalJson.Serialize(roundTrip));
    }

    [Fact]
    public void LlmUsageRecord_ResolveTotalTokensOrSum_prefers_total_when_present()
    {
        var r = new LlmUsageRecord(1, 2, 99, null, null);
        Assert.Equal(99, r.ResolveTotalTokensOrSum());
    }

    [Fact]
    public void LlmUsageRecord_ResolveTotalTokensOrSum_sums_when_total_missing()
    {
        var r = new LlmUsageRecord(3, 4, null, "cl100k", null);
        Assert.Equal(7, r.ResolveTotalTokensOrSum());
    }

    [Fact]
    public void LlmUsageRecord_ResolveTotalTokensOrSum_returns_null_when_incomplete()
    {
        var r = new LlmUsageRecord(3, null, null, null, null);
        Assert.Null(r.ResolveTotalTokensOrSum());
    }

    [Fact]
    public void LlmCostRecord_allows_estimated_or_actual_without_policy()
    {
        var est = new LlmCostRecord("USD", 0.01m, null, "v1");
        var act = new LlmCostRecord("USD", null, 0.012m, "v1");
        Assert.NotNull(est.EstimatedCost);
        Assert.Null(est.ActualCost);
        Assert.Null(act.EstimatedCost);
        Assert.NotNull(act.ActualCost);
    }

    [Fact]
    public void ToolCallRecord_is_mechanical_has_no_routing_fields()
    {
        var t = typeof(ToolCallRecord);
        foreach (var p in t.GetProperties())
        {
            Assert.False(Regex.IsMatch(p.Name, @"\b(Route|Rank|Plan|Policy|Score)\b", RegexOptions.IgnoreCase), p.Name);
        }
    }

    [Fact]
    public void ModelCapabilityDescriptor_is_descriptive_not_selector()
    {
        var d = new ModelCapabilityDescriptor("acme", "gpt-test", true, true, false, 128000, "1");
        Assert.True(d.SupportsStreaming);
        Assert.Equal("gpt-test", d.Model);
    }

    [Fact]
    public void OntogonyEnvelope_of_LlmRequestEnvelope_validates_with_DefaultEnvelopeValidator()
    {
        var inner = CreateSampleRequest();
        var envelope = new OntogonyEnvelope<LlmRequestEnvelope>
        {
            EventId = "req-event-1",
            EventType = "ontogony.llm.emit",
            Source = "https://emitter.example/out",
            OccurredAt = FixedInstant,
            TraceId = inner.TraceId,
            Protocol = ProtocolNames.GenericJson,
            Payload = inner
        };

        var validator = new DefaultEnvelopeValidator();
        var result = validator.Validate(envelope);
        Assert.True(result.IsValid, result.IsValid ? "" : string.Join("; ", result.Errors.Select(e => $"{e.Field}:{e.Message}")));
    }

    [Fact]
    public void Public_contract_surface_avoids_banned_semantic_tokens()
    {
        var asm = typeof(LlmRequestEnvelope).Assembly;
        var rx = new Regex(
            "(BestModel|RouteDecision|Canon|Belief|Authority|\\bPlan\\b)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        const BindingFlags memberFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        foreach (var t in asm.GetExportedTypes())
        {
            Assert.False(rx.IsMatch(t.Name), $"Type {t.FullName}");

            if (t.IsEnum)
            {
                foreach (var name in Enum.GetNames(t))
                    Assert.False(rx.IsMatch(name), $"Enum member {t.FullName}.{name}");
                continue;
            }

            foreach (var p in t.GetProperties(memberFlags))
            {
                if (p.GetIndexParameters().Length > 0)
                    continue;

                Assert.False(rx.IsMatch(p.Name), $"Property {t.FullName}.{p.Name}");
            }

            foreach (var e in t.GetEvents(memberFlags))
                Assert.False(rx.IsMatch(e.Name), $"Event {t.FullName}.{e.Name}");

            foreach (var m in t.GetMethods(memberFlags))
            {
                if (m.IsSpecialName)
                    continue;

                Assert.False(rx.IsMatch(m.Name), $"Method {t.FullName}.{m.Name}");
            }

            foreach (var f in t.GetFields(memberFlags))
                Assert.False(rx.IsMatch(f.Name), $"Field {t.FullName}.{f.Name}");

            foreach (var nt in t.GetNestedTypes(memberFlags))
                Assert.False(rx.IsMatch(nt.Name), $"Nested type {t.FullName}+{nt.Name}");
        }
    }

    private static LlmRequestEnvelope CreateSampleRequest() =>
        new()
        {
            RequestId = "req-1",
            TraceId = "trace-abc",
            OperationName = "completion",
            Provider = "acme",
            Model = "acme-large",
            TenantId = "t1",
            WorkspaceId = null,
            ProjectId = null,
            PromptHash = "deadbeef",
            RawProviderRequestHash = null,
            Parameters = new Dictionary<string, string> { ["temperature"] = "0" },
            RequestedTools = null,
            CreatedAt = FixedInstant
        };
}
