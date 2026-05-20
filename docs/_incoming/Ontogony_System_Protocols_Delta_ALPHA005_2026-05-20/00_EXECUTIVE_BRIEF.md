# Executive brief

## Verdict

Prepare the next system package as a **post-SYSTEM-ALPHA-005 protocol delta**, not as another general cohesion package.

The original protocol/cohesion package was directionally correct, but many of its P0 recommendations now exist in code and docs:

- compatibility/environment/auth/route/test matrices exist;
- runtime lock exists;
- Allagma model purposes use Conexus aliases rather than hard-coded provider models;
- cross-service error envelope exists in Ontogony.Errors and Allagma mapping docs;
- context propagation exists;
- system cohesion smoke exists;
- Conexus tool/tool_choice/function-call pass-through exists;
- Kanon connection evidence has advanced through KANON-CONNECT-001..007;
- frontend Evidence Spine B-013 has been cleared.

The next phase should therefore be narrower and more exacting:

1. Close B-012 / SYS-OBS-004A observability quarantine.
2. Revalidate current moving-main deltas.
3. Cut a new runtime lock only after validation.
4. Create a machine-readable protocol registry.
5. Add stale incoming-package detection.
6. Audit generated/handwritten feature matrices against actual route inventories.
7. Keep real external tool execution blocked.

## Recommended sprint name

`SYSTEM-PROTOCOL-DELTA-006 — Post-Alpha-005 protocol reconciliation and lock hardening`

## Definition of done

A new developer/operator can answer:

- What is the current cut baseline?
- Which repos and commits are locked?
- Which current-main changes are outside the lock?
- Which route/auth/env/error/evidence contracts are canonical?
- Which evidence gates are PASS?
- Which quarantines remain?
- Which old package items are superseded?
- Which safety boundaries are still enforced?
