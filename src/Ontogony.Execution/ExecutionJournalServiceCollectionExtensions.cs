using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Runtime;

namespace Ontogony.Execution;

/// <summary>
/// Registers the in-memory execution journal for tests, examples, and single-process hosts.
/// </summary>
public static class ExecutionJournalServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="InMemoryExecutionJournal"/> as the singleton <see cref="IExecutionJournal"/>.
    /// When the host environment is not <see cref="Microsoft.Extensions.Hosting.Environments.Development"/>, registers a startup warning that this journal is not durable for production multi-instance use.
    /// </summary>
    public static IServiceCollection AddOntogonyInMemoryExecutionJournal(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<InMemoryExecutionJournal>();
        services.TryAddSingleton<IExecutionJournal>(sp => sp.GetRequiredService<InMemoryExecutionJournal>());

        services.AddOntogonyInMemoryNonDurableStartupWarning("Ontogony.Execution: InMemoryExecutionJournal");

        return services;
    }
}
