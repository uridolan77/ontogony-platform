# SYSTEM-RC-001F — Agent Interaction / AG-UI spine certification

## Intent

Certify the backend AG-UI integration layer for the Ontogony governed runtime.

In this package, **AG-UI** is treated as the backend-facing **Agent Interaction Spine**: a deterministic, redacted, replayable stream of run/model/policy/human-gate events that can drive an operator UI or AG-UI adapter.

This is not a frontend component sprint. It is a backend contract and evidence sprint.

## Scope

### In scope

- Validate Platform's canonical agent interaction schema and matrix.
- Validate Allagma's run interaction event JSONL export.
- Validate Allagma's interaction event SSE stream and resume behavior.
- Validate Conexus model-call lifecycle projection into interaction events.
- Validate Kanon human-gate / review queue / assistance-review mapping into interrupt/review semantics.
- Produce one golden AG-UI interaction event transcript from a real governed run.
- Produce negative-path transcripts for interruption, missing evidence, and denied gate paths.
- Prove redaction: no raw prompts, completions, secrets, provider keys, or unapproved sensitive payloads.

### Out of scope

- Completing frontend AG-UI screens.
- Replacing Evidence Spine with AG-UI.
- Adding real external tool execution.
- Adding production IAM.
- Changing public Kanon/Conexus error contracts.

## Required backend surfaces

| Repo | Surface | Requirement |
| --- | --- | --- |
| `ontogony-platform` | `ontogony-agent-interaction-event-v0` schema + validator | Canonical contract validates sample run and human-gate interrupt fixtures. |
| `allagma-dotnet` | `GET /allagma/v0/runs/{runId}/interaction-events` | Exports deterministic JSONL event sequence for a run. |
| `allagma-dotnet` | `GET /allagma/v0/runs/{runId}/interaction-events/stream` | Streams the same logical event sequence over SSE with resume semantics. |
| `conexus-dotnet` | Model-call interaction projector | Projects model-call lifecycle, route/evidence links, usage/cost hints, and evidence bundle links. |
| `kanon-dotnet` | Human gate / review queue / assistance-review evidence | Provides IDs and semantics that can be rendered as interrupt, review, accept, reject, resume, or denied interaction phases. |

## Golden scenario

Use a deterministic local fake-provider run that includes:

1. Allagma run created.
2. Task classified.
3. Topology selected.
4. Kanon plan requested and compiled.
5. At least one policy/action evaluation.
6. Conexus model call requested.
7. Conexus route decision linked.
8. Model call completed.
9. Run completed.
10. Audit/evidence links resolvable.

Optional but strongly preferred:

11. Human-gate pause.
12. Human-gate resume or denial.
13. Streaming model purpose event lifecycle.

## Required artifacts

```text
artifacts/agui/<timestamp>/golden-run-interaction-events.jsonl
artifacts/agui/<timestamp>/golden-run-interaction-events.schema-validation.json
artifacts/agui/<timestamp>/golden-run-interaction-stream-transcript.txt
artifacts/agui/<timestamp>/golden-run-interaction-stream-resume.json
artifacts/agui/<timestamp>/conexus-model-call-interaction-projection.jsonl
artifacts/agui/<timestamp>/kanon-human-gate-interrupt-mapping.json
artifacts/agui/<timestamp>/agui-redaction-report.json
artifacts/agui/<timestamp>/agui-evidence-link-resolution-report.json
```

## Acceptance criteria

- Event stream is stable under repeated export for the same run.
- JSONL export and SSE stream describe the same event sequence.
- SSE resume from `Last-Event-ID` does not duplicate already-consumed events.
- Every model-call event that references a `modelCallId` links to Conexus evidence surfaces.
- Every Kanon decision/human-gate reference is either resolvable or explicitly marked as missing with a reason code.
- Event stream contains no raw prompt, raw completion, provider secret, API key, or unapproved sensitive field.
- Interrupt/human-gate semantics are explicit enough for an operator UI to render pause/resume/deny states.
- Failure/missing evidence appears as structured event metadata, not silent omissions.

## Suggested validation commands

Run what exists today; add scripts only where gaps remain.

```powershell
# Platform schema / contract
cd C:\dev\ontogony-platform
pwsh ./scripts/validate-agent-interaction-spine.ps1

# Allagma interaction event export / stream tests
cd C:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AgentRunInteractionEvent"
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AgentRunInteractionStream"

# Conexus model-call interaction projector
cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Application.Tests -c Release --filter "FullyQualifiedName~ModelCallInteractionEventProjector"
dotnet test tests/Conexus.Api.Tests -c Release --filter "FullyQualifiedName~ModelCallInteractionEventExport"

# Kanon human gate / review queue event projection support
cd C:\dev\kanon-dotnet
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~HumanGate|FullyQualifiedName~ReviewQueue|FullyQualifiedName~EvidenceSpine"
```

## Implementation notes

- Prefer adding validators and fixtures before adding new runtime endpoints.
- Do not create a second evidence model. AG-UI events should point to Evidence Spine / audit / Conexus / Kanon objects by ID and link metadata.
- Keep event payloads UI-friendly but backend-owned: bounded labels, IDs, timestamps, phase, actor, status, evidence links, and redaction status.
- Treat missing downstream evidence as first-class metadata with `missingReasonCodes`.

## Closeout

Add a closeout doc:

```text
docs/evidence/SYSTEM_RC_001F_AGUI_SPINE_CERTIFICATION_EVIDENCE.md
```

It must list artifact paths, command outputs, unresolved gaps, and whether the AG-UI backend spine is certified for the alpha RC.
