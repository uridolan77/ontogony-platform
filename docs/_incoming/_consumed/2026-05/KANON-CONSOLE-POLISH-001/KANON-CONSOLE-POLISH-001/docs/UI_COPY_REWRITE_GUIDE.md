# UI copy rewrite guide — Kanon console polish

## Principles

1. Tell the operator what is authoritative and what is advisory.
2. Prefer short factual copy over marketing language.
3. Keep warnings visible but do not repeat them in every card.
4. Explain unavailable actions at the point of action.
5. Never use secret-looking sample values.

## Assistance workbench copy

### Current problem pattern

```text
Context (JSON object)
{
  "summary": "Operator review context (non-secret).",
  "apiKey": "secret-live-key"
}
Allowed fields: summary,apiKey
Force-redact fields: apiKey
```

### Replacement pattern

```text
Context sent for draft assistance
Only include non-secret review context. This request can produce draft notes, but it cannot mutate Kanon semantic state.
```

Default JSON:

```json
{
  "summary": "Operator review context for a non-secret semantic change.",
  "sourceType": "domain-pack",
  "changeIntent": "review-notes"
}
```

Allowed fields:

```text
summary,sourceType,changeIntent
```

Force-redact fields:

```text

```

Helper copy:

```text
Use force-redact only for fields that must be removed before model assistance. Do not paste third-party provider keys, service tokens, passwords, or production secrets here.
```

## Domain-pack action copy

Instead of detached unavailable-action block:

```text
Load unavailable
This version is already active.
```

Use inline control state:

```text
Load
Disabled: this version is already active.
```

Or accessible tooltip:

```text
Load disabled — this version is already active.
```

## Role guidance copy

```text
Current actor roles: Admin
Mutation-capable local operator. Use this for local lifecycle testing only.
```

```text
Read-only semantic authority preset: Auditor, ProvenanceReader
Can inspect packs, provenance, decisions, and evidence without lifecycle mutation.
```

## Partial state copy

Bad:

```text
Partial
```

Better:

```text
Partial — live data loaded, but policy review actions are not exposed by the current client snapshot.
```

Bad:

```text
No items.
```

Better:

```text
No review items returned for the current ontology version and actor role. Try refreshing, changing ontology version, or checking whether the review workflow is enabled.
```

## Evidence link copy

Bad:

```text
Spine
Decision
Open
Copy ID
```

Better:

```text
Open validate decision
Open load decision
Open Evidence Spine for selected pack
Copy load decision ID
Copy pack content hash
```
