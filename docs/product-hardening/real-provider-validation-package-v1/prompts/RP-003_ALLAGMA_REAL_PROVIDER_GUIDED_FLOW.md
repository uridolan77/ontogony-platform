# RP-003 — Allagma Real Provider Guided Flow Prompt

```text
We are starting RP-003 — Allagma real-provider guided flow.

Repos:
- uridolan77/allagma-dotnet
- uridolan77/ontogony-platform
- uridolan77/conexus-dotnet only if direct fix is needed

Prerequisite:
- RP-001 done
- RP-002 done

Goal:
Run a tiny Allagma guided flow through Conexus real-provider mode and verify run/eval/correlation/evidence behavior.

Boundary:
- Local validation only.
- No production readiness.
- No CI real-provider test.
- No secrets committed.
- No benchmark prompts.
- No external sandbox/tool execution.
- No customer data.

Tasks:
1. Inspect guided flow and Conexus call boundary.
2. Add local-only real-provider guided flow script if useful.
3. Use one non-sensitive small prompt.
4. Run one subject flow and one minimal eval if needed.
5. Verify run/eval persistence, trace/correlation IDs, Conexus evidence, Kanon topology behavior, and eval evidence export.
6. Redact raw provider payloads.
7. Disable real mode and rerun fake-provider guided flow.

Deliver:
- allagma docs/evidence/RP_003_ALLAGMA_REAL_PROVIDER_GUIDED_FLOW_EVIDENCE.md
- platform docs/evidence/RP_003_ALLAGMA_REAL_PROVIDER_GUIDED_FLOW_EVIDENCE.md

Acceptance:
- Allagma completes controlled real-provider flow or external failure is classified
- fake-provider regression passes
- no secrets
- not production readiness

Suggested commits:
- feat(allagma): RP-003 validate real provider guided flow
- docs(product): RP-003 record Allagma real provider guided flow
```
