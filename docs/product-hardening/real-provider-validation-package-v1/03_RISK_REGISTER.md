# 03 — Risk Register

| Risk | Severity | Control |
| --- | --- | --- |
| Secrets committed | Critical | Local env only, grep/secret scan, evidence redaction |
| CI real calls | Critical | No CI trigger, manual local script only |
| Unexpected cost | High | max calls, token caps, manual confirmation |
| Raw provider payload leak | High | sanitized evidence only |
| Provider outage misclassified | Medium | classify auth/network/rate-limit separately |
| Fake-provider regression | High | rerun fake-provider smoke after real-provider validation |
| TLS/proxy issue | Medium | reuse local CA injection discipline if needed |
| Frontend hides provider error | Medium | explicit unavailable/auth/rate-limit state |

## Failure classes

- missing key
- invalid key
- provider auth
- provider network/TLS
- provider rate limit
- model/config mismatch
- budget exceeded
- Conexus routing defect
- Allagma orchestration defect
- frontend visibility defect
- evidence/redaction defect
- docs/operator mismatch
