# Architecture Tests Adoption

This guide shows how product repositories enforce forbidden dependency rules with `Ontogony.Testing.Architecture`.

## Scope

Use platform mechanics only. Each product repo defines its own forbidden fragments and namespaces.

The helpers detect:

```text
PackageReference / ProjectReference / PackageVersion (single-line and multiline Include)
using / global using
using static / global using static
using alias = Namespace
```

## Package reference

```xml
<PackageReference Include="Ontogony.Testing" />
```

When developing against a sibling `ontogony-platform` checkout, test projects may use a project reference instead.

## Example: forbidden MSBuild references

```csharp
using Ontogony.Testing.Architecture;
using Xunit;

namespace MyProduct.Tests;

public sealed class ForbiddenDependencyTests
{
    private static readonly string[] ForbiddenFragments =
    [
        "OpenAI",
        "Anthropic",
        "Azure.AI.OpenAI",
        "Google.AI",
        "Agentor",
        "Conexus"
    ];

    [Fact]
    public void Product_does_not_reference_forbidden_packages_or_projects()
    {
        var repoRoot = ArchitectureScanTargets.FindRepoRoot(
            AppContext.BaseDirectory,
            "MyProduct.sln");

        var scanTargets = ArchitectureScanTargets.CollectMsBuildScanTargets(
            repoRoot,
            additionalRelativeFiles:
            [
                "eng/Ontogony.References.props",
                "eng/Directory.Packages.props"
            ]);

        Assert.NotEmpty(scanTargets);

        ArchitectureReferenceAssertions.AssertNoForbiddenReferences(
            repoRoot,
            scanTargets,
            ForbiddenFragments);
    }
}
```

`CollectMsBuildScanTargets` scans:

```text
src/**/*.csproj
tests/**/*.csproj
Directory.Packages.props
Directory.Build.props
Directory.Build.targets
```

plus any additional relative files you pass in.

Multiline MSBuild entries are supported, for example:

```xml
<PackageReference
  Include="OpenAI"
  Version="1.0.0" />
```

## Example: forbidden using directives

```csharp
[Fact]
public void Product_source_does_not_import_forbidden_namespaces()
{
    var repoRoot = ArchitectureScanTargets.FindRepoRoot(
        AppContext.BaseDirectory,
        "MyProduct.sln");

    ArchitectureReferenceAssertions.AssertNoForbiddenUsingDirectives(
        repoRoot,
        sourceGlobs:
        [
            "src/**/*.cs",
            "tests/**/*.cs"
        ],
        forbiddenNamespaces:
        [
            "OpenAI",
            "Agentor",
            "Conexus"
        ]);
}
```

The scanner flags:

```csharp
using OpenAI;
global using Agentor.Contracts;
using static Conexus.Sdk.Client;
using OAI = OpenAI.Chat;
```

## Targeted inspection helpers

Use these in focused unit tests for scanner behavior:

```csharp
var violations = ArchitectureReferenceAssertions.FindForbiddenReferences(
    "Directory.Packages.props",
    File.ReadAllText("Directory.Packages.props"),
    ["OpenAI"]);

var usingViolations = ArchitectureReferenceAssertions.FindForbiddenUsingDirectives(
    "Sample.cs",
    source,
    ["OpenAI"]);
```

## Verification checklist

1. Marker solution file name matches `FindRepoRoot(..., "YourProduct.sln")`.
2. Central package files under `eng/` are listed in `additionalRelativeFiles` when used.
3. Forbidden fragments are product-specific and documented in repo integration docs.
4. Architecture tests run in CI on every PR.

## Related

- [../packages/Ontogony.Testing.md](../packages/Ontogony.Testing.md)
- [integration-metrics-adoption.md](integration-metrics-adoption.md)
