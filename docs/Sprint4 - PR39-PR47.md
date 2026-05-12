# Main assessment

## What is strong

The platform now has the right shape for Conexus.NET:

```text
request tracing
error handling
HTTP resilience
canonical event envelope
AI telemetry DTOs
usage/cost records
artifact references
execution journal
idempotency
Postgres outbox
test fixtures
hosting defaults
```

The `IExecutionJournal` design is appropriately mechanical. It records execution facts and avoids workflow semantics. Its remarks explicitly say duplicate primary IDs are rejected, list APIs return append order, and DTOs do not validate lifecycle consistency. 

`InMemoryExecutionJournal` is a useful reference implementation for tests and starter services, but it correctly warns that it is not durable and not suitable as a multi-node source of truth. 

This is the right style.

## What is not polished

The README is stale. It still lists the old `src/` layout and omits `Ontogony.AI.Contracts`, `Ontogony.Artifacts`, `Ontogony.Execution`, and `Ontogony.ProtocolIngress` from the main tree. 

It also still has contradictory HTTP resilience notes:

```text
HTTP resilience // linear backoff; Retry-After / jitter not implemented
HTTP resilience // supports Retry-After and jitter
```

That should be cleaned before using the repo as the canonical base.

The repo also still targets `.NET 9` centrally: `Directory.Build.props` sets `TargetFramework` to `net9.0`, and `global.json` pins SDK `9.0.100` with `rollForward: latestFeature`.  

Package versions are centralized, which is good, but they are also pinned to the .NET 9 package line. 

For the future Conexus.NET starter, we should centralize framework decisions and avoid scattering package choices.

---

# Strategic decision

I would **not add major new packages now**.

The current AI-runtime plan still lists future packages like `Ontogony.Redaction`, `Ontogony.Quota`, `Ontogony.AI.Replay`, `Ontogony.Knowledge.Contracts`, `Ontogony.Evaluation.Contracts`, and `Ontogony.Policy.Contracts`. 

Those are tempting, but they are too much before the first real Conexus.NET consumer.

The right next move is:

```text
Freeze expansion.
Polish the platform.
Use it in Conexus.NET.
Only add new platform packages when Conexus.NET actually needs them.
```

---

# Recommended final platform shape

Think of Ontogony.Platform as four clean levels.

## Level 0 — Foundation

```text
Ontogony.Primitives
Ontogony.Hashing
Ontogony.Configuration
```

Purpose:

```text
time
IDs
canonical JSON
hashing
startup validation
```

## Level 1 — Service mechanics

```text
Ontogony.Hosting
Ontogony.Observability
Ontogony.Errors
Ontogony.Http
Ontogony.Security
```

Purpose:

```text
ASP.NET service defaults
trace/correlation
error shape
resilient outbound HTTP
service identity / actor context
```

## Level 2 — Event and consistency mechanics

```text
Ontogony.Contracts
Ontogony.Messaging
Ontogony.Idempotency
Ontogony.Persistence
Ontogony.Persistence.Postgres
Ontogony.ProtocolIngress
```

Purpose:

```text
envelopes
events
idempotency
outbox
processed-message tracking
protocol normalization
```

## Level 3 — AI runtime mechanics

```text
Ontogony.AI.Contracts
Ontogony.Artifacts
Ontogony.Execution
```

Purpose:

```text
LLM request/response telemetry
usage/cost/error records
large payload references
execution journal facts
checkpoints
```

That is enough for Conexus.NET v1.

---

# Plan to polish and finalize Ontogony.Platform

## PR39 — Repository truth and documentation cleanup

Goal: make the repo describe what it actually is after PR38.1.

Tasks:

```text
Update README package layout to include all 18 packages.
Remove stale “first adoption path” language that assumes existing repos matter.
Replace adoption-first framing with “starter substrate” framing.
Remove contradictory HTTP resilience notes.
Add “Current finalized package layers” section.
Add “Do not add product semantics” section.
Add “Conexus.NET starter target” section.
```

