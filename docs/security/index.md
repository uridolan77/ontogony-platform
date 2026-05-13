# Security & Compliance

This guide covers **mechanical** security building blocks shipped in `Ontogony.Security` and how services should use them. It is not a substitute for your organization’s threat model, key management, or compliance program.
## Repository Security

- **[SECURITY.md](../../SECURITY.md)** — Vulnerability reporting, supported versions, and security scope.
- **[PLAT-NP-003 Supply-Chain Evidence](PLAT-NP-003-supply-chain-first-run-evidence.md)** — CodeQL, dependency review, SBOM, and supply-chain workflow proof.
---

## Core Principles

```text
1. Secrets are never logged or exposed.
2. Service-to-service trust uses explicit headers and HMAC (or dev-only static checks), not trace IDs.
3. Trace IDs are audit correlation — they are not authentication.
4. Error responses never leak internal implementation details.
5. Timestamp acceptance uses bounded clock skew (MaxTimestampSkew).
```

---

## Service identity (PR29 model)

Inbound requests can be attributed to a **calling service** through HTTP headers. Outbound calls can be signed by `OntogonyServiceIdentitySigningHandler` so downstream services reuse the same verification rules.

### Types and registration

| Concept | Role |
|--------|------|
| `ServiceIdentityOptions` | Header names, HMAC vs static mode, skew, nonce requirements, body limits, signing secret maps |
| `AddOntogonyServiceIdentityActorContext(...)` | Registers `ICurrentActorAccessor` backed by `ServiceIdentityCurrentActorAccessor` |
| `OntogonyServiceIdentitySigningHandler` | `DelegatingHandler` that stamps canonical headers and HMAC on **outgoing** `HttpClient` requests |
| `IServiceSigningSecretResolver` / `OptionsServiceSigningSecretResolver` | Resolves **current and previous** signing material for a service id and optional key id |
| `ServiceSigningSecret` / `ServiceSigningSecretSet` | One secret version (`KeyId`, `Secret`, `IsCurrent`) and the resolved set for verification |
| `IServiceSecretResolver` | Legacy single-secret resolution; used when `ServiceSigningSecrets` is empty |
| `INonceReplayStore` | Optional distributed store for nonce replay protection when `RequireNonce` is true |

### Canonical headers (`OntogonyServiceIdentityHeaders`)

These are the **default** names (override per property on `ServiceIdentityOptions` if needed):

```http
X-Ontogony-Service-Id
X-Ontogony-Service-Key-Id
X-Ontogony-Service-Timestamp
X-Ontogony-Service-Nonce
X-Ontogony-Service-Body-Hash
X-Ontogony-Service-Signature
```

- **`ServiceKeyIdHeaderName`** — default `X-Ontogony-Service-Key-Id`. When **`RequireKeyIdForHmacSignature`** is true, callers must send a key id so verification selects the correct signing secret during rotation.
- **Timestamp** is Unix epoch **seconds** as a decimal string (signing handler and accessor agree on this format).

### HMAC canonical string

`ServiceIdentityHmacSignatureHelper` signs the UTF-8 canonical string:

```text
METHOD + "\n" + PATH_AND_QUERY + "\n" + TIMESTAMP + "\n" + NONCE + "\n" + BODY_SHA256_HEX_LOWER
```

The signature header value is **Base64(HMAC-SHA256(UTF-8(canonical), UTF-8(secret)))**.

### Inbound configuration (actor context / verification)

```csharp
services.AddOntogonyServiceIdentityActorContext(opts =>
{
    opts.RequireHmacSignature = true;
    opts.MaxTimestampSkew = TimeSpan.FromSeconds(30);
    opts.RequireNonce = true;
    opts.RequireKeyIdForHmacSignature = true; // recommended in production with rotation

    // Dev / single-secret: map service id -> shared secret (see also IServiceSecretResolver)
    opts.ServiceSecrets["caller-service"] = configuration["Ontogony:CallerSecret"]!;

    // Rotation-friendly: multiple key ids per caller (OptionsServiceSigningSecretResolver)
    opts.ServiceSigningSecrets["caller-service"] =
    [
        new ServiceSigningSecret("k2026-02", configuration["Ontogony:CallerSecretPrev"]!, IsCurrent: false),
        new ServiceSigningSecret("k2026-05", configuration["Ontogony:CallerSecretCurrent"]!, IsCurrent: true),
    ];
});

// Optional: distributed nonce replay protection (production)
services.AddSingleton<INonceReplayStore, MyRedisNonceReplayStore>();
```

**Inbound HMAC validation** is part of **actor-context mechanics**: `ICurrentActorAccessor.Current` is populated only when headers verify according to `ServiceIdentityOptions` and resolved secrets.

