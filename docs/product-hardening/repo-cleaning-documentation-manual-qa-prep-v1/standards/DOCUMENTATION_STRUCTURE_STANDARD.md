# Documentation Structure Standard v1

## Universal structure

Use this as the target structure for current/future docs. Do not create empty folders just to comply.

```text
docs/
  README.md
  architecture/
  api/
  development/
  deployment/
  evidence/
  operators/
  releases/
  testing/
  troubleshooting/
```

## Universal rules

1. Every repo should have a docs index.
2. Evidence belongs under `docs/evidence/`.
3. Operator-facing docs belong under `docs/operators/`.
4. Local development docs belong under `docs/development/`.
5. Service deployment/readiness docs belong under `docs/deployment/`.
6. Release closeouts and scorecards belong under `docs/releases/`.
7. Historical docs may remain in place; add pointers instead of mass-moving.
8. Every evidence file must record date, scope, commands, results, limitations, and boundary.
9. Never include raw secrets, raw provider payloads, or unredacted connection strings.
10. Every environment/product-hardening closeout must state whether it is **not production readiness**.

## Repo-specific structure

### ontogony-platform

```text
docs/
  README.md
  architecture/
  environments/
  evidence/
  operators/
  product-hardening/
  releases/
  testing/
  troubleshooting/
```

### allagma-dotnet / kanon-dotnet / conexus-dotnet

```text
docs/
  README.md
  api/
  architecture/
  development/
  deployment/
  evidence/
  operators/
  releases/
  testing/
  troubleshooting/
```

### ontogony-frontend

```text
docs/
  README.md
  architecture/
  development/
  evidence/
  operators/
  testing/
  releases/
  troubleshooting/
```

### ontogony-ui

```text
docs/
  README.md
  architecture/
  components/
  development/
  evidence/
  releases/
  testing/
  troubleshooting/
```

## Manageable reorganization rule

Allowed:

- add docs indexes
- add pointers
- fix stale status text
- move a small number of current docs if clearly misplaced
- leave archives in place

Avoid:

- mass moving historical docs
- rewriting old planning docs
- changing many links without validation
- mixing docs reorg with feature implementation
