# ALLAGMA-STREAM-001 — Plan Conexus streaming integration into Allagma progress events

**Priority:** P2  
**Repo:** allagma-dotnet  
**Theme:** Runtime UX

## Problem

Conexus supports streaming and Allagma has a streaming client path, but the operator event stream should expose safe progress before this becomes default.

## Scope

Define event types for model stream started/chunk/progress/completed; ensure no raw sensitive content is stored by default.

## Acceptance criteria

A non-default local smoke can stream through Conexus and produce redacted Allagma progress events.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
