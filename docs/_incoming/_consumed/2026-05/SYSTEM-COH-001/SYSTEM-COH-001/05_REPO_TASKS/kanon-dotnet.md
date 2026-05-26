# Repo task — kanon-dotnet

Kanon owns semantic authority participation in SYSTEM-COH-001.

## Required work

Add or update:

- `docs/integrations/SYSTEM_COHESION_KANON_ALIGNMENT.md`

The doc should state:

1. Which SYSTEM-COH acceptance scenarios Kanon participates in.
2. Which routes are used for plan compilation, action evaluation, human gates, decision lookup, provenance, semantic graph, canonical fact audit, and Conexus assistance.
3. Which routes are server-only/operator-only and must not become Allagma runtime dependencies.
4. How Kanon records trace/correlation/run identifiers.
5. How Kanon assistance remains advisory/draft-only until accepted through Kanon authority.
6. Which error families Allagma should classify as retryable/non-retryable.

## Tests

Add or extend docs/contract tests if Kanon already has handoff/manifest tests.

## Done when

A system reviewer can tell exactly how Kanon satisfies semantic authority, provenance, human-gate, and assistance obligations without reading source code.
