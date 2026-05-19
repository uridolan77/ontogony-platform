# Real provider validation v1 — known limitations

**Date:** 2026-05-19  
**Scope:** `RP-CLOSEOUT-001` (package sequence `RP-000` … `RP-005`)

**This is controlled local real-provider validation, not production readiness.**

Canonical package notes: [`06_KNOWN_LIMITATIONS_INITIAL.md`](../product-hardening/real-provider-validation-package-v1/06_KNOWN_LIMITATIONS_INITIAL.md)

These items are **accepted** for closing real provider validation v1. They are not blockers for repeating the documented local manual path unless you are targeting production deploy, cloud hosting, or CI real-provider automation.

## Program boundary

- **Not production readiness.**
- **Not cloud deployment.**
- **Not real user or customer traffic.**
- **Not a provider benchmark** (single provider/model smoke only).
- **Not load, performance, or SLO validation.**
- **Not enterprise cost governance** — local/manual caps only.
- **Local/manual real-provider key only** — shell env, never committed.
- **No CI real-provider calls** — forbidden by policy and gates.

## Inherited from prior closed programs

Prior Docker-local, post-hardening, and PFH limitations still apply unless superseded by RP-specific proof:

- [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md)
- [POST_DOCKER_LOCAL_HARDENING_KNOWN_LIMITATIONS.md](./POST_DOCKER_LOCAL_HARDENING_KNOWN_LIMITATIONS.md)
- [PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md)

Including: development credentials, `VITE_*` compile-time injection, and InMemory persistence modes where applicable.

## RP-specific limitations (accepted at closeout)

| Limitation | Detail |
| --- | --- |
| Script reporting bug | Guided-flow JSON marks `realProvider.status=classified` despite successful Allagma real run because PowerShell probe object lacks `selectedProviderKey`; use eval `route_provider_key` and run status |
| Token/cost metrics gap | `route_input_tokens` / `route_output_tokens` empty on eval metrics in RP-005 run; no cost totals in export |
| Conexus execution-run lookup | Admin path returned **404** for model-call id used as request id; route evidence still on eval |
| Docker UI banners | RP-004 source + E2E PASS; live container UI may need `docker compose build ontogony-frontend` |
| Single-session proof | One controlled end-to-end PASS documented; not repeated across operators/machines |
| TLS environment | RP-003A required Avast root CA inject in Docker runtime for this workstation |
| Budget enforcement | Manual/script caps (≤3 calls, small model); not billing integration |
| Provider scope | OpenAI `gpt-4o-mini` smoke only; no matrix of providers/models |
| Kanon | Topology unchanged; no new semantic authority from provider output |

## Accepted stance

Document limitations honestly. Do not claim production readiness, cloud readiness, or provider certification from this package.

## Required statement

```text
Real provider validation v1 proves one controlled local path.
Fake provider remains the default after kill switch.
Not production readiness.
```
