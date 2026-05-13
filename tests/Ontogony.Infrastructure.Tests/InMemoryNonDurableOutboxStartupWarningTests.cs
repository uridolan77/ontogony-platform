using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ontogony.Persistence;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class InMemoryNonDurableOutboxStartupWarningTests
{
    [Fact]
    public async Task In_memory_outbox_logs_warning_when_not_development()
    {
        var sink = new ConcurrentQueue<(LogLevel Level, string Message)>();
        var builder = Host.CreateApplicationBuilder();
        builder.Environment.EnvironmentName = Environments.Production;
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ListLoggerProvider(sink));
        builder.Services.AddOntogonyInMemoryOutboxStore();

        using var host = builder.Build();
        await host.StartAsync();

        Assert.Contains(
            sink,
            e => e.Level == LogLevel.Warning &&
                 e.Message.Contains("InMemoryOutboxStore", StringComparison.Ordinal) &&
                 e.Message.Contains("AddOntogonyPostgresOutbox", StringComparison.Ordinal));

        await host.StopAsync();
    }

    [Fact]
    public async Task In_memory_outbox_does_not_warn_in_development()
    {
        var sink = new ConcurrentQueue<(LogLevel Level, string Message)>();
        var builder = Host.CreateApplicationBuilder();
        builder.Environment.EnvironmentName = Environments.Development;
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ListLoggerProvider(sink));
        builder.Services.AddOntogonyInMemoryOutboxStore();

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
