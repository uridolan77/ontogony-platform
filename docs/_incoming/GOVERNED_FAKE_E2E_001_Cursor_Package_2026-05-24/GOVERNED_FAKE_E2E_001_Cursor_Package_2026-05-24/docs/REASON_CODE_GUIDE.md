# Reason Code Guide

Use reason codes instead of vague copy.

| Reason code | Meaning |
| --- | --- |
| `not_applicable` | This edge is not expected for this flow. |
| `not_recorded` | The event happened but no durable evidence was recorded. |
| `not_resolved` | Identifier is known but resolver did not resolve it. |
| `not_found` | Source returned a clean not-found. |
| `backend_missing` | Required backend endpoint does not exist. |
| `authorization_failed` | Request was forbidden or unauthorized. |
| `source_failure` | Backend returned unexpected error. |
| `fixture_only` | Evidence came from fixture/demo data only. |
| `not_implemented` | Capability is not implemented yet. |
| `id_mismatch` | Two sources disagree about identifier value. |
| `store_mismatch` | Writer and reader appear to use different stores. |
