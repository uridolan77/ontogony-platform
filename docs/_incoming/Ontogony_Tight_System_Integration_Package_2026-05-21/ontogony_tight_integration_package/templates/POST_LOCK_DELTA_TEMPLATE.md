# Post-lock delta register — <BASELINE>

| Repo | Base lock SHA | Current SHA | Delta class | Requires validation | Decision |
|---|---|---|---|---|---|
| ontogony-platform | <sha> | <sha> | docs_only/test_only/api_additive_safe/runtime_behavior_change/package_surface_change/security_surface_change | yes/no | absorb/defer/block |
| allagma-dotnet | <sha> | <sha> |  |  |  |
| kanon-dotnet | <sha> | <sha> |  |  |  |
| conexus-dotnet | <sha> | <sha> |  |  |  |

## Rules

- `api_breaking`, `security_surface_change`, and `package_surface_change` cannot be absorbed without explicit evidence.
- Runtime behavior changes require system cohesion smoke.
- Operator surface changes require frontend/operator smoke.
