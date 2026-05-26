# Generic Cursor unpack prompt

You are working inside one of the four Ontogony backend repos. Unpack and implement `ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001` for this repo only.

Rules:

1. Read `README.md`, `00_meta/README_FOR_AGENTS.md`, `01_master/IMPLEMENTATION_DEPTH_MASTER_PLAN.md`, and the repo-specific file under `02_repo_slices/`.
2. Inspect the live repo before changing anything. Do not assume the package is perfectly current.
3. Implement only the slices that belong to this repo.
4. Preserve architecture boundaries.
5. Do not enable real external tool execution.
6. Prefer small vertical PRs over a mega-PR unless the repo owner asks for a single bundle.
7. Update tests and generated artifacts.
8. Write evidence under `docs/evidence/`.
9. Update `CURRENT_STATE.md` and `KNOWN_LIMITATIONS.md` only when the implemented truth changes.
10. End with a concise closeout summary: changed files, tests run, limitations closed, limitations remaining, and next repo dependencies.
