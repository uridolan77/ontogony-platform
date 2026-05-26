# Ticket 001 — Reframe page and summary cards

## Goal

Make `/system/release-readiness` visibly a generated route-readiness artifact page, not a real release-candidate certification page.

## Problem

The current page shows strong green-style counts such as `Ready 24 / Partial 8 / Gap 0` while also saying it is fixture/demo-only and generated from `docs/generated/operator-release-readiness.json`.

That is internally honest in text but misleading in visual hierarchy.

## Implementation

1. Update page title/subtitle copy.
2. Add a top-level posture panel:

```text
Release-candidate posture: Not assessed
This scorecard is generated from route metadata. No live backend or semantic validation is attached to this readiness artifact.
```

3. Rename summary counts to artifact-specific language:

```text
Artifact ready
Artifact partial
Artifact gaps
Fixture/demo only
Unknown source
Live validation
```

4. Preserve old counts if useful, but ensure labels cannot be read as release-ready claims.

## Acceptance

- [ ] `Ready` does not appear alone without `artifact`, `route`, or equivalent qualifier.
- [ ] Top-level release-candidate posture is separate from route artifact status.
- [ ] Fixture/demo warning is visible above the route table.
- [ ] The page cannot be mistaken for production release certification.
