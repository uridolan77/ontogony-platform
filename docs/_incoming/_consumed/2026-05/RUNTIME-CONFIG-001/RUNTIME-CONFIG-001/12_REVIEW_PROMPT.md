# 12 — Post-implementation review prompt

Review the completed `RUNTIME-CONFIG-001` implementation across:

- `C:\dev\ontogony-frontend`
- `C:\dev\ontogony-platform`
- `C:\dev\ontogony-ui`
- `C:\dev\conexus-dotnet`
- `C:\dev\kanon-dotnet`
- `C:\dev\allagma-dotnet`

Do not rely on the plan alone. Inspect the actual diff and current files.

Answer these questions precisely:

1. Does the Docker/static frontend now load `/operator-runtime-config.json` at runtime?
2. Can Conexus/Kanon/Allagma browser base URLs change without rebuilding the frontend image?
3. Does Vite dev still work with and without a runtime config file?
4. Are build metadata `VITE_*` values still correctly build-time, while service/profile defaults are runtime-backed?
5. Are raw secrets absent from runtime config and rejected by validation/smoke checks?
6. Is the exact precedence implemented: fallback → runtime → browser-local → session → URL/test → page draft?
7. Does Settings clearly show runtime default vs local override vs session/test/fallback/legacy states?
8. Can a user clear one local override and return to runtime default?
9. Can a user reset all service URLs to runtime defaults without clearing credentials?
10. Are existing local-alpha credential warnings still accurate?
11. Does System Truth use effective service URLs?
12. Do Home, Topology, Evidence Spine, Domain Switcher, Start Run, Agent Interaction, and Conexus Observability use effective service URLs?
13. Does Domain Switcher still persist Kanon ontology choices intentionally?
14. Did Docker-local runtime config generation correctly use browser-facing host URLs, not compose-internal service names?
15. Does nginx serve runtime config with no-cache headers?
16. Do Playwright docker-live tests cover default runtime config, local override, clear override, invalid config, and docker-served config?
17. Does governed fake E2E still pass?
18. Did the implementation avoid backend route contract changes?
19. Did contract discipline stay green or receive appropriate artifact updates?
20. Did docs explain local, docker-local, and custom-stack profiles clearly?

Score the implementation:

- Runtime configurability: /10
- Settings/provenance clarity: /10
- Docker/local-working-system correctness: /10
- Test coverage: /10
- Scope discipline: /10
- Overall readiness for merge: /10

Then list:

- must-fix issues before merge;
- should-fix follow-ups;
- stale concerns from the original package that no longer apply;
- exact commands that passed/failed;
- final recommendation: merge / merge after fixes / do not merge.
