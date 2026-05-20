# ALLAGMA-EVIDENCE-001 — Tighten run audit and evaluation evidence export for frontend and system E2E use

**Priority:** P1  
**Repo:** allagma-dotnet  
**Theme:** Evidence / replay

## Problem

Allagma has rich audit/evaluation endpoints; the next value is ensuring evidence bundles serve both operator UX and automated cross-repo proof.

## Scope

Document canonical fields for run audit bundle, eval evidence, baseline comparison, topology summary, downstream references, and redaction notes.

## Acceptance criteria

Frontend can render an audit/evidence panel using live APIs; system E2E summary links every run to Kanon decision/provenance and Conexus model-call IDs.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
