# Cursor prompt — 02 frontend taxonomy adapters

In `ontogony-frontend`, add adapter functions that convert backend/page-specific state into the shared taxonomy.

## Adapter families

Create or update adapters for:

- service health/readiness;
- compatibility summary;
- evidence completeness;
- source attempts;
- Agent Interaction modes;
- topology edges;
- credential/source labels;
- evaluation quality/evidence rows;
- Kanon authorization/actor capability summaries;
- Conexus provider/routing posture;
- Allagma run/runtime posture.

## Rules

- Preserve reasons from source data.
- If a field is missing, say which field is missing.
- If state is inferred, mark authority as `inferred`.
- If data came from fixtures or generated artifacts, mark it visibly.
- Do not convert partial/degraded states to green just because one endpoint succeeded.

## Output

Frontend pages should receive neutral view models rather than raw backend DTOs where possible.
