# Risks and Non-Goals

## Risks

1. **Over-normalizing away useful debug information**
   Keep raw error details available in developer details, but never as the only operator-facing explanation.

2. **Making frontend compensate for backend truth gaps forever**
   Frontend can map failures, but Conexus should still fix route-decision persistence or explicitly type non-persistence.

3. **Kanon becoming a cross-service detail store**
   Kanon may reference Allagma/Conexus IDs, but it must not own their details.

4. **Fixture evidence contaminating live completeness**
   If fixture fallback occurs, label it and exclude it from live proof claims.

5. **Breaking existing exported evidence bundles**
   Prefer additive schema changes and maintain backward-compatible parsing.

## Non-goals

- Do not implement production IAM.
- Do not enable real external tools.
- Do not rewrite the full operator console.
- Do not redesign the entire Agent Interaction workbench.
- Do not solve health/readiness standardization in this work item, except where Evidence Spine consumes it.
- Do not hard-code fake IDs or fixtures to make tests pass.
