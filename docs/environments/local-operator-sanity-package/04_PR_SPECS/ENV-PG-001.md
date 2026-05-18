# ENV-PG-001 — Postgres durable environment mode

Repos: `allagma-dotnet`, `conexus-dotnet`

Goal: prove durable records survive restart.

Add docs/scripts for:

- starting local Postgres
- running migrations
- running main flow
- restarting Allagma
- verifying eval records and baseline comparison still fetch

Acceptance:

```text
baseline evaluation survives restart
subject evaluation survives restart
baseline comparison survives restart
```

Do not make Postgres mandatory for first fake-provider sanity.
