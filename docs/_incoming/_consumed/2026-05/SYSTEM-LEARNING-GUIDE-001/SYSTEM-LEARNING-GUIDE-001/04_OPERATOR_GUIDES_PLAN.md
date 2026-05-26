# 04 — Operator guides plan

## 02_RUN_LOCAL_SYSTEM.md must include

- prerequisites verified from repo scripts
- Docker/local-working-system sequence
- expected ports: document only after verifying scripts/config
- health/readiness checks
- frontend URL
- common failures/fixes
- clear distinction between live, fixture, and fake-provider mode

## 03_GOVERNED_FAKE_E2E.md must include

- what governed fake proves
- exact scripts and artifacts
- how to inspect Evidence Spine and Agent Interaction after the run
- runtime-lock relation
- expected pass/fail signals

## 04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md must include

- health vs readiness vs contract discipline
- System Truth page semantics
- release readiness artifact vs RC certification
- runtime-lock evidence relationship

## 14_DEBUGGING_PLAYBOOK.md must include

- service offline
- readiness not ready
- contract mismatch
- missing route decision
- missing Kanon decision
- partial Evidence Spine graph
- Domain Switcher mismatch
- Docker frontend stale bundle
- contract discipline failure
- runtime-lock artifact missing
