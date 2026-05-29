# Unpack prompt — ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001

You are implementing `ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001` in `uridolan77/ontogony-platform`.

## Mission

Turn Ontogony.Platform into the strict mechanical substrate for the six-backend Ontogony system by adding:

- a mandatory **mechanics-only proposal gate**;
- cross-repo **consumer conformance harnesses**;
- versioned **mechanical JSON schemas**;
- anti-semantic-leak tests and scan rules;
- per-consumer adoption checklists and evidence templates.

## Read first

- `README.md`
- `01_PACKAGE_MANIFEST.md`
- `02_CURRENT_STATE_BASELINE.md`
- `03_SCOPE_AND_BOUNDARY.md`
- `04_MASTER_IMPLEMENTATION_SEQUENCE.md`
- `05_ACCEPTANCE_MATRIX.md`

## Absolute rule

Every candidate Platform addition must answer **yes** to:

```text
Can this be reused by Conexus, Kanon, Allagma, Metabole, and Aisthesis without importing product meaning?
```

If no, reject or move the work to the product repo that owns it.

## Implementation target

Prefer additive, testable mechanics:

- schemas under `schemas/mechanics/v1/`;
- docs under `docs/contracts/`, `docs/conformance/`, `docs/governance/`;
- scripts under `scripts/conformance/` and `scripts/governance/`;
- tests under `tests/Ontogony.*.Tests/`;
- generated evidence under `artifacts/platform-mechanics-conformance/<timestamp>/`.

## Do not

- Move Allagma runtime lock authority into Platform.
- Add model-provider policy to Platform.
- Add Kanon ontology semantics to Platform.
- Add Metabole SLOD/domain mapping semantics to Platform.
- Add Aisthesis reconstructability scoring semantics to Platform.
- Add frontend/UI code to Platform.
- Fake consumer conformance by only testing Platform examples.
