# Current State and Score Targets

## Baseline verdict

The four backend repos are already a credible alpha runtime:

- Allagma orchestrates the governed loop.
- Kanon owns semantic authority.
- Conexus owns model gateway semantics.
- Ontogony.Platform owns shared mechanics.
- The system has typed clients, runtime locks, compatibility manifests, evidence routes, audit bundles, Postgres durability paths, and local smoke scripts.

The gap to `9+` is not mostly missing architecture. It is missing **certification discipline**.

## What `9+` means in this package

| Score range | Meaning |
|---|---|
| `8.x` | Implemented, documented, locally plausible |
| `9.0+` | Machine-gated, cross-repo validated, reproducible at pinned commits |
| `9.3+` | Includes negative-path tests, restart/durability, observability, evidence bundle, and stale-doc cleanup |
| `9.5+` | Release-candidate quality with live proof, operator runbooks, and no ambiguous ownership |
| `10` | Production/GA: enterprise IAM, SLO enforcement, scale/load proof, security review, operational maturity |

## Area-specific targets

### Cross-repo integration

Move from “typed clients + matrix + scripts” to:

- lock-pinned latest heads;
- release-mode lock validation;
- package-mode build/test;
- sibling-source full cohesion smoke;
- Kanon manifest conformance;
- Conexus streaming/client contract conformance;
- evidence index closeout.

### Runtime orchestration

Move from “Allagma coordinates the loop” to:

- every run state transition tested;
- every failure class mapped;
- retry/cancel/replay semantics proven;
- streaming lifecycle required;
- restart survival required;
- real tool execution explicitly blocked.

### Semantic authority

Move from “Kanon RC1 is strong” to:

- compatibility manifest as hard release gate;
- v0 freeze enforced;
- semantic negative paths tested;
- Postgres semantic smoke release-gated;
- assistance review loop included in full cohesion smoke.

### Model gateway

Move from “Conexus gateway strong” to:

- gateway certification matrix;
- fallback/quota/idempotency/streaming edge cases tested;
- model-call evidence bundle and usage drilldown tested;
- project/admin isolation tested;
- stale docs corrected;
- certified provider surface explicit.

### Shared platform mechanics

Move from “useful alpha substrate” to:

- platform RC contract;
- public API snapshot gate;
- protocol registry refreshed;
- consumer package-mode validation;
- operator failure taxonomy and evidence spine validators in CI.

### Evidence / audit / operator spine

Move from “contracts and links exist” to:

- one golden cross-service evidence journey;
- negative missing-evidence cases;
- deterministic evidence graph;
- redaction report;
- current artifacts indexed in release evidence.

### Observability

Move from “designed but PASS pending” to:

- live `observability-summary.json` with `verdict: PASS`;
- metrics/traces/logs checked for Allagma, Kanon, Conexus;
- dashboards provisioned;
- degraded modes classified;
- no retracted/pending PASS language remains.
