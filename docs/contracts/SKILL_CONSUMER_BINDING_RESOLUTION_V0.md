# Skill Consumer Binding Resolution Contract V0

## Purpose

`SkillConsumerBindingResolution` is the neutral cross-repo shape for a sandbox consumer asking Allagma which governed skill version it may use. Consumers **resolve**; they do not evaluate release policy.

**Prerequisite:** [SKILL_RELEASE_GOVERNANCE.md](../protocols/SKILL_RELEASE_GOVERNANCE.md) (`ONTOGONY-SKILL-RELEASE-GOVERNANCE-001`).

**HTTP (001B):** `POST /allagma/v0/skill-releases/bindings/resolve` — implementation in `allagma-dotnet`; this doc is the contract only.

## Request

```json
{
  "consumerId": "anti-sycophancy-chat-demo",
  "targetEnvironment": "sandbox",
  "skillArtifactId": "anti-sycophancy-response-style"
}
```

| Field | Required | Notes |
| --- | --- | --- |
| `consumerId` | yes | Stable consumer identity (e.g. demo app id) |
| `targetEnvironment` | yes | `sandbox`, `local-demo`, or `fixture-only` only in v0 |
| `skillArtifactId` | yes | Skill artifact to resolve |

Forbidden `targetEnvironment` values (reject with error, not unresolved):

```text
production
live
default-runtime
```

## Response — resolved

```json
{
  "resolved": true,
  "bindingId": "skillbind_release_sandbox_001",
  "skillArtifactId": "anti-sycophancy-response-style",
  "skillVersionId": "skillver_anti_syc_v2",
  "targetEnvironment": "sandbox",
  "sourcePromotionRequestId": "skillprom_demo_sandbox_001",
  "sourceOptimizationRunId": "skillopt_run_demo_001",
  "limitations": {
    "productionDeploymentAllowed": false,
    "sandboxOnly": true,
    "liveDeploymentAllowed": false
  },
  "evidenceRefs": []
}
```

## Response — unresolved (not an error)

Return unresolved (HTTP 200 with `resolved: false`) when no usable binding exists:

```json
{
  "resolved": false,
  "reason": "no_active_sandbox_binding",
  "fallbackSkillVersionId": "base",
  "limitations": {
    "productionDeploymentAllowed": false,
    "sandboxOnly": true,
    "liveDeploymentAllowed": false
  }
}
```

| `reason` (v0) | Meaning |
| --- | --- |
| `no_active_sandbox_binding` | No matching active binding |
| `binding_paused` | Binding exists but `bindingStatus` is `paused` |
| `binding_rolled_back` | Binding was rolled back |
| `binding_not_found` | No binding for consumer + artifact |
| `promotion_not_approved` | Promotion no longer `approved_for_sandbox` |

Consumers use `fallbackSkillVersionId` (e.g. `base`) for default behavior; they must not fabricate a governed version id.

## Resolver rules (001B semantics; fixed here)

Resolve only if:

```text
targetEnvironment is sandbox | local-demo | fixture-only
binding status is active
binding targetConsumerId matches consumerId
binding skillArtifactId matches skillArtifactId
promotion remains approved_for_sandbox
binding is not paused, rolled_back, or deprecated
```

## Limitation flags (v0)

| Flag | v0 value |
| --- | --- |
| `productionDeploymentAllowed` | `false` |
| `sandboxOnly` | `true` |
| `liveDeploymentAllowed` | `false` |

## Related

- [SKILL_CONSUMER_EXECUTION_CONTEXT_V0.md](./SKILL_CONSUMER_EXECUTION_CONTEXT_V0.md)
- [SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md](./SKILL_RELEASE_DEPLOYMENT_BINDING_V0.md)
- [SANDBOX_CONSUMER_ACTIVATION.md](../protocols/SANDBOX_CONSUMER_ACTIVATION.md)
