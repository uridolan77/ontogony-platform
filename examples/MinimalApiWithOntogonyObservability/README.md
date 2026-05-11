# MinimalApiWithOntogonyObservability

This example demonstrates PR2 adoption:

- AddOntogonyObservability registration
- UseOntogonyRequestTracing middleware
- Outbound HttpClient correlation propagation through Ontogony.Http

## Run

```powershell
dotnet run --project examples/MinimalApiWithOntogonyObservability/MinimalApiWithOntogonyObservability.csproj
```

## Verify

1) Call the root endpoint to see a generated trace id:

```powershell
curl http://localhost:5000/
```

2) Call propagation endpoint with incoming correlation headers:

```powershell
curl -H "X-Ontogony-Actor-Id: actor-1" -H "X-Ontogony-Tenant-Id: tenant-1" -H "X-Ontogony-Workspace-Id: workspace-1" -H "X-Ontogony-Project-Id: project-1" -H "X-Ontogony-Session-Id: session-1" http://localhost:5000/demo/propagation
```

The response includes:

- incoming: correlation values seen by request tracing middleware
- outboundObservedByUpstream: values observed by a loopback upstream endpoint called via a named Ontogony integration client

The outboundObservedByUpstream payload should include the same correlation fields and W3C trace headers (traceparent/tracestate).
