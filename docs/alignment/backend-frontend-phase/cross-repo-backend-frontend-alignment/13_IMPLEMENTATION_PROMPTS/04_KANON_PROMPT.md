# Prompt — BFA-005 Kanon Provenance/Replay Contract Hardening

Repo: uridolan77/kanon-dotnet

Goal:
Stabilize typed semantic/provenance/replay contracts consumed by frontend.

Tasks:
1. Review replay bundle lookup endpoints.
2. Ensure lookup by decisionId, traceId, entity reference, and runId where appropriate.
3. Add/strengthen typed schemas for replay bundle, provenance items, semantic plan references, domain-pack lifecycle decision IDs.
4. Add lifecycle history schema if currently loose.
5. Add redaction-safe error/details handling.
6. Add tests and docs.

Validation: `dotnet test`; generated OpenAPI.
