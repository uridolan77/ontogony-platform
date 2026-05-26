# KCP-005 — Kanon evidence-link consistency

## Problem

Kanon pages include generic evidence and decision links such as `Spine`, `Decision`, `Open`, and `Copy ID`. These are hard to scan and weak for accessibility.

## Scope

- Create or standardize evidence link rendering.
- Use contextual labels.
- Add accessible labels and copy feedback.
- Add tests if existing test setup supports it.

## Label examples

```text
Open validate decision
Open load decision
Open Evidence Spine for selected pack
Copy load decision ID
Copy pack content hash
Open assistance decision
Open Evidence Spine for assistance decision
```

## Acceptance

- Operator can tell what each link opens before clicking.
- Copy buttons have meaningful accessible labels.
- Duplicate evidence links are grouped or distinguished by context.
