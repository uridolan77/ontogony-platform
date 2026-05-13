# Operations & Deployment — Running Services on Ontogony

This guide covers running, monitoring, and releasing services that depend on Ontogony.

---

## Overview

Once your service adopts Ontogony, you inherit:

✅ **Distributed tracing** — Trace IDs propagate across service boundaries  
✅ **Unified error handling** — All services return canonical error shapes  
✅ **Resilient networking** — Automatic retry and circuit-breaker  
✅ **Audit trails** — All events signed and logged  
✅ **Security context** — Actor propagation and HMAC signing  

---

## Deployment

### Docker

Add `Dockerfile` to your service:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=builder /app/out .
ENTRYPOINT ["dotnet", "MyService.dll"]
```

### Environment Variables

Required:

```bash
# Ontogony configuration
ONTOGONY_POSTGRES_CONNECTION=Host=...
ONTOGONY_SHARED_SECRET=...

# Service identity
SERVICE_IDENTITY=ontogony://my-service

# Clock (usually not set; defaults to UTC now)
# Only override for testing!
ONTOGONY_USE_FAKE_CLOCK=false
```

Optional:

```bash
# Tracing
OTEL_EXPORTER_OTLP_ENDPOINT=http://collector:4317
OTEL_SERVICE_NAME=my-service

# Logging
LOG_LEVEL=Information
```

### Kubernetes

Example deployment:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: my-service
spec:
  replicas: 2
  selector:
    matchLabels:
      app: my-service
  template:
    metadata:
      labels:
        app: my-service
    spec:
      containers:
      - name: my-service
        image: my-registry/my-service:0.2.0
        ports:
        - containerPort: 5000
        env:
        - name: ONTOGONY_POSTGRES_CONNECTION
          valueFrom:
            secretKeyRef:
              name: ontogony-secrets
              key: postgres-connection
        - name: ONTOGONY_SHARED_SECRET
          valueFrom:
            secretKeyRef:
              name: ontogony-secrets
              key: shared-secret
        livenessProbe:
          httpGet:
            path: /health
            port: 5000
          initialDelaySeconds: 10
          periodSeconds: 30
        readinessProbe:
          httpGet:
            path: /ready
            port: 5000
          initialDelaySeconds: 5
          periodSeconds: 10
        resources:
          requests:
            memory: 256Mi
            cpu: 250m
          limits:
            memory: 512Mi
            cpu: 500m
```

---

## Monitoring & Observability

### Distributed Tracing

All requests automatically propagate the `X-Ontogony-Trace-Id` header.

**View traces:**

```bash
# Option 1: OpenTelemetry collector
# Ensure OTEL_EXPORTER_OTLP_ENDPOINT is set and collector is running

# Option 2: Manual correlation
# Log all entries with trace ID to correlate requests across services
GET /api/orders/123
  ↓ X-Ontogony-Trace-Id: trace-abc123
Payment Service receives trace-abc123
  ↓ propagates to Notification Service
Notification Service logs with trace-abc123
```

**Check trace propagation:**

```bash
# Send request with trace ID
curl -H "X-Ontogony-Trace-Id: test-trace-001" \
  https://my-service/api/orders

# Verify response echoes it
# Response header: X-Ontogony-Trace-Id: test-trace-001
```

### Health Checks

Each service must expose:

```csharp
app.MapGet("/health", () => Results.Ok("OK"));

// Example: verify PostgreSQL connectivity from your host (domain DbContext or NpgsqlDataSource)
app.MapGet("/ready", async (MyDbContext db) =>
{
    try
    {
        await db.Database.ExecuteSqlRawAsync("SELECT 1");
        return Results.Ok();
    }
    catch
    {
        return Results.StatusCode(503);
    }
});
```

Use for Kubernetes probes and load balancer health checks.

### Metrics

Ontogony automatically records:

```text
ontogony_http_request_duration_seconds (histogram)
  - service_name
  - method
  - path
  - status

ontogony_http_request_total (counter)
  - service_name
  - method
  - status

ontogony_circuit_breaker_state (gauge)
  - service_name
  - endpoint
  - state (open/half-open/closed)

ontogony_outbox_pending (gauge)
  - service_name
  - queue_name
```

Export via OTLP to Prometheus, Datadog, or equivalent.

### Logging

All logs include correlation context:

```json
{
  "timestamp": "2026-05-12T12:34:56Z",
  "level": "Information",
  "traceId": "trace-abc123",
  "tenantId": "tenant-xyz",
  "message": "Order created",
  "userId": "user-123",
  "orderId": "order-456"
}
```

Ensure your log aggregator (ELK, Datadog, etc.) can index by `traceId` for correlation.

---

## Database Management

### PostgreSQL Setup

Initialize database:

```bash
psql -U postgres -c "CREATE DATABASE ontogony;"
psql -U postgres -d ontogony -c "CREATE EXTENSION IF NOT EXISTS uuid-ossp;"
```

