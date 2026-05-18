# Master Prompt — Current Eval Durability to First Sanity

We are starting from completed eval-basing/system-smoke sequence:

```text
EVAL-FIX-001
CX-ROUTE-EVIDENCE-001
AGM-EVAL-001
SYS-EVAL-001
SYS-OBS-EVAL-001
```

Next work:

```text
EVAL-CLEAN-001
EVAL-DUR-001
EVAL-CI-001
EVAL-QUALITY-001
EVAL-DATA-001
FE-EVAL-002
BE-POLISH-001
FE-POLISH-001
ALIGN-EVAL-001
SYS-FULL-SANITY-001
RC-FIRST-SANITY-001
```

Rules:

- no real external execution
- no new topology modes
- no uncalibrated LLM judge gates
- no raw prompts/completions/secrets in artifacts or UI
- fixtures remain visibly distinct from live evidence
- manual eval POST remains gated
- Ontogony.Platform remains neutral

Start with `EVAL-CLEAN-001` if the evidence has not yet been normalized. Otherwise start `EVAL-DUR-001`.
