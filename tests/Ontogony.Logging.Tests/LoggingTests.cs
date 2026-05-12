using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ontogony.Logging;
using Ontogony.Observability;
using Ontogony.Redaction;
using Xunit;

namespace Ontogony.Logging.Tests;

public sealed class LoggingTests
{
    [Fact]
    public void Event_ids_are_stable()
    {
        Assert.Equal(1000, OntogonyLogEvents.RequestStarted.Id);
        Assert.Equal("RequestStarted", OntogonyLogEvents.RequestStarted.Name);
        Assert.Equal(2002, OntogonyLogEvents.AiRequestFailed.Id);
    }

    [Fact]
    public void Begin_scope_includes_correlation_fields()
    {
        var logger = new CapturingLogger();
        using var pushed = OntogonyCorrelationContext.Push(new CorrelationState(
            "trace-1",
            "op-1",
            TenantId: "tenant-1",
            ProjectId: "project-1"));

        using (logger.BeginOntogonyScope(new Dictionary<string, object?>
        {
            [OntogonyLogFields.Operation] = "test"
        }))
        {
        }

        Assert.Equal("trace-1", logger.LastScope![OntogonyLogFields.TraceId]);
        Assert.Equal("op-1", logger.LastScope[OntogonyLogFields.OperationId]);
        Assert.Equal("tenant-1", logger.LastScope[OntogonyLogFields.TenantId]);
        Assert.Equal("project-1", logger.LastScope[OntogonyLogFields.ProjectId]);
        Assert.Equal("test", logger.LastScope[OntogonyLogFields.Operation]);
    }

    [Fact]
    public void Begin_scope_redacts_sensitive_additional_fields_when_redactor_provided()
    {
        var logger = new CapturingLogger();
        var redactor = new DefaultRedactor(Options.Create(new RedactionOptions { RevealSuffixCharacters = 0 }));
        const string secret = "sk-live-abcdef";

        using (logger.BeginOntogonyScope(
                   new Dictionary<string, object?> { ["provider_api_key"] = secret },
                   new OntogonyLoggingOptions { IncludeTenantId = false, IncludeWorkspaceId = false, IncludeProjectId = false, IncludeActorId = false, IncludeSessionId = false },
                   redactor))
        {
        }

        var scoped = logger.LastScope!["provider_api_key"]?.ToString();
        Assert.NotNull(scoped);
        Assert.NotEqual(secret, scoped);
        Assert.DoesNotContain("abcdef", scoped, StringComparison.Ordinal);
    }

    private sealed class CapturingLogger : ILogger
    {
        public IReadOnlyDictionary<string, object?>? LastScope { get; private set; }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            LastScope = Assert.IsAssignableFrom<IReadOnlyDictionary<string, object?>>(state);
            return new Disposable();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
        }

        private sealed class Disposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}
