# ENV-DOCKER-FE-001 — Frontend Docker/local walkthrough

Repos:

```text
ontogony-frontend
ontogony-platform
```

## Goal

Verify the frontend against Docker-local APIs.

## Add/update

```text
ontogony-frontend/docs/development/DOCKER_LOCAL_OPERATOR_WALKTHROUGH.md
ontogony-frontend/scripts/docker/open-docker-local-operator-pages.ps1
ontogony-frontend/docs/evidence/ENV_DOCKER_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md
```

Optionally add platform pointer docs.

## Required pages

```text
/
 /allagma/evaluations
 /allagma/evaluations?dashboardFixture=ci-suite
 /allagma/runs/{subjectRunId}
 /allagma/evaluations/{subjectEvaluationRunId}
 /allagma/evaluations/baseline-comparisons/{baselineComparisonId}
```

## Acceptance

```text
frontend container starts
root page returns HTML
SPA fallback works
operator pages open
fixture mode remains available
live evidence IDs from Docker guided report open
```
