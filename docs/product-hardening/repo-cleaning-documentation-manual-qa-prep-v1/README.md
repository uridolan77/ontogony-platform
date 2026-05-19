# Ontogony Repo Cleaning + Documentation Standard + Manual QA Prep v1

This package comes **after product feature hardening v1** and **before full manual guided QA**.

The purpose is to clean and tighten the six repos, normalize the documentation structure, and then create/execute a deep manual QA checklist for all current implemented features.

## Scope

Repos:

- `ontogony-platform`
- `allagma-dotnet`
- `kanon-dotnet`
- `conexus-dotnet`
- `ontogony-frontend`
- `ontogony-ui`

## Boundary

This is **not production readiness**. It does not include real provider mode, cloud deployment, production auth/TLS/secrets, load testing, SLOs, or broad rewrites.

## Order

```text
1. Register this package.
2. Publish the shared documentation standard.
3. Run one code-cleaning PR per repo.
4. Run one documentation-cleaning PR per repo.
5. Create the full manual guided QA package.
6. Execute and record manual QA results.
```

## Key decision

Use one shared documentation structure, but keep the reorganization manageable:

- create/update indexes
- fix stale status drift
- link evidence and closeouts
- mark historical docs as historical
- avoid mass moving old archives
