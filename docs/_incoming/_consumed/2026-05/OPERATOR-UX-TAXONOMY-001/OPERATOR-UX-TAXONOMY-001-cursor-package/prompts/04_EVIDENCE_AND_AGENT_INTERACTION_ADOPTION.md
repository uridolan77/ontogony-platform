# Cursor prompt — 04 Evidence Spine and Agent Interaction taxonomy adoption

Use the shared taxonomy in Evidence Spine and Agent Interaction surfaces.

## Evidence Spine

- Use `resolved`, `partial`, `unresolved`, `not_applicable`, `unknown`.
- Show missing reasons with stable reason codes where available.
- Make data source visible: live, generated, imported, fixture.
- Distinguish authoritative source nodes from inferred/frontend-derived edges.
- Do not treat not-applicable as a failure.

## Agent Interaction

- Modes must be explicit: live lookup, fixture replay, imported JSONL.
- Fixture sessions must show `Demo fixture — not live evidence`.
- Imported sessions must show historical/imported authority.
- Live sessions should show authority for Allagma/Conexus/Kanon data.
- Every unresolved timeline event should say unresolved what and why.

Do not implement missing live functionality here; adopt taxonomy and truthful labels around whatever functionality currently exists.
