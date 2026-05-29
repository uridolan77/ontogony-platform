# Rollback plan

## If tests fail

1. Revert code changes first.
2. Keep docs that record failed state only if closeout is honest.
3. Do not change required-edge contract unless the failure is contract-related.

## If fixture smoke fails

1. Inspect batch ingestion.
2. Inspect required-edge evaluator.
3. Inspect fixture IDs and relations.
4. Inspect bundle/reconstructability mapping.
5. Revert scoring changes if they cause regression.

## If live proof fails

1. Do not modify Aisthesis first.
2. Identify missing producer evidence.
3. Check producer IDs and relation names.
4. Check Aisthesis read routes.
5. Record as live proof regression.

## If ReleaseMode fails

1. Check auth mode.
2. Check Postgres connection/migrations.
3. Check idempotency.
4. Check token config.
5. Do not silently fall back to in-memory unless marked as non-production.

## Rollback principle

Aisthesis must prefer an honest partial state over a fake pass.
