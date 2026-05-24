# 12 — Risk register

| Risk | Impact | Probability | Mitigation |
|---|---:|---:|---|
| Replay is marketed as exact when it is only reconstructed | Very high | Medium | Mandatory mode labels and eligibility reasons. Exact replay disabled unless proven. |
| Real provider call accidentally occurs during replay | Very high | Medium | Default `providerExecutionPolicy = forbid_real_providers`; tests assert adapters are not invoked. |
| Real tool or side effect accidentally occurs during replay | Very high | Medium | Default `toolExecutionPolicy = forbid_real_tools`; Allagma tool ledger must mark replay as no-execute. |
| Allagma absorbs semantic authority | High | Low | Kanon owns semantic eligibility and replay bundle claims. Allagma only orchestrates. |
| Allagma absorbs model routing authority | High | Low | Conexus owns model-call/route-decision replay classification. |
| Replay creates a second Evidence Spine | High | Medium | Replay artifacts are new Evidence Spine nodes/edges under existing resolver/export contracts. |
| UI becomes another overloaded raw payload page | Medium | High | Use canonical primitives, progressive disclosure, one primary workflow. |
| Contract discipline falls behind new routes | High | Medium | Route/OpenAPI/client/catalog/usage/parity checklist required for each route. |
| Replay smoke becomes flaky gate too early | Medium | Medium | Keep replay smoke optional/manual until stable. |
| Replay bundles leak prompt/user/provider-sensitive data | High | Medium | Redaction metadata required; raw payloads omitted or hashed by default. |
| Kanon replay bundle semantics differ from Allagma replay modes | Medium | Medium | Shared replay vocabulary in Platform; mapping tests in Allagma/Kanon. |
| Conexus idempotency replay is confused with runtime replay | Medium | High | Audit and docs distinguish idempotency replay store from operator replay workflow. |
| Missing source snapshots make deterministic simulation impossible | Medium | High | Eligibility must report missing source data; fall back to evidence-only/reconstructed. |
| Existing governed fake E2E breaks | High | Low | Additive changes only; smoke remains valid before replay is added to it. |
| UI library primitive gap encourages page-local wrappers | Low | Medium | Temporary bridge policy and promotion/deletion condition. |
