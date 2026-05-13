using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ontogony.Runtime;

/// <summary>
/// Logs a one-time startup warning when an in-memory, non-durable Ontogony mechanism is registered and the host is not in the Development environment (PR-PLAT-009).
/// </summary>
internal sealed class InMemoryNonDurableStartupWarningHostedService : IHostedService
{
    private const string LoggerCategory = "Ontogony.Runtime.InMemoryNonDurable";
    private readonly IHostEnvironment _environment;
    private readonly ILogger _logger;
    private readonly string _mechanismDisplayName;
    private readonly string _durableReplacementGuidance;

    public InMemoryNonDurableStartupWarningHostedService(
        IHostEnvironment environment,
        ILoggerFactory loggerFactory,
        string mechanismDisplayName,
        string durableReplacementGuidance)
    {
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(mechanismDisplayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(durableReplacementGuidance);

        _environment = environment;
        _logger = loggerFactory.CreateLogger(LoggerCategory);
        _mechanismDisplayName = mechanismDisplayName;
        _durableReplacementGuidance = durableReplacementGuidance;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_environment.IsDevelopment())
        {
            return Task.CompletedTask;
        }

        _logger.LogWarning(
            "Ontogony: non-durable in-memory registration active ({Mechanism}). {DurableReplacementGuidance}",
            _mechanismDisplayName,
            _durableReplacementGuidance);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

internal static class InMemoryNonDurableStartupWarningRegistration
{
    internal static IServiceCollection AddOntogonyInMemoryNonDurableStartupWarning(
        this IServiceCollection services,
        string mechanismDisplayName,
        string durableReplacementGuidance)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(mechanismDisplayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(durableReplacementGuidance);

        services.AddSingleton<IHostedService>(sp =>
            new InMemoryNonDurableStartupWarningHostedService(
                sp.GetRequiredService<IHostEnvironment>(),
                sp.GetRequiredService<ILoggerFactory>(),
                mechanismDisplayName,
                durableReplacementGuidance));

        return services;
    }
}
