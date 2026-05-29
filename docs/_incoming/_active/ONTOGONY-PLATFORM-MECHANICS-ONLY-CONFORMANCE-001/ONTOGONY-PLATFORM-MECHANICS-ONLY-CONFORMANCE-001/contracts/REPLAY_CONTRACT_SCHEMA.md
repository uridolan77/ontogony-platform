# Replay contract schema

## Purpose

Define replay manifest mechanics, not replay execution semantics.

## Platform owns

- replay request envelope;
- trace references;
- artifact/evidence refs;
- fingerprint rules;
- dry-run flag;
- redaction markers.

## Product repos own

- whether an operation is replayable;
- side-effect handling;
- semantic equivalence;
- workflow reconstruction.
