# Recommended Implementation Order

## 1. `ontogony-platform`

Add cross-repo protocol docs and package tracking. This gives other repos a shared target.

## 2. `kanon-dotnet`

Define governance semantics first: skill objects, edit validation, decisions, evidence graph roots. This prevents Allagma/Conexus from inventing ungoverned skill behavior.

## 3. `conexus-dotnet`

Add optimizer/target role distinction and skill injection metadata. Fake provider first.

## 4. `allagma-dotnet`

Add orchestration run lifecycle and deterministic fixture. Link to Kanon and Conexus evidence.

## 5. `ontogony-ui`

Add reusable components.

## 6. `ontogony-frontend`

Add Skill Lab page and integrate links from existing system pages.

## 7. Cross-repo verification

Run tests, regenerate route inventories/OpenAPI snapshots, verify fixture ids, update docs.
