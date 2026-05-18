# PR Spec — CX-MODEL-CAP-001 — Model Capability Profiles

## Repo

`conexus-dotnet`

## Goal

Attach operational capability metadata to model aliases/routes.

## Profile fields

```text
capabilityProfileVersion
supportsStreaming
supportsJsonSchema
supportsToolCalling
supportsLongContext
latencyClass
costClass
reasoningClass
safeForHighRisk
recommendedTopologyModes
blockedTopologyModes
notes
```

## Rule

Conexus may expose capability facts, but Kanon/Allagma decide whether topology is allowed.

## Tests

- config validation.
- admin read endpoint.
- route decision includes profile version.
- invalid topology vocabulary rejected if referencing standard contract package.

## Acceptance

- docs updated.
- no provider secrets exposed.
