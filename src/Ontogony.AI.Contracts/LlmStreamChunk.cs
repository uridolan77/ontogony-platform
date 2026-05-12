namespace Ontogony.AI.Contracts;

/// <summary>
/// One streaming chunk for a request (hashes are mechanical fingerprints of deltas / wire bytes).
/// </summary>
public sealed record LlmStreamChunk(
    string ChunkId,
    string RequestId,
    string TraceId,
    int Sequence,
    string? DeltaTextHash,
    string? ProviderChunkHash,
    bool IsFinal,
    DateTimeOffset ReceivedAt);
