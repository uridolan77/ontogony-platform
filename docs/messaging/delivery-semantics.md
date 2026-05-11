# Messaging delivery semantics

## Terms

| Term | Meaning in `Ontogony.Messaging` |
|------|--------------------------------|
| **Publish** | An envelope is validated (optional), optionally hashed, and accepted into the publisher pipeline. |
| **Capture** | For test and diagnostics, `InMemoryEventSink` stores a copy of published envelopes. This is **published-event capture**, not a broker offset ledger, not an outbox, and not a delivery guarantee. |
| **Dispatch** | Registered `IEventHandler<TPayload>` instances are invoked in process, in registration order (subject to failure policy). |
| **Handled** | A handler completed `HandleAsync` without throwing. |

## Operation modes (`EventPublisherOperationMode`)

- **`PublishAndDispatch`** — Default. Capture to sink (when using `InMemoryEventPublisher`), then invoke handlers.
- **`PublishOnly`** — Capture to sink and record publish-side metrics; **handlers are not invoked**.
- **`CaptureOnly`** — Append to the sink only (tests / spies); **no handler dispatch** and **no publish/dispatch observability counters** from this path (avoids implying a full publish pipeline).

## Results and failures

- `InMemoryEventSink` reflects **what was published to the publisher**, not whether handlers succeeded.
- `PublishWithResultAsync` returns `EventPublishResult` with per-handler timing and outcome. When `ContinueOnHandlerException` is `true`, all handlers are attempted and failures are aggregated; the sink still shows the published envelope.

## Metrics (`Ontogony.Observability`)

Counters and histograms use the `ontogony.event.publish.count`, `ontogony.event.dispatch.count`, `ontogony.event.dispatch.failure.count`, and `ontogony.event.handler.duration.ms` instruments on `OntogonyDiagnostics`, with tags including `event_type`, `protocol`, `operation_mode`, and `handler_description` where applicable.

## Cancellation

Handler invocations respect `CancellationToken`. If cancellation is signaled mid-dispatch, handlers not yet started may not run; results reflect what completed before cancellation.
