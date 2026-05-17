# Risk Register

| Risk | Impact | Mitigation |
|---|---|---|
| Backend adds routes not represented in OpenAPI | Frontend cannot safely consume | CI OpenAPI artifact + drift check |
| Frontend removes limitation banner too early | Operators see fake capability | Require backend route + OpenAPI + E2E before removal |
| Trace metadata casing diverges | Correlation breaks | Platform constants and conformance tests |
| Evidence endpoints leak secrets | Security issue | Backend redaction tests + frontend corpus tests |
| Allagma operation semantics unclear | Dangerous run control | Capability metadata + status-specific tests |
| SSE streaming contract remains undocumented | Client/server mismatch | Document SSE capability or companion markdown contract |
| Historical Agentor naming reappears | Confusion | Keep no-Agentor source gate; docs clarify former name |
| Cross-repo versions drift | RC not reproducible | System compatibility evidence bundle with all SHAs |
| Local stack CI becomes too heavy | CI instability | Start with smoke profile and mocks/test providers |
