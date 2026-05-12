namespace Ontogony.ProtocolIngress;

/// <summary>
/// Adapter for normalizing protocol-specific events into validated OntogonyEnvelope records.
/// Each protocol (CloudEvents, MCP, A2A, AG-UI, etc.) implements this interface to provide
/// mechanical normalization without product semantics.
/// </summary>
/// <typeparam name="TRaw">The raw protocol event type.</typeparam>
public interface IProtocolIngressAdapter<in TRaw>
{
    /// <summary>
    /// Normalizes a raw protocol event into an OntogonyEnvelope with mechanical processing.
    /// </summary>
    /// <param name="raw">The raw protocol event to normalize.</param>
    /// <param name="context">The ingress context containing trace ID and other options.</param>
    /// <returns>A ProtocolIngressResult containing either the normalized envelope or validation errors.</returns>
    ProtocolIngressResult Normalize(TRaw raw, ProtocolIngressContext context);
}
