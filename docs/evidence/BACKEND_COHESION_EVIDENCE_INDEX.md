# Backend cohesion evidence index

Platform and cross-repo evidence for **backend cohesion feature spines** indexed by [`ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json`](../system/backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json).

**Boundary:** This index points at proof artifacts. Runtime golden-path evidence from validation packages lives in **product repos** (`allagma-dotnet`, `kanon-dotnet`, etc.).

---

## Platform package closeout

| Item | File |
| --- | --- |
| ONTOGONY-BACKEND-COHESION-PLATFORM-001 | [ONTOGONY_BACKEND_COHESION_PLATFORM_001.md](./ONTOGONY_BACKEND_COHESION_PLATFORM_001.md) |

---

## Closed spine evidence (platform repo)

| Spine | Evidence |
| --- | --- |
| Decision reconstructability | [PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md](./PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md) |
| Skill optimization | [ONTOGONY_SKILL_OPTIMIZATION_SPINE_001_CLOSEOUT.md](./ONTOGONY_SKILL_OPTIMIZATION_SPINE_001_CLOSEOUT.md) |
| Skill release governance | [ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md](./ONTOGONY_SKILL_RELEASE_GOVERNANCE_001_CLOSEOUT.md) |
| Sandbox consumer activation | [ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001G.md](./ONTOGONY_SANDBOX_CONSUMER_ACTIVATION_001G.md) |
| Error envelope | [PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md](./PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md) |
| Trace / correlation / idempotency | [TRACE_CONTRACT_001_EVIDENCE.md](./TRACE_CONTRACT_001_EVIDENCE.md) |
| Evidence export / read-model | [SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md](./SYS_TIGHT_002_SYSTEM_EVIDENCE_SPINE_CONTRACT_EVIDENCE.md) |

---

## Runtime validation packages (sibling repos)

| Repo | Package | Expected evidence prefix |
| --- | --- | --- |
| Allagma | `ALLAGMA-BACKEND-COHESION-VALIDATION-001` | `ALLAGMA_BACKEND_COHESION_*` |
| Kanon | `KANON-BACKEND-COHESION-VALIDATION-001` | `KANON_BACKEND_COHESION_*` |
| Conexus | `CONEXUS-BACKEND-COHESION-VALIDATION-001` | `CONEXUS_BACKEND_COHESION_*` |
| Metabole | Foundation + SLOD profiling | `METABOLE_*` (follow-on) |

---

## Schema validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests -c Release --filter FullyQualifiedName~BackendCohesionManifestSchemaTests
```

---

## Non-claims

Evidence listed here does not imply production deployment, live LLM providers, or live Candor Chat consumer E2E.
