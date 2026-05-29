# Contract — Aisthesis producer edge authorization v0

## Purpose

Prevent producer tokens from spoofing cross-system edges.

## Policy

A producer token may write:

- envelopes where `producerSystem` matches the token producer;
- batches where all envelope producers match token producer;
- edges under relations owned by the producer namespace;
- edge-only batches only when `producerSystem` is declared;
- direct edges only if the direct edge request carries producer identity or the token is wildcard/system.

## Relation ownership

```text
allagma.* -> allagma
kanon.* -> kanon
conexus.* -> conexus
metabole.* -> metabole
aisthesis.* -> aisthesis/system
```

## Admin repair

Wildcard/system tokens may repair edges, but must be audited.
