# 11 — Risk register

| Risk | Severity | Why it matters | Mitigation |
|---|---:|---|---|
| Local settings silently override new runtime config | High | Operator changes Docker profile but browser keeps old localhost/custom URLs. | Add provenance map, legacy-local state, and clear-to-runtime controls. |
| Runtime config accidentally contains secrets | High | Static JSON is public to the browser. | Validator rejects secret-like keys; docs and smoke checks forbid secrets. |
| Browser runtime config uses compose-internal service names | High | Browser cannot resolve `kanon-api:8080`. | Docs and generator distinguish browser URLs from internal backend URLs. Smoke checks actual browser-facing URLs. |
| Build-time VITE service URLs remain authoritative | High | Requirement not met; image rebuild still needed. | Demote/remove frontend service URL Docker args; contract checks assert runtime config path. |
| Missing runtime config breaks Vite dev | Medium | Developer friction. | Missing config fails soft with safe fallback and visible warning. |
| Invalid runtime config causes blank app | Medium | Bad JSON should not brick local console. | Loader never throws past bootstrap; invalid state uses fallback. |
| Provenance UI becomes noisy | Medium | Settings page becomes more confusing. | Use one summary + disclosure; compact per-field source text only. |
| Runtime config becomes hidden authority | Medium | Operators cannot tell why values changed. | Show runtime profile/status and per-field source. Runtime config only supplies defaults. |
| Tests pass through stale local storage | Medium | E2E masks real runtime config defects. | Clear storage in relevant tests; add explicit runtime config assertions. |
| Contract discipline treats config as backend route | Medium | False route parity failures. | Document static asset classification; do not add to service OpenAPI inventories. |
| Runtime config changes reset user choices unexpectedly | Medium | Bad user experience and dangerous operator confusion. | Local override wins; reset only when operator explicitly clears. |
| `defaultOperatorSettings` remains globally fixed | Medium | Runtime defaults do not flow everywhere. | Convert to factory and thread snapshot through provider; keep constant as fallback only. |
| Backend CORS blocks custom frontend profile | Medium | Custom stack appears down. | Docs explain CORS must allow frontend origin; smoke surfaces CORS/unreachable distinctly. |
| Docker bind-mounted config update unreliable on Windows | Low/Medium | Runtime config may not refresh until restart. | Acceptance requires no image rebuild; document container restart if needed. |
| Over-scoping into IAM/security | High | Sprint becomes too large and violates non-goals. | Keep local-alpha warnings; do not alter auth modes or implement OIDC. |

## Anti-patterns to reject during implementation

- Adding API keys/service tokens to runtime config “for convenience”.
- Replacing operator settings with a new environment page.
- Requiring frontend rebuild to change service URLs.
- Silently falling back from live service calls to fixtures.
- Hiding invalid runtime config warnings.
- Using Docker internal service DNS names in browser config.
- Adding `/operator-runtime-config.json` to backend route inventories.
- Removing existing credential separation/local consent behavior.
- Conflating Conexus model alias with provider model name.
- Treating runtime config as production identity/security control.
