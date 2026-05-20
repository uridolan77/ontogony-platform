# Governed Run Workbench Spec

Enhance existing Allagma routes:

```text
/allagma/runs
/allagma/runs/start
/allagma/runs/:runId
/allagma/gates
```

## Required run templates

1. Simple governed run.
2. Human-gated approve.
3. Human-gated deny.
4. Conexus fallback.
5. Kanon assistance draft-only.
6. Streaming model-purpose.

## Run detail requirements

- run id;
- status;
- objective;
- actor;
- trace/correlation ids;
- event timeline;
- Kanon decision refs;
- Conexus model-call refs;
- human gate refs;
- operations panel;
- Evidence Spine link.
