# Recommended sprint plan

## Sprint 0 — Quarantine and baseline hygiene

1. `SYS-OBS-004A` — close B-012 Docker OTLP + Grafana readiness.
2. `SYS-E2E-REVALIDATE-006` — re-run full cohesion suite against current moving-main.
3. `SYS-EVIDENCE-INDEX-006` — reconcile evidence indexes to Alpha-005 plus moving-main deltas.
4. `SYS-LOCK-006` — cut next runtime lock only after validation.

## Sprint 1 — Protocol truth spine

5. `SYS-PROTOCOL-REGISTRY-001` — create machine-readable protocol registry.
6. `SYS-STALE-PACKAGE-GUARD-001` — stale incoming-package detector.
7. `SYS-CONNECT-MATRIX-AUDIT-001` — audit Allagma feature connection matrix against generated source inventories.

## Sprint 2 — Cross-service operator evidence

8. `KANON-CONNECT-LOCK-001` — promote KANON-CONNECT 001–007 into next baseline decision.
9. `CONEXUS-EVIDENCE-FLOW-001` — connect Conexus model-call evidence flow into frontend/Evidence Spine.
10. `CONEXUS-POSTLOCK-001` — classify post-lock Conexus deltas.

## Sprint 3 — Operator posture and safety invariants

11. `ALLAGMA-RUNTIME-POSTURE-001` — expose/consume runtime posture.
12. `SYS-REAL-TOOLS-BLOCK-VERIFY-001` — recurring verification that real external execution remains blocked.

## Work explicitly not in this package

- Production hardening.
- Enterprise IAM.
- Real external tool execution.
- New product/domain features unrelated to protocol truth.
