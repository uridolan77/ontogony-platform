# Migration: PR16–PR17 (Security HMAC hardening + Contracts polish)

## Ontogony.Security

- **`IRequestBodyHashProvider`** now exposes `RequestBodyHashResult TryComputeSha256HexLower(HttpRequest)` instead of returning an unbounded synchronous `string`. Implementations must respect `ServiceIdentityOptions.MaxSignedBodyBytes` (default 1,000,000).
- **`Sha256RequestBodyHashProvider.Instance`** was removed. Use DI (`AddOntogonyServiceIdentityActorContext` registers a default singleton) or `new Sha256RequestBodyHashProvider(serviceIdentityOptions)` / `new Sha256RequestBodyHashProvider(Options.Create(...))`.
- **`ServiceIdentityOptions`**: new `MaxSignedBodyBytes` and `AllowUnsignedEmptyBody` (see `docs/security/service-identity.md`).
- **`InMemoryNonceReplayStore`**: constructor accepts `InMemoryNonceReplayStoreOptions` (`NonceRetention`, `MaxStoredNonces`) and optional `Func<DateTimeOffset>` clock for tests. Still not suitable for multi-node production; use a distributed `INonceReplayStore`.
- **Pipeline**: optional `app.UseOntogonyServiceIdentityBodyHashPreload()` for async bounded body reads (see `OntogonySecurityApplicationBuilderExtensions`).

## Ontogony.Contracts

- **`DefaultEnvelopeValidator`**: `Source` must be an **absolute URI** (`UriKind.Absolute`). Error text is now consistent with the JSON schema description.
- **`CloudEventConversionOptions`**: new `AllowNullCloudEventData` (default `false`). When `false`, `ToOntogonyEnvelope` throws if `CloudEventEnvelope.Data` is null. Non-nullable value types still throw when data is null even if the option is `true`.

## Downstream repos

- **Athanor / Agentor / Conexus**: if you implemented `IRequestBodyHashProvider`, update to `TryComputeSha256HexLower`. If envelopes used non-URI `Source` strings, fix sources to absolute URIs or relax validation outside the platform.
