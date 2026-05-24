# Cursor prompt — final review

Before final response, review the implementation against this checklist:

1. Does Agent Interaction default to live when Allagma is reachable?
2. Can a real Allagma run ID be resolved?
3. Are fixture sessions strongly marked as demo/non-live?
4. Do fixture/imported sessions avoid readiness/system-connected claims?
5. Are real Allagma events shown chronologically?
6. Are messages rendered from real data or explicitly marked not-recorded/redacted?
7. Are tool intents and human gates first-class timeline rows?
8. Are Kanon decisions shown distinctly?
9. Are Conexus model-call/provider details shown distinctly?
10. Are unresolved enrichments explained with reason codes?
11. Is Allagma run list free of raw fake responses and unlabeled unknown?
12. Does Start Run have preview and open actions?
13. Are tests added and passing?
14. Are no real external tools enabled?
15. Is exported interaction bundle non-duplicative and source-labelled?

Final response should include:

- summary of behavior change
- repos/files changed
- routes used/added
- tests run
- one validation run ID if available
- known limitations