### Schema and migrations

**Ontogony.Persistence** packages provide **outbox / processed-message / dead-letter** storage contracts and PostgreSQL-backed implementations. They do **not** replace your application's domain model, ORM, or migration workflow.

- **Your domain tables:** use your own migration tool (EF Core, FluentMigrator, hand-written SQL, etc.).
- **Ontogony Postgres tables:** created for the outbox provider (`AddOntogonyPostgresOutbox`) when you opt in via `PostgresOutboxOptions.EnsureSchemaOnStartup`, or provision them separately to match the expected table names.

**Register the PostgreSQL outbox:**

```csharp
services.AddOntogonyPostgresOutbox(opts =>
{
    opts.ConnectionString = configuration.GetConnectionString("Postgres")!;
    opts.EnsureSchemaOnStartup = true; // dev/sandbox convenience; review for production
});
```

**If you also use EF Core for domain data**, run EF migrations the way you already do for **your** `DbContext` — that is unrelated to Ontogony outbox schema initialization.

---

## Secrets Management

### Local Development

Use user secrets:

```bash
dotnet user-secrets set "Ontogony:SharedSecret" "dev-secret-abc123"
dotnet user-secrets set "Ontogony:PostgresConnection" "Host=localhost;..."
```

### Production

Use **environment variables** or **secrets vault**:

```bash
# Option 1: Environment (for container orchestration)
# Set values from your secret store or orchestrator — never commit real material.
export ONTOGONY_SHARED_SECRET
export ONTOGONY_POSTGRES_CONNECTION

# Option 2: Kubernetes secrets
kubectl create secret generic ontogony-secrets \
  --from-literal=shared-secret="$ONTOGONY_SHARED_SECRET" \
  --from-literal=postgres-connection="$ONTOGONY_POSTGRES_CONNECTION"

# Option 3: Azure Key Vault
dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets
dotnet add package Azure.Identity

# In Program.cs
var keyVaultUrl = new Uri(builder.Configuration["KeyVault:VaultUri"]);
builder.Configuration.AddAzureKeyVault(
    keyVaultUrl,
    new DefaultAzureCredential()
);
```

**Never:**
- ❌ Commit secrets to git
- ❌ Hardcode secrets in code
- ❌ Pass secrets in URLs
- ❌ Log secrets

---

## Release Process

### Versioning

Ontogony uses **semantic versioning**: `MAJOR.MINOR.PATCH[-PRERELEASE]`

Examples:
- `0.2.0` — stable release
- `0.3.0-alpha.1` — pre-release
- `0.3.0-local` — local development build

### Creating a Release

1. **Update version in `global.json`:**

```json
{
  "sdk": {
    "version": "9.0.0",
    "rollForward": "latestMinor"
  },
  "version": "0.3.0"
}
```

2. **Update `CHANGELOG.md`:**

```markdown
## 0.3.0

- Feature A
- Bug fix B
```

3. **Commit and tag:**

```bash
git add global.json CHANGELOG.md
git commit -m "Release 0.3.0"
git tag v0.3.0-alpha.1
git push origin main --tags
```

4. **GitHub Actions releases automatically:**
   - Builds and tests
   - Validates changelog
   - Generates packages and manifest
   - Creates GitHub Release with artifacts

### Package Manifest

`scripts/generate-package-manifest.ps1` writes `PACKAGE_MANIFEST.json` (default: repo root). That path is **gitignored** so local pack runs do not dirty the tree; CI and Releases attach the manifest from the workflow workspace.

After release, `PACKAGE_MANIFEST.json` contains:

```json
{
  "version": "0.3.0-alpha.1",
  "generated": "2026-05-12T12:34:56Z",
  "commit": "abc123def456",
  "packageCount": 23,
  "packages": [
    {
      "id": "Ontogony.Contracts",
      "version": "0.3.0-alpha.1",
      "filename": "Ontogony.Contracts.0.3.0-alpha.1.nupkg",
      "sizeBytes": 29982,
      "sha256": "378E89860F3AF948A31D219D02258B890A45E2723911FE6D558C4216329420AD"
    }
    // ... 22 more shipping packages (non-symbol .nupkg only)
  ]
}
```

This proves:
- Which packages were built
- Exact versions
- File sizes
- Checksums for integrity verification

### Installing Released Packages

From GitHub Packages feed:

```bash
# Add feed (one-time)
dotnet nuget add source \
  --name github \
  --username YOUR_GITHUB_USER \
  --password YOUR_GITHUB_TOKEN \
  --store-password-in-clear-text \
  "https://nuget.pkg.github.com/YOUR_ORG/index.json"

# Install package
dotnet add package Ontogony.Observability --version 0.3.0
```

---

## Troubleshooting

### "Trace ID not propagating to logs"

**Symptom:** Logs don't include `traceId` field.

