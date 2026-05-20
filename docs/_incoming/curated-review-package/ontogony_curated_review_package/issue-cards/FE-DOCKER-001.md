# FE-DOCKER-001 — Complete Docker-local frontend composition and ontogony-ui build strategy

**Priority:** P1  
**Repo:** ontogony-frontend or ontogony-platform  
**Theme:** Local environment

## Problem

The frontend is meant to operate against the three-node local stack; workspace-linked UI package must be reliable inside containers.

## Scope

docker-compose local service, VITE_CONEXUS_API_URL/VITE_KANON_API_URL/VITE_ALLAGMA_API_URL mapping, Nginx config, UI package build/copy strategy.

## Acceptance criteria

One local command serves the frontend and reaches Conexus/Kanon/Allagma health plus at least one live page per domain.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
