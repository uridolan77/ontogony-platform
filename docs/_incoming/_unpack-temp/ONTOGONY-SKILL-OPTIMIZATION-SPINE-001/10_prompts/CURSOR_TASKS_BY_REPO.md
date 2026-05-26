# Cursor Tasks by Repo

## `ontogony-platform`

Create cross-repo docs for the Skill Optimization Spine. Add protocol docs, local fixture notes, feature flags, and a lightweight verification checklist. Do not implement backend business logic here unless existing platform scripts require it.

## `kanon-dotnet`

Implement semantic governance for skill artifacts, skill versions, skill edits, evaluation gates, rejected edit buffers, and deployment bindings. Add validation/classification services, decision records, evidence graph roots/edges, tests, docs, and route inventory/OpenAPI updates.

## `conexus-dotnet`

Implement skill injection metadata and separate optimizer/target model call roles. Add fake provider support for deterministic optimizer edit output. Add tests proving normal inference does not call optimizer. Update docs and admin route baselines if routes are added.

## `allagma-dotnet`

Implement the `SkillOptimization` run type/lifecycle and deterministic fixture orchestration. Add operation availability matrix, run timeline, candidate/gate/rejected-buffer projections, replay fixture, tests, docs, and route inventories.

## `ontogony-ui`

Add reusable components for skill artifacts, lifecycle badges, gate score panels, edit diff cards, rejected buffers, deployment bindings, and evidence links. Keep components compact and operator-friendly.

## `ontogony-frontend`

Add Skill Lab pages/panels and integrate links from Allagma/Kanon/Conexus/Evidence Spine pages. Use fixtures first; avoid crowding existing pages. Add frontend tests for key states.

## Final cross-repo task

After implementation, run the full local verification path and update all docs to describe the actual implementation. Mark anything deferred explicitly.
