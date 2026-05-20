# conexus-dotnet plan

## Priority

Conexus should close gateway-contract gaps that block agentic callers and operator drill-down.

## Keep

- OpenAI-compatible chat completions.
- Provider abstraction and routing/fallback.
- Project API key client contract.
- Streaming support.
- Governance usage summary endpoint.
- Admin model-call, route-decision, diagnostics, retention, and evidence surfaces.

## Implement next

1. `CONEXUS-TOOLS-001`
2. `CONEXUS-GOV-DRILLDOWN-001`
3. `CONEXUS-IDEMP-001`
4. `CONEXUS-STREAM-COST-001`
5. `CONEXUS-ADMIN-SAFETY-001`
6. `CONEXUS-RETENTION-001`

## Notes

The package does not treat governance as a missing stub because a project-scoped usage endpoint exists. The real work is drill-down completeness, evidence contracts, and frontend alignment.
