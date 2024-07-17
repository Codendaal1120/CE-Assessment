namespace CE.Assessment.Application.Model;

public record ProductSalesRecord
{
    public string? Description { get; init; }
    public string? Gtin { get; init; }
    public string? ProductNo { get; init; }
    public int Quantity { get; init; }
}