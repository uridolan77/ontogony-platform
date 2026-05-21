# Implementation order

## Day 1: contracts and evidence schema

1. Add runtime release evidence schema.
2. Add package-mode release summary schema.
3. Add system cohesion scheduled summary schema.
4. Add validators that fail closed.

Do not start with workflow YAML. Start with evidence contracts.

## Day 2: local commands

1. Implement package-mode release train command.
2. Implement cohesion scheduled command as manual local script.
3. Implement locked-runtime release command as orchestrator.

The local command must work before GitHub Actions wiring.

## Day 3: workflows

1. Add manual locked release gate workflow.
2. Add scheduled/manual cohesion workflow.
3. Add package-mode release workflow or upgrade existing Allagma CI job into reusable workflow.

## Day 4: first full run

1. Run package-mode only.
2. Run cohesion only.
3. Run locked release gate.
4. Validate evidence bundle.
5. Write closeout evidence.

## Day 5: tighten and document

1. Fix flaky scripts.
2. Document troubleshooting.
3. Add branch protection guidance.
4. Mark package closed.
