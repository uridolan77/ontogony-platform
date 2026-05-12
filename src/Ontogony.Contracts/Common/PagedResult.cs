namespace Ontogony.Contracts.Common;

/// <summary>
/// A page of items with total count and paging parameters (mechanical DTO; no repository semantics).
/// </summary>
/// <typeparam name="T">Item type.</typeparam>
/// <param name="Items">Items on this page.</param>
/// <param name="TotalCount">Total items across all pages.</param>
/// <param name="Page">Zero-based page index.</param>
/// <param name="PageSize">Page size.</param>
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize);
