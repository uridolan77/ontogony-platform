# Ontogony.Secrets.AzureKeyVault

Mechanical `ISecretValueResolver` for Azure Key Vault.

## Schemes

- `vault` (preferred; matches Conexus admin tests)
- `akv` (alias)

## Locator format

- `my-secret-name`
- `providers/openai/api-key@optional-version-id`

## Configuration

```json
{
  "Ontogony": {
    "Secrets": {
      "AzureKeyVault": {
        "VaultUri": "https://my-vault.vault.azure.net/"
      }
    }
  }
}
```

Or environment variable `AZURE_KEY_VAULT_URI`.

Authentication uses `DefaultAzureCredential` (managed identity, Azure CLI, environment credentials in dev).

## Registration

```csharp
services.AddOntogonyEnvironmentSecretValueResolver();
services.AddOntogonyAzureKeyVaultSecretValueResolver(configuration);
services.AddOntogonyCompositeSecretValueResolver();
```

Do not log resolved secret values.
