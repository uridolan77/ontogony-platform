# Operator Copy Rules

## Forbidden or restricted phrases

Avoid page-level claims like:

```text
All services are healthy
Live with fixture fallback
Service versions appear aligned
Ready 24 / Gap 0
```

unless backed by structured truth state.

## Preferred wording

Use:

```text
Conexus: live · not ready · contract valid
Kanon: live · ready · contract valid
Allagma: live · ready · contract valid
Compatibility: cannot verify — version metadata missing
Data source: generated artifact
Fixture data: demo only, not readiness evidence
```

## Unknowns

Never render bare `unknown`.

Use:

```text
Provider: unknown
Compatibility: unknown
Task type: unknown
Readiness: unknown
Data source: unknown
```

## Fixture data

Fixture data must always say:

```text
Demo fixture — not live evidence
```

and should not contribute to readiness or release posture.
