# Prompt 06 — Final Review

```text
Review the completed EVIDENCE-SPINE-REAL-001 implementation against acceptance criteria.

Check:
1. Did we fix route-decision resolution or make its failure typed and truthful?
2. Did we stop treating direct Conexus roots as missing Kanon decisions?
3. Did governed Allagma roots still require Kanon decision evidence?
4. Did placeholder graph nodes merge with authoritative nodes?
5. Are modelCallId, requestId, executionRunId, and routeDecisionId separately modeled and displayed?
6. Are all source attempts structured with endpoint, system, identifier, status, latency, reason code, and message?
7. Are Allagma API routes normalized to /allagma/v0/...?
8. Is partial graph completeness explained by relationship and reason code?
9. Does export include missing/applicability/source-attempt metadata?
10. Are fixture/imported data sources clearly labeled and excluded from live completeness claims?
11. Do tests cover direct Conexus, governed Allagma, baseline comparison, Kanon decision, route decision, and trace/correlation cases?
12. Are docs/operator messages updated without over-broad console polish beyond this task?

Produce a final report:
- files changed;
- behavior before/after;
- tests run;
- remaining gaps;
- recommended next work item.
```
