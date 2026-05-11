namespace Athanor.Infrastructure.Options;

public sealed class BackgroundWorkersOptions
{
    public const string SectionName = "BackgroundWorkers";

    public bool ProjectionOutboxEnabled { get; init; } = false;
    public bool WorkflowRunnerEnabled { get; init; } = false;
    public bool MaintenancePolicyEvaluatorEnabled { get; init; } = false;
    public bool StaleProcessingJobRecoveryEnabled { get; init; } = false;
    public bool EmbeddingQueueEnabled { get; init; } = false;

    public int PollIntervalMs { get; init; } = 2000;
}

