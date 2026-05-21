# 10 — Identity and security roadmap

## Current alpha posture

| Service | Current auth | Production status |
|---|---|---|
| Allagma | shared service token | alpha only |
| Kanon | development trusted headers or service token | alpha/pre-prod only |
| Conexus | project API key + admin alpha keys/scoped keys | alpha/pre-prod only |
| Frontend/operator | local/dev auth assumptions | not enterprise-ready |

## Roadmap tracks

### Track A — Service identity

- Define service identity claims: service name, environment, deployment, tenant/project, actor context trust level.
- Move from trusted headers to verified identity assertions.
- Candidate production options: OIDC workload identity, mTLS, service mesh identity, signed internal gateway headers.

### Track B — Actor propagation

- Standardize headers:
  - `traceparent`,
  - `X-Correlation-ID`,
  - `X-Ontogony-Actor-Id`,
  - `X-Ontogony-Actor-Type`,
  - `X-Ontogony-Roles`,
  - `Idempotency-Key`,
  - `X-Allagma-Run-Id`,
  - `X-Kanon-Decision-Id`,
  - `X-Conexus-Model-Call-Id`.
- Document which headers are accepted from external clients vs minted internally.

### Track C — Secrets and keys

- Replace dev defaults with environment-specific secrets in non-dev.
- Add rotation runbook for:
  - Allagma service token,
  - Kanon service token,
  - Conexus project keys,
  - Conexus admin scoped keys,
  - provider API keys,
  - replay export signing keys.

### Track D — Real tool execution safety

Real external tool execution remains blocked. Graduation requires:

- per-tool permission model,
- outbound allowlist,
- filesystem allowlist,
- per-tool secret scope,
- dry-run vs execute split,
- durable side-effect ledger,
- no replay re-execution of side effects,
- cancellation/timeout policy,
- human gate for consequential actions,
- audit/export evidence.

## Acceptance for production auth planning

- No service trusts arbitrary external actor headers.
- Each downstream call has verified service identity.
- Operator actions are traceable to user/service actor.
- All secrets have rotation guidance.
- Production readiness docs do not imply real tool execution is enabled.
