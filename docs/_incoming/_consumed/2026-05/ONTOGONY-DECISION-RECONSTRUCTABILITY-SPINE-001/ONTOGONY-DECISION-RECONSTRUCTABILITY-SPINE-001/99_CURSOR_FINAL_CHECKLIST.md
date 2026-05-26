# Final Checklist

Use this before declaring the package complete.

## Architecture

- [ ] Decision reconstructability is implemented as an Evidence Spine overlay, not a separate disconnected system.
- [ ] Each report links to source fragments.
- [ ] The classifier is deterministic.
- [ ] Governance status is separate from strict score.
- [ ] Critical action blocking rules are implemented.

## Safety

- [ ] No hidden chain-of-thought is captured, stored, or displayed.
- [ ] Reasoning evidence is described as safe surrogate evidence.
- [ ] Tests prevent accidental hidden-CoT field additions.

## Backend

- [ ] Kanon contracts compile and serialize.
- [ ] Kanon classifier tests cover F/P/S/O.
- [ ] Allagma emits/provides run operation and human gate fragments.
- [ ] Conexus emits/provides route/model-call fragments.
- [ ] Route inventories/OpenAPI baselines updated where applicable.
- [ ] Docs describe actual implemented routes.

## Frontend

- [ ] Decision Reconstruction panel exists.
- [ ] Evidence Spine nodes link to it.
- [ ] Allagma run/operation detail links to it where possible.
- [ ] Property matrix and missing evidence diagnostics render clearly.
- [ ] Raw fragments are collapsible.
- [ ] Fixture data is marked.

## Tests

- [ ] Full reconstructable action fixture passes.
- [ ] Missing policy fixture fails governance.
- [ ] Missing operator fixture fails governance.
- [ ] Safe reasoning surrogate fixture does not require hidden CoT.
- [ ] Non-mutating action uses structural/not-applicable post-condition.
- [ ] Mutating action without state-after produces diagnostic.
- [ ] Blocked/denied action reconstructs as a valid denial.

## Operator outcome

- [ ] Local operator can inspect one decision and answer: what happened, on whose authority, under which policy, with which evidence, and what state resulted.
