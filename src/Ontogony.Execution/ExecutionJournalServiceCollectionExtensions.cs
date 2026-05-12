using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ontogony.Execution;

/// <summary>
/// Registers the in-memory execution journal for tests, examples, and single-process hosts.
/// </summary>
public static class ExecutionJournalServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="InMemoryExecutionJournal"/> as the singleton <see cref="IExecutionJournal"/>.
    /// </summary>
    public static IServiceCollection AddOntogonyInMemoryExecutionJournal(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<InMemoryExecutionJournal>();
        services.TryAddSingleton<IExecutionJournal>(sp => sp.GetRequiredService<InMemoryExecutionJournal>());

        return services;
    }
}
