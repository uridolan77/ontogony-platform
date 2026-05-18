# Acceptance Scorecard

Score each category 0-10. Release requires no category below 8 and safety categories at 10.

| Category | Target | Score |
|---|---:|---:|
| Allagma RC closeout clean | 10 |  |
| OpenAPI/provenance refreshed | 9 |  |
| Frontend adapter typed and tested | 9 |  |
| Sandbox evidence UI visible | 9 |  |
| Timeline events visible | 9 |  |
| Capability banners accurate | 9 |  |
| No fake functionality | 10 |  |
| No raw content/secrets exposed | 10 |  |
| Compatibility report produced | 9 |  |
| Eval-basing readiness | 8 |  |

## Release blockers

- [ ] Real external execution remains blocked.
- [ ] Sandbox execution forbidden in production.
- [ ] No raw marker content displayed.
- [ ] `SandboxExecuteReplaySkipped` shown as replay-safe, not failure.
- [ ] Frontend handles old audit bundles without crashing.
- [ ] Limitation banners are backed by capability metadata or explicit unsupported state.
