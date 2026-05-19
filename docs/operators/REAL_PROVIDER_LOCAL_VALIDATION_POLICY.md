# Real provider local validation policy

**Item:** `RP-001` (Real provider validation v1)  
**Effective:** 2026-05-19  
**Authority repo:** `uridolan77/ontogony-platform`  
**Implementation repo (Conexus):** `uridolan77/conexus-dotnet`  
**Control package:** [`docs/product-hardening/real-provider-validation-package-v1/`](../product-hardening/real-provider-validation-package-v1/)

**Boundary:** Local-only safety posture for controlled real LLM validation after fake-provider manual QA PASS. **Not production readiness.** No cloud deployment, no CI real-provider calls, no secrets in git.

---

## 1. Purpose

After `PRODUCT-MANUAL-QA-002R1` passed on the Docker-local **fake-provider** stack, this policy defines how operators may enable a **small, manual, budgeted** real-provider path through Conexus — without changing the default closed-local posture.

Goals:

1. Keep **fake provider** the default for compose, bootstrap, and regression.
2. Require **explicit opt-in** before any real outbound provider call.
3. Cap cost and blast radius with documented budgets and a kill switch.
4. Forbid real-provider execution in CI and committed secrets.
5. Classify failures so operator evidence stays actionable and redacted.

