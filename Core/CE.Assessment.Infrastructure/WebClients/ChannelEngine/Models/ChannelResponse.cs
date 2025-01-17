﻿namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;

public class ChannelResponse<T>
{
    public T? Content { get; init; }
    public required int StatusCode { get; init; }
    public string? RequestId { get; init; }
    public string? LogId { get; init; }
    public required bool Success { get; init; }
    public string? Message { get; init; }
    public string? ExceptionType { get; init; }
    public Dictionary<string, IReadOnlyCollection<object>>? ValidationErrors { get; init; }
}