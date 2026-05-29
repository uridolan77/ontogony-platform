using System.Text.Json;
using Ontogony.Errors;

namespace Ontogony.SystemCompatibility;

public static class CrossServiceErrorEnvelopeConformance
{
    public static SystemCompatibilityCheck CheckMatrixArtifacts(SystemCompatibilityWorkspace workspace)
    {
        var matrixPath = Path.Combine(workspace.PlatformRoot, "docs/system/cross-service-error-envelope.matrix.json");
        var schemaPath = Path.Combine(workspace.PlatformRoot, "docs/system/schemas/cross-service-error-envelope-v0.schema.json");
        var contractPath = Path.Combine(workspace.PlatformRoot, "docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md");
        var taxonomyMatrixPath = Path.Combine(workspace.PlatformRoot, "docs/system/operator-failure-taxonomy.matrix.json");

        var missing = new List<string>();
        foreach (var path in new[] { matrixPath, schemaPath, contractPath, taxonomyMatrixPath })
        {
            if (!File.Exists(path))
            {
                missing.Add(path);
            }
        }

        if (missing.Count > 0)
        {
            return Fail("error-envelope-matrix", "Error envelope conformance matrix", $"Missing: {string.Join("; ", missing)}");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var issues = new List<string>();

        if (!string.Equals(
                matrix.RootElement.GetProperty("schema").GetString(),
                "ontogony-cross-service-error-envelope-v1",
                StringComparison.Ordinal))
        {
            issues.Add("unexpected matrix schema");
        }

        var requiredFields = matrix.RootElement.GetProperty("requiredEnvelopeFields")
            .EnumerateArray()
            .Select(e => e.GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var field in new[] { "code", "message", "system", "traceId", "correlationId", "retryable", "downstreamSystem" })
        {
            if (field is "code" or "message" or "system")
            {
                if (!requiredFields.Contains(field))
                {
                    issues.Add($"requiredEnvelopeFields missing {field}");
                }
            }
        }

        var contractText = File.ReadAllText(contractPath);
        foreach (var field in requiredFields)
        {
            if (!contractText.Contains(field, StringComparison.Ordinal))
            {
                issues.Add($"contract doc missing field {field}");
            }
        }

        return issues.Count == 0
            ? Pass("error-envelope-matrix", "Error envelope conformance matrix", "Matrix, schema, contract, and taxonomy matrix are present.")
            : Fail("error-envelope-matrix", "Error envelope conformance matrix", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckPlatformSamples(SystemCompatibilityWorkspace workspace)
    {
        var matrixPath = Path.Combine(workspace.PlatformRoot, "docs/system/cross-service-error-envelope.matrix.json");
        if (!File.Exists(matrixPath))
        {
            return Fail("error-envelope-samples", "Error envelope wire samples", $"Missing matrix: {matrixPath}");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var issues = new List<string>();

        foreach (var sample in matrix.RootElement.GetProperty("platformSamples").EnumerateArray())
        {
            var id = sample.GetProperty("id").GetString()!;
            var wireShape = sample.GetProperty("wireShape").GetString()!;
            var relPath = sample.GetProperty("path").GetString()!;
            var samplePath = Path.Combine(workspace.PlatformRoot, relPath.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(samplePath))
            {
                issues.Add($"sample {id} missing file {samplePath}");
                continue;
            }

            using var document = JsonDocument.Parse(File.ReadAllText(samplePath));
            var root = document.RootElement;

            switch (wireShape)
            {
                case "CrossServiceErrorEnvelope":
                    if (!CrossServiceErrorEnvelopeExtensions.TryParse(root, out _))
                    {
                        issues.Add($"sample {id} is not a valid CrossServiceErrorEnvelope");
                    }

                    break;
                case "Ontogony.ApiError":
                    if (!root.TryGetProperty("code", out var code) || code.ValueKind != JsonValueKind.String
                        || !root.TryGetProperty("message", out var message) || message.ValueKind != JsonValueKind.String)
                    {
                        issues.Add($"sample {id} missing ApiError code/message");
                    }

                    if (root.TryGetProperty("system", out _))
                    {
                        issues.Add($"sample {id} must not include system on Kanon wire shape");
                    }

                    break;
                case "OpenAI.error":
                    if (!root.TryGetProperty("error", out var error)
                        || !error.TryGetProperty("message", out var errMessage)
                        || errMessage.ValueKind != JsonValueKind.String)
                    {
                        issues.Add($"sample {id} missing OpenAI error.message");
                    }

                    break;
                default:
                    issues.Add($"sample {id} unknown wireShape {wireShape}");
                    break;
            }
        }

        return issues.Count == 0
            ? Pass("error-envelope-samples", "Error envelope wire samples", "Platform wire samples match declared shapes.")
            : Fail("error-envelope-samples", "Error envelope wire samples", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckTaxonomyAdapterMappings(SystemCompatibilityWorkspace workspace)
    {
        var taxonomyPath = Path.Combine(workspace.PlatformRoot, "docs/system/operator-failure-taxonomy.matrix.json");
        if (!File.Exists(taxonomyPath))
        {
            return Fail("error-envelope-taxonomy", "Operator taxonomy adapter conformance", $"Missing {taxonomyPath}");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(taxonomyPath));
        var root = doc.RootElement;
        var requiredKinds = root.GetProperty("requiredTaxonomyKinds")
            .EnumerateArray()
            .Select(e => e.GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        var producedKinds = new HashSet<string>(StringComparer.Ordinal);
        var issues = new List<string>();

        foreach (var row in root.GetProperty("representativeMappings").EnumerateArray())
        {
            var id = row.GetProperty("id").GetString()!;
            var expected = row.GetProperty("expectedTaxonomy").GetString()!;
            var envelopeElement = row.GetProperty("envelope");

            if (!CrossServiceErrorEnvelopeExtensions.TryParse(envelopeElement, out var envelope) || envelope is null)
            {
                issues.Add($"mapping {id} envelope does not parse");
                continue;
            }

            var view = OperatorFailureTaxonomyAdapter.FromCrossServiceEnvelope(envelope);
            if (!string.Equals(expected, view.Taxonomy, StringComparison.Ordinal))
            {
                issues.Add($"mapping {id} expected {expected} got {view.Taxonomy}");
            }

            producedKinds.Add(view.Taxonomy);
        }

        foreach (var kind in requiredKinds)
        {
            if (!producedKinds.Contains(kind))
            {
                issues.Add($"requiredTaxonomyKind {kind} has no representative mapping");
            }
        }

        return issues.Count == 0
            ? Pass(
                "error-envelope-taxonomy",
                "Operator taxonomy adapter conformance",
                $"All {root.GetProperty("representativeMappings").GetArrayLength()} mappings match OperatorFailureTaxonomyAdapter.")
            : Fail("error-envelope-taxonomy", "Operator taxonomy adapter conformance", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckOpenApiEnvelopeSchema(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.AllagmaRoot is null && workspace.FrontendRoot is null)
        {
            return Skipped("error-envelope-openapi", "OpenAPI CrossServiceErrorEnvelope", "No Allagma or frontend sibling for OpenAPI checks.");
        }

        var issues = new List<string>();

        if (workspace.AllagmaRoot is not null)
        {
            ValidateOpenApiComponent(
                Path.Combine(workspace.AllagmaRoot, "docs/api/allagma-openapi-v1.snapshot.json"),
                "allagma",
                issues);
        }

        if (workspace.FrontendRoot is not null)
        {
            ValidateOpenApiComponent(
                Path.Combine(workspace.FrontendRoot, "openapi/allagma.v0.json"),
                "ontogony-frontend",
                issues);
        }

        return issues.Count == 0
            ? Pass("error-envelope-openapi", "OpenAPI CrossServiceErrorEnvelope", "Allagma and frontend OpenAPI snapshots expose required envelope fields.")
            : Fail("error-envelope-openapi", "OpenAPI CrossServiceErrorEnvelope", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckSiblingIntegrationDocs(SystemCompatibilityWorkspace workspace)
    {
        var matrixPath = Path.Combine(workspace.PlatformRoot, "docs/system/cross-service-error-envelope.matrix.json");
        if (!File.Exists(matrixPath))
        {
            return Fail("error-envelope-sibling-docs", "Sibling error integration docs", $"Missing matrix: {matrixPath}");
        }

        using var matrix = JsonDocument.Parse(File.ReadAllText(matrixPath));
        var contracts = matrix.RootElement.GetProperty("integrationContracts");
        var issues = new List<string>();

        foreach (var entry in contracts.EnumerateObject())
        {
            var rel = entry.Value.GetString()!;
            var parts = rel.Split('/', 2);
            if (parts.Length != 2)
            {
                issues.Add($"invalid integration contract path {rel}");
                continue;
            }

            var repoRoot = parts[0] switch
            {
                "allagma-dotnet" => workspace.AllagmaRoot,
                "kanon-dotnet" => workspace.KanonRoot,
                "conexus-dotnet" => workspace.ConexusRoot,
                "metabole-dotnet" => workspace.MetaboleRoot,
                "aisthesis-dotnet" => workspace.AisthesisRoot,
                _ => null
            };

            if (repoRoot is null)
            {
                issues.Add($"sibling repo {parts[0]} not present for {entry.Name}");
                continue;
            }

            var fullPath = Path.Combine(workspace.DevRoot, rel.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath))
            {
                issues.Add($"missing {entry.Name} contract: {fullPath}");
                continue;
            }

            var text = File.ReadAllText(fullPath);
            if (!text.Contains("CrossServiceErrorEnvelope", StringComparison.Ordinal)
                && !text.Contains("ApiError", StringComparison.Ordinal)
                && !text.Contains("OpenAI", StringComparison.Ordinal))
            {
                issues.Add($"{entry.Name} contract does not reference a known error wire shape");
            }
        }

        return issues.Count == 0
            ? Pass("error-envelope-sibling-docs", "Sibling error integration docs", "Product repo error integration contracts are present.")
            : Fail("error-envelope-sibling-docs", "Sibling error integration docs", string.Join("; ", issues));
    }

    public static SystemCompatibilityCheck CheckFrontendTaxonomyModule(SystemCompatibilityWorkspace workspace)
    {
        if (workspace.FrontendRoot is null)
        {
            return Skipped("error-envelope-frontend", "Frontend error taxonomy module", "ontogony-frontend not present.");
        }

        var adapterPath = Path.Combine(workspace.FrontendRoot, "src/system/errors/operatorFailureTaxonomy.ts");
        var bannerPath = Path.Combine(workspace.FrontendRoot, "src/system/errors/OperatorFailureBanner.tsx");
        var matrixTestPath = Path.Combine(workspace.FrontendRoot, "src/system/errors/operatorFailureTaxonomy.matrix.test.ts");

        var missing = new List<string>();
        foreach (var path in new[] { adapterPath, bannerPath })
        {
            if (!File.Exists(path))
            {
                missing.Add(path);
            }
        }

        if (missing.Count > 0)
        {
            return Fail("error-envelope-frontend", "Frontend error taxonomy module", $"Missing: {string.Join("; ", missing)}");
        }

        var adapterText = File.ReadAllText(adapterPath);
        var issues = new List<string>();

        foreach (var field in new[] { "code", "message", "system", "retryable", "downstreamSystem", "traceId" })
        {
            if (!adapterText.Contains(field, StringComparison.Ordinal))
            {
                issues.Add($"operatorFailureTaxonomy.ts missing {field}");
            }
        }

        if (!File.Exists(matrixTestPath))
        {
            issues.Add($"missing matrix test {matrixTestPath}");
        }

        return issues.Count == 0
            ? Pass("error-envelope-frontend", "Frontend error taxonomy module", "Frontend taxonomy adapter and banner are present.")
            : Fail("error-envelope-frontend", "Frontend error taxonomy module", string.Join("; ", issues));
    }

    private static void ValidateOpenApiComponent(string openApiPath, string label, List<string> issues)
    {
        if (!File.Exists(openApiPath))
        {
            issues.Add($"{label} missing OpenAPI {openApiPath}");
            return;
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(openApiPath));
        if (!doc.RootElement.TryGetProperty("components", out var components)
            || !components.TryGetProperty("schemas", out var schemas)
            || !schemas.TryGetProperty("CrossServiceErrorEnvelope", out var envelopeSchema))
        {
            issues.Add($"{label} OpenAPI missing components.schemas.CrossServiceErrorEnvelope");
            return;
        }

        if (!envelopeSchema.TryGetProperty("properties", out var properties))
        {
            issues.Add($"{label} CrossServiceErrorEnvelope missing properties");
            return;
        }

        foreach (var required in new[] { "code", "message", "system" })
        {
            if (!properties.TryGetProperty(required, out _))
            {
                issues.Add($"{label} CrossServiceErrorEnvelope.properties missing {required}");
            }
        }
    }

    private static SystemCompatibilityCheck Pass(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Pass, detail);

    private static SystemCompatibilityCheck Fail(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Fail, detail);

    private static SystemCompatibilityCheck Skipped(string id, string title, string detail) =>
        new(id, title, SystemCompatibilityCheckStatus.Skipped, detail);
}
