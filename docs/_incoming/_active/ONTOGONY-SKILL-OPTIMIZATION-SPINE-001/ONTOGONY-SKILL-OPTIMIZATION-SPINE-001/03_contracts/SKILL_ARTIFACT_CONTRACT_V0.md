# SkillArtifact Contract V0

## Purpose

`SkillArtifact` is the stable identity of a reusable procedural skill family. It is not a specific document version. It is the thing whose versions are optimized, evaluated, published, deployed, superseded, and retired.

## Required fields

```json
{
  "skillArtifactId": "demo.spreadsheet-analysis",
  "name": "Spreadsheet Analysis Skill",
  "description": "Procedural guidance for spreadsheet inspection and answer formatting.",
  "domainId": "demo",
  "ownerService": "allagma",
  "createdAt": "2026-05-26T00:00:00Z",
  "createdByActorId": "local-operator",
  "status": "active",
  "defaultVersionId": "skillver_demo_spreadsheet_v1",
  "tags": ["fixture", "spreadsheet", "analysis"],
  "authority": {
    "kanonOntologyId": "gaming-core",
    "kanonOntologyVersion": "gaming-core@0.1.0",
    "policyRefs": ["skill-optimization-policy-v0"]
  },
  "compatibility": {
    "agentProfiles": ["local-fixture-agent"],
    "harnesses": ["direct", "allagma-run"],
    "modelFamilies": ["fake", "openai"]
  }
}
```

## Status enum

```text
draft
active
retired
archived
```

## Rules

- A `SkillArtifact` may have many immutable `SkillVersion` objects.
- It must not contain mutable skill content directly except for optional draft notes.
- It must expose the currently recommended/default version, but actual runtime use must still go through `SkillDeploymentBinding`.
- It must link to Kanon authority where the skill has semantic or policy meaning.
