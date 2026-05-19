# 07 — Scenario dataset checklist

Primary route: `/allagma/evaluations/datasets`

## Dataset route checks

- [ ] Route loads without crash
- [ ] Dataset list (or explicit empty state) renders clearly
- [ ] Dataset detail/expansion behavior (if present) is stable
- [ ] Dataset identifiers and labels are readable and consistent

## Boundary checks

- [ ] Surface remains read-only (no authoring workflow implied)
- [ ] No fake create/edit success patterns are present
- [ ] Any missing backend data is represented honestly

## Integration checks

- [ ] Dataset IDs referenced by eval dashboard filters align with this route
- [ ] Navigation from datasets to related eval surfaces (if present) is valid
- [ ] No contradictory language about production data quality appears

## Error/degraded checks

- [ ] Unauthorized state is explicit when applicable
- [ ] Network/degraded state is explicit when applicable
- [ ] Empty state guidance is operator-usable

## Evidence

- [ ] Screenshot: dataset list state
- [ ] Screenshot: dataset empty/degraded state (if encountered)
- [ ] Note: read-only contract verified
