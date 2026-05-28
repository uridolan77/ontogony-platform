# Cursor unpack prompt — AISTHESIS-PRODUCER-CROSS-REPO-ALIGNMENT-004

You are working across the Ontogony backend workspace.

## Goal

Align `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, and `metabole-dotnet` with the Aisthesis 003A required-edge evidence spine so a future coordinated run can produce a real Aisthesis live proof.

Aisthesis 003A is green for **fixture/harness scope**:
- 10 required-edge rules.
- fixture smoke PASS.
- `requiredEdges.present = 10`, `requiredEdges.missing = 0`.
- `reconstructabilityGrade = complete`.
- stable trace bundle fingerprint.
- live five-service orchestration is still deferred.

## Recommended unpack location

```text
ontogony-platform/docs/_incoming/packages/AISTHESIS-PRODUCER-CROSS-REPO-ALIGNMENT-004/
```

Copy each repo-specific prompt from `repo-prompts/` into the matching backend repo when implementing.

## Read first

1. `01_PACKAGE_MANIFEST.md`
2. `02_CURRENT_STATE_BASELINE.md`
3. `03_SCOPE_AND_BOUNDARY.md`
4. `04_ACCEPTANCE_MATRIX.md`
5. `07_REQUIRED_EDGE_ALIGNMENT_MATRIX.md`
6. The relevant workstream under `workstreams/`
7. The relevant prompt under `repo-prompts/`

## Hard boundary

Aisthesis receives, links, scores, and exports evidence. It must not become the owner of:
- Allagma workflow/run execution.
- Kanon semantic authority.
- Conexus model routing/provider selection.
- Metabole data transformation/mapping.

## Implementation order

1. Allagma producer alignment.
2. Kanon producer alignment.
3. Conexus producer alignment.
4. Metabole producer alignment.
5. Cross-repo live smoke harness coordination.
6. Aisthesis `-Mode Live` proof.
7. Frontend handoff update only after a real live bundle exists.

## Completion rule

Do not mark the cross-repo package fully complete unless a real native-producer live run produces:

```json
{
  "mode": "Live",
  "status": "PASS",
  "requiredEdges": { "present": 10, "missing": 0 },
  "reconstructabilityGrade": "complete"
}
```

Fixture ingestion into Aisthesis is not live producer proof.
