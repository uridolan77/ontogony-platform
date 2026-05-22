# AG-UI / Agent Interaction Spine required commands

Run these as part of `SYSTEM-RC-001F`.

## Platform

```powershell
cd C:\dev\ontogony-platform
pwsh ./scripts/validate-agent-interaction-spine.ps1
```

Expected:

```text
PASS: schema exists
PASS: matrix exists
PASS: sample fixtures validate
PASS: no unresolved placeholder paths
```

## Allagma

```powershell
cd C:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AgentRunInteractionEvent"
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AgentRunInteractionStream"
```

Expected:

```text
PASS: JSONL export endpoint covered
PASS: SSE stream endpoint covered
PASS: resume/Last-Event-ID behavior covered, or explicit gap recorded
PASS: event sequence redacted
```

## Conexus

```powershell
cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Application.Tests -c Release --filter "FullyQualifiedName~ModelCallInteractionEventProjector"
dotnet test tests/Conexus.Api.Tests -c Release --filter "FullyQualifiedName~ModelCallInteractionEventExport"
```

Expected:

```text
PASS: model-call lifecycle projection
PASS: evidence-bundle links included
PASS: no raw prompt/completion in projected events
PASS: route-decision/modelCallId consistency
```

## Kanon

```powershell
cd C:\dev\kanon-dotnet
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~HumanGate|FullyQualifiedName~ReviewQueue|FullyQualifiedName~EvidenceSpine"
```

Expected:

```text
PASS: human-gate state can be represented by interaction events
PASS: review queue / assistance-review IDs are linkable
PASS: decision/provenance IDs are resolvable or explicitly classified missing
```

## Full system AG-UI proof

```powershell
cd C:\dev\allagma-dotnet
pwsh ./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices `
  -IncludeKanonAssistance -IncludeConexusFallback -IncludeStreamingEvidence
```

Then export interaction events for the produced run and save:

```text
artifacts/agui/<timestamp>/golden-run-interaction-events.jsonl
artifacts/agui/<timestamp>/golden-run-interaction-stream-transcript.txt
artifacts/agui/<timestamp>/agui-redaction-report.json
```

If no script exists for this export, add a small `scripts/export-agui-golden-run-evidence.ps1` in `allagma-dotnet` that takes `-RunId` and writes the artifact set.
