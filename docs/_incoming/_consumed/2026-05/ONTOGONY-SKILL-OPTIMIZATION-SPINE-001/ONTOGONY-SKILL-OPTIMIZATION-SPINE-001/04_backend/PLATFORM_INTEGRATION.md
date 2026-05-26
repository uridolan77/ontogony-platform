# Platform Integration Plan

## Role

`ontogony-platform` should host cross-repo protocol documentation, local compose notes, and validation scripts for the Skill Optimization Spine.

## Add docs

Suggested paths:

```text
docs/protocols/SKILL_OPTIMIZATION_SPINE.md
docs/protocols/SKILL_ARTIFACT_LIFECYCLE.md
docs/protocols/SKILL_DEPLOYMENT_BINDINGS.md
docs/local/SKILL_OPTIMIZATION_LOCAL_FIXTURE.md
docs/rc/SKILL_OPTIMIZATION_RC_GATE.md
```

## Add integration checklist

Create a cross-repo checklist that verifies:

```text
Allagma run route exists
Kanon skill decision/evidence routes exist
Conexus skill injection metadata exists
Frontend Skill Lab route exists
UI components are exported
Fake fixture produces accepted + rejected candidates
No normal inference optimizer call
```

## Local compose

Add only if useful:

```text
ONTOGONY_ENABLE_SKILL_OPTIMIZATION=true
ONTOGONY_SKILL_OPTIMIZATION_FIXTURE_MODE=true
CONEXUS_ENABLE_SKILL_INJECTION=true
CONEXUS_ENABLE_OPTIMIZER_CALLS=false for normal local inference unless explicitly running optimization
```

## Cross-repo script

Suggested script:

```text
scripts/verify-skill-optimization-spine.ps1
scripts/verify-skill-optimization-spine.sh
```

The script should not be overly ambitious. It can check docs, generated route inventories, fixture files, and optional smoke endpoints.
