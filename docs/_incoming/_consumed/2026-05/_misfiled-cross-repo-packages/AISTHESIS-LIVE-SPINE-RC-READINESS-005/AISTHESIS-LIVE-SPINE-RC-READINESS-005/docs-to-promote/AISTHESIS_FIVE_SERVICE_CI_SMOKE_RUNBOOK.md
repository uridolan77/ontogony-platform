# Aisthesis five-service CI smoke runbook

## Purpose

Run a repeatable smoke test proving Aisthesis can query reconstructability evidence produced by the Ontogony backend services.

## Modes

```powershell
./scripts/system/run-five-service-ci-smoke.ps1 -Mode Preflight
./scripts/system/run-five-service-ci-smoke.ps1 -Mode Fixture
./scripts/system/run-five-service-ci-smoke.ps1 -Mode Live
```

## Preflight

Use for CI setup validation. Does not claim live proof.

## Fixture

Uses the required-edge fixture. Claims Aisthesis evaluator/harness proof only.

## Live

Requires all five services and a configured live workflow trigger. Claims live proof only if native producer evidence is observed.

## Redaction

Summary artifacts must not include tokens, raw prompts, raw payloads, or sensitive payload references dereferenced into content.
