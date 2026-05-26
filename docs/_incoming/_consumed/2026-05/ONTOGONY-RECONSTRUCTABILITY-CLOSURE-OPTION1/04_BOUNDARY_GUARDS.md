# Boundary guards

## Platform

Allowed:
- mechanics;
- DTO abstractions where protocol-neutral;
- conformance kits;
- error/header/idempotency/observability utilities.

Forbidden:
- ontology concepts;
- Kanon classifier rules;
- Allagma run lifecycle semantics;
- Conexus provider routing semantics;
- product workflows.

## Kanon

Allowed:
- reconstructability classifier;
- semantic policies;
- decision profiles;
- decision-record projection;
- cross-service event classification.

Forbidden:
- executing Allagma runs;
- routing LLM provider calls;
- owning provider pricing/fallback;
- storing raw model prompts by default.

## Allagma

Allowed:
- governed run execution;
- event projection;
- linking Kanon and Conexus identifiers;
- classifier integration tests using Kanon;
- evidence bundle assembly.

Forbidden:
- implementing classifier logic locally;
- implementing Conexus route/provider decisions;
- raw provider SDK references;
- semantic ontology authority.

## Conexus

Allowed:
- model access;
- route/model/quota/cache/streaming decision-event emitters;
- evidence bundles;
- usage/cost/provider capability metadata.

Forbidden:
- Kanon semantic policy rules;
- Allagma run orchestration;
- tool execution;
- raw prompt/response exposure in reconstructability exports unless explicitly redacted/hardened.

## Frontend

Allowed:
- render backend-provided reconstructability results;
- link evidence;
- show PASS/WARN/FAIL and missing fragments.

Forbidden:
- computing classifier results client-side;
- inventing semantic grades;
- bypassing backend redaction.
