# PR-PLAT-TEST-001 — Architecture Test Helpers

## Purpose

Provide reusable test helpers for forbidden dependencies and architecture rules.

## Proposed home

```text
Ontogony.Testing.Architecture
```

## API sketch

```csharp
public static class ArchitectureReferenceAssertions
{
    public static void AssertNoForbiddenReferences(
        string repoRoot,
        IReadOnlyList<string> files,
        IReadOnlyList<string> forbiddenFragments);

    public static void AssertNoForbiddenUsingDirectives(
        string repoRoot,
        IReadOnlyList<string> sourceGlobs,
        IReadOnlyList<string> forbiddenNamespaces);
}
```

## Must scan

```text
.csproj
.props
.targets
Directory.Packages.props
Directory.Build.props
Directory.Build.targets
```

Recognize:

```text
PackageReference
ProjectReference
PackageVersion
```

## Non-goals

No Kanon/Agentor/Conexus-specific rules baked into platform. Product repos pass their own rules.
