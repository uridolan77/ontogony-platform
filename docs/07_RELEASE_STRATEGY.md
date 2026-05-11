# 07 — Release Strategy

## Package maturity stages

### 0.1 — Starter

- packages compile
- tests exist
- no production guarantee
- project references acceptable

### 0.2 — Adopted by one service

- Agentor or Athanor consumes observability/errors
- migration docs exist
- CI passes in consuming repo

### 0.3 — Adopted by two services

- Agentor and Athanor both consume core packages
- trace correlation verified end-to-end

### 0.5 — Protocol recorder ready

- AG-UI/MCP recorder can use envelope + hashing + messaging
- outbox/event schema stable enough for demos

### 1.0 — Internal stable

- semver discipline
- migration docs
- private NuGet feed
- Athanor/Agentor/Conexus integration story documented

## Release checklist

- [ ] `dotnet restore`
- [ ] `dotnet build --no-restore`
- [ ] `dotnet test --no-build`
- [ ] package versions updated
- [ ] changelog updated
- [ ] migration notes if breaking
- [ ] consuming repos listed
- [ ] sample Program.cs fragments still valid
