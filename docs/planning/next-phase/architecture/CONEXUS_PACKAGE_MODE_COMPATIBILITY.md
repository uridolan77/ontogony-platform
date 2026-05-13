# Conexus package-mode compatibility

The current platform package smoke proves a Conexus-shaped skeleton builds against packed Ontogony packages. It does not prove the real `conexus-dotnet` repository can run without sibling project references.

**Contract and Conexus CI:** see `docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md` and the `conexus-ontogony-package-mode` job in `conexus-dotnet` (restore/build/test with `UseOntogonyPackages=true` and a local feed from packed Ontogony).

## Preferred home

The strongest implementation belongs in `conexus-dotnet` CI:

1. consume Ontogony packages from GitHub Packages or a locally packed artifact;
2. disable sibling project references;
3. restore/build/test Conexus;
4. run provider and persistence smoke paths that depend on Ontogony packages.

## Platform-side role

Ontogony should keep the small package smoke because it prevents obvious package breakage before publishing. It should not import Conexus product code or gateway semantics.
