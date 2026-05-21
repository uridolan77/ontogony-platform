# Reviewer checklist

## SYSTEM-ALPHA-007

- [ ] Runtime lock is read from Allagma.
- [ ] Locked mode checks out exact pinned commits.
- [ ] Evidence bundle contains four repo SHAs.
- [ ] Release mode rejects moving-main evidence.
- [ ] Bundle validates against schema.
- [ ] Bundle includes package-mode summary.
- [ ] Bundle includes system-cohesion summary.
- [ ] No secrets in bundle.

## SYSTEM-ALPHA-008

- [ ] Manual dispatch works.
- [ ] Nightly schedule exists.
- [ ] Scenarios include completed run, human gates, assistance, fallback, restart, streaming, capacity.
- [ ] Scenario-level summary exists.
- [ ] Artifacts upload on failure.
- [ ] Drift mode is labeled as non-release evidence.

## PLATFORM-REL-001

- [ ] Package versions match runtime lock.
- [ ] Allagma restore/build/test uses package flags.
- [ ] No sibling ProjectReference path used in package mode.
- [ ] Summary JSON validates.
- [ ] Package-mode summary is consumed by SYSTEM-ALPHA-007.
