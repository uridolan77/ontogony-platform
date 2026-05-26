# Trace attribute catalog (`ontogony.*`)

Frozen keys from `OntogonySpanAttributes`. Apply using `SystemCorrelationConventions.ApplyToActivity`.

| Attribute | Semantics |
| --- | --- |
| `ontogony.trace_id` | Distributed trace id (`X-Ontogony-Trace-Id`) |
| `ontogony.operation_id` | Operation / correlation id |
| `ontogony.actor_id` | Current actor |
| `ontogony.tenant_id` | Tenant scope |
| `ontogony.workspace_id` | Workspace scope |
| `ontogony.project_id` | Project scope (Conexus project context) |
| `ontogony.session_id` | Session scope |
| `ontogony.protocol` | Protocol discriminator on integration/event spans |
| `ontogony.event_type` | Event type on dispatch spans |

## HTTP semantic attributes (coexisting)

Middleware may also set:

```text
http.request.method
url.path
http.response.status_code
service.name
```

These follow OpenTelemetry HTTP conventions and are not renamed to `ontogony.*`.
