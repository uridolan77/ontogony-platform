# KANON-OP-002 — Operational diagnostics evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**  
**Statement:** Post-Docker-local Kanon topology troubleshooting; **not production readiness**.

## Scope

- Shared `_docker-local-env.ps1` for `.env` / `.env.example` token and port resolution.
- Env-aware `inspect-kanon-topology-evidence.ps1` (KANON-OP-001 improvement).
- `diagnose-kanon-topology-ops.ps1` + `validate-kanon-topology-diagnostics-report.ps1`.
- Platform and Kanon operator documentation.

No service `src/` changes.

## Delivered

```text
docker/local-working-system/scripts/_docker-local-env.ps1
docker/local-working-system/scripts/diagnose-kanon-topology-ops.ps1
docker/local-working-system/scripts/validate-kanon-topology-diagnostics-report.ps1
docker/local-working-system/scripts/inspect-kanon-topology-evidence.ps1 (env-aware)
docker/local-working-system/scripts/validate-kanon-topology-evidence-report.ps1 (env-aware secrets)
docker/local-working-system/README.md
docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/KANON-OP-002.md
docs/evidence/KANON_OP_002_OPERATIONAL_DIAGNOSTICS_EVIDENCE.md
kanon-dotnet/docs/operators/TOPOLOGY_DIAGNOSTICS.md
```

## Diagnosis taxonomy

| Code | Operator meaning |
| --- | --- |
| `BASELINE_NULL_BY_DESIGN` | Baseline did not require Kanon topology authorization |
| `ALLAGMA_NO_KANON_CALL` | Subject path unexpectedly has `requiresKanonAuthorization=false` |
| `SUBJECT_MISSING_AUTH_ID` | Auth required but Allagma has no `topologyAuthorizationDecisionId` |
| `KANON_DECISION_NOT_FOUND` | Kanon 404 on decision-record GET |
| `KANON_AUTH_FAILURE` | Kanon 401/403 — check `KANON_SERVICE_TOKEN` and `ProvenanceReader` |
| `KANON_UNAVAILABLE` | Kanon health probe failed |
| `KANON_DENY` | Policy denied topology |
| `KANON_HUMAN_GATE` | Run may be paused for human gate |
| `ARTIFACT_MISSING` | No guided/seed report |
| `REPORT_STALE` | Report decision ID ≠ live Allagma topology summary |

## Commands

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\inspect-kanon-topology-evidence.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-evidence-report.ps1
.\docker\local-working-system\scripts\diagnose-kanon-topology-ops.ps1
.\docker\local-working-system\scripts\validate-kanon-topology-diagnostics-report.ps1
```

## Live validation (2026-05-19)

| Check | Result |
| --- | --- |
| `inspect-kanon-topology-evidence.ps1` | **0** |
| `validate-kanon-topology-evidence-report.ps1` | **0** |
| `diagnose-kanon-topology-ops.ps1` | **0** |
| `validate-kanon-topology-diagnostics-report.ps1` | **0** |
| Env source | `.env.example` (tokens loaded dynamically) |
| Diagnostics findings | `BASELINE_NULL_BY_DESIGN`, `SUBJECT_AUTH_REQUIRED`, `KANON_ALLOW` |
| Raw secrets in reports | **none** |

## Validation checks (repo diff)

| Check | Result |
| --- | --- |
| Tokens loaded from `.env` / `.env.example` | **yes** |
| Report secret-pattern validation | **yes** |
| No Allagma/Kanon `src/` changes | **yes** |
| No workflow changes | **yes** |
| Production readiness claimed | **no** |

## Cross-references

- KANON-OP-001 evidence: `docs/evidence/KANON_OP_001_OPERATOR_EVIDENCE.md`
- Kanon diagnostics: `kanon-dotnet/docs/operators/TOPOLOGY_DIAGNOSTICS.md`
