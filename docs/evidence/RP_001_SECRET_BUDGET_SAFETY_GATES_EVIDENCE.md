# RP-001 — Secret, budget, and safety gates evidence (platform)

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — safety policy and placeholders only

**Boundary:** Documentation and `.env.example` guardrails only. **No real provider calls.** **No secrets committed.** **Not production readiness.**

## Deliverables

| Artifact | Path | Status |
| --- | --- | --- |
| Cross-repo operator policy | `docs/operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md` | **added** |
| Docker-local placeholders | `docker/local-working-system/.env.example` | **updated** |
| Operators index | `docs/operators/README.md` | **updated** |
| Product hardening index | `docs/product-hardening/README.md` | **updated** |
| Conexus companion doc | `conexus-dotnet/docs/development/REAL_PROVIDER_LOCAL_VALIDATION.md` | **added** (sibling repo) |
| Conexus evidence | `conexus-dotnet/docs/evidence/RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md` | **added** (sibling repo) |

## Policy summary

| Gate | Value |
| --- | --- |
| Fake provider default | **yes** — unchanged; compose/bootstrap/manual QA baseline |
| Real provider enable | `CONEXUS_REAL_PROVIDER_ENABLED=false` by default; explicit opt-in required |
| Provider secrets | Local `CONEXUS_PROVIDER_*_API_KEY` only; never in git |
| Budget (initial) | max **3** calls, **256** output tokens, small model |
| Kill switch | Disable gate + unset keys + restart + fake regression |
| CI real calls | **forbidden** |
| Config validation code | **deferred** to RP-002 (gate not implemented in RP-001) |

## Validation result

| Check | Result |
| --- | --- |
| Real-provider disabled-by-default documented | **PASS** |
| Fake provider remains default | **PASS** |
| Budget caps documented | **PASS** |
| Kill switch documented | **PASS** |
| Failure classes documented | **PASS** (13 classes) |
| CI real-provider calls forbidden | **PASS** |
| `.env.example` placeholders only | **PASS** |
| No runtime source changes (`src/`, `*.cs`, `docker/` compose logic) | **PASS** (`.env.example` comments only in platform) |
| No workflow changes | **PASS** |
| No secrets / provider keys in diff | **PASS** |
| No real-provider calls made | **PASS** |

## Commands (guard)

```powershell
git diff --name-only HEAD -- 'src/' '.github/' '*.cs' '*.csproj'
Select-String -Path 'docs/operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md' -Pattern 'sk-[a-zA-Z0-9]{20,}'
```

## Next step

**`RP-002`** — Conexus real-provider local mode (`prompts/RP-002_CONEXUS_REAL_PROVIDER_LOCAL_MODE.md`).

## Required statement

```text
RP-001 defines local-only safety gates before any real provider call.
Fake provider remains the default.
No CI real-provider calls.
No secrets in git.
This is not production readiness.
```
