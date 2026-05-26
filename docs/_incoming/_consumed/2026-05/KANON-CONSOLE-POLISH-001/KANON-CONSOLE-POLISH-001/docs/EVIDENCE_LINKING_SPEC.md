# Evidence linking specification — Kanon console polish

## Goal

Kanon pages should make semantic authority auditable. Every decision, pack lifecycle action, assistance draft, and provenance artifact should link to the right evidence surface when identifiers exist.

## Link types

| Link type | Required label pattern | Identifier kind |
|---|---|---|
| Kanon decision | `Open {decision purpose} decision` | `kanonDecisionId` |
| Evidence Spine decision | `Open Evidence Spine for {decision purpose}` | `kanonDecisionId` |
| Domain pack | `Open Evidence Spine for selected pack` | `domainPackId` or pack/version composite if supported |
| Content hash | `Copy pack content hash` | hash copy only |
| Replay bundle | `Open replay bundle for decision` | `kanonDecisionId` |
| Provenance | `Open provenance for {entity}` | entity/provenance ID if supported |

## Accessibility

- Link text must be meaningful without surrounding context.
- Icon-only links require `aria-label`.
- Copy buttons require `aria-label` and visible or announced feedback.

## Missing identifiers

Do not render dead links. Render one of:

```text
Evidence not recorded for this state.
Decision ID not returned by the backend response.
Replay bundle requires a planning decision ID.
```

## Partial evidence

When evidence is partial, show:

- what is available;
- what is missing;
- whether lookup was attempted;
- whether failure was auth, route, missing id, or backend error.

## Anti-patterns

Avoid generic labels:

```text
Open
Spine
Decision
Copy
```

unless there is adjacent text that unambiguously identifies the target and the accessible label is specific.
