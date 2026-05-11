using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Ontogony.Contracts.Events;

namespace Ontogony.Security;

/// <summary>
/// Accessor for service-to-service identity verification.
/// Validates that the caller is an authorized service, not a human user.
/// Uses header-based service identity (service ID and optional signature).
/// </summary>
public sealed class ServiceIdentityCurrentActorAccessor : ICurrentActorAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ServiceIdentityOptions _options;

    public ServiceIdentityCurrentActorAccessor(
        IHttpContextAccessor httpContextAccessor,
        ServiceIdentityOptions? options = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options ?? new ServiceIdentityOptions();
    }

    public CurrentActor? Current
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is null) return null;

            // Extract service ID from header
            var serviceId = context.Request.Headers[_options.ServiceIdHeaderName].ToString();
            if (string.IsNullOrWhiteSpace(serviceId))
                return null;

            // Verify signature if verification is enabled
            if (_options.RequireSignatureVerification)
            {
                if (!VerifyServiceSignature(context, serviceId))
                    return null;
            }

            return new CurrentActor(
                serviceId,
                OntogonyActorTypes.Service,
                new[] { OntogonyRoleNames.Service },
                context.Request.Headers[OntogonyEventHeaders.TenantId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.WorkspaceId].ToString(),
                context.Request.Headers[OntogonyEventHeaders.ProjectId].ToString());
        }
    }

    /// <summary>
    /// Verify that the service signature matches the expected value.
    /// Can be overridden to implement HMAC or other verification strategies.
    /// </summary>
    private bool VerifyServiceSignature(HttpContext context, string serviceId)
    {
        var signature = context.Request.Headers[_options.SignatureHeaderName].ToString();
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        // Simple implementation: check against configured secret
        // In production, use HMAC or other cryptographic verification
        var expectedSignature = _options.GetExpectedSignature(serviceId);
        if (string.IsNullOrWhiteSpace(expectedSignature))
        {
            return false;
        }

        var providedBytes = Encoding.UTF8.GetBytes(signature);
        var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);
        return CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }
}

/// <summary>
/// Configuration for ServiceIdentityCurrentActorAccessor.
/// </summary>
public sealed class ServiceIdentityOptions
{
    /// <summary>
    /// Header name containing service ID.
    /// Default: X-Service-Id
    /// </summary>
    public string ServiceIdHeaderName { get; set; } = "X-Service-Id";

    /// <summary>
    /// Header name containing service signature.
    /// Default: X-Service-Signature
    /// </summary>
    public string SignatureHeaderName { get; set; } = "X-Service-Signature";

    /// <summary>
    /// If true, require signature verification for all service requests.
    /// If false, accept any request with valid service ID.
    /// Default: false (no verification)
    /// </summary>
    public bool RequireSignatureVerification { get; set; } = false;

    /// <summary>
    /// Dictionary mapping service IDs to their shared secrets.
    /// Used for signature verification.
    /// </summary>
    public Dictionary<string, string> ServiceSecrets { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Get the expected signature for a service ID.
    /// Can be overridden to implement custom signature strategies.
    /// </summary>
    public string? GetExpectedSignature(string serviceId)
    {
        ServiceSecrets.TryGetValue(serviceId, out var secret);
        return secret;
    }
}
