# Execution Plan

## Phase 0 — workspace hygiene

1. Confirm branches and clean working trees.
2. Confirm .NET SDK `9.0.203` or compatible `9.0.x`.
3. Confirm Docker Desktop is healthy.
4. Stop all ad-hoc service processes.
5. Start the canonical five-service stack only after port lock verification is updated.

## Phase 1 — runtime identity alignment

Implement `PLATFORM-RUNTIME-PORT-LOCK-ALIGNMENT-001`.

No live cert should be trusted until:

```text
:5081 = Kanon
:5082 = Conexus
:5083 = Allagma
:5084 = Metabole
:5085 = Aisthesis
```

## Phase 2 — producer-trigger closure

Implement `METABOLE-AISTHESIS-EVIDENCE-EDGE-LIVE-001` and `ALLAGMA-AISTHESIS-LIVE-TRIGGER-001`.

Goal: Aisthesis live cert must receive at least one valid producer trace from Metabole and one valid producer trace from Allagma.

## Phase 3 — contract/package/metrics drift cleanup

Implement:

1. `ALLAGMA-PHENOMENOLOGICAL-BRIDGE-CONTRACT-SYNC-001`
2. `KANON-PACKAGE-MODE-REPLAYTARGET-FIX-001`
3. `CONEXUS-CACHE-METRICS-ACCEPTANCE-FIX-001`

Goal: repo-local suites and package gates stop failing on known drift.

## Phase 4 — live-cert diagnostics

Implement `AISTHESIS-LIVE-CERT-DIAGNOSTICS-001` so future failures show exact service identity, trigger URL/body/profile, HTTP status, response excerpt, trace extraction result, producer observation status, and missing edge list.

## Phase 5 — full rerun

Run the canonical rerun sequence in `scripts/run-full-rc-certification-rerun.ps1`.

## Phase 6 — closeout

Write one system-level closeout plus one closeout per repo touched.
