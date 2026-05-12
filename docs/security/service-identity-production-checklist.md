# Service Identity Production Deployment Checklist

Use this checklist when enabling HMAC service identity in production.

- [ ] `RequireHmacSignature = true` for participating services.
- [ ] `RequireNonce = true` with a distributed `INonceReplayStore`.
- [ ] `RequirePreloadedBodyHashForHmacBodies = true` for body-bearing endpoints.
- [ ] `UseOntogonyServiceIdentityBodyHashPreload()` added before `UseRouting()`.
- [ ] `EnableBodyHashPreloadOrderDiagnostics = true`.
- [ ] `ThrowOnBodyHashPreloadOrderViolation = true` in production.
- [ ] `RequireKeyIdForHmacSignature` explicitly set to desired policy.
- [ ] `IServiceSigningSecretResolver` returns current + previous keys during rollout.
- [ ] Outbound callers use `OntogonyServiceIdentitySigningHandler`.
- [ ] Key rotation runbook validated in a non-production environment.
- [ ] HMAC vectors verified against [hmac-signing-vectors.md](./hmac-signing-vectors.md).
- [ ] Secrets are sourced from secure configuration providers, never source-controlled.
