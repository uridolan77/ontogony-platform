# Schema versioning policy

## Rules

1. Schemas are immutable once marked stable.
2. Additive optional fields are allowed in the same major version.
3. Required-field changes require a new major version.
4. Product-specific fields must live in product repos.
5. Every schema has valid and invalid fixtures.
6. Every schema has a consumer impact section.

## Stability labels

- `draft`
- `alpha`
- `stable`
- `deprecated`
