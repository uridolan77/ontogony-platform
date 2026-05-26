# 06 — Repo Targets

## ontogony-platform

Likely target for shared contracts:

```text
HealthV1Response
ReadyV1Response
ReadinessCheckContract
ServiceBuildInfo
CompatibilitySummaryContract
```

Also possible target for:

```text
Health endpoint mapping helpers
Readiness endpoint mapping helpers
Stable error envelope
```

## conexus-dotnet

Implement:

```text
/health → health.v1
/ready → ready.v1 with Conexus-specific checks
clear Conexus not-ready reasons
route-decision store readiness check
provider registry and provider credential posture checks
version/build metadata
```

Do not make missing optional OpenAI credentials fail local fake readiness unless aliases require OpenAI.

## kanon-dotnet

Implement:

```text
/health → health.v1
/ready → ready.v1
active ontology and domain-pack readiness checks
version/build metadata
```

Also normalize active ontology version ID if endpoint currently exposes only `0.1.0`.

## allagma-dotnet

Implement:

```text
/health → health.v1
/ready → ready.v1
downstream Kanon/Conexus checks
model purpose config check
real external blocked as safety posture, not failure
sandbox disabled as posture, not failure unless required
kill switch missing as local-alpha warning
version/build metadata
```

## ontogony-ui

Add/extend neutral primitives:

```text
ConnectivityBadge
ReadinessBadge
ContractHealthBadge
DataSourceBadge
AuthorityBadge
CompatibilityBadge
Status vocabulary docs
```

## ontogony-frontend

Implement:

```text
health.v1 parser
ready.v1 parser
compatibility artifact reader
Home truth model
Settings truth model
Topology truth model
Release readiness downgrade
data-source badges
contract warning details
Conexus readiness details
```