The README should say:

```text
Ontogony.Platform is the mechanical infrastructure base for new Ontogony services.
It is safe to break before v1 because no external consumers exist.
The first target consumer is Conexus.NET.
```

This should replace the older “careful adoption” language.

## PR40 — Package layering and dependency rules

Goal: make the package graph intentional and enforceable.

Tasks:

```text
Create docs/architecture/package-levels.md.
Define Level 0, 1, 2, 3 exactly.
Add package dependency matrix.
Add forbidden dependency rules.
Add script validate-package-levels.ps1.
Add CI step for package-level validation.
```

Important rule:

```text
Lower levels must not depend on higher levels.
AI runtime packages must not depend on product packages.
Contracts must stay provider-neutral.
Execution must not depend on Artifacts directly.
```

`Ontogony.Execution` currently avoids depending on `Ontogony.Artifacts`, using only opaque `PayloadArtifactId`; that is good and should be preserved. 

## PR41 — Breaking cleanup: remove duplicate/old surfaces

Because backward compatibility does not matter, clean aggressively.

Tasks:

```text
Review Ontogony.Messaging vs Ontogony.Persistence outbox types.
Remove or rename duplicated old IOutboxStore / OutboxMessage surfaces if still present.
Keep durable outbox contracts only under Ontogony.Persistence.
Keep event publishing only under Ontogony.Messaging.
Delete obsolete docs that refer to old APIs.
Regenerate package index.
```

Target boundary:

```text
Ontogony.Messaging = publish/dispatch mechanics
Ontogony.Persistence = outbox/processed/dead-letter mechanics
```

No two packages should define competing “outbox message” concepts.

## PR42 — Normalize naming and contract consistency

Goal: make the public API feel like one designed system.

Tasks:

```text
Review all public record names.
Review all DI extension method names.
Review all option names.
Review all status/result types.
Ensure every package has a package README.
Ensure each package has “What this is / What this is not.”
Ensure all public opaque strings are documented as opaque.
Ensure all in-memory implementations say test/single-process only.
```

Naming convention I recommend:

```text
AddOntogonyX()
UseOntogonyX()
MapOntogonyX()
I<X>Store only for storage abstractions
I<X>Journal only for append/read journals
I<X>Publisher only for dispatch/event emission
```

## PR43 — Conexus.NET readiness package check

Goal: ensure the platform has exactly what Conexus.NET needs, no more.

Create:

```text
docs/consumer-blueprints/conexus-dotnet-platform-readiness.md
```

It should specify the minimal package set for Conexus.NET:

```text
Required:
- Ontogony.Hosting
- Ontogony.Observability
- Ontogony.Errors
- Ontogony.Http
- Ontogony.Security
- Ontogony.Idempotency
- Ontogony.Hashing
- Ontogony.Contracts
- Ontogony.AI.Contracts
- Ontogony.Artifacts
- Ontogony.Execution

Optional later:
- Ontogony.Messaging
- Ontogony.Persistence
- Ontogony.Persistence.Postgres
- Ontogony.ProtocolIngress
- Ontogony.Testing
```

Also add a proposed Conexus.NET request flow:

```text
POST /v1/chat/completions
→ request tracing
→ project API key auth
→ idempotency/fingerprint
→ route resolution in Conexus domain
→ provider call through Microsoft.Extensions.AI / IChatClient
→ LlmRequestEnvelope
→ LlmResponseEnvelope or LlmProviderError
→ usage/cost record
→ optional ArtifactRef for raw payload
→ optional ExecutionRunRecord for internal processing trace
```

No routing policy should go into Ontogony.Platform.

## PR44 — Framework and version centralization

Goal: make the starter zip easy and future-proof.

Current state:

```text
TargetFramework = net9.0
SDK = 9.0.100
Microsoft.Extensions.* = 9.0.0
```

That is centralized, which is good.  

For finalization, add:

```text
docs/FRAMEWORK_BASELINE.md
```

