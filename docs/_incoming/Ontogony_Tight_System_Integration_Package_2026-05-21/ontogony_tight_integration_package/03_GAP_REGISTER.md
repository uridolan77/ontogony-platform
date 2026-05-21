# 03 — Integration gap register

## Priority 0 — protect the baseline

| Gap | Impact | Owner | Recommendation |
|---|---|---|---|
| Moving-main drift after runtime lock | Operators may believe `main` equals the released baseline | Allagma + Platform | Add post-lock delta register validation to release gate; every repo delta classified before lock bump |
| Ad hoc feature pressure in frozen repos | Breaks Kanon/Conexus boundary discipline | All | Require explicit next-stage spec for any capability expansion |
| Production-readiness overclaiming | Misleading stakeholder expectations | Allagma | Every closeout must include non-claims and blocked safety areas |

## Priority 1 — tight evidence journey

| Gap | Impact | Owner | Recommendation |
|---|---|---|---|
| Operator evidence is split across run, Kanon, Conexus pages | Audit is slow and non-obvious | Frontend + Allagma | Implement a single cross-service audit journey driven by runId/traceId/modelCallId/decisionId |
| Conexus route-preview/quota safe APIs not surfaced | Operators cannot diagnose routing/quota before calls | Frontend + Conexus | Add operator panels and evidence-spine hooks for these endpoints |
| Streaming evidence is metadata-only and optional | Hard to debug streamed runs | Allagma + Conexus + Frontend | Add stream lifecycle panel; keep payload persistence off by default |
| Kanon evidence-spine handoff not fully consumed | Semantic evidence remains available but not first-class | Frontend + Platform | Use generated entrypoint index in evidence resolver |

## Priority 2 — compatibility and CI gates

| Gap | Impact | Owner | Recommendation |
|---|---|---|---|
| Package-mode compatibility is multi-repo and expensive | Package drift can slip until late | Allagma | Keep full package mode as release gate; add targeted preflight for package version mismatch |
| Cross-service error contracts remain service-specific | Operator failure UX inconsistent | Platform + Allagma | Standardize adapter-level error normalization without forcing Conexus/Kanon public contract breaks |
| Runtime lock validation not universally mandatory | Releases can skip evidence | Allagma + Platform | Release-mode lock validator must require evidence paths and package versions |

## Priority 3 — production-roadmap gaps

| Gap | Impact | Owner | Recommendation |
|---|---|---|---|
| Service tokens/project keys are alpha auth | Not enterprise secure | Platform + all services | Plan OIDC/mTLS/service mesh actor propagation |
| Real tool execution blocked | Correctly limits agent capability | Allagma | Keep blocked; produce safety graduation package separately |
| Durable artifact store incomplete | Replay/export retention limited | Platform + services | Design external artifact storage and retention before production |
| Managed observability not complete | Local evidence not enough for ops | Platform + Allagma | Create SLO dashboards and incident runbooks as separate production readiness track |
