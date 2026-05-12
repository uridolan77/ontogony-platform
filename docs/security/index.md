# Security & Compliance

This guide covers security practices and compliance requirements for Ontogony and services built on it.

---

## Core Principles

```text
1. Secrets are never logged or exposed.
2. All inter-service communication is authenticated and signed.
3. Trace IDs enable audit trails, not security.
4. Error responses never leak internal details.
5. Clock skew is bounded for timestamp validation.
```

---

## Authentication & Signing

### Service Identity

Each service has a **service identity** and **shared secret**:

```csharp
services.Configure<OntogonySecurityOptions>(opts =>
{
    opts.ServiceIdentity = "ontogony://order-service";
    opts.SharedSecret = configuration["Ontogony:SharedSecret"];
    opts.ClockSkew = TimeSpan.FromSeconds(30);
});
```

**Service identity format:**

```
ontogony://SERVICE-NAME
```

Examples:
- `ontogony://order-service`
- `ontogony://notification-service`
- `ontogony://payment-service`

**Never:**
- ❌ Use HTTP URLs as identity
- ❌ Include version or environment in identity
- ❌ Change identity without migration plan

---

### HMAC-SHA256 Signing

Inter-service requests are signed with HMAC-SHA256.

**Signature algorithm:**

```
CanonicalString = METHOD + "\n" + PATH + "\n" + TIMESTAMP + "\n" + NONCE + "\n" + BODY_SHA256

Signature = Base64(HMAC-SHA256(CanonicalString, SharedSecret))
```

**Example:**

```
Method: POST
Path: /api/events
Timestamp: 2026-05-12T12:34:56Z
Nonce: abc-123-def-456
BodyHash: sha256:1234567890abcdef...
CanonicalString: "POST\n/api/events\n2026-05-12T12:34:56Z\nabc-123-def-456\nsha256:1234567890abcdef..."
```

**Headers added to request:**

```http
X-Ontogony-Signature: SIGNATURE_VALUE
X-Ontogony-Timestamp: 2026-05-12T12:34:56Z
X-Ontogony-Nonce: abc-123-def-456
X-Ontogony-Body-Hash: sha256:1234567890abcdef...
```

---

### Signature Validation

**Clock skew check:**

```csharp
var requestTimestamp = DateTimeOffset.Parse(request.Headers["X-Ontogony-Timestamp"]);
var now = _clock.UtcNow;
var age = Math.Abs((now - requestTimestamp).TotalSeconds);

if (age > _options.ClockSkew.TotalSeconds)
    throw new SecurityException("Request too old");
```

**Nonce deduplication:**

```csharp
var nonce = request.Headers["X-Ontogony-Nonce"];
if (_nonceStore.Contains(nonce))
    throw new SecurityException("Replay attack: nonce already seen");

_nonceStore.Record(nonce);
```

**Signature verification:**

```csharp
var expectedSignature = ServiceIdentityHmacSignatureHelper.SignRequest(
    secret: _options.SharedSecret,
    method: request.Method.ToString(),
    path: request.Path,
    timestamp: requestTimestamp,
    nonce: nonce,
    bodyHash: actualBodyHash
);

if (expectedSignature.Signature != actualSignature)
    throw new SecurityException("Invalid signature");
```

---

## Secrets Management

### At Rest

**Never store secrets in code:**

```csharp
// ❌ Bad
private const string SHARED_SECRET = "super-secret-123";

// ✅ Good
var sharedSecret = configuration["Ontogony:SharedSecret"];
```

**Storage options:**

| Location | Use Case | Security |
|----------|----------|----------|
| Environment variables | Containers, Kubernetes | Encrypted at rest by orchestrator |
| Azure Key Vault | Azure hosted | Managed identity, RBAC, audit logs |
| HashiCorp Vault | Multi-cloud | Encryption, dynamic rotation, audit |
| Kubernetes Secrets | K8s | etcd encryption, RBAC |

**Recommendation:**

```bash
# Development
dotnet user-secrets set "Ontogony:SharedSecret" "dev-secret"

# Production
# Use environment variable or vault, never committed to git
```

### In Transit

**All inter-service communication must use HTTPS:**

```bash
# ❌ Bad
services.AddIntegrationHttpClient<IMyServiceClient>(c =>
    c.BaseAddress = new Uri("http://my-service")  // Plain HTTP
);

# ✅ Good
services.AddIntegrationHttpClient<IMyServiceClient>(c =>
    c.BaseAddress = new Uri("https://my-service")  // HTTPS
);
```

**Certificate validation:**

```csharp
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = 
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator  // ❌ Never in prod
};

// ✅ Good (default)
var handler = new HttpClientHandler();  // Uses system certificate store
```

### Rotation

**Plan for secret rotation:**

```
1. Generate new secret in vault
2. Deploy code that accepts both old and new secrets
3. Wait for all instances to start accepting new secret
4. Disable old secret in vault
5. Remove acceptance of old secret in next release
```

**Example:**

```csharp
var secrets = configuration.GetSection("Ontogony:Secrets").Get<string[]>();
// Try validation with each secret (new first, old second)
foreach (var secret in secrets)
{
    try
    {
        await ValidateSignature(request, secret);
        return;
    }
    catch (SecurityException) { }
}
throw new SecurityException("Invalid signature with all known secrets");
```

---

## Error Response Security

### Canonical Error Shape

Never expose internal details:

```json
{
  "type": "https://api.example.com/errors/validation",
  "title": "Validation Failed",
  "status": 400,
  "traceId": "trace-abc123"
}
```

