# SYSTEM-COH-001 current-state basis

## Important correction from earlier reviews

Earlier cohesion reviews identified several gaps that may already be partly or fully closed in the current repos. Cursor must verify before acting.

Likely already present:

1. **Allagma system matrices** — `docs/system/README.md` may already list compatibility, environment, auth, route, test, runtime correctness, trace/context, and feature-connection matrices.
2. **Runtime lock** — `ontogony-runtime.lock.json`, `ONTOGONY_RUNTIME_LOCK.md`, and `validate-runtime-lock.ps1` may already exist.
3. **Model-purpose aliasing** — Allagma may already map model purposes to `ConexusModelAlias` and forbid provider SDK/routing policy in Allagma.
4. **Streaming purpose support** — Allagma may already support opt-in streaming per model purpose and document Conexus streaming idempotency boundaries.
5. **Trace/context propagation** — Allagma may already document `traceparent`, `X-Ontogony-Correlation-Id`, actor headers, derived downstream idempotency keys, and `X-Allagma-Run-Id`.
6. **System cohesion E2E helper** — `scripts/lib/system-cohesion-e2e.ps1` may already exercise correlation chain, downstream audit references, and service health snapshots.
7. **Evidence Spine enriched chain** — frontend/Kanon/platform may already support enriched Kanon roots and review/fact/audit fan-out.

Do not produce stale fixes for closed issues. SYSTEM-COH-001 should become a baseline closure / evidence certification sprint.

## Still valuable work

The remaining high-value work is to create a hard, auditable acceptance layer across what already exists:

- single closeout report;
- machine-readable acceptance matrix;
- release-mode validator;
- evidence artifact schema;
- optional-vs-required smoke classification;
- observability evidence gate;
- operator-console inspection proof;
- known deferral register;
- exact “alpha-closed” definition.

## Current risk areas to verify

| Risk | Why it matters | Expected handling |
|---|---|---|
| Matrix drift | Multiple docs can disagree as repos evolve quickly | validator + generated summary |
| Optional smoke ambiguity | Optional flags can hide missing live proof | release-mode classification |
| Moving-main drift | runtime lock may pin commits while docs mention main | post-lock delta register check |
| Error shape inconsistency | services intentionally expose different public error shapes | compatibility matrix + Allagma translation proof |
| Observability evidence | metrics/scripts may exist without a recorded live pass | evidence gate and artifact pointer |
| Real tools | simulation-only status must remain explicit | blocked conformance + trust model link |
| Frontend/operator proof | backend evidence without operator visibility is incomplete | Evidence Spine / console route coverage proof |