Conexus-specific mechanics: [`conexus-dotnet/docs/development/REAL_PROVIDER_LOCAL_VALIDATION.md`](https://github.com/uridolan77/conexus-dotnet/blob/main/docs/development/REAL_PROVIDER_LOCAL_VALIDATION.md).

---

## 2. Default posture (non-negotiable)

| Rule | Requirement |
| --- | --- |
| Fake provider default | Docker-local bootstrap, `gpt-4o-mini` dev alias, and manual QA baselines use the **fake** provider unless an operator explicitly enables real mode. |
| Real provider disabled | `CONEXUS_REAL_PROVIDER_ENABLED` must be **`false` or unset** in committed examples, compose defaults, and CI. |
| Explicit opt-in | Real mode requires operator action: set enable gate **and** supply a provider key via **local env only** (see §4). |
| CI | **No** real-provider calls in GitHub Actions or default `dotnet test` filters. External provider smoke remains opt-in locally only. |
| Frontend | **No** provider API keys in browser env, Vite build args, or UI storage. |
| Production readiness | This policy does **not** authorize production deployment, hosted traffic, or prod identity/TLS/secrets programs. |

---

## 3. Enable gate and kill switch

### Enable gate (all required before first real call)

```text
CONEXUS_REAL_PROVIDER_ENABLED=true          # explicit opt-in (RP-002 implements enforcement)
CONEXUS_REAL_PROVIDER_NAME=<provider>       # e.g. openai
CONEXUS_REAL_PROVIDER_MODEL=<small-model>   # cheap / small model only
<provider-api-key via existing env name>    # see §4 — never commit
```

Optional budget overrides (defaults in §5):

```text
CONEXUS_REAL_PROVIDER_MAX_CALLS=3
CONEXUS_REAL_PROVIDER_MAX_OUTPUT_TOKENS=256
```

Operator must confirm: local machine only, manual session, fake-provider baseline recently **PASS**, and evidence will be redacted.

### Kill switch (immediate return to safe mode)

1. Set `CONEXUS_REAL_PROVIDER_ENABLED=false` (or unset).
2. Remove provider API keys from the shell / `.env` (do not commit).
3. Restart Conexus (and Allagma if it cached provider mode) in the Docker-local stack.
4. Rerun fake-provider smoke / `PRODUCT-MANUAL-QA-002R1` regression checklist before closing the RP phase.

---

## 4. Secret handling (local only)

### Allowed

- Operator-local `.env` (gitignored)
- Process environment variables for the current shell session
- OS user secrets / password manager copy-paste into local shell
- Existing Conexus secret locators (`env:CONEXUS_PROVIDER_*_API_KEY`)

### Forbidden

- Committed API keys, bearer tokens, or connection strings
- Real keys in `.env.example`, Dockerfiles, `appsettings*.json`, or GitHub Actions
- Raw keys, prompts, or completions in evidence markdown
- Provider keys in frontend or Allagma client configuration exposed to the browser

### Preferred key env names (reuse existing Conexus keys)

| Provider family | Environment variable |
| --- | --- |
| OpenAI | `CONEXUS_PROVIDER_OPENAI_API_KEY` |
| OpenRouter (OpenAI-compatible) | `CONEXUS_PROVIDER_OPENROUTER_API_KEY` |
| Anthropic | `CONEXUS_PROVIDER_ANTHROPIC_API_KEY` |
| Gemini | `CONEXUS_PROVIDER_GEMINI_API_KEY` |

`CONEXUS_REAL_PROVIDER_API_KEY` is a **documentation alias** for “set the matching `CONEXUS_PROVIDER_*` key locally”; do not add a parallel committed secret name unless RP-002 introduces a validated binding.

---

## 5. Budget caps (initial)

| Control | Default | Notes |
| --- | ---: | --- |
| Max real calls per manual session | **3** | Hard stop; record count in evidence |
| Max output tokens per call | **256** | Small completion only |
| Model | **small / cheap** | Provider-supported mini/haiku/flash class |
| Streaming | Off for first smoke unless RP-002 charter says otherwise | Reduces cost and log surface |
| Manual only | **true** | No scheduled jobs, no CI |
| CI real calls | **disabled** | See [`EXTERNAL_PROVIDER_SMOKE_TESTS`](https://github.com/uridolan77/conexus-dotnet/blob/main/docs/testing/EXTERNAL_PROVIDER_SMOKE_TESTS.md) |

Exceeding budget → classify as **`budget_exceeded`**; do not raise caps in-repo without updating this policy and RP evidence.

---

## 6. Failure classes

Use these labels in evidence and checklists (sanitized detail only):

| Class | Meaning | Typical operator action |
| --- | --- | --- |
| `missing_key` | Enable gate on but provider key env empty | Set local key; never commit |
| `invalid_key` | Provider rejected credentials | Rotate key locally; verify project/account |
| `provider_auth` | 401/403 from provider | Fix key scope or provider account |
| `provider_network_tls` | DNS/TLS/proxy failure | Check local network; reuse Docker CA injection discipline if needed |
| `provider_rate_limit` | 429 / quota | Wait or reduce calls; stay within §5 |
| `model_config_mismatch` | Unknown model or route | Fix `CONEXUS_REAL_PROVIDER_MODEL` / alias |
| `budget_exceeded` | Session cap hit | Kill switch; document in evidence |
| `conexus_routing_defect` | Unexpected route/provider selection | File RP-002 defect; stay in fake mode |
| `allagma_orchestration_defect` | Run/eval path failed after Conexus OK | RP-003 scope |
| `frontend_visibility_defect` | Operator UI hides error state | RP-004 scope |
| `evidence_redaction_defect` | Secret or raw payload in evidence | Stop; redact; do not publish |

---

## 7. Evidence rules

Evidence **may** include: provider name, model name, HTTP status, route decision IDs, model call IDs, run/eval IDs, token counts (if available), sanitized summaries.

Evidence **must not** include: API keys, bearer tokens, connection strings, raw sensitive prompts, raw sensitive completions, or full provider JSON payloads.

Templates: [`real-provider-validation-package-v1/templates/`](../product-hardening/real-provider-validation-package-v1/templates/).

---

## 8. CI and automation

| Surface | Real provider |
| --- | --- |
| Default `dotnet test` / GitHub Actions | **Forbidden** |
| `Category=ExternalProviderSmoke` | Opt-in **local only** with operator keys |
| Docker-local compose default | **Fake provider** |
| Load/soak harness | Fake-provider-centric; no real keys |

---

## 9. Program sequence

| Step | Item | Status |
| --- | --- | --- |
| 0 | `RP-000` package setup | **DONE** |
| 1 | `RP-001` secret, budget, safety gates (this policy) | **DONE** |
| 2 | `RP-002` Conexus real-provider local mode | **Next** |
| 3–6 | Allagma, frontend, manual QA, closeout | Not started |

Evidence: [`docs/evidence/RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md`](../evidence/RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md).

---

## 10. Required statement

```text
Real provider local validation is a controlled local program after fake-provider manual QA PASS.
Fake provider remains the default.
No CI real-provider calls.
No secrets in git.
This is not production readiness.
```
