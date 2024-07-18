using CE.Assessment.Application.Model;
using System.Diagnostics.CodeAnalysis;

namespace CE.Assessment.Web.Models;

[ExcludeFromCodeCoverage]
public class HomeViewModel
{
    public string? ErrorMessage { get; init; }
    public ProductSalesRecord[] Records { get; init; } = Array.Empty<ProductSalesRecord>();
}
