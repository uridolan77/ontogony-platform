# Migration and Storage Notes

## Storage strategy

For first slice, prefer lightweight storage consistent with each repo's current pattern. Do not introduce a large new persistence layer unless the repo already uses one.

Recommended sequence:

1. Contract DTOs.
2. Fixture/in-memory provider.
3. Deterministic file-backed fixture if needed.
4. Real persistence only after contract and lifecycle are stable.

## Immutable content storage

Skill content should be immutable by version.

Store as:

```text
contentUri
contentHash
tokenEstimate
createdAt
baseVersionId
```

## Version ids

Use stable, readable fixture ids for demos and test ids for generated cases. Production ids can be ULID/GUID style if existing repos use that.

## Backward compatibility

Do not require existing runs/model calls/decisions to have skill metadata. Existing records should render as:

```text
skillInjection.enabled = false
skillVersionId = null
```

## Data retention

Rejected edits and gate results should be retained because they are part of the optimization history. If pruning is later required, preserve summaries and hashes.

## Security / safety

Skills may influence agent behavior. Treat published/deployed skill versions as governed artifacts:

- require actor identity;
- preserve audit trail;
- support rollback;
- avoid global default deployment;
- prevent unreviewed high-risk edits from deployment.