### Body hash preload (`UseOntogonyServiceIdentityBodyHashPreload`)

For HMAC mode, verification needs a SHA-256 of the **raw request body** in the same form the sender hashed. Middleware must run **before** any middleware or endpoint reads the body stream.

```csharp
var app = builder.Build();

app.UseOntogonyServiceIdentityBodyHashPreload(); // before UseRouting / body-consuming middleware
app.UseRouting();
// ...
```

`ServiceIdentityBodyHashPreloadMiddleware` optionally buffers the body (subject to `MaxSignedBodyBytes`), stores a precomputed hash on `HttpContext.Items`, and replaces `HttpRequest.Body` with a rewindable stream so downstream components can still read the payload.

### Outbound signing

```csharp
services.AddHttpClient("downstream")
    .AddHttpMessageHandler(_ => new OntogonyServiceIdentitySigningHandler(
        serviceId: configuration["Ontogony:ServiceId"]!,
        secret: configuration["Ontogony:SigningSecret"]!,
        keyId: configuration["Ontogony:SigningKeyId"])); // optional; omit if not using key ids
```

Use a **real** `IServiceSigningSecretResolver` (or vault-backed options) in production — not hard-coded secrets in source control.

### Production checklist

- Use **HTTPS** for every hop; validate TLS certificates normally.
- Prefer **`ServiceSigningSecrets`** + **`RequireKeyIdForHmacSignature`** so you can rotate **current / previous** keys without dual secrets on every instance forever.
- Provide a **distributed** `INonceReplayStore`; the in-box defaults are not a replay database.
- Treat **`X-Ontogony-Trace-Id`** (from `Ontogony.Observability`) as correlation for audits and support — **not** proof of caller identity.

### Static shared secret (development only)

`RequireSignatureVerification` (without HMAC) compares the signature header to a configured string using a fixed-time UTF-8 compare. That is **not** an HMAC over the request and is unsuitable as a production inter-service authentication scheme.

---

## Secrets management

### At rest

Never store secrets in source:

```csharp
// Bad
private const string Secret = "hardcoded";

// Good
var secret = configuration["Ontogony:SigningSecret"];
```

Prefer vault or environment variables in production; use `dotnet user-secrets` or a secrets manager locally.

### Rotation (signing keys)

1. Add a new `ServiceSigningSecret` with a new `KeyId`, mark it `IsCurrent: true`, keep the previous key with `IsCurrent: false`.
2. Deploy senders so they stamp **`X-Ontogony-Service-Key-Id`** with the new id.
3. After traffic has moved, remove the old secret version from configuration.

---

## Error response safety

Use `Ontogony.Errors` mapping so unhandled exceptions become **canonical JSON** without stack traces or internal messages. See [Errors package](../packages/Ontogony.Errors.md) and [Examples](../examples/index.md).

---

## Audit trails

Log **metadata** (trace id, route, status, duration). Do not log signing secrets, raw HMAC values, or sensitive payloads.

```csharp
var envelope = new OntogonyEnvelope<OrderCreatedEvent>
{
    EventId = Guid.NewGuid().ToString("n"),
    TraceId = OntogonyCorrelationContext.TraceId,
    Source = "ontogony://order-service/domain",
    Payload = order
};
```

`TraceId` ties user-visible errors and logs together; it does not replace service identity verification.

---

## Common issues

### Timing-safe comparisons

Use `CryptographicOperations.FixedTimeEquals` when comparing fixed secrets or digests you manage manually.

### Replay protection

When `RequireNonce` is true, the platform expects `INonceReplayStore` to reject duplicates within your replay window.

### MITM

Use TLS everywhere; avoid custom certificate validation that disables chain validation in production.

---

## Compliance-oriented checklists

Use these as starting prompts for your own security reviews (Ontogony does not certify compliance):

- **Access:** service identity verified on inbound edges that matter; least privilege on data stores.
- **Logging:** no secrets; trace ids for correlation only.
- **Rotation:** documented process for signing key ids and connection strings.
- **Incident response:** rotate signing material and review `INonceReplayStore` / access logs after a suspected leak.

---

## Related reading

- [Adoption — Security & signing](../adoption/index.md)
- [PR29 migration notes](../migrations/2026-05-12-pr29-security-production-hardening.md)
- [PLAT-NP-003 supply-chain evidence (CodeQL / SBOM / submission)](./PLAT-NP-003-supply-chain-first-run-evidence.md)
- [Service identity production checklist](./service-identity-production.md)

---

**Last Updated:** May 2026  
**Version:** 0.3.0-alpha.1
