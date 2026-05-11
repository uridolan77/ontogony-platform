# Outbox Contract

This document defines SQL-agnostic outbox contracts in Ontogony.Persistence.

## Purpose

Provide stable, mechanical contracts for durable event dispatch without introducing service-specific schemas or repositories.

## Interfaces

- IOutboxWriter: writes outbox records.
- IOutboxReader: reads available records for dispatch.
- IOutboxDispatcher: marks dispatch success/failure and schedules retries.
- IUnitOfWorkBoundary: executes atomic work boundaries.

## Core Record

OutboxMessage fields:

- message_id
- event_id
- event_type
- source
- trace_id
- occurred_at
- available_at
- attempt_count
- last_error
- payload_json
- payload_hash
- metadata_json

## Constraints

- Contracts are persistence-provider neutral.
- No service/domain semantics are part of these contracts.
- No generic repository abstractions are required.
- Retry scheduling is mechanical and deterministic via OutboxContracts.CalculateNextAvailableAt.

## Notes

Provider-specific implementations (for example PostgreSQL) should live in dedicated packages and map these contracts to storage models.
