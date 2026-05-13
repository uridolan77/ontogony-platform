# Security Policy

## Supported Versions

Ontogony.Platform is currently in **pre-release alpha** status (`v0.3.0-alpha.1` and later).

Security support during the alpha phase is **best-effort**. Formal security support and SLAs will be defined once the project reaches a stable release.

## Reporting Security Vulnerabilities

If you discover a security vulnerability, please **do not** disclose it publicly in GitHub Issues.

### Preferred reporting method

If **GitHub private vulnerability reporting** is enabled for this repository:
- Use the **Security** tab → **Report a vulnerability** to file a private security advisory.
- This creates a private discussion between you and the maintainers.

### Alternative reporting method

If private vulnerability reporting is not yet enabled:
- Contact the maintainer through the repository owner's listed contact information.
- **Avoid** opening public issues that describe the vulnerability in detail.

## Scope

Security fixes apply to:

- **Ontogony NuGet packages** published on the public NuGet feed.
- **CI/release workflows** in `.github/workflows/`.
- **Package publishing pipeline** and artifact integrity.

### Out of Scope

The following are **not** part of Ontogony.Platform's security scope:

- Conexus product-specific routing, provider selection, or pricing policy.
- User application deployment misconfiguration or misuse of the platform APIs.
- Security of downstream consumer applications built with Ontogony packages.

## Response Expectations

When we receive a vulnerability report:

1. **Acknowledge receipt** as soon as possible.
2. **Triage** the reported issue for severity and impact.
3. **Fix** the vulnerability in a patch or pre-release version as appropriate.
4. **Notify** the reporter when a fix is available and provide guidance on upgrading.

## Guidelines for Reports

When reporting a vulnerability:

- **Do not** include live secrets, credentials, or sensitive production data.
- Provide clear steps to reproduce the issue if possible.
- Indicate the affected version(s) of Ontogony packages.
- Describe the potential impact.

## Security Best Practices for Users

When using Ontogony.Platform packages:

- **Keep packages updated** to the latest stable version to receive security fixes.
- **Review release notes** and CHANGELOG entries before upgrading.
- **Use your own secret management** (e.g., Azure Key Vault, Hashicorp Vault) for production credentials.
- **Validate configurations** before deploying to production.

## Questions or Feedback

For general security questions or suggestions, please open a GitHub Discussion or issue on this repository.
