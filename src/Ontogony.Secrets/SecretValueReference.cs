namespace Ontogony.Secrets;

/// <summary>
/// Scheme-specific secret locator (for example scheme <c>env</c> with locator equal to an environment variable name).
/// </summary>
/// <param name="Scheme">Case-insensitive scheme name understood by a resolver (for example <c>env</c>).</param>
/// <param name="Locator">Scheme-specific key or path (for example an environment variable name).</param>
public readonly record struct SecretValueReference(string Scheme, string Locator);
