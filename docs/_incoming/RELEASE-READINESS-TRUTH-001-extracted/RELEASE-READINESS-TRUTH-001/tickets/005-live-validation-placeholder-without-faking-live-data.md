# Ticket 005 — Live validation section without faking live data

## Goal

Create a clear placeholder/section for live validation that honestly reports whether live validation exists.

## Problem

The page says no live backend client, but the dominant readiness counts still imply a release gate.

## Implementation

Add a section or card:

```text
Live validation
Status: Not attached
This page currently renders a generated route artifact. It does not call Conexus, Kanon, Allagma, or Evidence Spine to prove route behavior.
```

If the current repo already has live validation data, display it separately and clearly.

Do not invent live validation.

Potential future checks to document:

- service health and readiness schemas
- route existence probes
- generated-client coverage
- auth/config readiness
- one live correlation/evidence-spine resolution
- route-specific smoke checks

## Acceptance

- [ ] Page clearly states whether live validation is present.
- [ ] No fake live data is added.
- [ ] Future live validation checks are documented as follow-ups.
- [ ] Existing generated artifact rendering continues to work.
