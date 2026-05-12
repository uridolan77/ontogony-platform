namespace Ontogony.Persistence.Postgres;

internal readonly record struct PostgresSqlNames(string SchemaName, string Outbox, string Processed, string DeadLetter)
{
    public static PostgresSqlNames FromOptions(PostgresOutboxOptions options)
    {
        return new PostgresSqlNames(
            QuoteIdentifier(options.SchemaName),
            QuoteIdentifier(options.OutboxTableName),
            QuoteIdentifier(options.ProcessedTableName),
            QuoteIdentifier(options.DeadLetterTableName));
    }

    public string QualifiedOutbox => $"{SchemaName}.{Outbox}";

    public string QualifiedProcessed => $"{SchemaName}.{Processed}";

    public string QualifiedDeadLetter => $"{SchemaName}.{DeadLetter}";

    private static string QuoteIdentifier(string value)
    {
        return $"\"{value}\"";
    }
}
