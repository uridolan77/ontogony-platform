# Platform metrics catalog (`Ontogony.Platform`)

Frozen instrument logical names from `OntogonyDiagnostics`. Changes require a migration note and test updates in `DiagnosticsContractSmokeTests`.

| Instrument | Kind | Unit | Description |
| --- | --- | --- | --- |
| `ontogony.http.server.request.count` | Counter | — | HTTP requests observed by middleware |
| `ontogony.http.server.error.count` | Counter | — | HTTP errors (4xx/5xx per middleware policy) |
| `ontogony.http.server.duration.ms` | Histogram | ms | Request duration |
| `ontogony.integration.call.count` | Counter | — | Outbound integration calls |
| `ontogony.integration.error.count` | Counter | — | Outbound integration errors |
| `ontogony.integration.duration.ms` | Histogram | ms | Outbound integration duration |
| `ontogony.event.publish.count` | Counter | — | Events accepted for publish |
| `ontogony.event.dispatch.count` | Counter | — | Successful handler dispatch |
| `ontogony.event.dispatch.failure.count` | Counter | — | Handler dispatch failures |
| `ontogony.event.handler.duration.ms` | Histogram | ms | Per-handler dispatch duration |

## Product catalogs

| Repo | Catalog type |
| --- | --- |
| Allagma | `AllagmaMetricsCatalog` / `AllagmaMetricDefinition` |
| Conexus | Gateway metrics contracts + conformance tests |
| Kanon | Runtime meters (when exported) — document in Kanon repo |

Tag dimensions: [STANDARD_METRIC_TAGS.md](./STANDARD_METRIC_TAGS.md).
