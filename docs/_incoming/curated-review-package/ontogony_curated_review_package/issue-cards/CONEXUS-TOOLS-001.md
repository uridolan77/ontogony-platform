# CONEXUS-TOOLS-001 — Add OpenAI-compatible tools/tool_choice/function-call pass-through

**Priority:** P0  
**Repo:** conexus-dotnet  
**Theme:** Gateway feature completeness

## Problem

Agentic runtimes need tool declarations to pass through the gateway; current chat request contract has model/messages/temperature/top_p/max_tokens/stream/metadata/user but no tools or tool_choice.

## Scope

Extend contracts, provider abstractions, provider adapters, validation, redaction, raw artifact capture, tests, and OpenAPI examples.

## Acceptance criteria

A request containing tools and tool_choice round-trips through Conexus to an OpenAI-compatible provider or fake provider without contract loss.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
