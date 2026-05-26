# Expected Source Attempt Display

## Good source attempt

```text
conexus · error · GET /admin/v0/route-decisions/{routeDecisionId}
routeDecisionId: rd-0HNLMJJQFVG3N-00000003 · 10ms · backend_missing · 404
Route decision ID was recorded in model-call evidence links, but no route-decision detail record exists.
Suggested next step: verify Conexus route-decision persistence or rerun with route-decision recording enabled.
```

## Good not-applicable attempt

```text
kanon · not applicable · /ontology/v0/decision-records/{decisionId}
root: direct Conexus model call · not_applicable
This model call was resolved directly from Conexus and is not known to have passed through Allagma/Kanon governance.
```

## Good success attempt

```text
allagma · success · GET /allagma/v0/runs/{runId}
runId: run_fcde... · 7ms · 200
Resolved run detail and governance metadata.
```

## Bad source attempt

```text
conexus
error
/admin/v0/route-decisions/{routeDecisionId}
An unexpected error occurred.
```
