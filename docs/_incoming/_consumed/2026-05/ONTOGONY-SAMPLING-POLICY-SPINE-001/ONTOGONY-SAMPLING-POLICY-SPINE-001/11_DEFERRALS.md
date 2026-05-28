# Deferrals / Future Packages

This package intentionally stops at governed named profiles and traceability. The following should be deferred unless trivial during implementation.

## Adaptive sampling controller

A later package can select temperature/top-p based on confidence, retrieval quality, domain volatility, or previous failure mode.

Possible name:

```text
CONEXUS-ADAPTIVE-SAMPLING-CONTROLLER-001
```

## Deterministic replay harness

A future package can capture provider seed/model/backend fingerprints and compare replay drift.

Possible name:

```text
ONTOGONY-LLM-REPLAY-DETERMINISM-HARNESS-001
```

## Multi-candidate adjudication protocol

DiversityProbe should eventually pair with explicit adjudication:

- generate alternatives;
- score alternatives;
- choose deterministically;
- optionally ask human;
- only then execute.

Possible name:

```text
ALLAGMA-DIVERSITY-PROBE-ADJUDICATION-001
```

## Profile authoring/admin UI

v0 should use read-only registry. Editing profiles from UI should wait until governance and audit semantics are strong.

Possible name:

```text
CONEXUS-SAMPLING-PROFILE-ADMIN-001
```

## Provider-specific optimal profiles

Do not overfit v0 profiles to a single provider. A future package can add provider/model-specific profile overlays.
