using System.Net;

namespace Ontogony.Errors;

public sealed class OntogonyExceptionMappingOptions
{
    private readonly Dictionary<Type, ExceptionMapping> _mappings = new();

    public OntogonyExceptionMappingOptions Map<TException>(
        HttpStatusCode statusCode,
        string errorCode,
        string? publicMessage = null,
        bool includeExceptionMessage = false,
        bool logAsWarning = true,
        bool includeDetails = false)
        where TException : Exception
    {
        _mappings[typeof(TException)] = new ExceptionMapping(
            statusCode,
            errorCode,
            publicMessage,
            includeExceptionMessage,
            logAsWarning,
            includeDetails);
        return this;
    }

    public ExceptionMapping? Find(Exception exception)
    {
        var type = exception.GetType();
        while (type is not null)
        {
            if (_mappings.TryGetValue(type, out var mapping)) return mapping;
            type = type.BaseType;
        }

        return null;
    }
}

public sealed record ExceptionMapping(
    HttpStatusCode StatusCode,
    string ErrorCode,
    string? PublicMessage = null,
    bool IncludeExceptionMessage = false,
    bool LogAsWarning = true,
    bool IncludeDetails = false);
