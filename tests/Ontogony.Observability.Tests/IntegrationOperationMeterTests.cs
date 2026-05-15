using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class IntegrationOperationMeterTests
{
    [Fact]
    public void IntegrationMetricDimension_Keys_Are_Stable()
    {
        Assert.Equal("source_service", IntegrationMetricDimensions.SourceService);
        Assert.Equal("target_service", IntegrationMetricDimensions.TargetService);
        Assert.Equal("operation", IntegrationMetricDimensions.Operation);
        Assert.Equal("status", IntegrationMetricDimensions.Status);
        Assert.Equal("error_code", IntegrationMetricDimensions.ErrorCode);
        Assert.Equal("http_status", IntegrationMetricDimensions.HttpStatus);
    }

    [Fact]
    public void AddOntogonyObservability_Registers_IntegrationOperationMeter()
    {
        var services = new ServiceCollection();
        services.AddOntogonyObservability(opts => opts.ServiceName = "test-service");

        using var provider = services.BuildServiceProvider();
        var meter = provider.GetRequiredService<IIntegrationOperationMeter>();

        Assert.IsType<IntegrationOperationMeter>(meter);
    }

    [Fact]
    public void RecordSuccess_Emits_Call_And_Duration_With_Standard_Dimensions()
    {
        var measurements = CaptureMeasurements();
        var meter = CreateMeter("caller-service");

        meter.RecordSuccess("kanon", "GetFact", TimeSpan.FromMilliseconds(42));

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.call.count"
                 && m.Measurement == 1
                 && HasTag(m.Tags, IntegrationMetricDimensions.SourceService, "caller-service")
                 && HasTag(m.Tags, IntegrationMetricDimensions.TargetService, "kanon")
                 && HasTag(m.Tags, IntegrationMetricDimensions.Operation, "GetFact")
                 && HasTag(m.Tags, IntegrationMetricDimensions.Status, "success"));

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.duration.ms"
                 && Math.Abs(m.Measurement - 42d) < 0.001
                 && HasTag(m.Tags, IntegrationMetricDimensions.TargetService, "kanon")
                 && HasTag(m.Tags, IntegrationMetricDimensions.Operation, "GetFact")
                 && HasTag(m.Tags, IntegrationMetricDimensions.Status, "success"));

        Assert.DoesNotContain(
            measurements,
            m => m.InstrumentName == "ontogony.integration.error.count");
    }

    [Fact]
    public void RecordFailure_Emits_Call_Error_And_Duration_With_ErrorCode()
    {
        var measurements = CaptureMeasurements();
        var meter = CreateMeter("caller-service");

        meter.RecordFailure(
            "conexus",
            "Complete",
            "timeout",
            TimeSpan.FromMilliseconds(99),
            new Dictionary<string, string> { [IntegrationMetricDimensions.HttpStatus] = "504" });

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.call.count"
                 && HasTag(m.Tags, IntegrationMetricDimensions.Status, "failure")
                 && HasTag(m.Tags, IntegrationMetricDimensions.ErrorCode, "timeout")
                 && HasTag(m.Tags, IntegrationMetricDimensions.HttpStatus, 504));

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.error.count"
                 && HasTag(m.Tags, IntegrationMetricDimensions.ErrorCode, "timeout"));
    }

    [Fact]
    public void Legacy_OntogonyMetrics_Maps_Integration_Name_To_Target_Service()
    {
        var measurements = CaptureMeasurements();

        OntogonyMetrics.RecordIntegrationCall("kanon", "GET", 200);
        OntogonyMetrics.RecordIntegrationDuration("kanon", "GET", 12.5);
        OntogonyMetrics.RecordIntegrationError("kanon", "GET", 503);

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.call.count"
                 && HasTag(m.Tags, IntegrationMetricDimensions.TargetService, "kanon")
                 && HasTag(m.Tags, IntegrationMetricDimensions.HttpStatus, 200)
                 && HasTag(m.Tags, IntegrationMetricDimensions.Status, "success"));

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.duration.ms"
                 && HasTag(m.Tags, IntegrationMetricDimensions.TargetService, "kanon"));

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.error.count"
                 && HasTag(m.Tags, IntegrationMetricDimensions.TargetService, "kanon")
                 && HasTag(m.Tags, IntegrationMetricDimensions.HttpStatus, 503)
                 && HasTag(m.Tags, IntegrationMetricDimensions.ErrorCode, "http_503"));
    }

    [Fact]
    public void StartCall_Disposes_Without_Emitting_Metrics()
    {
        var measurements = CaptureMeasurements();
        var meter = CreateMeter("caller-service");

        using (meter.StartCall("kanon", "GetFact"))
        {
        }

        Assert.Empty(measurements);
    }

    [Fact]
    public void IntegrationMetricDimensions_IsReserved_Recognizes_Standard_Keys()
    {
        Assert.True(IntegrationMetricDimensions.IsReserved(IntegrationMetricDimensions.SourceService));
        Assert.True(IntegrationMetricDimensions.IsReserved(IntegrationMetricDimensions.TargetService));
        Assert.True(IntegrationMetricDimensions.IsReserved(IntegrationMetricDimensions.Operation));
        Assert.True(IntegrationMetricDimensions.IsReserved(IntegrationMetricDimensions.Status));
        Assert.True(IntegrationMetricDimensions.IsReserved(IntegrationMetricDimensions.ErrorCode));
        Assert.True(IntegrationMetricDimensions.IsReserved(IntegrationMetricDimensions.HttpStatus));
        Assert.False(IntegrationMetricDimensions.IsReserved("retry_attempt"));
    }

    [Fact]
    public void StartCall_Ignores_Reserved_Dimensions_On_Activity_But_Keeps_Standard_Tags()
    {
        Activity? captured = null;
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == OntogonyDiagnostics.DefaultActivitySourceName,
            Sample = static (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            ActivityStopped = activity => captured = activity,
        };
        ActivitySource.AddActivityListener(listener);

        var meter = CreateMeter("caller-service");
        using (meter.StartCall(
            "kanon",
            "GetFact",
            new Dictionary<string, string>
            {
                [IntegrationMetricDimensions.SourceService] = "override-source",
                [IntegrationMetricDimensions.TargetService] = "override-target",
                [IntegrationMetricDimensions.Operation] = "override-operation",
                ["retry_attempt"] = "1",
            }))
        {
        }

        Assert.NotNull(captured);
        Assert.Equal("caller-service", captured.GetTagItem(IntegrationMetricDimensions.SourceService));
        Assert.Equal("kanon", captured.GetTagItem(IntegrationMetricDimensions.TargetService));
        Assert.Equal("GetFact", captured.GetTagItem(IntegrationMetricDimensions.Operation));
        Assert.Equal("1", captured.GetTagItem("retry_attempt"));
        Assert.Null(captured.GetTagItem(IntegrationMetricDimensions.Status));
    }

    [Fact]
    public void RecordSuccess_Ignores_Reserved_Dimensions_In_Caller_Supplied_Metric_Tags()
    {
        var measurements = CaptureMeasurements();
        var meter = CreateMeter("caller-service");

        meter.RecordSuccess(
            "kanon",
            "GetFact",
            TimeSpan.FromMilliseconds(10),
            new Dictionary<string, string>
            {
                [IntegrationMetricDimensions.SourceService] = "override-source",
                [IntegrationMetricDimensions.TargetService] = "override-target",
                [IntegrationMetricDimensions.Status] = "override-status",
                ["transport_policy"] = "retry-safe",
            });

        Assert.Contains(
            measurements,
            m => m.InstrumentName == "ontogony.integration.call.count"
                 && HasTag(m.Tags, IntegrationMetricDimensions.SourceService, "caller-service")
                 && HasTag(m.Tags, IntegrationMetricDimensions.TargetService, "kanon")
                 && HasTag(m.Tags, IntegrationMetricDimensions.Status, "success")
                 && HasTag(m.Tags, "transport_policy", "retry-safe")
                 && !HasTagKey(m.Tags, IntegrationMetricDimensions.TargetService, "override-target"));
    }

    private static IntegrationOperationMeter CreateMeter(string sourceService) =>
        new(Options.Create(new OntogonyObservabilityOptions { ServiceName = sourceService }));

    private static List<MeasurementCapture> CaptureMeasurements()
    {
        var measurements = new List<MeasurementCapture>();

        var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, meterListener) =>
        {
            if (instrument.Meter.Name == OntogonyDiagnostics.Meter.Name)
            {
                meterListener.EnableMeasurementEvents(instrument);
            }
        };

        listener.SetMeasurementEventCallback<long>((instrument, measurement, tags, _) =>
            measurements.Add(new MeasurementCapture(instrument.Name, measurement, tags.ToArray())));

        listener.SetMeasurementEventCallback<double>((instrument, measurement, tags, _) =>
            measurements.Add(new MeasurementCapture(instrument.Name, measurement, tags.ToArray())));

        listener.Start();
        return measurements;
    }

    private static bool HasTag(KeyValuePair<string, object?>[] tags, string key, object expected) =>
        tags.Any(tag => string.Equals(tag.Key, key, StringComparison.Ordinal)
                        && Equals(NormalizeTagValue(tag.Value), NormalizeTagValue(expected)));

    private static bool HasTagKey(KeyValuePair<string, object?>[] tags, string key, object unexpectedValue) =>
        tags.Any(tag => string.Equals(tag.Key, key, StringComparison.Ordinal)
                        && Equals(NormalizeTagValue(tag.Value), NormalizeTagValue(unexpectedValue)));

    private static object? NormalizeTagValue(object? value) =>
        value switch
        {
            int i => i,
            long l => l,
            string s when int.TryParse(s, out var parsed) => parsed,
            _ => value
        };

    private sealed record MeasurementCapture(
        string InstrumentName,
        double Measurement,
        KeyValuePair<string, object?>[] Tags);
}
