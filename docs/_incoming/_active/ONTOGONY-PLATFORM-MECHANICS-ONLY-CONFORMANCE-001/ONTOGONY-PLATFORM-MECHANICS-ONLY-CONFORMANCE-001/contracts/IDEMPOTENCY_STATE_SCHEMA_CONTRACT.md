# Idempotency state schema contract

## Purpose

Define common idempotency ledger mechanics for replay-safe operations.

## States

- reserved;
- running;
- completed;
- failed;
- expired;
- conflict.

## Required fields

- key;
- operation;
- request fingerprint;
- status;
- created/updated timestamps;
- response reference or error reference.

## Product-specific behavior

Product repos decide what operation is safe to retry. Platform only defines ledger shape.
