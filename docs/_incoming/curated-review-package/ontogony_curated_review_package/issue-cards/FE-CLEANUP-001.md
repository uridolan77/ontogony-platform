# FE-CLEANUP-001 — Resolve duplicate root config artifacts

**Priority:** P0  
**Repo:** ontogony-frontend  
**Theme:** Build correctness

## Problem

The repo contains both TypeScript source configs and generated JavaScript/declaration configs; this creates build ambiguity and Docker context risk.

## Scope

Decide canonical config files; remove or regenerate intentionally; update .gitignore and CI to prevent accidental committed build artifacts.

## Acceptance criteria

Only intended Vite/Vitest/Tailwind configs are present; CI proves Vite and Vitest resolve the canonical files.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
