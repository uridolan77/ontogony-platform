# Correlation/header schema contract

## Purpose

Define the neutral propagation envelope for trace, correlation, causation, actor, tenant, and idempotency context.

## Required header families

- trace id;
- correlation id;
- causation id;
- idempotency key;
- actor context reference;
- request source service;
- redaction marker when payloads are references.

## Rule

Platform defines the transport mechanics. Product repos define what their operations mean.
