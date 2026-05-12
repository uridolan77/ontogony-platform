using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Persistence.Postgres;

internal readonly record struct PostgresSqlNames(
    string SchemaName,
    string Outbox,
    string Processed,
    string DeadLetter,
    string OutboxAvailableIndex,
    string OutboxClaimIndex)
{
    public static PostgresSqlNames FromOptions(PostgresOutboxOptions options)
    {
        var outboxAvailableIndex = BuildScopedName(options.OutboxTableName, "available");
        var outboxClaimIndex = BuildScopedName(options.OutboxTableName, "claim");

        return new PostgresSqlNames(
            QuoteIdentifier(options.SchemaName),
            QuoteIdentifier(options.OutboxTableName),
            QuoteIdentifier(options.ProcessedTableName),
            QuoteIdentifier(options.DeadLetterTableName),
            QuoteIdentifier(outboxAvailableIndex),
            QuoteIdentifier(outboxClaimIndex));
    }

    public string QualifiedOutbox => $"{SchemaName}.{Outbox}";

    public string QualifiedProcessed => $"{SchemaName}.{Processed}";

    public string QualifiedDeadLetter => $"{SchemaName}.{DeadLetter}";

    private static string QuoteIdentifier(string value)
    {
        return $"\"{value}\"";
    }

    private static string BuildScopedName(string tableName, string suffix)
    {
        const int maxIdentifierLength = 63;

        var candidate = $"ix_{tableName}_{suffix}";
        if (candidate.Length <= maxIdentifierLength)
        {
            return candidate;
        }

        var hash = ComputeHash(candidate, 8);
        var prefixLength = maxIdentifierLength - hash.Length - 1;
        if (prefixLength <= 0)
        {
            return hash;
        }

        return $"{candidate[..prefixLength]}_{hash}";
    }

    private static string ComputeHash(string input, int hexLength)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var hex = Convert.ToHexString(bytes).ToLowerInvariant();
        return hex[..Math.Min(hex.Length, hexLength)];
    }
}
