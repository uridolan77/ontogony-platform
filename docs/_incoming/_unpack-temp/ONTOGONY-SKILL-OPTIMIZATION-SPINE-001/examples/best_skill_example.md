# Skill: Spreadsheet Analysis

## Applicability

Use this skill when answering questions about spreadsheet workbooks, CSV-derived tables, or tabular files where values may depend on hidden sheets, filters, formulas, named ranges, or multiple tabs.

## Procedure

1. Identify all available worksheets/tables before answering.
2. Check whether relevant data may appear in hidden sheets, named ranges, formulas, filters, or pivot summaries.
3. Trace the requested value to its source cells or rows.
4. Perform calculations explicitly and verify units/currency/date assumptions.
5. Answer with the final value and a short evidence summary.

## Tool-use Rules

- Use spreadsheet inspection tools before arithmetic if the workbook structure is unknown.
- Do not infer missing sheets or columns without evidence.
- Prefer exact cell/range evidence when available.

## Output Contract

Return:

```text
Answer: <value>
Evidence: <short source summary>
Assumptions: <only if needed>
```

## Failure Modes

- Skipping hidden sheets.
- Answering from the first visible table only.
- Ignoring named ranges or formulas.
- Reporting a value without unit/currency context.

## Slow/Meta Update

Preserve workbook inventory as a mandatory first step for multi-sheet tasks.
