# Durability Boundaries and In-Memory Implementations

## Overview

Ontogony.Platform ships several abstractions that manage state (execution journals, artifact stores, quota ledgers, etc.). Most have **in-memory reference implementations** to enable local development and testing. This document clarifies when these are appropriate and when durable implementations should be required.

## Current Status

### In-Memory Implementations

Ontogony provides in-memory reference implementations for:

- **Execution Journal** (`Ontogony.Execution`)
- **Artifact Store** (`Ontogony.Artifacts`)
- **Quota Ledger** (`Ontogony.Quotas`)
- **Persistence/Outbox** (`Ontogony.Persistence`)

These are **registered via DI helpers** like:

```csharp
services.AddOntogonyInMemoryExecutionJournal();
services.AddOntogonyInMemoryArtifactStore();
services.AddOntogonyInMemoryQuotaLedger();
services.AddOntogonyInMemoryOutboxStore();
```

### Startup Warnings

When in-memory implementations are registered **outside of Development environment**, Ontogony logs a warning at startup:

```
WARN: Ontogony in-memory execution journal is not durable. Use for development/test only.
```

This is **intentional**. The in-memory implementations are:

✅ **Suitable for:**
- Local development with `ASPNETCORE_ENVIRONMENT=Development`
- Automated tests
- Bootstrap scenarios
- Prototypes

❌ **Not suitable for:**
- Production deployments
- Multi-instance scenarios (state is not shared)
- Long-running services requiring persistence
- High-availability setups

## Revival Criteria for Durable Implementations

Ontogony should add durable implementations only when:

### Criterion 1: Multiple Consumers Need Identical Semantics

At least **two independently-developed services** (Conexus, Agentor, Athanor, etc.) demonstrate they need the same reusable durable behavior.

- **Single consumer:** That service owns the implementation.
- **Multiple consumers with identical contracts:** Ontogony adopts as a shared primitive.
- **Multiple consumers with divergent semantics:** Each service maintains its own.

### Criterion 2: The Semantics Are Genuinely Reusable

The behavior must be **mechanical and value-agnostic**, not tied to product policy:

- **Reusable:** "Persist an opaque event record with timestamp and idempotency key."
- **Not reusable:** "Apply Conexus quota policy," "check Agentor plan limits," "enforce Athanor approval rules."

### Criterion 3: Evidence Is Collected

Before implementing a durable store:

1. Document which consumers need it and why.
2. Collect use cases in `docs/planning/robustness/DEFERRED_ITEMS.md` with links to consumer code/issues.
3. Run a spike or PoC if the design is unclear.

## Deferred Durable Implementations

### Durable Execution Journal

**Status:** Deferred; consumer-pressure driven.

**Why:** Conexus and Agentor have not yet applied cross-service execution tracing at scale. Ontogony's in-memory journal is adequate for MVP tracing within a single service.

**Revival:** When two services independently propose a durable execution store contract, move forward.

**Owner:** Ontogony (once consensus exists) or individual service if consensus never forms.

### Durable Artifact Store

**Status:** Deferred; consumer-pressure driven.

**Why:** Artifact retention policies are often product-specific (Conexus may want S3, Agentor may want Blob Storage, etc.). In-memory is suitable for testing.

**Revival:** When a common cloud-agnostic contract emerges (e.g., "save binary blob, retrieve by ID, list by namespace").

**Owner:** Ontogony if unified, or individual service providers if divergent.

### Durable Quota Ledger

**Status:** Deferred (PR-PLAT-012 remains deferred).

**Why:** Quota mechanics are tightly coupled to product pricing and rate limits. Conexus owns its EF-based quota ledger; Agentor/Athanor have not articulated a shared need yet.

**Revival:** If two or more services standardize on identical quota window / ledger semantics (e.g., "rolling per-minute, per-tenant"), consider a generic mechanical store. Until then, each service owns its quota enforcement.

**Owner:** Individual services (Conexus, Agentor, etc.) own quota policy. Ontogony provides only mechanical building blocks (e.g., window arithmetic).

## Guidelines for Service Owners

### If you own a service and need a durable store:

1. **Start with in-memory** during alpha/MVP.
2. **Implement your own** in your service repo (e.g., `Conexus.Persistence.QuotaStore`).
3. **Document** the contract and semantics clearly.
4. **Propose to Ontogony** only if another service needs identical behavior and has already prototyped a working implementation.

### If proposing a shared implementation to Ontogony:

Provide:

- Use cases from **at least two services** with code links.
- A **contract specification** (interfaces, behavior, edge cases).
- Evidence that the behavior is **not tied to product policy** (no "Conexus pricing," no "Agentor approval rules").
- A **spike or POC** showing the design is feasible.
- **Test coverage** of the mechanical behavior, not the consumer logic.

## Ontogony's Role

Ontogony will:

- ✅ Ship **mechanical primitives** (options, validation, hashing, signing, tracing, etc.).
- ✅ Ship **in-memory reference implementations** for testing and bootstrap.
- ✅ Add **durable shared implementations** when evidence supports it.
- ❌ NOT add **product-specific stores** (those belong in product repos).
- ❌ NOT ship **cloud-specific SDKs** (each service integrates its cloud provider).

`PLAT-NP-008` governs warning coverage for current in-memory registrations only. It does not change the deferred, consumer-pressure-driven posture for durable execution, artifact, or quota stores.

## See Also

- [DEFERRED_ITEMS.md](../../planning/robustness/DEFERRED_ITEMS.md) — detailed status of each deferred item and measurement.
- [PLAT_ROBUSTNESS_SEQUENCE.md](../../planning/robustness/PLAT_ROBUSTNESS_SEQUENCE.md) — PR roadmap and ownership.
- [AGENTS.md](../../AGENTS.md) — shared infrastructure vs. product semantics.
