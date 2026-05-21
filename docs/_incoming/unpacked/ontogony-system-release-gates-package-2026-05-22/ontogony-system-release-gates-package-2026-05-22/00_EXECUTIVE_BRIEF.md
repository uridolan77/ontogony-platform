# Executive brief

The Ontogony .NET backend has reached a meaningful alpha state: Platform supplies mechanics, Kanon owns semantic authority, Conexus owns model access, and Allagma owns governed runtime cohesion. The next bottleneck is not another service feature. The next bottleneck is release discipline.

This package creates a release train around three related moves:

## SYSTEM-ALPHA-007 — full locked-runtime release gate

A release operator selects or confirms the committed Allagma runtime lock, checks out all four repos at the pinned commits, runs the required validation matrix, and emits one release evidence bundle.

This is the release-candidate gate.

## SYSTEM-ALPHA-008 — scheduled full-system cohesion

A nightly/manual workflow runs the real multi-service smoke set on a schedule. It detects drift early, especially when moving `main` continues after a locked baseline.

This is the ongoing confidence gate.

## PLATFORM-REL-001 — package-mode release train

Platform, Kanon, and Conexus are packed into local NuGet packages. Allagma restores/builds/tests against packages only, with sibling project references disabled. The package versions are validated against the runtime lock.

This is the consumer-compatibility gate.

## Why these belong together

The release gate without package-mode can pass while package consumers break. Package-mode without system cohesion can pass while runtime behavior breaks. Scheduled cohesion without a locked release bundle gives smoke confidence but not reproducible release evidence.

Together they form:

```text
compile correctness
+ contract correctness
+ package compatibility
+ runtime behavior
+ restart behavior
+ gateway capacity baseline
+ evidence reproducibility
```

## Key design principles

1. Allagma remains the runtime lock owner.
2. Ontogony.Platform owns release mechanics and canonical operator docs.
3. Kanon and Conexus provide contract/capacity evidence but do not own the release train.
4. Release workflows run against pinned commits by default.
5. Nightly workflows may run moving-main drift mode but must label it as drift, not release evidence.
6. Evidence bundles are immutable artifacts, not edited status prose.
