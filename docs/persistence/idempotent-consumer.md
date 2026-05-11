# Idempotent Consumer Contract

This document defines neutral contracts for consumer-side deduplication in Ontogony.Persistence.

## Purpose

Prevent duplicate processing when the same message is delivered more than once.

## Interface

- IProcessedMessageStore

Methods:

- HasProcessedAsync(consumerName, messageId)
- MarkProcessedAsync(ProcessedMessage)

## Processed Message Record

ProcessedMessage fields:

- consumer_name
- message_id
- processed_at
- trace_id (optional)

## Key Construction

Use OutboxContracts.BuildProcessedMessageKey(consumerName, messageId) to produce deterministic keys.

## Guidelines

- Keys should be unique per consumer and message pair.
- Writes should be atomic with business side effects where supported.
- Keep implementation free of service-specific meaning.
