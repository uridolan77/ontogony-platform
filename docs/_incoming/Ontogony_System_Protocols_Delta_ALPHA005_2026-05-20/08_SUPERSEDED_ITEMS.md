# Superseded or stale items from older packages/reviews

Do not re-open these as fresh work unless new evidence shows regression.

| Old item / concern | Current status |
|---|---|
| Create compatibility/environment/auth/route/test matrices | Implemented in Allagma system docs. |
| Remove Allagma hard-coded `gpt-4o-mini` | Implemented via `IAllagmaModelPurposeCatalog` and `ConexusModelAlias`. |
| Add shared cross-service error envelope | Implemented in `Ontogony.Errors`; Allagma mapping docs exist. |
| Context propagation across Allagma → Kanon/Conexus | Implemented through `AllagmaRunContextPropagation`; still worth validating in system E2E. |
| Conexus tools/tool_choice missing | Current contract includes `tools`, `tool_choice`, `response_format`, `tool_calls`, `tool_call_id`, `function_call`. |
| Kanon mixed local error bodies | README now documents normalized paths and typed DTO exceptions. Do not use old wording without checking. |
| Evidence Spine Kanon decision graph B-013 | Cleared by FE-EVIDENCE-SPINE-002 with Docker-live Playwright 5/5. |
| Frontend format B-010 | Cleared. |
| Conexus production exposure fixture B-011 | Cleared. |
| Sprint 4.5 says runtime baseline promotion pending Alpha-004 | Superseded by Alpha-005 lock/closeout. |
| Broad “prepare system cohesion” package | Superseded by this narrower post-Alpha-005 delta package. |
