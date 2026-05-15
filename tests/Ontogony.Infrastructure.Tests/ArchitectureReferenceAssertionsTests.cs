using Ontogony.Testing.Architecture;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ArchitectureReferenceAssertionsTests
{
    private static readonly string[] ForbiddenFragments = ["OpenAI", "Agentor"];

    [Fact]
    public void FindForbiddenReferences_Detects_PackageVersion()
    {
        const string content = """
            <Project>
              <ItemGroup>
                <PackageVersion Include="OpenAI" Version="9.9.9" />
              </ItemGroup>
            </Project>
            """;

        var violations = ArchitectureReferenceAssertions.FindForbiddenReferences(
            "Directory.Packages.props",
            content,
            ForbiddenFragments);

        Assert.Single(violations);
        Assert.Contains("PackageVersion", violations[0], StringComparison.Ordinal);
        Assert.Contains("OpenAI", violations[0], StringComparison.Ordinal);
    }

    [Fact]
    public void FindForbiddenReferences_Detects_PackageReference_And_ProjectReference()
    {
        const string content = """
            <Project>
              <ItemGroup>
                <PackageReference Include="Anthropic.SDK" />
                <ProjectReference Include="..\\..\\agentor\\Agentor.Api\\Agentor.Api.csproj" />
              </ItemGroup>
            </Project>
            """;

        var violations = ArchitectureReferenceAssertions.FindForbiddenReferences(
            "Sample.csproj",
            content,
            ["Anthropic", "Agentor"]);

        Assert.Equal(2, violations.Count);
        Assert.Contains(violations, v => v.Contains("PackageReference", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("ProjectReference", StringComparison.Ordinal));
    }

    [Fact]
    public void AssertNoForbiddenReferences_Throws_With_Violation_Details()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-arch-" + Guid.NewGuid().ToString("n"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var file = Path.Combine(tempDir, "Bad.csproj");
            File.WriteAllText(
                file,
                """
                <Project>
                  <ItemGroup>
                    <PackageReference Include="OpenAI" />
                  </ItemGroup>
                </Project>
                """);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                ArchitectureReferenceAssertions.AssertNoForbiddenReferences(
                    tempDir,
                    [file],
                    ForbiddenFragments));

            Assert.Contains("Forbidden dependency references found", ex.Message, StringComparison.Ordinal);
            Assert.Contains("OpenAI", ex.Message, StringComparison.Ordinal);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void CollectMsBuildScanTargets_Includes_Csproj_And_Root_Props_Files()
    {
        var repoRoot = ArchitectureScanTargets.FindRepoRoot(
            AppContext.BaseDirectory,
            "Ontogony.Platform.sln");

        var targets = ArchitectureScanTargets.CollectMsBuildScanTargets(repoRoot);

        Assert.Contains(targets, path => path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(targets, path => string.Equals(Path.GetFileName(path), "Directory.Packages.props", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void FindForbiddenUsingDirectives_Detects_Using_And_GlobalUsing()
    {
        const string content = """
            using System;
            global using OpenAI;
            using Agentor.Contracts;
            namespace Sample;
            """;

        var violations = ArchitectureReferenceAssertions.FindForbiddenUsingDirectives(
            "Sample.cs",
            content,
            ["OpenAI", "Agentor"]);

        Assert.Equal(2, violations.Count);
        Assert.Contains(violations, v => v.Contains("global using OpenAI", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("using Agentor.Contracts", StringComparison.Ordinal));
    }

    [Fact]
    public void FindForbiddenReferences_Detects_Multiline_Include_Attributes()
    {
        const string content = """
            <Project>
              <ItemGroup>
                <PackageReference
                  Include="OpenAI"
                  Version="1.0.0" />
                <ProjectReference
                  Include=
                  "..\\..\\agentor\\Agentor.Api\\Agentor.Api.csproj" />
                <PackageVersion
                  Include="Azure.AI.OpenAI"
                  Version="1.0.0" />
              </ItemGroup>
            </Project>
            """;

        var violations = ArchitectureReferenceAssertions.FindForbiddenReferences(
            "Sample.csproj",
            content,
            ["OpenAI", "Agentor", "Azure.AI.OpenAI"]);

        Assert.Equal(4, violations.Count);
        Assert.Contains(violations, v => v.Contains("PackageReference", StringComparison.Ordinal) && v.Contains("-> OpenAI", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("ProjectReference", StringComparison.Ordinal) && v.Contains("Agentor", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("PackageVersion", StringComparison.Ordinal) && v.Contains("Azure.AI.OpenAI", StringComparison.Ordinal));
        Assert.Equal(2, violations.Count(v => v.Contains("PackageVersion", StringComparison.Ordinal)));
    }

    [Fact]
    public void FindForbiddenUsingDirectives_Detects_Alias_And_Static_Directives()
    {
        const string content = """
            using OAI = OpenAI.Chat;
            using static Agentor.Contracts.RunStatus;
            global using static OpenAI;
            global using ConexusAlias = Conexus.Sdk;
            namespace Sample;
            """;

        var violations = ArchitectureReferenceAssertions.FindForbiddenUsingDirectives(
            "Sample.cs",
            content,
            ["OpenAI", "Agentor", "Conexus"]);

        Assert.Equal(4, violations.Count);
        Assert.Contains(violations, v => v.Contains("using OAI = OpenAI.Chat", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("using static Agentor.Contracts.RunStatus", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("global using static OpenAI", StringComparison.Ordinal));
        Assert.Contains(violations, v => v.Contains("global using ConexusAlias = Conexus.Sdk", StringComparison.Ordinal));
    }

    [Fact]
    public void AssertNoForbiddenUsingDirectives_Throws_For_Resolved_Source_Globs()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "ontogony-arch-src-" + Guid.NewGuid().ToString("n"));
        var srcDir = Path.Combine(tempDir, "src");
        Directory.CreateDirectory(srcDir);

        try
        {
            File.WriteAllText(
                Path.Combine(srcDir, "Bad.cs"),
                """
                using OpenAI;
                namespace Sample;
                """);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                ArchitectureReferenceAssertions.AssertNoForbiddenUsingDirectives(
                    tempDir,
                    ["src/**/*.cs"],
                    ["OpenAI"]));

            Assert.Contains("Forbidden using directives found", ex.Message, StringComparison.Ordinal);
            Assert.Contains("using OpenAI", ex.Message, StringComparison.Ordinal);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
