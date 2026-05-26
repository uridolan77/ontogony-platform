# 10 — UI canonicalization requirements

## Goal

Add replay without UI clutter.

## Required UI behavior

Replay UI must:

- have one primary task: classify and execute a replay/simulation/reconstruction for a root identifier;
- show replay mode clearly at all times;
- show provider/tool execution policy clearly;
- show unavailable and skipped reasons clearly;
- preserve Evidence Spine links;
- keep raw payloads and source attempts collapsed by default;
- support export without duplicating Evidence Spine export UX;
- use canonical components;
- remain navigable by keyboard;
- avoid visual overload.

## Required canonical primitives

Use these from `ontogony-ui`:

- `OperatorPageFrame`
- `OperatorSignalSummary`
- `OperatorDisclosure`
- `OperatorDataTable`
- `ConfirmDialog`
- `DestructiveConfirmDialog` only for future truly risky/destructive actions.

## Component rules

### Page frame

Use one `OperatorPageFrame`. Do not create a replay-specific local page frame.

### Summary

Use `OperatorSignalSummary` for mode/status/safety/evidence coverage. Do not create badge soup.

### Progressive disclosure

Use `OperatorDisclosure` for:

- raw service attempts;
- raw request/response payload excerpts;
- evidence graph snapshot details;
- redaction metadata;
- build/runtime metadata.

### Tables

Use canonical table/list patterns for:

- service attempts;
- delta comparisons;
- evidence refs;
- unavailable reasons.

### Confirmation

Use `ConfirmDialog` for deterministic simulation/dry-run execution. The confirmation copy must explicitly say real providers/tools are blocked.

## Warning/banner policy

At most one top-level warning banner. Prefer structured signal summary over repeated banners.

## Bad UI patterns to avoid

- New page-local wrapper components named generically like `Panel`, `Card`, `BadgeRow` when canonical alternatives exist.
- Expanding all raw JSON by default.
- Hiding replay mode inside a small badge only.
- Showing exact replay as an option when unavailable.
- Combining Evidence Spine graph, Agent Interaction timeline, raw payloads, and delta comparison all open by default.
- Adding replay links everywhere in navigation.

## Temporary bridge policy

If a one-off wrapper is unavoidable, name it with `TemporaryBridge` or classify it in docs as `temporary_bridge`, with:

- why it exists;
- canonical primitive gap;
- deletion/promotion condition;
- owning follow-up ticket.
