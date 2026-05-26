# KCP-003 — Kanon settings semantic health polish

## Problem

The Settings page contains useful Kanon state, but role guidance, credential warnings, and source labels are too noisy or ambiguous.

## Scope

- Improve Kanon semantic health copy.
- Add role-preset guidance.
- Reduce repeated credential/local-storage warnings.
- Replace `unknown source` when storage mode is known.
- Preserve health warnings truthfully.

## Implementation details

Role guidance:

```text
Local admin — Admin: mutation-capable local operator.
Read-only semantic authority — Auditor, ProvenanceReader: inspect packs, provenance, decisions, and evidence without lifecycle mutation.
System service — System: service-to-service automation where configured.
```

Credential source labels should prefer:

```text
session
local
default
env
not set
unknown
```

`unknown` should only appear when the UI truly cannot infer source.

## Acceptance

- Settings no longer repeats the same credential warning under every field.
- Kanon role/capability state is clearer.
- Health payload warning remains visible but is not duplicated excessively.
