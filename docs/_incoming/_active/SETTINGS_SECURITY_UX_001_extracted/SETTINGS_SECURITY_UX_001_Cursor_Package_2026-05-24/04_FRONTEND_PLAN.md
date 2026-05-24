# Frontend Plan

## Main frontend deliverables

### 1. Settings page restructuring

Create sections:

1. Connection & service URLs
2. Credentials
3. Actor context & role profile
4. Model purpose / Conexus aliases
5. Execution posture
6. Diagnostics export

Each section should be compact, scannable, and action-oriented.

### 2. Credential row component

Create or update:

```tsx
<CredentialStatusRow
  label="Conexus project API key"
  status="configured"
  source="local_browser"
  scope="this browser profile"
  risk="local-alpha"
  lastTestedAt={...}
  actions={...}
/>
```

No raw secret rendering.

### 3. Warning group

Create:

```tsx
<LocalCredentialStorageNotice />
```

Only one instance per Settings page.

### 4. Actor preset selector

Create:

```tsx
<OperatorActorPresetSelector
  currentActorId={...}
  currentRoles={...}
  selectedPreset={...}
  onApplyPreset={...}
/>
```

Presets:

```text
Kanon read-only
Local admin
System service
Custom
```

### 5. Capability summary

Create:

```tsx
<ActorCapabilitySummary roles={roles} />
```

Display concrete grants:

```text
Kanon domain-pack read: granted
Kanon domain-pack load: denied/granted
Kanon provenance read: granted
Conexus admin read: configured/unknown
Allagma run start: configured/unknown
```

If backend capability introspection is not available, mark as `estimated from local roles`.

### 6. Redaction preview

Create:

```tsx
<RedactionPreviewPanel preview={preview} />
```

Must support:

- kept fields
- removed fields
- reason code
- outbound payload preview
- copy redacted payload
- warning when sensitive-looking fields were detected

### 7. Diagnostics privacy notice

Create:

```tsx
<DiagnosticsPrivacyNotice />
```

Place near export/download/copy controls.

### 8. Actor labels

Create reusable actor display:

```tsx
<ActorIdentityCard kind="currentOperator" actorId="local-operator" roles={["Admin"]} />
<ActorIdentityCard kind="historicalRun" actorId="env-seed-001-agent" roles={...} />
<ActorIdentityCard kind="service" actorId="kanon-domain-pack-loader" />
```

### 9. Settings copy cleanup

Forbidden UI phrases after this sprint:

```text
unknown source
Allagma defaults
Kanon trusts headers
secret-live-key
apiKey as sample allowed field
```

Allowed only in tests/docs explaining removed legacy wording, not live UI.

## Likely file targets

Search and update:

```text
src/system/pages/OperatorSettingsPage.tsx
src/app/settings/OperatorSettingsProvider.tsx
src/app/settings/operatorSettingsTypes.ts
src/kanon/components/KanonOperatorContextCard.tsx
src/kanon/auth/kanonActorAuthorization.ts
src/kanon/pages/KanonOverviewPage.tsx
src/kanon/pages/DomainPacksPage.tsx
src/kanon/pages/OntologyVersionsPage.tsx
src/kanon/pages/ConexusAssistancePage.tsx
src/evidence-spine/*
src/system/pages/AgentInteractionPage.tsx
src/allagma/*
```

If `@ontogony/ui` already has suitable primitives, extend them there instead of duplicating.
