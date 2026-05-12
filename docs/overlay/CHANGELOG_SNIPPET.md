PR48–PR52 — logging, redaction, secrets, quotas, replay contracts:

- **New package `Ontogony.Logging`:** structured log field constants, stable event IDs, correlation scope helper, ASP.NET logging-scope middleware, and service registration for provider-neutral logging mechanics.
- **New package `Ontogony.Redaction`:** deterministic redaction primitives, sensitive-field matching, default redactor, safe value helpers, and DI registration.
- **New package `Ontogony.Secrets`:** secret references, safe display metadata, development-only protector, secret fingerprinting, and masking helpers for provider credentials.
- **New package `Ontogony.Quotas`:** quota scopes, windows, limits, consumption records, decisions, in-memory quota ledger, and DI registration.
- **New package `Ontogony.Replay.Contracts`:** replay manifest/input/output/environment/determinism DTOs for later deterministic debugging; contracts only, no replay engine.
- **Docs:** added package docs and model docs for logging, redaction, secrets, quotas, and replay.
- **Tests:** added focused unit tests for constants, masking, protection/fingerprints, quota decisions, and replay DTO round-trips.
