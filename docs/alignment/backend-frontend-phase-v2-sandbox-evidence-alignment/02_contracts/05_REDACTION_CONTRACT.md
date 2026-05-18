# Redaction Contract

## Operator-visible by default

Safe: IDs, statuses, phases, timestamps, effect fingerprints, relative paths, executor refs, bounded reason codes, bounded failure classes, registry versions, and contract versions.

Potentially sensitive: absolute local paths, raw marker content, raw tool inputs, raw prompts, raw model outputs, secrets, bearer tokens, provider headers, and environment variables.

## Frontend display rule

Default UI must show `Raw content hidden by default`.

For future explicit opt-in raw views, require backend support, role/auth check, redaction pass, audit event, warning state, and no secrets.
