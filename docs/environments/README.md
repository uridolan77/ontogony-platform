# Environment documentation

Platform-hosted notes for **local operator environments**. Product-specific environment overlays remain in each backend repo.

---

## Docker local working system (active)

| Document | Purpose |
| --- | --- |
| [`docker-local-working-system/`](./docker-local-working-system/) | Manifest, architecture, exact settings, compose plan |
| [`../docker/local-working-system/README.md`](../docker/local-working-system/README.md) | Compose tree, scripts, operator commands |

**Boundary:** Development credentials only. **Not production readiness.**

Ports on host: Kanon `5081`, Conexus `5082`, Allagma `5083`. See [`DEVELOPMENT.md`](../DEVELOPMENT.md).
