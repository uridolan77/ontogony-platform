# Eval-Based Release Checklist

## Required

- [ ] All repo builds pass.
- [ ] All default tests pass.
- [ ] Cross-repo eval smoke passes.
- [ ] Eval summary validates schema.
- [ ] No real tool execution enabled.
- [ ] High-risk topology requires Kanon authorization.
- [ ] Route decision records exist for model calls.
- [ ] Audit bundle includes topology/eval refs.
- [ ] Observability docs updated.
- [ ] Release evidence index updated.

## Artifact paths

```text
artifacts/eval/<timestamp>/summary.json
artifacts/eval/<timestamp>/audit-bundle.json
artifacts/eval/<timestamp>/route-decision.json
artifacts/eval/<timestamp>/kanon-topology-decision.json
```
