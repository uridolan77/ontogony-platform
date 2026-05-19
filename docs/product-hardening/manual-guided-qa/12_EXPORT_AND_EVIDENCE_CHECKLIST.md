# 12 — Export and evidence checklist

Collect auditable evidence from core product-hardening surfaces.

## Required exports

- [ ] Evaluation detail evidence export (`/allagma/evaluations/{id}`)
- [ ] Run detail triage evidence export (`/allagma/runs/{id}`)
- [ ] Replay workbench evidence export (`/allagma/replay`)
- [ ] Kanon provenance/replay evidence export when route data allows

## Export quality checks

- [ ] File download succeeds or explicit error captured
- [ ] Export payload includes the expected subject ID
- [ ] Export payload timestamp is current for session
- [ ] Export payload reflects redaction policy (no secrets)

## Evidence package structure (per execution run)

Suggested folder:

```text
artifacts/manual-qa/<date>/<session-id>/
  screenshots/
  exports/
  notes/
  results.md
```

Checks:

- [ ] Screenshots stored for each checklist step
- [ ] Exports stored with route + ID in filename
- [ ] Notes include pass/fail + reproduction steps for failures
- [ ] `13_RESULTS_TEMPLATE.md` completed for this session

## Boundary checks

- [ ] No secrets committed into evidence artifacts
- [ ] No production readiness statement in evidence conclusions
- [ ] Limitations are recorded as limitation outcomes, not silently ignored

## Exit criteria for this step

- [ ] All required artifacts collected or failures documented with cause
- [ ] Package is ready for `PRODUCT-MANUAL-QA-002` execution evidence closeout
