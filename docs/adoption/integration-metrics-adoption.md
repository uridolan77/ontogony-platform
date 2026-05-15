# Integration Metrics Adoption

This guide shows how to record outbound service integration metrics through `Ontogony.Observability`.

## Scope

Adopt shared measurement mechanics:

- `IIntegrationOperationMeter` for explicit success/failure recording
- `IntegrationMetricDimensions` for stable tag keys
- `ontogony.integration.call.count`, `ontogony.integration.error.count`, `ontogony.integration.duration.ms`

Do not add product-specific metric names or dimensions in platform packages.

## Wiring

```csharp
builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "agentor-api";
});
```

`AddOntogonyObservability` registers `IIntegrationOperationMeter` as a singleton.

## Recording pattern

```csharp
public sealed class KanonIntegrationClient
{
    private readonly IIntegrationOperationMeter _meter;

    public KanonIntegrationClient(IIntegrationOperationMeter meter)
    {
        _meter = meter;
    }

    public async Task<MyResponse> GetFactAsync(string factId, CancellationToken cancellationToken)
    {
        using (_meter.StartCall("kanon", "GetFact"))
        {
            var started = Stopwatch.GetTimestamp();
            try
            {
                var response = await _http.GetAsync($"/facts/{factId}", cancellationToken);
                var duration = Stopwatch.GetElapsedTime(started);

                if (response.IsSuccessStatusCode)
                {
                    _meter.RecordSuccess(
                        "kanon",
                        "GetFact",
                        duration,
                        new Dictionary<string, string>
                        {
                            [IntegrationMetricDimensions.HttpStatus] = ((int)response.StatusCode).ToString()
                        });
                }
                else
                {
                    _meter.RecordFailure(
                        "kanon",
                        "GetFact",
                        $"http_{(int)response.StatusCode}",
                        duration,
                        new Dictionary<string, string>
                        {
                            [IntegrationMetricDimensions.HttpStatus] = ((int)response.StatusCode).ToString()
                        });
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MyResponse>(cancellationToken)
                    ?? throw new InvalidOperationException("Empty response body.");
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _meter.RecordFailure("kanon", "GetFact", "transport_error", Stopwatch.GetElapsedTime(started));
                throw;
            }
        }
    }
}
```

`StartCall` opens a client `Activity` for tracing. Metrics are emitted only through `RecordSuccess` or `RecordFailure`.

## Legacy static helpers

Existing code may continue using:

```csharp
OntogonyMetrics.RecordIntegrationCall(clientName, method, (int)response.StatusCode);
OntogonyMetrics.RecordIntegrationDuration(clientName, method, durationMs);
OntogonyMetrics.RecordIntegrationError(clientName, method, (int)response.StatusCode);
```

These delegate to the same recorder and map historical `integration` parameters to `target_service`. They may omit `source_service`.

Prefer `IIntegrationOperationMeter` for new code so `source_service` is populated from `OntogonyObservabilityOptions.ServiceName`.

## Dimension rules

Reserved keys (callers cannot override these on metrics or trace activities):

```text
source_service
target_service
operation
status
error_code
http_status
```

Low-cardinality custom dimensions are allowed (for example `retry_attempt`, `transport_policy`).

## Do not do this

Do not use high-cardinality metric or activity dimensions:

```text
user_id
request_id
decision_id
trace_id
prompt_id
artifact_id
raw URL paths with identifiers
```

Use logs or request-scoped trace attributes for identifiers that would explode metric cardinality.

## Verification checklist

1. `AddOntogonyObservability` sets `ServiceName` to the calling service.
2. `target_service` names the downstream service (`kanon`, `conexus`, etc.), not a URL host with environment-specific noise.
3. `operation` is a stable logical name (RPC or HTTP verb + resource class), not a path with IDs.
4. Failures set a mechanical `error_code` (for example `timeout`, `http_503`, `transport_error`).
5. Reserved dimension keys are not passed in caller `dimensions` to override platform tags.

## Related

- [../observability/metrics-catalog.md](../observability/metrics-catalog.md)
- [../packages/Ontogony.Observability.md](../packages/Ontogony.Observability.md)
- [http-client-adoption.md](http-client-adoption.md)
