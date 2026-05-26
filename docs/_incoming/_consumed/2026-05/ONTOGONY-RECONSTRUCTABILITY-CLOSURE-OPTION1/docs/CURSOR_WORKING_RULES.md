# Cursor working rules

## Start every PR by reading

```text
README.md
03_IMPLEMENTATION_ORDER.md
04_BOUNDARY_GUARDS.md
05_ACCEPTANCE_CRITERIA.md
pr-specs/<current-pr>.md
```

## Never do these

```text
Do not implement Conexus before Allagma classifier closure.
Do not implement frontend before golden trace.
Do not weaken Kanon classifier to pass weak events.
Do not move product semantics into Platform.
Do not introduce real provider keys.
Do not enable real external tool execution.
Do not hide failing tests behind broad filters.
```

## Always do these

```text
Inspect git diff first.
Record commands run.
Keep docs/current-state accurate.
Regenerate route inventories when routes change.
Add tests before claiming closure.
Prefer improving evidence over weakening rules.
Preserve trace/correlation identity.
Use redaction-safe hashes for user/provider payloads.
```

## Implementation notes

Use this file in each repo:

```text
docs/_incoming_active/ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1/IMPLEMENTATION_NOTES.md
```

Record:

```text
repo
branch
commands
test results
files changed
known failures
environment-gated failures
next action
```
