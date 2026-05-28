# Metabole producer alignment

Metabole owns pipeline, data profile, mapping candidate, and artifact materialization.

## Required

- Emit pipeline evidence with `pipelineRunId`.
- Emit data profile with `dataProfileId`.
- Emit mapping candidate with `mappingCandidateId`.
- Emit output/SLOD artifact with `artifactId`.
- Emit:
  - `metabole.pipeline_to_profile`
  - `metabole.pipeline_to_mapping_candidate`
  - `metabole.mapping_candidate_to_artifact`
- Add tests for all envelope types and edges.
