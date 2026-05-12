namespace Ontogony.Persistence.Postgres;

/// <summary>
/// Configuration for PostgreSQL outbox storage.
/// </summary>
public sealed class PostgresOutboxOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string SchemaName { get; set; } = "public";

    public string OutboxTableName { get; set; } = "ontogony_outbox_messages";

    public string ProcessedTableName { get; set; } = "ontogony_processed_messages";

    public string DeadLetterTableName { get; set; } = "ontogony_dead_letter_messages";

    public TimeSpan ClaimLeaseDuration { get; set; } = TimeSpan.FromSeconds(30);

    public int? MoveToDeadLetterAfterAttempts { get; set; }

    public bool EnsureSchemaOnStartup { get; set; }

    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new InvalidOperationException("PostgresOutboxOptions.ConnectionString is required.");
        }

        ValidateIdentifier(SchemaName, nameof(SchemaName));
        ValidateIdentifier(OutboxTableName, nameof(OutboxTableName));
        ValidateIdentifier(ProcessedTableName, nameof(ProcessedTableName));
        ValidateIdentifier(DeadLetterTableName, nameof(DeadLetterTableName));

        if (ClaimLeaseDuration <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("PostgresOutboxOptions.ClaimLeaseDuration must be positive.");
        }

        if (MoveToDeadLetterAfterAttempts is <= 0)
        {
            throw new InvalidOperationException("PostgresOutboxOptions.MoveToDeadLetterAfterAttempts must be greater than zero when set.");
        }
    }

    private static void ValidateIdentifier(string identifier, string paramName)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new InvalidOperationException($"{paramName} is required.");
        }

        foreach (var ch in identifier)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '_')
            {
                throw new InvalidOperationException($"{paramName} contains an invalid character '{ch}'.");
            }
        }
    }
}
