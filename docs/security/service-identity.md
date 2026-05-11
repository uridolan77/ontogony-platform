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

**Clock skew**: Requests outside `MaxTimestampSkew` from UTC now are rejected.

**Nonce replay**: When `RequireNonce` is `true`, an `INonceReplayStore` must reject reused nonces for the same service id. If no store is registered, verification fails.

**Body hashing**: The server recomputes the SHA-256 of the body (buffering may be required) and compares it to `X-Ontogony-Service-Body-Hash` using constant-time equality before validating the HMAC.

## Static shared secret mode (internal / development only)

When `RequireSignatureVerification` is `true` and `RequireHmacSignature` is `false`, the accessor performs **StaticSharedSecret** verification: the signature header must **exactly match** the configured secret string for that service id (constant-time UTF-8 byte comparison). This is **not** a keyed MAC over request bytes and must not be used as a public-internet authentication mechanism.

## Pluggability

- `IServiceSecretResolver` — resolves the shared secret for a service id (defaults to `ServiceSecrets` on `ServiceIdentityOptions` when no custom resolver is registered).
- `INonceReplayStore` — optional replay protection for HMAC mode.
- `IRequestBodyHashProvider` — computes the lowercase hex SHA-256 of the request body (defaults to a built-in provider; may be replaced if you pre-hash elsewhere).

## Registration

Use `AddOntogonyServiceIdentityActorContext` and configure `ServiceIdentityOptions`. Register `INonceReplayStore` (and optionally `IServiceSecretResolver`, `IRequestBodyHashProvider`) in DI when using HMAC mode with non-default behavior.
