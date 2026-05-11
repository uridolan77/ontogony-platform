# Current capabilities (human-readable map)

Short **capability-oriented** overview of what the Athanor repo does **today**. For PR-by-PR detail, invariants, and historical notes, use [`CURRENT_IMPLEMENTATION_STATUS.md`](CURRENT_IMPLEMENTATION_STATUS.md). For “what Athanor means” in product terms, see [`ATHANOR_CONCEPTUAL_POSITION.md`](ATHANOR_CONCEPTUAL_POSITION.md).

---

## Identity and authority

- Workspaces, projects, actors, and **active** `project_actor_role` rows determine **project-local** authority.
- Central **policy** evaluation complements service checks; canonical-risk paths require appropriate roles and **ReviewEvent** (and related rules).
- **Human-only** finalization for canonical-risk accept/finalize paths; agents may propose and operate non-canonizing commands per policy.

## Source and evidence

- Sources, source versions, project attachment, **source chunks** (retrieval units), **evidence spans** (citation-grade binding).
- Source assessment / trust workflow and dashboard surfacing of trust metrics.

## Ingestion

- Source and version creation, chunk generation, processing jobs, workflow runs, ingestion indices (sources, versions, jobs, runs).
- **Ingestion candidate** persistence and read models linking proposals to jobs and evidence context.

## Extraction

- Manual extraction jobs (validated proposals), optional **generated** extraction (feature-flagged; fake/live provider seams as documented).
- Extraction profiles, observability fields on jobs/candidates, idempotency conventions (see [`INGESTION_IDEMPOTENCY.md`](INGESTION_IDEMPOTENCY.md)).

## Review

- Accept / reject / defer object versions with **ReviewEvent** targets.
- **Review inbox**: queue, claim, resolve, dismiss patterns; ingestion-origin queue and **human** execution of a strict allowlist from inbox (where implemented).
- Batch **queue-for-review** for ingestion links (triage only; no batch accept).

## Canonization

- Project object state per knowledge object; supersession and contradiction participation as modeled in core schema and services.
- Relations: candidate create, accept/reject with review.
- Contradictions: manual cases, participants, resolution with resolution + review events.

## Contradictions

- Open/all contradiction lists, case detail, integration with accepted state and object detail summaries (blocking counts, etc.).

## Snapshots and exports

- Create snapshot, list/detail, items, contradictions in snapshot, diff between snapshots, export (JSON/Markdown), snapshot hash verification.
- **Snapshot readiness** aggregated report and **canonical package export** (see API surface and operator guides).

## Graph / read-model projections

- Optional **Neo4j** projection driven from PostgreSQL (outbox, retries, dead-letter path when enabled).
- **Cockpit graph page**: client-composed canonical-state graph from GET payloads (not a second write path).

## Frontend cockpit

- **Internal-alpha cockpit** (Vite/React): dashboard, accepted state, review inbox (including claim), contradictions, snapshots (including readiness panel), traces, source trust, semantic candidates, ingestion page (create source/version, chunks, candidates, queue-for-review, batch queue), object detail, graph/provenance view.
- **Operator comfort**: saved contexts, paste-import of seed state JSON (full demo and transduction pilot), pilot checklist, readiness drawer, role switching for demo, structured error reporting where implemented.

## Operator comfort

- Seed scripts (`Seed-Demo`, `Seed-FullDemo`, `Seed-TransductionPilot`), demo verification scripts, CI demo smoke, contract parity script.
- Alpha operator/developer guides, risk register, freeze checklist.

## Known internal-alpha limits

- **No public auth**; caller-supplied `actorId` is trusted only in controlled environments.
- Cockpit does not expose every backend command; some flows are API/Bruno-first.
- Neo4j and projection behavior follow current docs—not a separate source of truth.

---

## Where to go next

| Need | Document |
| --- | --- |
| HTTP commands | [`API_SURFACE.md`](API_SURFACE.md) |
| Local DB + migrations order | [`LOCAL_SETUP.md`](LOCAL_SETUP.md), [`../db/migrations/README.md`](../db/migrations/README.md) |
| Ingestion map | [`INGESTION_SYSTEM_OVERVIEW.md`](INGESTION_SYSTEM_OVERVIEW.md) |
| Pilot rehearsal | [`TRANSDUCTION_PILOT_SCENARIO.md`](TRANSDUCTION_PILOT_SCENARIO.md), [`TRANSDUCTION_PILOT_OPERATOR_GUIDE.md`](TRANSDUCTION_PILOT_OPERATOR_GUIDE.md) |
| Alpha quick start | [`../README_ALPHA.md`](../README_ALPHA.md) |
