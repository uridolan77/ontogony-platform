# Master Implementation Prompt — Ontogony Operator V1 Build Package

Implement the Ontogony Operator V1 Build Package.

## Goal

Build a coherent first operator product on top of the governed runtime:

- operator home;
- guided governed-run workbench;
- protocol-aware evidence;
- Evidence Spine operator graph;
- Conexus route/usage/evidence panels;
- sandbox/tool-mode workbench;
- demo flows;
- first-version RC.

## Repos

- allagma-dotnet
- kanon-dotnet
- conexus-dotnet
- ontogony-frontend
- ontogony-platform
- ontogony-ui if needed

## Before coding

1. Pull/review current `main` in all repos.
2. Compare against `SYSTEM-ALPHA-006` lock.
3. Identify already-implemented pieces.
4. Do not duplicate existing pages or APIs.
5. Preserve hard boundaries.

## Hard boundaries

- No real external tool execution.
- No production-readiness claim.
- No enterprise IAM.
- No semantic authority outside Kanon.
- Model assistance remains draft-only/non-authoritative.

## Implement order

1. SYS-REAL-TOOLS-BLOCK-VERIFY-001A
2. SYS-POSTLOCK-DELTA-REGISTER-001
3. SYS-LOCAL-SETTINGS-HYGIENE-001
4. SYS-Q006-004-RESTART-PATH-DECISION
5. SYS-OPERATOR-HOME-001
6. ALLAGMA-OPERATOR-RUNS-001
7. SYS-PROTOCOL-RUNTIME-001
8. EVIDENCE-SPINE-OPERATOR-001
9. CONEXUS-OPERATOR-001
10. ALLAGMA-SANDBOX-WORKBENCH-001
11. SYSTEM-DEMO-FLOWS-001
12. FIRST-VERSION-RC-001

## Final acceptance

A local operator can run Ontogony, open the frontend, start and inspect governed runs, follow Kanon and Conexus evidence, handle human gates, inspect Evidence Spine, and clearly see that real external execution remains blocked.
