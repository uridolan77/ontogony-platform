using System.Text.Json;
using Json.Schema;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class PhenomenologicalAuthorityCertificationSchemaTests
{
    [Fact]
    public void Fixture_summary_passes_certification_v2_schema()
    {
        var root = GetProjectRoot();
        var schema = JsonSchema.FromFile(
            Path.Combine(root, "schemas/phenomenological-authority-certification-v2.schema.json"));
        var fixturePath = Path.Combine(
            root,
            "schemas/fixtures/phenomenological-authority-certification-v2.pass.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(fixturePath));

        var result = schema.Evaluate(doc.RootElement);
        Assert.True(result.IsValid, string.Join("; ", result.Errors?.Select(e => e.Value) ?? []));
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Ontogony.Platform.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not locate Ontogony.Platform.sln.");
    }
}
