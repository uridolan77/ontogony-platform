# 09 — Acceptance Checklist

## Package setup acceptance (`PFH-000`)

- [ ] Zip copied to `docs/_incoming/`.
- [ ] Package unpacked to `docs/product-hardening/eval-alignment-frontend-depth/`.
- [ ] README and manifest present.
- [ ] PR specs present.
- [ ] Evidence file added.
- [ ] No runtime code changes.
- [ ] No workflow changes.
- [ ] No secrets.
- [ ] Not production readiness.

## Audit acceptance (`PFH-001`)

- [ ] All relevant eval routes inventoried.
- [ ] Frontend eval/replay surfaces inventoried.
- [ ] OpenAPI/generated client state recorded.
- [ ] Fixture/live/degraded states mapped.
- [ ] Product gaps separated from production-readiness gaps.
- [ ] First implementation PR confirmed.
- [ ] Evidence added.

## Implementation acceptance

Every product implementation PR must include goal, boundary, contract/OpenAPI impact, frontend impact, fixture/live/degraded state, tests, evidence, known limitations, and a not-production-readiness statement.
