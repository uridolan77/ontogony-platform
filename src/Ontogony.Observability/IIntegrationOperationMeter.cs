namespace Ontogony.Observability;

/// <summary>
/// Records consistent outbound integration metrics in the shared Ontogony metric namespace.
/// </summary>
public interface IIntegrationOperationMeter
{
    /// <summary>
    /// Starts an outbound integration activity scope. Dispose ends the diagnostic activity only;
    /// metrics are recorded through <see cref="RecordSuccess"/> or <see cref="RecordFailure"/>.
    /// </summary>
    IDisposable StartCall(
        string targetService,
        string operation,
        IReadOnlyDictionary<string, string>? dimensions = null);

    /// <summary>Records a successful outbound integration call.</summary>
    void RecordSuccess(
        string targetService,
        string operation,
        TimeSpan duration,
        IReadOnlyDictionary<string, string>? dimensions = null);

    /// <summary>Records a failed outbound integration call.</summary>
    void RecordFailure(
        string targetService,
        string operation,
        string errorCode,
        TimeSpan duration,
        IReadOnlyDictionary<string, string>? dimensions = null);
}
