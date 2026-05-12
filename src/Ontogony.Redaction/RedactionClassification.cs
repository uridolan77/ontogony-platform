namespace Ontogony.Redaction;

public enum RedactionClassification
{
    None = 0,
    Secret = 1,
    Credential = 2,
    Token = 3,
    PersonalData = 4,
    Prompt = 5,
    Response = 6,
    Internal = 7
}
