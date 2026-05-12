namespace Ontogony.Redaction;

/// <summary>
/// Coarse category for why a value was redacted (telemetry and policy hints only).
/// </summary>
public enum RedactionClassification
{
    /// <summary>No redaction applied.</summary>
    None = 0,

    /// <summary>Generic secret material.</summary>
    Secret = 1,

    /// <summary>Passwords or similar credentials.</summary>
    Credential = 2,

    /// <summary>Bearer tokens, API keys, refresh tokens, etc.</summary>
    Token = 3,

    /// <summary>Personally identifying information.</summary>
    PersonalData = 4,

    /// <summary>Model or user prompt text.</summary>
    Prompt = 5,

    /// <summary>Model or tool response text.</summary>
    Response = 6,

    /// <summary>Internal-only diagnostic material.</summary>
    Internal = 7
}
