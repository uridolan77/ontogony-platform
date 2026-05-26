# Allagma runtime lock promotion prompt

After Platform, Conexus, and Kanon contract slices land, work in `allagma-dotnet`.

Task: promote the runtime lock and close `ONTOGONY-CONTRACT-DISCIPLINE-OVER9-001`.

Steps:

1. Read latest compatibility manifests from Conexus/Kanon and Platform compatibility gate output.
2. Update `docs/system/SYSTEM_COMPATIBILITY_MATRIX.md` with current generated truth.
3. Refresh `docs/system/ontogony-runtime.lock.json` after validation, not before.
4. Reset/shrink `docs/system/post-lock-deltas.json` so it reflects only genuine post-promotion deltas.
5. Run `validate-runtime-lock.ps1 -ReleaseMode`, feature connection matrix validation, and cross-repo conformance.
6. Write `docs/reviews/CONTRACT_DISCIPLINE_OVER9_CLOSEOUT_REPORT.md`.
