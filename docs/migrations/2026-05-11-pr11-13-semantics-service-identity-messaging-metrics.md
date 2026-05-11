# Migration: PR11–PR13 semantics, service identity HMAC, messaging metrics (2026-05-11)

## Who is affected

- Services that configure **service identity** headers or dashboards that referenced **legacy event metric names**.
- Consumers of `Ontogony.Messaging` that relied on `Ontogony.Messaging` having **no** dependency on `Ontogony.Observability`.

## Service identity headers

`ServiceIdentityOptions` defaults now use Ontogony-prefixed headers (see `OntogonyServiceIdentityHeaders`), including `ServiceIdHeaderName` defaulting to `X-Ontogony-Service-Id` instead of `X-Service-Id`.

**Action:** Update callers still sending `X-Service-Id` to either send the new header or set `ServiceIdHeaderName` (and related header names) explicitly to the legacy values.

## Static shared secret mode

`RequireSignatureVerification` without `RequireHmacSignature` is documented as **StaticSharedSecret** mode (literal secret comparison). Prefer **`RequireHmacSignature`** with `INonceReplayStore` for production service-to-service calls.

## OpenTelemetry metric instruments

The following instruments were **renamed or replaced**:

| Removed | Replacement |
|---------|-------------|
| `ontogony.event.published.count` | `ontogony.event.publish.count` (tag `operation_mode` added) |
| `ontogony.event.handled.count` | Use `ontogony.event.dispatch.count` and `ontogony.event.handler.duration.ms` |

New instruments:

- `ontogony.event.dispatch.count`
- `ontogony.event.dispatch.failure.count`
- `ontogony.event.handler.duration.ms`

**Action:** Update dashboards and alerts to the new names.

## Messaging package dependency

`Ontogony.Messaging` now references `Ontogony.Observability` so `InMemoryEventPublisher` can emit metrics when `EventDispatchOptions.RecordObservabilityMetrics` is `true` (default).

**Action:** If you need a metrics-free dependency graph, set `RecordObservabilityMetrics` to `false` on your publisher options (metrics types are still present transitively).

## New APIs (non-breaking)

- `InMemoryEventPublisher.PublishWithResultAsync` / `InMemoryEventBus.PublishWithResultAsync`
- `EventPublisherOperationMode` on `EventDispatchOptions`
- HMAC-related types in `Ontogony.Security` (`ServiceIdentityHmacSignatureHelper`, `INonceReplayStore`, etc.)

No required code changes unless you opt into HMAC mode or depend on the old metric names.
