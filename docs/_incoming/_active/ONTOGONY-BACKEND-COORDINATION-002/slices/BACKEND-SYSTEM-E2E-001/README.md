# Slice 6 — BACKEND-SYSTEM-E2E-001

**Owner:** `allagma-dotnet`  
**Depends on:** Slice 5  
**Prompt:** [`../prompts/P06_BACKEND_SYSTEM_E2E_001.md`](../prompts/P06_BACKEND_SYSTEM_E2E_001.md)

## Goal

Five-service **governed E2E smoke** with committed evidence JSON — Allagma orchestrates Kanon + Conexus with Metabole and Aisthesis in stack.

## Scenarios (minimum)

1. **First loop:** `POST /allagma/v0/runs` → Kanon evaluate → Conexus completion → completed run + ordered events.
2. **Correlation:** trace ID visible across at least Allagma → Kanon → Conexus in logs/evidence.
3. **Real tools blocked:** conformance test still PASS.

Optional:

4. Human-gate pause/resume (`smoke-first-real-system.ps1`)
5. Postgres restart survival

## Deliverables

1. `SYSTEM_E2E_SCENARIOS.md` updated
2. `SYSTEM_TEST_MATRIX.md` reflects five-service commands
3. Evidence JSON under `docs/evidence/BACKEND_SYSTEM_E2E_001_*.json`
4. Package-mode path documented

## Evidence gate

```json
{
  "package": "BACKEND-SYSTEM-E2E-001",
  "status": "PASS",
  "services": ["kanon", "conexus", "allagma", "metabole", "aisthesis"],
  "scenario": "governed-first-loop"
}
```

## Non-claims

- Not a load test.
- Not production certification.
