# KCP-006 — Tests, docs, and regression guard

## Problem

Console polish tends to regress unless copy/status semantics are tested.

## Scope

- Add focused tests for new behavior.
- Update any existing route/readiness docs if the frontend owns them.
- Add PR summary notes.

## Required tests

At minimum:

1. Assistance defaults contain no secret-like field/value.
2. Redaction preview renders included/redacted/omitted fields.
3. Domain-pack disabled actions render reasons.
4. Domain-pack inventory distinguishes disk packs from active ontology versions.
5. Review Queue / Policies partial states render reasons.
6. Evidence links have contextual labels.

## Acceptance

- Existing test suite passes.
- New tests fail on the previous unsafe/vague behavior.
- PR summary names all commands run.
