# Master Prompt — First Working Environment

We are setting up the first working local Ontogony environment.

Workspace:

```text
C:\dev\
  ontogony-platform\
  allagma-dotnet\
  kanon-dotnet\
  conexus-dotnet\
  ontogony-frontend\
  ontogony-ui\
```

Implement:

```text
ENV-SETUP-001
ENV-SETUP-002
ENV-RUN-001
ENV-FE-001
ENV-PG-001
ENV-UI-001
ENV-REAL-PROVIDER-001 optional
ENV-CLOSEOUT-001
```

Rules:

- fake/local Conexus provider first
- no real external execution
- no raw provider secrets in frontend/UI/reports
- manual eval POST only in Development with `Allagma__Evaluation__ManualWriteEnabled=true`
- not production readiness
