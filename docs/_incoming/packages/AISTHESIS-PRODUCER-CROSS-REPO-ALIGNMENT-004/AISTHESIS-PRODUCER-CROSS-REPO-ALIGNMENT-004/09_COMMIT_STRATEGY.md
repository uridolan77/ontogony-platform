# Commit strategy

Recommended sequence:

1. `platform: add Aisthesis producer alignment 004 package`
2. `allagma: align Aisthesis producer edges`
3. `kanon: align Aisthesis semantic evidence edges`
4. `conexus: align Aisthesis route/model evidence edges`
5. `metabole: align Aisthesis pipeline/mapping/artifact evidence`
6. `platform: add producer alignment evidence index`
7. `aisthesis: record live proof summary` only after actual live PASS

Avoid a single giant commit across all repos.
