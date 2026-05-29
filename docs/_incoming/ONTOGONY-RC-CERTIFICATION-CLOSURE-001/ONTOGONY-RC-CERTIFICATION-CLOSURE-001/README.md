# ONTOGONY-RC-CERTIFICATION-CLOSURE-001

Cross-repo development package to close the current Ontogony RC-certification blockers.

This package is intentionally **system-first**. It does not add major new features. It aligns runtime ports, fixes producer-trigger failures, closes route/package/metrics drift, improves live-cert diagnostics, and then forces a full rerun across all repositories.

## Target work items

1. `PLATFORM-RUNTIME-PORT-LOCK-ALIGNMENT-001`
2. `METABOLE-AISTHESIS-EVIDENCE-EDGE-LIVE-001`
3. `ALLAGMA-AISTHESIS-LIVE-TRIGGER-001`
4. `ALLAGMA-PHENOMENOLOGICAL-BRIDGE-CONTRACT-SYNC-001`
5. `KANON-PACKAGE-MODE-REPLAYTARGET-FIX-001`
6. `CONEXUS-CACHE-METRICS-ACCEPTANCE-FIX-001`
7. `AISTHESIS-LIVE-CERT-DIAGNOSTICS-001`
8. Full rerun gate: all repo tests, package-mode gates, five-service live cert, system cohesion acceptance.

## Intended outcome

Move the system from strong alpha / RC-certification partial to RC-candidate if and only if all live and package gates pass.

Production readiness remains explicitly **not claimed**.
