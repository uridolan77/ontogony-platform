# Acceptance Checklist

## Contracts

- [ ] `sampling.policy.contract.v0` documented.
- [ ] `sampling.profiles.v0` JSON registry added.
- [ ] Schema/golden fixture tests added.
- [ ] Profile IDs are stable and documented.

## Conexus

- [ ] Profile registry implemented.
- [ ] Resolver implemented.
- [ ] Validator implemented.
- [ ] Provider parameter translator implemented.
- [ ] Resolve/validate/list routes added.
- [ ] All LLM call paths attach effective sampling policy.
- [ ] Raw legacy parameter usage produces warning.
- [ ] Contract-bound operations cannot use creative/diverse sampling silently.
- [ ] Provider capability warnings are traced.

## Kanon

- [ ] Authority mode config added: off/advisory/strict.
- [ ] Sampling-policy evaluation contract added or documented as deferred.
- [ ] Advisory fail-open behavior tested.
- [ ] Strict fail-closed behavior tested.
- [ ] No fake Kanon approval labels when unavailable.

## Allagma

- [ ] Sampling events appear in workflow timeline/evidence.
- [ ] DiversityProbe cannot directly execute side effects.
- [ ] Override events are modeled.
- [ ] Human gate behavior tested or explicitly deferred.

## Frontend

- [ ] Trace drawer shows profile badge.
- [ ] Requested-vs-effective values shown.
- [ ] Warnings/violations shown.
- [ ] Profile legend/read-only view added.
- [ ] Missing sampling data handled gracefully.

## Docs / evidence

- [ ] Implementation notes added.
- [ ] Route inventory/OpenAPI updated where applicable.
- [ ] Evidence index updated where applicable.
- [ ] Deferrals listed explicitly.

## Final gates

- [ ] Existing backend tests pass.
- [ ] Existing frontend tests pass.
- [ ] Package-specific sampling tests pass.
- [ ] No new architectural-boundary violations.
