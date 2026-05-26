# 11 — Acceptance Criteria

## Must pass

- [ ] A live governed fake run can be started locally.
- [ ] Run uses `gaming-core@0.1.0` or configured active ontology.
- [ ] Run uses model purpose `summarize-player-risk`.
- [ ] Conexus resolves to alias `risk-summary-v0`.
- [ ] Provider attempt shows `fake / fake.chat`.
- [ ] Run has trace id.
- [ ] Run has correlation id.
- [ ] Run has Kanon planning decision id.
- [ ] Run has Conexus model call id.
- [ ] Conexus model call has route decision id.
- [ ] Route decision id resolves through `/admin/v0/route-decisions/{id}`.
- [ ] Evidence Spine resolves from run id.
- [ ] Evidence Spine resolves from trace id.
- [ ] Evidence Spine resolves from model call id.
- [ ] Evidence Spine graph contains Allagma, Kanon, Conexus, trace, correlation nodes.
- [ ] No fixture/demo ids are used in the live proof.
- [ ] Direct Conexus chat classifies Kanon decision as not applicable, not missing.
- [ ] Generic "unexpected error" is not shown for route-decision lookup.
- [ ] Exported evidence bundle includes graph, source attempts, missing reason codes, and build metadata.

## Should pass

- [ ] Agent Interaction live lookup can open the run.
- [ ] Latest completed governed fake run shortcut works.
- [ ] Run detail links to Evidence Spine, Kanon decision, Conexus model call.
- [ ] Route detail shows fallback chain, even if empty.
- [ ] UI labels distinguish model call id, request id, and execution run id.

## Must not regress

- [ ] Direct Conexus fake chat still works.
- [ ] Fake provider remains blocked in Production if that guard exists.
- [ ] No real external execution is enabled.
- [ ] No real OpenAI call is required.
- [ ] Existing route/admin auth remains enforced.
