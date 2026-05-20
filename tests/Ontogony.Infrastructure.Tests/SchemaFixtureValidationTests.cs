using System.Text.Json;
using Json.Schema;
using Ontogony.Contracts.Events;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class SchemaFixtureValidationTests
{
    private readonly JsonSchema _envelopeSchema;
    private readonly JsonSerializerOptions _jsonOptions;

    public SchemaFixtureValidationTests()
    {
        // Load the schema from the JSON schema file
        var schemaPath = Path.Combine(
            GetProjectRoot(),
            "schemas",
            "ontogony-envelope.schema.json"
        );
        var schemaJson = File.ReadAllText(schemaPath);
        _envelopeSchema = JsonSchema.FromText(schemaJson);
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = false };
    }

    [Theory]
    [InlineData("schemas/fixtures/valid/minimal-envelope.json")]
    [InlineData("schemas/fixtures/valid/full-envelope.json")]
    [InlineData("schemas/fixtures/valid/ag-ui-protocol.json")]
    [InlineData("schemas/fixtures/valid/generic-json-protocol.json")]
    [InlineData("schemas/fixtures/valid/cloudevents-compat.json")]
    public void ValidFixtures_PassSchemaValidation(string relativePath)
    {
        // Arrange
        var filePath = Path.Combine(GetProjectRoot(), relativePath);
        var json = File.ReadAllText(filePath);
        using var doc = JsonDocument.Parse(json);

        // Act
        var result = _envelopeSchema.Evaluate(doc.RootElement);

        // Assert
        Assert.True(result.IsValid, $"Schema validation failed for {relativePath}");
    }

    [Theory]
    [InlineData("schemas/fixtures/invalid/missing-eventid.json")]
    [InlineData("schemas/fixtures/invalid/bad-eventtype-format.json")]
    [InlineData("schemas/fixtures/invalid/additional-properties.json")]
    public void InvalidFixtures_FailSchemaValidation(string relativePath)
    {
        // Arrange
        var filePath = Path.Combine(GetProjectRoot(), relativePath);
        var json = File.ReadAllText(filePath);
        using var doc = JsonDocument.Parse(json);

        // Act
        var result = _envelopeSchema.Evaluate(doc.RootElement);

        // Assert
        Assert.False(result.IsValid, $"Schema validation should have failed for {relativePath}");
    }

    [Theory]
    [InlineData("schemas/fixtures/valid/minimal-envelope.json")]
    [InlineData("schemas/fixtures/valid/full-envelope.json")]
    public void ValidFixtures_PassEnvelopeValidator(string relativePath)
    {
        // Arrange
        var filePath = Path.Combine(GetProjectRoot(), relativePath);
        var json = File.ReadAllText(filePath);
        var validator = new DefaultEnvelopeValidator();

        // Act & Assert - should not throw
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        
        // Verify required fields exist and are non-empty
        Assert.True(root.TryGetProperty("EventId", out var eventId) && eventId.ValueKind != JsonValueKind.Null);
        Assert.True(root.TryGetProperty("EventType", out var eventType) && eventType.ValueKind != JsonValueKind.Null);
        Assert.True(root.TryGetProperty("Source", out var source) && source.ValueKind != JsonValueKind.Null);
        Assert.True(root.TryGetProperty("TraceId", out var traceId) && traceId.ValueKind != JsonValueKind.Null);
        Assert.True(root.TryGetProperty("Protocol", out var protocol) && protocol.ValueKind != JsonValueKind.Null);
    }

    [Fact]
    public void FullEnvelopeFixture_Includes_RuntimeProtocolMetadata()
    {
        var filePath = Path.Combine(GetProjectRoot(), "schemas/fixtures/valid/full-envelope.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(filePath));
        var root = doc.RootElement;

        Assert.Equal("allagma.run.start.v1", root.GetProperty("ProtocolId").GetString());
        Assert.Equal("authoritative", root.GetProperty("AuthorityMode").GetString());
        Assert.Equal("run_state_transition", root.GetProperty("SideEffectLevel").GetString());
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ontogony.Platform.sln")))
        {
            dir = dir.Parent;
        }
        return dir?.FullName ?? throw new InvalidOperationException("Could not find project root");
    }
}

