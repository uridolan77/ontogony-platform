namespace Athanor.Infrastructure.Options;

public sealed class ProjectionOutboxOptions
{
    public const string SectionName = "ProjectionOutbox";

    public bool Enabled { get; init; } = false;

    public int BatchSize { get; init; } = 25;

    public int MaxAttempts { get; init; } = 10;
}

