# Library and Framework Decisions

## .NET Aspire

Use outside platform shipping packages. Good for a future `ontogony-devhost` / AppHost composition.

## Microsoft Agent Framework

Do not add to platform. Correct home: Agentor.

## Orleans

Do not add to platform now. Correct home: Agentor if durable distributed sessions/actors are needed.

## YARP

Do not add to platform packages. Correct home: gateway/apphost composition.

## Wolverine / MassTransit

Do not add now. Evaluate only if existing platform messaging/outbox/idempotency mechanics are insufficient.

## Provider SDKs

Never add to platform. Provider SDKs belong behind Conexus.

## Npgsql / Postgres

Keep reusable Postgres mechanics in `Ontogony.Persistence.Postgres`. Product semantic schemas remain in product repos.
