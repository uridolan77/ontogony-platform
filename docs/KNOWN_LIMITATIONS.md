# Known limitations

Honest boundaries for **Ontogony.Platform** and closed operator programs that used this repo. None of this is production readiness.

---

## Platform substrate (ongoing)

| Limitation | Status |
| --- | --- |
| Pre-1.0 API line | `0.3.0-alpha.1` — breaking changes possible with changelog + consumer validation |
| HTTP resilience | Retry-After, jitter, and circuit breaking exist; richer policies and budgets still evolve |
| Public XML (`CS1591`) | Tier A (Conexus baseline packages) enforced; other shipped libs deferred — see [`quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](./quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md) |
| Coverage thresholds | CI produces reports; numeric gates remain advisory |
| Replay | Contracts only; no replay engine in Platform |
| Production readiness | **Not started** |

---

## Docker-local and operator programs (closed — inherited constraints)

These programs closed PASS but their constraints still apply when using local/docker tooling:

| Area | Limitation |
| --- | --- |
| Conexus default | Fake/local provider in Development; no real external calls unless explicitly opted in |
| Credentials | Development keys only; no production TLS/identity |
| Frontend config | `VITE_*` compile-time injection in Docker images; runtime nginx env injection deferred |
| Allagma real execution | Disabled by default (`RealExternalExecution.Enabled: false`) |
| Manual eval POST | Gated (`ManualWriteEnabled` + non-production only) |
| Secrets | Never in `VITE_*`, committed reports, or docs |
| Real provider CI | **Forbidden** — local/manual only per [`operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](./operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md) |

Real-provider validation v1 proved one controlled local OpenAI smoke path; fake provider remains default after kill switch. Not provider certification.

---

## CI and documentation

| Area | Limitation |
| --- | --- |
| Path-scoped CI | Docs-only PRs skip dotnet build; use `run-full-ci` label or dispatch for full validation |
| Branch protection | Not configured by platform docs; when enabled, prefer aggregate checks |
| `_donors/` | Historical Agentor/Athanor extraction code — not active product naming |

---

## Where to look for system status

| Need | Location |
| --- | --- |
| Runtime baseline lock | `allagma-dotnet` evidence index |
| System cohesion / E2E | `allagma-dotnet/docs/e2e/` |
| Semantic / gateway behavior | `kanon-dotnet`, `conexus-dotnet` docs |

Platform [`CURRENT_STATE.md`](./CURRENT_STATE.md) describes **mechanical** delivery only.
