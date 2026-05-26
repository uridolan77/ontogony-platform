# Risks and Non-goals

## Risk: hidden chain-of-thought confusion

Do not represent `reasoningEvidence` as private model chain-of-thought. Use safe, explicit surrogates only.

Mitigation:

- add docs;
- add tests asserting no hidden CoT field is persisted;
- use labels like “safe rationale evidence,” not “model thoughts.”

## Risk: duplicate evidence systems

Ontogony already has Evidence Spine concepts. Do not create an isolated parallel graph.

Mitigation:

- link reports to existing evidence graph nodes;
- reuse existing IDs;
- add classification as an overlay, not a new universe.

## Risk: fake completeness

A report should not mark `F` just because a field is non-null. It must be specific enough to reconstruct the property.

Mitigation:

- require authoritative fragment refs for `F`;
- mark fixture data as fixture;
- use `P` for hash-only or incomplete bindings.

## Risk: UI overload

The console is already dense. Do not add a giant raw-trace view to top-level pages.

Mitigation:

- show one compact status chip;
- use drawer/panel for details;
- collapse raw fragments.

## Risk: route proliferation

Avoid adding routes when existing details endpoints can be extended.

Mitigation:

- inspect route inventory first;
- prefer projection fields and one canonical Kanon classification endpoint.

## Non-goals

- Full vendor benchmark reproduction.
- Statistical evaluation of trace regimes.
- Raw chain-of-thought capture.
- Rewriting existing Allagma/Kanon/Conexus domain models.
- Making every legacy fixture fully reconstructable in this slice.
