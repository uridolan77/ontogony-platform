# Cursor prompt — 07 tests and regression guards

Add tests and string-scan guards for OPERATOR-UX-TAXONOMY-001.

## Unit tests

In `ontogony-ui`, test:

- label helpers;
- severity helpers;
- no-bare-unknown helper;
- data-source readiness eligibility;
- evidence not_applicable vs unresolved;
- topology planned vs validated.

## Component/page tests

In `ontogony-frontend`, test representative pages/components:

- Home service card: live + not_ready is not healthy.
- Fixture fallback appears as badge, not headline.
- Evidence partial lists reason.
- Agent Interaction fixture mode shows demo badge.
- Topology planned edge is visually/semantically planned.
- Settings secret source missing is `not set`, not `unknown source`.

## String scan

Add or document a script that flags:

- `Live with fixture fallback` in headings;
- bare `unknown` cells/text;
- `All services are healthy` when not computed from all clean dimensions;
- `Failed runs (sample)`;
- `Backend-waiting list APIs`;
- raw route copy in primary cards if testable.
