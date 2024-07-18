using CE.Assessment.Application.Model;
using CE.Assessment.Application.Services;
using CE.Assessment.Application;
using CE.Assessment.Options;
using CommandLine;
using System.Data.Common;

namespace CE.Assessment;

internal class ComandLineRunner
{
    private OrderService _orderService;
    private ProductsService _productsService;

    private const int TableWidth = 70;

    public ComandLineRunner(OrderService orderService, ProductsService productsService)
    {
        _orderService = orderService;
        _productsService = productsService;
    }

    public async Task Run(string? arguments)
    {
        var args = arguments?.Split();

        Parser.Default.ParseArguments<UpdateProductOptions, GetTopNoptions>(args)
                  .WithParsed<UpdateProductOptions>(async o =>
                  {
                      await RunUpdateCommand(o);
                  })
                  .WithParsed<GetTopNoptions>(async o =>
                  {
                      await RunGetTopProducts(o);
                  });
    }

    private async Task RunUpdateCommand(UpdateProductOptions options)
    {

    }

    private async Task RunGetTopProducts(GetTopNoptions options)
    {
        var response = await _orderService.GetTopNOrders(options.Count, CancellationToken.None);
        if (!response.Try(out var topNorders, out var error))
        {
            Console.Error.WriteLine(error);
        }

        Console.WriteLine($"Top {options.Count} products sold\n");

        PrintHeader();
        foreach (var record in topNorders!)
        {
            PrintOrder(record);
        }
        PrintFooter();
    }

    private void PrintHeader()
    {
        var width = TableWidth / 3;
        string row = "|";

        row += CentreText(nameof(ProductSalesRecord.ProductNo), width) + "|";
        row += CentreText(nameof(ProductSalesRecord.Gtin), width) + "|";
        row += CentreText(nameof(ProductSalesRecord.Quantity), width) + "|";

        Console.WriteLine("|" + new string('-', TableWidth + 1) + "|");
        Console.WriteLine(row);
        Console.WriteLine("|" + new string('-', TableWidth + 1) + "|");
    }

    private void PrintFooter()
    {
        Console.WriteLine("|" + new string('-', TableWidth + 1) + "|");
    }

    private void PrintOrder(ProductSalesRecord record)
    {
        var width = TableWidth / 3;
        string row = "|";

        row += CentreText(record.ProductNo ?? string.Empty, width) + "|";
        row += CentreText(record.Gtin ?? string.Empty, width) + "|";
        row += CentreText(record.Quantity.ToString(), width) + "|";

        Console.WriteLine(row);
    }

    private string CentreText(string txt, int width)
    {
        txt = txt.Length > width ? txt.Substring(0, width - 3) + "..." : txt;

        if (string.IsNullOrEmpty(txt))
        {
            return new string(' ', width);
        }
        else
        {
            return txt.PadRight(width - (width - txt.Length) / 2).PadLeft(width);
        }
    }
}
