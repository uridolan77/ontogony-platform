namespace Ontogony.Messaging;

public sealed class EventDispatchOptions
{
    public bool ContinueOnHandlerException { get; set; }

    public bool ComputePayloadHash { get; set; }

    public bool ValidateRequiredEnvelopeFields { get; set; } = true;
}