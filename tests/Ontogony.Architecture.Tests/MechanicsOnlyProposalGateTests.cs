using System.Text.Json;
using Json.Schema;
using Ontogony.Testing.Architecture;
using Xunit;

namespace Ontogony.Architecture.Tests;

/// <summary>PLAT-MECH-001/002/003 — mechanics-only proposal gate tests.</summary>
public sealed class MechanicsOnlyProposalGateTests
{
    private static readonly string[] ForbiddenProposalPhrases =
    [
        "provider fallback policy",
        "model routing",
        "canonical fact",
        "workflow orchestration",
        "reconstructability scoring",
    ];

    [Theory]
    [InlineData("fixtures/mechanics/v1/valid/platform-proposal-accepted.json", true)]
    [InlineData("fixtures/mechanics/v1/invalid/platform-proposal-rejected.json", false)]
    public void Proposal_gate_fixture_matches_schema_and_routing(string fixtureRelativePath, bool shouldBeAccepted)
    {
        var repoRoot = FindRepoRoot();
        var schemaPath = Path.Combine(repoRoot, "schemas", "mechanics", "v1", "platform-proposal-gate.schema.json");
        var fixturePath = Path.Combine(repoRoot, fixtureRelativePath);

        var schema = JsonSchema.FromText(File.ReadAllText(schemaPath));
        using var doc = JsonDocument.Parse(File.ReadAllText(fixturePath));
        Assert.True(schema.Evaluate(doc.RootElement).IsValid, "Proposal fixture must match platform-proposal-gate schema.");

        var decision = doc.RootElement.GetProperty("ownerRoutingDecision").GetString();
        if (shouldBeAccepted)
        {
            Assert.Equal("accepted_platform_mechanics", decision);
            Assert.DoesNotContain(
                ForbiddenProposalPhrases,
                phrase => doc.RootElement.GetRawText().Contains(phrase, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            Assert.Equal("rejected_product_semantics", decision);
        }
    }

    [Fact]
    public void Rejects_model_routing_policy_in_platform_proposal_text()
    {
        var proposal = "provider fallback policy for Conexus model calls";
        Assert.Contains("provider fallback policy", proposal, StringComparison.OrdinalIgnoreCase);
        Assert.False(IsMechanicallyNeutralProposal(proposal));
    }

    [Fact]
    public void Accepts_actor_context_schema_as_mechanical()
    {
        var proposal = "actor context propagation for cross-service headers";
        Assert.Contains("actor context", proposal, StringComparison.OrdinalIgnoreCase);
        Assert.True(IsMechanicallyNeutralProposal(proposal));
    }

    [Fact]
    public void Platform_source_has_no_product_semantic_leakage_via_gate_policy()
    {
        var repoRoot = ArchitectureScanTargets.FindRepoRoot(AppContext.BaseDirectory, "Ontogony.Platform.sln");
        var policyPath = Path.Combine(repoRoot, "scripts", "product-semantic-boundary-policy.json");
        var policy = ProductSemanticBoundaryScanner.LoadPolicy(policyPath);
        ProductSemanticBoundaryScanner.AssertNoProductSemanticLeakage(repoRoot, policy);
    }

    private static bool IsMechanicallyNeutralProposal(string proposalText)
    {
        return !ForbiddenProposalPhrases.Any(
            phrase => proposalText.Contains(phrase, StringComparison.OrdinalIgnoreCase));
    }

    private static string FindRepoRoot()
    {
        return ArchitectureScanTargets.FindRepoRoot(AppContext.BaseDirectory, "Ontogony.Platform.sln");
    }
}
