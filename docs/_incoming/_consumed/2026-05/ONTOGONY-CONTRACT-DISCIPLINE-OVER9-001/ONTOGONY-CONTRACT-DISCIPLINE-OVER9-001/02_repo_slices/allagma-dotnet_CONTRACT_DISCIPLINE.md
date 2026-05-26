# AGM-CONTRACT-001 to AGM-CONTRACT-007 — Allagma contract-discipline slice

## Objective

Raise Allagma contract discipline above 9 by making it the clean runtime truth owner.

## AGM-CONTRACT-001 — Runtime lock promotion protocol

Codify and execute the promotion protocol:

1. verify latest compatible commits across four repos;
2. run validation gates;
3. update locked commits;
4. update evidence pointers;
5. reset or shrink post-lock delta register;
6. update matrix.

Acceptance:

- `validate-runtime-lock.ps1 -ReleaseMode` passes;
- `post-lock-deltas.json` no longer carries stale resolved work.

## AGM-CONTRACT-002 — Feature connection matrix hardening

Feature matrix must classify each feature as implemented backend, partial backend, frontend consumed, evidence present, blocked intentionally, or not started.

## AGM-CONTRACT-003 — Allagma route/OpenAPI inventory discipline

Make Allagma route inventory as strict as Kanon’s: generated route inventory, OpenAPI snapshot, route doc fragments, and replay route ordering tests.

## AGM-CONTRACT-004 — Cross-service replay contract freeze

Freeze replay eligibility response, bundle response, downstream result classification, retry/idempotency semantics, and not-raw-prompt evidence rules.

## AGM-CONTRACT-005 — Error contract discipline

Allagma external errors should use stable cross-service envelope where possible, with documented exceptions.

## AGM-CONTRACT-006 — Model-purpose alias contract

Allagma config must never regress to provider model strings. Test scans infrastructure code/config defaults for forbidden concrete model usage outside examples/tests.

## AGM-CONTRACT-007 — Package-mode and sibling-mode reconciliation

Package-mode and sibling-source mode must agree on contract surfaces. Package version drift must fail validation.
