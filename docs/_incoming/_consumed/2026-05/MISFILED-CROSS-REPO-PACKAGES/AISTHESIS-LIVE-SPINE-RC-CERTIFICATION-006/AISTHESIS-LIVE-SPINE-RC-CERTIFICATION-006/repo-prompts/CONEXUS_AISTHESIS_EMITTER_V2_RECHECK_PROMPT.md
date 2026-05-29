# Cursor prompt — Conexus Aisthesis emitter v2 recheck

Apply inside `C:\Dev\conexus-dotnet` only if Aisthesis certification reports missing Conexus edges.

Check that Conexus emits or can emit:

- model call -> route decision;
- model call -> provider attempt;
- model call -> provider fallback when fallback occurs;
- model call -> provider error when error occurs;
- model call -> usage/cost record when available;
- streaming session -> completion summary when streaming is used.

Do not move routing authority out of Conexus.
