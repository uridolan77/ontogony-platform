# AISTHESIS_LIVE_CERT_DIAGNOSTICS_V1

Required live-cert diagnostics: trigger profile, URL, method, request body hash, HTTP status, response excerpt, extracted traceId, service identity preflight, failure kind, producers observed, missing edge groups.

Failure kinds:

```text
service_identity_mismatch
producer_trigger_http_failure
producer_trigger_no_trace
producer_trace_not_ingested
producer_edges_missing
producer_edges_incomplete
certification_pass
```
