namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;

public sealed class ChannelResponse<T>
{  
    public IReadOnlyCollection<T>? Content { get; set; }
    public required int Count { get; init; }
    public required int TotalCount { get; init; }
    public required int ItemsPerPage { get; init; }
    public required int StatusCode { get; init; }
    public string? RequestId { get; init; }
    public string? LogId { get; init; }
    public required bool Success { get; init; }
    public string? Message { get; init; }
    public string? ExceptionType { get; init; }
    public Dictionary<string, IReadOnlyCollection<object>>? ValidationErrors { get; set; }

    public ChannelResponse(int count, int totalCount, int itemsPerPage, int statusCode, bool success)
    {
        Count = count;
        TotalCount = totalCount;
        ItemsPerPage = itemsPerPage;
        StatusCode = statusCode;
        Success = success;
    }
}