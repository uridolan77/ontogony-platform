# Consumer summary evidence

Place consumer conformance summaries here during closeout.

Expected shape:

```text
artifacts/platform-mechanics-conformance/<consumer>/<timestamp>/summary.json
```

Status interpretation:

- PASS: all applicable checks passed.
- PARTIAL: missing optional/fixture/live evidence, no hard violation.
- FAIL: violation found.
- NOT_RUN: consumer repo/config unavailable.
