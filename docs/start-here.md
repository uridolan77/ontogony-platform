# Ontogony Platform — Start Here

Welcome to **Ontogony**, the shared infrastructure platform for Athanor, Agentor, Conexus, and future protocol recorder services.

This is not a product library. This is the **common nervous system** for cross-service mechanics: tracing, errors, contracts, resilience, security, and release automation.

---

## What is Ontogony?

Ontogony is a **.NET 9 / C# 13 infrastructure platform** that provides:

| Pillar | Purpose | Use When |
|--------|---------|----------|
| **Contracts** | Event envelopes, protocol names, canonical headers | Any service publishing events or managing cross-service state |
| **Observability** | Trace correlation context, middleware, diagnostic sources | Building request-scoped async-local state and distributed tracing |
| **Errors** | HTTP error mapping, exception shapes, error codes | Translating domain/framework exceptions into safe JSON responses |
| **HTTP Resilience** | Retry, circuit-breaker, timeout coordination | Calling external services reliably |
| **Security** | HMAC signing, actor context, claims validation | Authenticating requests between services |
| **Hashing & Idempotency** | Payload hashing, fingerprints, idempotency keys | Building deterministic, repeatable event processing |
| **Messaging & Outbox** | Protocol-neutral event bus, outbox pattern | Guaranteeing event delivery across service boundaries |
| **ProtocolIngress** | HTTP/gRPC request unpacking, enum validation | Converting external protocol messages into canonical envelopes |
| **Persistence** | Database patterns, migrations, audit trails | Durable event storage and state management |
| **Testing** | Test doubles, conformance kits, harnesses | Verifying consumer services correctly adopt platform mechanics |

---

## Where to Start

### 1. You are **adopting Ontogony in a consumer service** (Athanor, Agentor, Conexus)

**Start here:** [Adoption Path](../adoption/)

- Install base package: `dotnet add package Ontogony.Observability`
- Follow step-by-step integration guide
- Run conformance tests to prove adoption

### 2. You are **contributing to Ontogony platform itself**

**Start here:** [Architecture & Decisions](../architecture/)

- Review the design principles: "Share mechanics, not meaning"
- Understand package boundaries
- Check existing design records (ADRs)

### 3. You are **running services that depend on Ontogony**

**Start here:** [Operations & Deployment](../operations/)

- Learn trace propagation and correlation
- Understand release artifacts and package feeds
- Review security and secrets handling

### 4. You are **curious about a specific package**

**Start here:** [Package Reference](../packages/)

Browse by namespace:
- `Ontogony.Contracts` — DTOs and protocol constants
- `Ontogony.Observability` — Tracing and correlation
- `Ontogony.Errors` — HTTP error handling
- `Ontogony.Http` — HTTP clients and resilience
- `Ontogony.Security` — Authentication and signing
- ... (14 packages total)

---

## Core Principles

```text
Share mechanics, not meaning.
```

Ontogony contains **infrastructure primitives only**. 

✅ **Allowed:**
- Trace header propagation
- Event envelope conventions
- HTTP retry and circuit-breaker mechanics
- Exception-to-HTTP error mapping
- Deterministic hashing and idempotency

❌ **Forbidden:**
- Domain semantics (canonization, contradictions, epistemic logic)
- Product approval rules (iGaming refunds, LLM pricing)
- Service orchestration (plan/tool/skill invocation)
- Business workflows

If a change requires understanding what a product feature means, it belongs in the **product repo**, not here.

---

## Quick Links

| Purpose | Link |
|---------|------|
| Installation & setup | [Adoption](../adoption/) |
| API reference | [Packages](../packages/) |
| How to extend | [Architecture](../architecture/) |
| Release process | [Operations](../operations/) |
| Security & compliance | [Security](../security/) |
| Code examples | [Examples](../examples/) |
| Changelog | [CHANGELOG.md](../../CHANGELOG.md) |

---

## Key Files

| File | Purpose |
|------|---------|
| `AGENTS.md` | Contributor rules and code review checklist |
| `Ontogony.Platform.sln` | Main solution file |
| `Directory.Build.props` | Shared MSBuild properties |
| `scripts/pack-all.ps1` | Build and package all projects |
| `scripts/generate-package-manifest.ps1` | Create auditable release manifest |
| `.github/workflows/ci.yml` | Continuous integration pipeline |
| `.github/workflows/release-packages.yml` | Release automation |

---

## Running the Platform Locally

### Prerequisites
- .NET 9.0 SDK
- PowerShell 7+ (Core)
- PostgreSQL 16 (for integration tests)

### Build

```bash
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln
```

### Test

```bash
dotnet test Ontogony.Platform.sln
```

Expected: **389 tests passing**, 0 failures (as of latest main)

### Pack

```bash
$env:PACKAGE_VERSION = "0.2.0"
./scripts/pack-all.ps1
```

This creates 15 NuGet packages in `artifacts/packages/`.

### Release

```bash
git tag v0.3.0
git push origin v0.3.0
```

GitHub Actions workflow will automatically:
1. Build, test, validate changelog
2. Generate packages with symbol files
3. Create auditable manifest (SHA256 hashes)
4. Create GitHub Release with artifacts

---

## Current State

| Component | Status | Note |
|-----------|--------|------|
| Core contracts | ✅ Stable | Widely adopted |
| Observability | ✅ Stable | In production |
| HTTP resilience | ✅ Good | Needs timeout context polish |
| Testing/conformance | ✅ Strong | Ready for consumer adoption |
| Release automation | ✅ Working | Dry-run release only (for now) |
| Documentation | 🆕 Starting | This page; more coming |

---

## Support & Issues

- **Bugs & feature requests:** Create issue in this repo with label `ontogony-platform`
- **Design discussions:** See `docs/adr/` for architecture decision records
- **Integration help:** Check `docs/adoption/` for step-by-step guides
- **Release questions:** See `.github/workflows/release-packages.yml` and `docs/operations/`

---

## Next Steps

1. **Read** [Why Ontogony Exists](../planning/why-ontogony-exists.md)
2. **Install** per your role: [Adoption](../adoption/) or [Architecture](../architecture/)
3. **Explore** specific packages: [Package Index](../packages/)
4. **Verify** your service adopts correctly: [Conformance Tests](../adoption/conformance-testing.md)

---

**Version:** 0.2.0  
**Last Updated:** May 2026  
**License:** MIT
