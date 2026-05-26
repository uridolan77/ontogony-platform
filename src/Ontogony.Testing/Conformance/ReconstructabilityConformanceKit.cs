using System.Text.Json;

namespace Ontogony.Testing.Conformance;

/// <summary>
/// Facade for reconstructability export safety conformance.
/// </summary>
public static class ReconstructabilityConformanceKit
{
    /// <summary>Runs redaction + shape checks on a decision-event JSON document.</summary>
    public static void AssertDecisionEventFixture(string json)
    {
        ReconstructabilityExportConformanceAssertions.AssertExportJsonDoesNotLeakSecrets(json);
        using var doc = JsonDocument.Parse(json);
        ReconstructabilityExportConformanceAssertions.AssertDecisionEventExportShape(doc.RootElement);
    }

    /// <summary>Runs redaction checks only (for bundled multi-event exports).</summary>
    public static void AssertExportBundleRedactionSafe(string json)
    {
        ReconstructabilityExportConformanceAssertions.AssertExportJsonDoesNotLeakSecrets(json);
    }
}