It should state:

```text
Current platform baseline.
Supported SDK.
Target framework.
Central package version file.
Upgrade procedure.
Starter-template baseline.
```

Before generating the Conexus.NET zip, we should decide whether to keep `net9.0` or move the platform/starter to the current stable .NET line. I cannot verify external Microsoft package recency from this chat environment, so I would not hardcode claims about latest versions here; the repo should make the framework version a single centralized decision.

## PR45 — CI and release polish

Current CI already restores, builds, tests, validates docs, validates AI runtime boundaries, packs, and generates a manifest. 

Polish it:

```text
Add package-level validation.
Add stale docs/API-name validation for new AI packages.
Add solution package-count check.
Add README package-count check.
Add pack smoke for all packages.
Add one “starter consumer” compile test.
```

The starter consumer test is important. Add something like:

```text
examples/ConexusDotNetSkeleton/
```

Not a product yet — only a compile-time smoke project proving that the platform packages can be consumed together.

## PR46 — Version reset for “single-dev no-compat” mode

Because no one consumes it, do not pretend SemVer stability.

Set the next version as:

```text
0.3.0-alpha.1
```

or:

```text
0.3.0-dev
```

But document the new rule:

```text
Before 1.0, breaking changes are allowed.
No migration notes are required for unused internal consumers.
Only final package shape matters.
```

Once Conexus.NET starts using it:

```text
0.4.x = Conexus.NET incubation baseline
1.0.0 = after Conexus.NET boots, calls provider, records telemetry, and passes smoke
```

## PR47 — Delete speculative roadmap or demote it

The AI runtime implementation order currently encourages several future packages. 

Keep the ideas, but demote them to “parking lot”:

```text
docs/parking-lot/future-platform-packages.md
```

Do not let them drive the next work.

Current focus:

```text
Finish platform.
Start Conexus.NET.
```

Future packages only when demanded by Conexus.NET:

```text
Redaction — when request/response logging needs safe storage.
Quota — when project limits need enforcement.
Replay — when provider request replay/debug becomes real.
Policy.Contracts — when governance decisions become shared facts.
```

---

# Finalized platform acceptance criteria

Before starting Conexus.NET starter zip, I would require:

```text
1. README accurately lists all packages and levels.
2. Package index matches solution package count.
3. No duplicated outbox abstractions.
4. All packages have “what this is / what this is not.”
5. CI validates docs, package levels, boundary terms, build, tests, pack.
6. Framework baseline is centralized and documented.
7. Conexus.NET readiness blueprint exists.
8. Example skeleton compiles against the platform.
9. Changelog clearly marks PR38.1 as current state.
10. Future package roadmap is parked, not active.
```

---

# My recommended next sequence

```text
PR39 — Docs truth + README/package map cleanup
PR40 — Package levels + dependency validation
PR41 — Breaking API cleanup, especially Messaging/Persistence outbox boundary
PR42 — Public naming/API consistency pass
PR43 — Conexus.NET readiness blueprint
PR44 — Framework/version baseline docs
PR45 — CI/release/starter-consumer smoke
PR46 — Version reset to 0.3.0-alpha.1
PR47 — Move speculative packages to parking lot
```

After that, stop polishing.

Then generate the Conexus.NET starter zip.

---

# Final judgment

The current Ontogony.Platform after PR38.1 is **good enough architecturally**, but not yet **finished enough as a foundation**.

The right move now is not more packages.

It is:

```text
clean
level
document
validate
freeze
then consume
```

Once this polish pass is done, Conexus.NET can start cleanly as the first serious consumer:

```text
Conexus.NET
  built on Ontogony.Platform
  using Microsoft.Extensions.AI / IChatClient-style model access
  exposing OpenAI-compatible gateway APIs
  recording all LLM calls through Ontogony.AI.Contracts
  using Artifacts for large payloads
  using Execution for internal operation traces
```

That is the right way to avoid yet another unfinished codebase: finish the base, then build one concrete service on it.
