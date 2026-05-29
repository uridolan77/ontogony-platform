using System.Text.Json;

namespace Ontogony.SystemTests.Infrastructure;

public static class JsonRunDocuments
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static async Task<JsonDocument?> TryParseAsync(HttpResponseMessage response)
    {
        var text = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        try
        {
            return JsonDocument.Parse(text);
        }
        catch
        {
            return null;
        }
    }

    public static string? GetString(JsonElement root, params string[] path)
    {
        var current = root;
        foreach (var segment in path)
        {
            if (!current.TryGetProperty(segment, out current))
            {
                return null;
            }
        }

        return current.ValueKind switch
        {
            JsonValueKind.String => current.GetString(),
            JsonValueKind.Number => current.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => null
        };
    }

    public static string RequireRunId(JsonDocument doc) =>
        GetString(doc.RootElement, "runId")
        ?? throw new InvalidOperationException("Response missing runId.");

    public static string RequireStatus(JsonDocument doc) =>
        GetString(doc.RootElement, "status")
        ?? throw new InvalidOperationException("Response missing status.");

    public static IReadOnlyList<string> GetEventTypes(JsonDocument eventsArrayDoc)
    {
        if (eventsArrayDoc.RootElement.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var list = new List<string>();
        foreach (var item in eventsArrayDoc.RootElement.EnumerateArray())
        {
            var eventType = GetString(item, "eventType");
            if (!string.IsNullOrWhiteSpace(eventType))
            {
                list.Add(eventType);
            }
        }

        return list;
    }

    public static string GetHumanGateIdFromEvents(JsonDocument eventsArrayDoc)
    {
        if (eventsArrayDoc.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected events array.");
        }

        string? lastGateId = null;
        foreach (var item in eventsArrayDoc.RootElement.EnumerateArray())
        {
            var eventType = GetString(item, "eventType");
            if (!string.Equals(eventType, AllagmaEventTypes.RunHumanGatePaused, StringComparison.Ordinal))
            {
                continue;
            }

            if (!item.TryGetProperty("payload", out var payload))
            {
                continue;
            }

            lastGateId = GetString(payload, "humanGateId")
                ?? GetString(payload, "HumanGateId");
        }

        if (string.IsNullOrWhiteSpace(lastGateId))
        {
            throw new InvalidOperationException($"Expected {AllagmaEventTypes.RunHumanGatePaused} with payload.humanGateId.");
        }

        return lastGateId;
    }

    public static void AssertOrderedSubsequence(IReadOnlyList<string> actual, IReadOnlyList<string> required)
    {
        var index = 0;
        foreach (var requiredType in required)
        {
            while (index < actual.Count && !string.Equals(actual[index], requiredType, StringComparison.Ordinal))
            {
                index++;
            }

            if (index >= actual.Count)
            {
                throw new InvalidOperationException(
                    $"Missing required event '{requiredType}'. Observed: {string.Join(", ", actual)}");
            }

            index++;
        }
    }
}
