# Repo Targets

## ontogony-frontend

Primary repo for this sprint.

Likely targets:

```text
src/system/pages/OperatorSettingsPage.tsx
src/app/settings/OperatorSettingsProvider.tsx
src/app/settings/operatorSettingsTypes.ts
src/app/settings/*
src/system/diagnostics/*
src/kanon/components/KanonOperatorContextCard.tsx
src/kanon/components/KanonActorAuthorizationCard.tsx
src/kanon/pages/*
src/kanon/api/*
src/kanon/assistance/*
src/evidence-spine/*
src/allagma/*
src/shared/security/*
src/shared/components/*
```

Tests:

```text
src/app/settings/*.test.ts
src/system/pages/OperatorSettingsPage.test.tsx
src/kanon/components/*.test.tsx
src/kanon/pages/*Assistance*.test.tsx
src/shared/security/*.test.ts
e2e/settings-security-ux.spec.ts
```

## ontogony-ui

Only if reusable UI primitives belong here:

```text
src/security/
src/status/
src/forms/
src/feedback/
src/operator/
```

Potential components:

```text
CredentialStatusRow
DiagnosticsPrivacyNotice
ActorIdentityCard
RedactionPreviewPanel
```

## kanon-dotnet

Avoid backend changes unless capability introspection or authorization errors need shape improvement.

## conexus-dotnet

Avoid backend changes unless provider/credential posture must expose non-secret metadata.

## allagma-dotnet

Avoid backend changes unless diagnostics/export metadata needs support.

## ontogony-platform

Avoid changes unless a shared redaction/privacy/diagnostics contract already belongs here.
