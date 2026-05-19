# 06 — Initial Known Limitations

**Package:** **CLOSED** — consolidated limitations: [REAL_PROVIDER_VALIDATION_KNOWN_LIMITATIONS.md](../../releases/REAL_PROVIDER_VALIDATION_KNOWN_LIMITATIONS.md)

## Program boundary

- Not production readiness.
- Not cloud deployment.
- Not a provider benchmark.
- No real user/customer data.
- No real external sandbox/tool execution.
- Local/manual only.
- Fake provider remains default.
- Cost controls are validation controls, not enterprise billing controls.
- No CI real-provider calls.
- Local/manual real-provider API key in shell only (never in git).

## RP closeout gaps (accepted)

- Guided-flow JSON may mark `realProvider.status=classified` when `selectedProviderKey` is missing on the probe object despite a successful Allagma run — use eval `route_provider_key`.
- `route_input_tokens` / `route_output_tokens` may be empty on eval metrics.
- Docker `ontogony-frontend` image may need rebuild for live RP-004 UI banners.

Provider-specific behavior must be checked against official provider docs during implementation.
