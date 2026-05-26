namespace Ontogony.Testing.Conformance;

/// <summary>
/// Mechanical checks for OpenTelemetry meter and Prometheus-style instrument naming.
/// </summary>
public static class ObservabilityNamingConformanceAssertions
{
    /// <summary>Asserts the meter name uses the expected service prefix (for example <c>Conexus.</c>).</summary>
    public static void AssertMeterNameUsesPrefix(string meterName, string requiredPrefix)
    {
        if (string.IsNullOrWhiteSpace(meterName))
            throw new ArgumentException("meterName cannot be empty.", nameof(meterName));
        if (string.IsNullOrWhiteSpace(requiredPrefix))
            throw new ArgumentException("requiredPrefix cannot be empty.", nameof(requiredPrefix));

        if (!meterName.StartsWith(requiredPrefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Meter name '{meterName}' must start with prefix '{requiredPrefix}'.");
        }
    }

    /// <summary>
    /// Asserts logical dotted instrument names map to lowercase snake_case Prometheus names.
    /// </summary>
    public static void AssertPrometheusExportName(string logicalInstrumentName, string exportedMetricName)
    {
        if (string.IsNullOrWhiteSpace(logicalInstrumentName))
            throw new ArgumentException("logicalInstrumentName cannot be empty.", nameof(logicalInstrumentName));
        if (string.IsNullOrWhiteSpace(exportedMetricName))
            throw new ArgumentException("exportedMetricName cannot be empty.", nameof(exportedMetricName));

        var expected = logicalInstrumentName
            .Replace(".", "_", StringComparison.Ordinal)
            .ToLowerInvariant() + "_total";

        if (!string.Equals(exportedMetricName, expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected Prometheus export name '{expected}' for logical '{logicalInstrumentName}' but was '{exportedMetricName}'.");
        }
    }
}
