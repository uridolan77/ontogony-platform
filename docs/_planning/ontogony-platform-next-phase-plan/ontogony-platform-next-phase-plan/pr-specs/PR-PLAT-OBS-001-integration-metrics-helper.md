# PR-PLAT-OBS-001 — Integration Metrics Helper

## Purpose

Make outbound service integration metrics consistent.

## Proposed API

```csharp
public interface IIntegrationOperationMeter
{
    IDisposable StartCall(string targetService, string operation, IReadOnlyDictionary<string, string>? dimensions = null);
    void RecordSuccess(string targetService, string operation, TimeSpan duration, IReadOnlyDictionary<string, string>? dimensions = null);
    void RecordFailure(string targetService, string operation, string errorCode, TimeSpan duration, IReadOnlyDictionary<string, string>? dimensions = null);
}
```

## Metric names

```text
ontogony.integration.call.count
ontogony.integration.error.count
ontogony.integration.duration.ms
```

## Dimensions

```text
source_service
target_service
operation
status
error_code
http_status
```

## Non-goals

No product-specific metric names or dimensions.
