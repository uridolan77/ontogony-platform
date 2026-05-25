# Evidence Spine

> **Audience:** operator, frontend developer  
> **Applies to:** `ontogony-frontend`, `ontogony-platform/docs/operators`, `kanon-dotnet`, `conexus-dotnet`, `allagma-dotnet`  
> **Source of truth:** [`../operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](../operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md)  
> **Last verified:** 2026-05-25

## Purpose

Paste one identifier (`runId`, `traceId`, `decision_…`, `chatcmpl-…`, etc.) and get a **single governed execution graph** across Allagma, Kanon, and Conexus. v1 resolution is **client-side** in the operator console — there is no unified backend resolve API.

## Authority (do not blur)

| Service | Owns |
| --- | --- |
| Allagma | Run lifecycle, audit bundle, eval/baseline ids on runs |
| Kanon | Decision records, provenance, semantic graph |
| Conexus | Model-call evidence, route decisions |
| Frontend | `resolveEvidenceSpine`, workbench UI |

## Canonical artifacts (link, do not copy)

| Artifact | Repo |
| --- | --- |
| Contract index | `ontogony-platform/docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md` |
| Identifier taxonomy | `ontogony-platform/docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md` |
| Resolution matrix | `ontogony-platform/docs/system/system-evidence-spine-resolution.matrix.json` |
| Graph taxonomy | `ontogony-platform/docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md` |
| Export schema | `ontogony-platform/docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json` |
| Resolver | `ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts` |
| Kanon entrypoints | `kanon-dotnet/docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json` |
| Conexus flow | `conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md` |

## Operator workflow

1. Complete a governed run ([03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md)).
2. Open **Evidence Spine** in the console.
3. Paste `runId` or `traceId` from smoke output.
4. Expand nodes; export bundle when debugging ([14_DEBUGGING_PLAYBOOK.md](./14_DEBUGGING_PLAYBOOK.md)).

## Docker verification (platform)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\run-evidence-spine-008a-docker-live-verification.ps1
```

Preflight artifact (runtime lock pointer): `docker/local-working-system/artifacts/evidence-spine-008a-live-api-preflight.json`

## Tests (frontend)

```powershell
cd C:\dev\ontogony-frontend
npm run test -- src/evidence-spine
npx playwright test e2e/system-truth.spec.ts
```

Platform contract tests: `ontogony-platform/tests/Ontogony.Infrastructure.Tests/SystemEvidenceSpineContractTests.cs`

## Next

- Agent stream UI: [06_AGENT_INTERACTION.md](./06_AGENT_INTERACTION.md)
- Identifiers glossary: [GLOSSARY.md](./GLOSSARY.md)
