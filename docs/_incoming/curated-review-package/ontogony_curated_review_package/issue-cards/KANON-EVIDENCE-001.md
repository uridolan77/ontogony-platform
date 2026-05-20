# KANON-EVIDENCE-001 — Add workflow evidence packs for major Kanon surfaces

**Priority:** P0  
**Repo:** kanon-dotnet  
**Theme:** Feature proof

## Problem

Kanon implements substantial behavior; each operator workflow should have reproducible request/response evidence.

## Scope

docs/evidence for ontology lifecycle, source binding test/review, canonical fact resolution, action policy evaluation, human gate resolve, decision provenance/replay, and domain-pack promotion.

## Acceptance criteria

Each evidence pack has commands, expected responses, storage/persistence notes, and failure cases.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
