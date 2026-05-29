# Review checklist

## Mechanics-only

- [ ] Does every new file pass the reuse test?
- [ ] Does every new Platform concept stay provider-neutral?
- [ ] Does every new Platform concept stay ontology-neutral?
- [ ] Does every new Platform concept stay workflow-neutral?
- [ ] Does every new Platform concept stay domain-neutral?

## Conformance

- [ ] Scripts accept `-RepoRoot`, `-ConsumerName`, `-OutputDirectory`.
- [ ] Scripts produce JSON evidence.
- [ ] Scripts distinguish PASS/PARTIAL/FAIL/NOT_RUN.
- [ ] Scripts do not require secrets.
- [ ] Scripts do not call real providers.
- [ ] Scripts can run in fixture/static mode.

## Schemas

- [ ] Every schema has `$id`, `title`, `type`, and version.
- [ ] Every schema has at least one valid fixture and one invalid fixture.
- [ ] Schemas are neutral and product-agnostic.
- [ ] Schema registry lists owner, stability, compatibility policy.

## Closeout

- [ ] No production readiness claim.
- [ ] No runtime lock authority claim.
- [ ] No product semantics added to Platform.
- [ ] Consumer adoption status is honest.
