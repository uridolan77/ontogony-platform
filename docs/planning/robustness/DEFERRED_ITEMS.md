# Deferred robustness items

This register records **deferred** platform work from [`PLAT_ROBUSTNESS_SEQUENCE.md`](./PLAT_ROBUSTNESS_SEQUENCE.md) so status is explicit (not “silent backlog”). Status for the full sequence is summarized there; this file holds **rationale** and **revival criteria**.

## PR-PLAT-003 — Typed HTTP client support for Ontogony HTTP resilience

| Field | Detail |
| --- | --- |
| **Status** | Deferred / not implemented |
| **Rationale** | No current Ontogony consumer required a separate typed-client pipeline beyond the existing correlation-aware `HttpClient` registration and policies already used by integrators (including Conexus). Adding typed clients would increase surface area without proven cross-service demand. |
| **Revival criteria** | Implement when a shipping consumer (Conexus, Agentor, Athanor, or a new service) needs first-class typed clients **and** that need cannot be met by the current `Ontogony.Http` registration patterns without duplicating policy wiring in each consumer. |

## PR-PLAT-012 — Durable quota ledger design spike

| Field | Detail |
| --- | --- |
| **Status** | Deferred / not implemented |
| **Rationale** | Only Conexus has pressed for a **product** quota story with durable enforcement today; that ledger lives in Conexus (EF-backed). A platform-wide durable quota store would need multiple consumers to justify shared semantics-free mechanics. |
| **Revival criteria** | Re-open when **at least one consumer besides Conexus** needs the same durable quota ledger abstraction, or when Conexus and platform agree to lift a proven mechanical ledger into Ontogony with no gateway/plan semantics. |

## Related review

Post-PLAT-011 judgment and per-PR status: [`docs/planning/next-phase/CURRENT_REVIEW_AFTER_PLAT011.md`](../next-phase/CURRENT_REVIEW_AFTER_PLAT011.md).
