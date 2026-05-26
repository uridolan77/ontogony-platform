# Prometheus alert rules (PLAT-9-005)

## Starter rules file

[`alerts.prometheus.rules.yml`](./alerts.prometheus.rules.yml)

| Group | Alerts | Focus |
| --- | --- | --- |
| `ontogony-platform-mechanics` | `OntogonyHttpErrorRateHigh`, `OntogonyIntegrationErrorBurst` | Shared `ontogony.*` middleware metrics |
| `allagma-mechanics` | `AllagmaHttpErrorsElevated`, `AllagmaIntegrationErrorsToDownstream` | Allagma API + outbound integration |
| `conexus-mechanics` | `ConexusHttpErrorsElevated`, `ConexusProviderErrorsElevated` | Gateway HTTP + provider errors |
| `kanon-mechanics` | `KanonHttpErrorsElevated`, `KanonHttpTrafficAbsent` | Kanon API health + idle detection |

These are **starter** thresholds for docker-local and burn-in environments. Tune `for:` windows and expr thresholds per environment before production.

## Import into Prometheus

Add to `rule_files` in `prometheus.yml`:

```yaml
rule_files:
  - /etc/prometheus/rules/ontogony-observability-mechanics.yml
```

Copy the YAML into the collector volume (for example alongside `allagma-dotnet/docker/observability/prometheus/` rules).

## Related product alert packs

| Repo | Location |
| --- | --- |
| Allagma MGMT-007 | `allagma-dotnet/docker/observability/prometheus/allagma-mgmt-007-alerts.yml` |
| Conexus runbooks | `conexus-dotnet/docs/runbooks/` |

Platform rules intentionally avoid semantic/policy wording — they signal operational degradation only.

## Validate locally

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-observability-pack-artifacts.ps1
.\scripts\run-observability-mechanics-conformance.ps1
```
