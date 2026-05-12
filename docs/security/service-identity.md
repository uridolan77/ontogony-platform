# Service identity verification

`Ontogony.Security` supports two mechanical verification styles for the service identity accessor. Neither replaces API authorization inside your domain.

## Header surface (HMAC production mode)

When `ServiceIdentityOptions.RequireHmacSignature` is `true`, callers should send:

| Header | Role |
|--------|------|
| `X-Ontogony-Service-Id` | Logical caller service id (configurable via `ServiceIdHeaderName`). |
| `X-Ontogony-Service-Timestamp` | Unix epoch seconds (UTC) as decimal string. |
| `X-Ontogony-Service-Nonce` | Unique nonce per request (recommended). |
| `X-Ontogony-Service-Body-Hash` | Lowercase hex SHA-256 of the raw request body (empty body hashes the empty byte sequence). |
| `X-Ontogony-Service-Signature` | Base64 encoding of `HMACSHA256(secret, canonical)` bytes. |

**Canonical string** (UTF-8, `\n` is a single newline character):

```text
{HTTP_METHOD}\n{PATH_AND_QUERY}\n{TIMESTAMP}\n{NONCE}\n{BODY_HASH}
```

- `HTTP_METHOD` is uppercased (for example `POST`).
- `PATH_AND_QUERY` is `Path.Value` plus `QueryString.Value` (query includes the leading `?` when present). If path is empty, `/` is used.

**Clock skew**: Requests outside `MaxTimestampSkew` from `IClock.UtcNow` are rejected. The accessor resolves `Ontogony.Primitives.IClock` from DI (registered by `AddOntogonyServiceIdentityActorContext` when missing); tests may inject a fixed clock.

**Nonce replay**: When `RequireNonce` is `true`, an `INonceReplayStore` must reject reused nonces for the same service id. If no store is registered, verification fails.

**Production clusters** must use a **distributed** `INonceReplayStore` (shared database, Redis, etc.). `InMemoryNonceReplayStore` is process-local only: it evicts by `NonceRetention` and caps size with `MaxStoredNonces`, but it does not coordinate across nodes.

## Body hashing, size limits, and async preload

- **`MaxSignedBodyBytes`** (default `1_000_000`): the server never hashes more than this many raw body bytes for comparison. Larger bodies fail verification.
- **`AllowUnsignedEmptyBody`** (default `true`): when the request is classified as definitely empty (for example `GET`/`HEAD`/`OPTIONS`/`TRACE`, or `Content-Length: 0`), a missing `X-Ontogony-Service-Body-Hash` header is treated as the hash of the **empty** byte sequence. For `POST`/`PUT`/`PATCH`/`DELETE`, callers should send the body hash header unless the body is provably empty the same way.

**`IRequestBodyHashProvider`** implements `TryComputeSha256HexLower(HttpRequest)` and returns `RequestBodyHashResult` (`Succeeded` / `TooLarge`). The default `Sha256RequestBodyHashProvider` uses `IOptions<ServiceIdentityOptions>` for limits.

**Recommended for ASP.NET Core hosts**: register `app.UseOntogonyServiceIdentityBodyHashPreload()` early in the pipeline (before endpoints read the body). The middleware asynchronously reads the body up to `MaxSignedBodyBytes`, stores the digest on `HttpContext.Items` (`ServiceIdentityBodyHashContext`), and swaps `HttpRequest.Body` with a bounded in-memory stream so the rest of the pipeline can re-read it. `ServiceIdentityCurrentActorAccessor` prefers this preload when present.

**`RequirePreloadedBodyHashForHmacBodies`** (default `false`): when `true`, HMAC verification for requests that are **not** classified as definitely empty refuses to fall back to synchronous body hashing via `IRequestBodyHashProvider`. Production hosts that set this must always run preload middleware ahead of actor resolution so body-bearing requests never trigger sync reads on the hot path. Empty-body fast paths (`AllowUnsignedEmptyBody` + definitely empty) may still verify without preload.

## Static shared secret mode (internal / development only)

When `RequireSignatureVerification` is `true` and `RequireHmacSignature` is `false`, the accessor performs **StaticSharedSecret** verification: the signature header must **exactly match** the configured secret string for that service id (constant-time UTF-8 byte comparison). This is **not** a keyed MAC over request bytes and must not be used as a public-internet authentication mechanism.

## Pluggability

- `IServiceSecretResolver` — resolves the shared secret for a service id (defaults to `ServiceSecrets` on `ServiceIdentityOptions` when no custom resolver is registered).
- `INonceReplayStore` — optional replay protection for HMAC mode.
- `IRequestBodyHashProvider` — computes the lowercase hex SHA-256 of the request body with explicit size results (defaults to `Sha256RequestBodyHashProvider`; may be replaced if you pre-hash elsewhere).

## Registration

Use `AddOntogonyServiceIdentityActorContext` and configure `ServiceIdentityOptions`. Register `INonceReplayStore` (and optionally `IServiceSecretResolver`, `IRequestBodyHashProvider`) in DI when using HMAC mode with non-default behavior. For production HMAC on mutating HTTP methods, also call `UseOntogonyServiceIdentityBodyHashPreload()`.