**Fix:** Ensure logging is configured to include correlation context.

```csharp
services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddConsole(opts =>
    {
        opts.IncludeScopes = true;  // Include correlation scope
    });
});
```

### "Circuit breaker opens too frequently"

**Symptom:** External service calls stop working even though service is healthy.

**Fix:** Tune circuit-breaker thresholds.

```csharp
services.Configure<TransportResilienceOptions>(opts =>
{
  opts.CircuitFailureThreshold = 10;  // Increase threshold
  opts.CircuitOpenDurationSeconds = 60;  // Allow recovery time
});
```

### "Database connection pool exhausted"

**Symptom:** `NpgsqlException: The operation has timed out` even under light load.

**Fix:** Check connection string pool settings.

```csharp
// Your service: configure Npgsql / EF Core connection pooling (Ontogony does not own your DbContext)
services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("Postgres"),
        postgresOpts => postgresOpts.CommandTimeout(30)));

// Ontogony outbox: separate registration (see Ontogony.Persistence.Postgres)
services.AddOntogonyPostgresOutbox(opts =>
{
    opts.ConnectionString = configuration.GetConnectionString("Postgres")!;
});

// Verify pool config in connection string:
// "Host=...;Min Pool Size=5;Max Pool Size=100;..."
```

### "Shared secret validation failing"

**Symptom:** Service rejects incoming signed requests from other services.

**Fix:** Verify shared secret matches.

```bash
# Check deployed secret
kubectl get secret ontogony-secrets -o jsonpath='{.data.shared-secret}' | base64 -d

# Verify sending service is using same secret
# It should be in both services' configuration
```

---

## Capacity Planning

### CPU & Memory

Typical per-instance:

| Load | CPU | Memory | Notes |
|------|-----|--------|-------|
| Dev/test | 100m | 256Mi | Single instance |
| Light (< 10 req/s) | 250m | 512Mi | 2 instances |
| Medium (10-100 req/s) | 500m | 1Gi | 3-4 instances |
| Heavy (100+ req/s) | 1000m+ | 2Gi+ | Horizontal scaling + cache |

### Database

**Storage:**

```text
Event log: ~1-5KB per event
Outbox: ~1-10KB per message (temporary)
Audit trail: ~5-50KB per transaction
```

Estimate: 100K events/day = ~500MB/month

**Connections:**

```text
Min pool size: num_services * 2
Max pool size: num_services * 10
```

Example: 5 services → min=10, max=50 connections

---

## Security Checklist

- [ ] **Secrets**
  - [ ] Never commit secrets to git
  - [ ] Use environment variables or vault
  - [ ] Rotate shared secrets quarterly

- [ ] **TLS/HTTPS**
  - [ ] All inter-service communication encrypted
  - [ ] Valid certificates (not self-signed in prod)
  - [ ] Certificate expiry monitored

- [ ] **Network**
  - [ ] Services only accessible from authorized networks
  - [ ] Firewalls restrict outbound to known hosts
  - [ ] No hardcoded IPs; use DNS

- [ ] **Logging**
  - [ ] Never log secrets, passwords, or tokens
  - [ ] Sanitize PII before logging
  - [ ] Logs retained but access-controlled

- [ ] **Deployments**
  - [ ] Images scanned for vulnerabilities
  - [ ] Security patches applied monthly
  - [ ] Code review on all PRs
  - [ ] Audit trail of all changes

---

## Performance Tuning

### HTTP Resilience

Balance **reliability** vs **latency**:

```csharp
// Example: tune resilience for named integration clients (see Ontogony.Http)
services.Configure<TransportResilienceOptions>(opts =>
{
    opts.MaxRetries = 5;
    opts.BaseDelayMilliseconds = 200;
    opts.MaxDelayMilliseconds = 30_000;
});

// Aggressive: fast fail, fewer retries
services.Configure<TransportResilienceOptions>(opts =>
{
    opts.MaxRetries = 2;
    opts.BaseDelayMilliseconds = 100;
    opts.MaxDelayMilliseconds = 5_000;
});
```

### Database

Use **connection pooling** and **prepared statements**:

```csharp
// Pooling configured in connection string
var connString = "Host=...;Min Pool Size=5;Max Pool Size=50;";

// Prepared statements (via EF Core)
var orders = await db.Orders
    .FromSqlInterpolated($"SELECT * FROM orders WHERE customer_id = {customerId}")
    .ToListAsync();
```

---

## Next Steps

- **Run docs validators locally:** `./scripts/validate-docs-links.ps1` and `./scripts/validate-docs-api-names.ps1` (same checks as CI)
- **Review health checks** in your service
- **Configure monitoring** and trace export
- **Plan database backups** and recovery
- **Document runbooks** for incidents
- **Schedule security reviews** and penetration testing

---

**Last Updated:** May 2026  
**Version:** 0.3.0-alpha.1
