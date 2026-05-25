# 09 — Doc generation and validation plan

## Proposed checks

- broken link check
- command existence check
- generated artifact existence check
- docs index completeness check
- stale-doc marker check
- no duplicate canonical guide titles
- glossary term coverage check

## Verification commands to include after inspection

- existing docs link/check script if present
- `contracts:discipline` if contract docs or frontend route/catalog docs are touched
- frontend typecheck only if docs import code, route catalogs, or generated schemas are touched
- backend tests only if generated docs are changed by scripts
- markdown lint if present

## Command existence rule

Every command in `docs/learn/*` must be verified against one of:

- `package.json` scripts
- `.ps1` / `.sh` scripts in repo
- `.csproj` / solution test command
- documented Docker compose file

If a command is proposed but not implemented, prefix with `Proposed:` and do not present it as runnable.
