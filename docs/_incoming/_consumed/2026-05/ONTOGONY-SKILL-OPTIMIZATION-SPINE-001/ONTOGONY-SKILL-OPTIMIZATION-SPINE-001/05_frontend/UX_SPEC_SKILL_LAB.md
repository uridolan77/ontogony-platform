# UX Spec: Skill Lab

## Goal

Create an operator-friendly Skill Lab that makes governed skill evolution understandable without flooding the page with raw traces and badges.

## Route suggestions

Use existing frontend conventions. Possible routes:

```text
/system/skills
/system/skills/:skillArtifactId
/system/skill-optimization/:runId
```

## Information architecture

### Skill Inventory

Show cards/table with:

```text
Skill name
Domain
Active version
Deployment status
Last gate result
Last optimization run
Risk/authority badge
```

Primary actions:

```text
Open
Run optimization fixture
View deployments
```

### Skill Detail

Tabs:

```text
Overview
Versions
Optimization Runs
Deployments
Evidence
```

Overview should show:

- active version;
- content hash;
- gate summary;
- deployment binding;
- rollback availability;
- linked evidence graph.

### Version Lineage

Visual sequence:

```text
v0 draft -> v1 accepted -> v1 published -> v1 deployed
             \-> bad candidate rejected
```

Avoid complex force graphs in first slice. A clean vertical timeline is enough.

### Optimization Run Detail

Sections:

1. Run summary.
2. Rollout evidence summary.
3. Candidate edits.
4. Validation status.
5. Held-out gate.
6. Acceptance/rejection decision.
7. Rejected edit buffer.
8. Links to Allagma/Kanon/Conexus evidence.

### Edit Diff

Show a readable diff per atomic edit:

```text
Operation: add
Section: /Procedure
Budget: 1
Risk: low
Expected effect: ...
Evidence: 3 rollout failures
Decision: accepted
```

### Gate Panel

Show incumbent vs candidate:

```text
Incumbent score: 0.66
Candidate score: 0.83
Delta: +0.17
Policy: strict improvement, ties rejected
Status: Accepted
```

Per-task rows should be collapsible.

### Deployment Panel

Show:

```text
Active binding
Scope
Route profile
Environment
Authorized by
Rollback target
```

## Page tone

Use operator language, not research language.

Prefer:

```text
Accepted because the candidate improved the held-out selection set.
```

Avoid:

```text
The optimizer converged via a textual gradient update.
```

## Empty states

- No skills yet: explain how to run the fixture.
- No deployment: accepted version exists but is not active.
- No rejected edits: no failed candidate has been recorded yet.
- Missing evidence: show precise missing ref, not a generic error.
