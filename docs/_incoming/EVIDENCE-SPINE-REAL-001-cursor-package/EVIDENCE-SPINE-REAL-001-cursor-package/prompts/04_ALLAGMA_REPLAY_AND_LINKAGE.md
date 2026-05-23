# Prompt 04 — Allagma Replay and Linkage

```text
Implement or adjust the Allagma side needed for EVIDENCE-SPINE-REAL-001.

Tasks:
1. Inspect run detail, event stream, audit bundle, evaluations, baseline comparison, and replay evidence routes.
2. Ensure run detail exposes planningDecisionId, action decision IDs if available, ontologyVersionId, Conexus modelCallId, traceId, correlationId, provider mode, and model purpose when known.
3. Ensure Allagma persists/exposes the Conexus model-call ID returned from Conexus, not only text output.
4. Ensure event stream contains enough lifecycle events for Evidence Spine to derive Allagma -> Kanon -> Conexus chain.
5. Normalize route documentation and returned links to /allagma/v0/... routes.
6. Add or update tests for a fake-provider completed run producing resolvable evidence IDs.
7. If action-evaluation decisions exist separately from planning decisions, expose them distinctly.

Do not enable real external tool execution in this task.
```
