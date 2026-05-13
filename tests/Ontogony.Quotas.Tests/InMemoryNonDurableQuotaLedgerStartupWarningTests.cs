using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ontogony.Quotas;
using Xunit;

namespace Ontogony.Quotas.Tests;

public sealed class InMemoryNonDurableQuotaLedgerStartupWarningTests
{
    [Fact]
    public async Task In_memory_quota_ledger_logs_warning_when_not_development()
    {
        var sink = new ConcurrentQueue<(LogLevel Level, string Message)>();
        var builder = Host.CreateApplicationBuilder();
        builder.Environment.EnvironmentName = Environments.Production;
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ListLoggerProvider(sink));
        builder.Services.AddOntogonyInMemoryQuotaLedger();

        using var host = builder.Build();
        await host.StartAsync();

        Assert.Contains(
            sink,
            e => e.Level == LogLevel.Warning &&
                 e.Message.Contains("InMemoryQuotaLedger", StringComparison.Ordinal) &&
                 e.Message.Contains("IQuotaLedger", StringComparison.Ordinal));

        await host.StopAsync();
    }

    [Fact]
    public async Task In_memory_quota_ledger_does_not_warn_in_development()
    {
        var sink = new ConcurrentQueue<(LogLevel Level, string Message)>();
        var builder = Host.CreateApplicationBuilder();
        builder.Environment.EnvironmentName = Environments.Development;
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ListLoggerProvider(sink));
        builder.Services.AddOntogonyInMemoryQuotaLedger();

        using var host = builder.Build();
        await host.StartAsync();

        Assert.DoesNotContain(sink, e => e.Level == LogLevel.Warning);
        await host.StopAsync();
    }

    private sealed class ListLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentQueue<(LogLevel Level, string Message)> _sink;

        public ListLoggerProvider(ConcurrentQueue<(LogLevel Level, string Message)> sink) => _sink = sink;

        public ILogger CreateLogger(string categoryName) => new ListLogger(_sink);

        public void Dispose()
        {
        }
    }

    private sealed class ListLogger : ILogger
    {
        private readonly ConcurrentQueue<(LogLevel Level, string Message)> _sink;

        public ListLogger(ConcurrentQueue<(LogLevel Level, string Message)> sink) => _sink = sink;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _sink.Enqueue((logLevel, formatter(state, exception)));
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
