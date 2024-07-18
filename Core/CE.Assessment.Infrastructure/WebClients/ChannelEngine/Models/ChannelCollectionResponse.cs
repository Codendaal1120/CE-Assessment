namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;

public sealed class ChannelCollectionResponse<T> : ChannelResponse<T>
{  
    public new IReadOnlyCollection<T>? Content { get; init; }
    public int Count { get; init; }
    public int TotalCount { get; init; }
    public required int ItemsPerPage { get; init; }
}
