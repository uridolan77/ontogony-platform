# Platform Integration Plan

## Role

`ontogony-platform` should hold cross-repo protocol docs, local demo instructions, and optional integration validation scripts.

## Add docs

Suggested files:

```text
docs/protocols/DECISION_RECONSTRUCTABILITY_SPINE.md
docs/protocols/DECISION_EVENT_SCHEMA_V0.md
docs/protocols/RECONSTRUCTABILITY_REPORT_V0.md
docs/operators/DECISION_RECONSTRUCTION_LOCAL_DEMO.md
```

## Add local demo fixture

Create or update a local fixture describing a complete cross-service action:

```text
Allagma run operation
  -> Conexus route decision/model call
  -> Kanon policy/semantic decision
  -> frontend Evidence Spine reconstruction
```

## Optional validation script

If the platform repo already has scripts for route/schema validation, add a check that each backend can return or fixture-generate one reconstructability report.

Candidate script:

```text
scripts/validate-decision-reconstructability.ps1
scripts/validate-decision-reconstructability.sh
```

The script should not be mandatory in CI until all repos expose stable endpoints.

## Compose/local docs

Document which local services must run and which fixture ids can be used in the console.