**Never include:**

```json
{
  "detail": "Email 'user@example.com' is already taken",  // ❌ Leaks business logic
  "stackTrace": "at MyService.CreateUser() in Program.cs:line 42",  // ❌ Leaks implementation
  "innerException": "..."  // ❌ Leaks framework details
}
```

---

## Audit Trails

### Request Logging

Log request metadata (not secrets):

```json
{
  "timestamp": "2026-05-12T12:34:56Z",
  "traceId": "trace-abc123",
  "method": "POST",
  "path": "/api/orders",
  "remoteIp": "10.0.1.5",
  "userId": "user-123",
  "statusCode": 201,
  "durationMs": 42
}
```

**Never log:**

```
- Passwords
- API keys
- Shared secrets
- Personal data (unless explicitly required)
- Request/response bodies with sensitive data
```

### Event Audit

All events are signed with trace ID:

```csharp
var envelope = new OntogonyEnvelope<OrderCreatedEvent>
{
    EventId = Guid.NewGuid().ToString("n"),
    TraceId = OntogonyCorrelationContext.TraceId,  // Audit tie-in
    Source = "ontogony://order-service/domain",
    Payload = order
};
```

**Audit checks:**

1. Who initiated? (trace ID → request → actor)
2. What changed? (event type and payload)
3. When? (OccurredAt timestamp)
4. Where? (service source)
5. Why? (trace ID links to original request)

---

## Compliance Checklists

### GDPR

- [ ] PII is encrypted at rest
- [ ] PII is not logged (unless audit-required)
- [ ] Services can export user data on request
- [ ] Services can delete user data on request
- [ ] Data retention policy is enforced (automatic deletion)
- [ ] Logs are deleted after 90 days

### SOC 2 Type II

- [ ] All access is authenticated (HMAC signing)
- [ ] All changes are logged (trace IDs, event audit)
- [ ] Secrets are rotated (documented rotation process)
- [ ] Services are monitored (health checks, alerts)
- [ ] Incidents are logged and reviewed
- [ ] Code changes require review (PR checks)

### Internal Compliance

- [ ] Service identity follows naming convention
- [ ] Shared secrets are stored securely (not in code)
- [ ] All inter-service communication is HTTPS
- [ ] Error responses are canonical (no leaks)
- [ ] Conformance tests pass
- [ ] Security review completed before release

---

## Common Vulnerabilities

### Timing Attacks

**Risk:** HMAC comparison takes variable time depending on input.

**Mitigation:** Use `CryptographicOperations.FixedTimeEquals`:

```csharp
// ❌ Bad (timing-variable)
if (expected == actual) { }

// ✅ Good (constant time)
using System.Security.Cryptography;
if (CryptographicOperations.FixedTimeEquals(expected, actual)) { }
```

### Replay Attacks

**Risk:** Attacker resends captured signed request multiple times.

**Mitigation:** Nonce deduplication + clock skew.

```csharp
// Each request must have unique nonce
var nonce = Guid.NewGuid().ToString("n");

// Verify timestamp is recent
if (age > TimeSpan.FromSeconds(30))
    throw new SecurityException("Request too old");

// Check nonce not seen before
if (_seen.Contains(nonce))
    throw new SecurityException("Replay attack");
```

### Man-in-the-Middle

**Risk:** Attacker intercepts unencrypted communication.

**Mitigation:** HTTPS with certificate pinning (for critical paths).

```csharp
// ✅ Use HTTPS by default
var handler = new HttpClientHandler();  // Validates certificates

// ⚠️ Certificate pinning (only for very sensitive services)
handler.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) =>
{
    // Verify certificate subject thumbprint
    if (cert.Thumbprint == "expected-thumbprint") return true;
    return false;
};
```

### Secret Exposure

**Risk:** Secrets committed to git or logged accidentally.

**Mitigation:**

```bash
# Pre-commit hook
npm install husky
husky install
npx husky add .husky/pre-commit 'dotnet user-secrets validate'

# Git scanning
git log -p | grep -i "secret\|password\|token"

# Automated scanning
# Use GitHub secret scanning, GitGuardian, or Snyk
```

---

## Incident Response

### If a Shared Secret is Compromised

1. **Immediately** disable the secret in vault
2. **Generate** a new secret
3. **Deploy** code that validates both (old rotated, new primary)
4. **Monitor** for unauthorized requests with old secret
5. **Notify** all downstream services of new secret
6. **Update** documentation

### If a Service is Breached

1. **Isolate** the service (stop traffic, disable endpoints)
2. **Audit** logs to determine scope (trace IDs, affected users)
3. **Review** event logs for unauthorized changes
4. **Rotate** all associated secrets
5. **Patch** the vulnerability
6. **Redeploy** with new secret
7. **Verify** conformance tests pass
8. **Document** in post-mortem

---

## Security Checklist (Per Release)

- [ ] All secrets are environment variables or vault
- [ ] HTTPS configured for all endpoints
- [ ] HMAC signing working for inter-service calls
- [ ] Nonce deduplication is implemented
- [ ] Error responses don't leak internal details
- [ ] Logs don't contain secrets
- [ ] Security review completed
- [ ] Conformance tests passing
- [ ] Dependencies scanned for vulnerabilities
- [ ] Incident response plan documented

---

## Next Steps

1. **Review** current services for secret exposure
2. **Implement** HMAC signing for critical paths
3. **Audit** logs and error responses
4. **Test** with OWASP Top 10 checklist
5. **Schedule** regular security reviews

---

**Last Updated:** May 2026  
**Version:** 0.2.0
