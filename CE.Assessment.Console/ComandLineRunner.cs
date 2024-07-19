using CE.Assessment.Application.Model;
using CE.Assessment.Application.Services;
using CE.Assessment.Application;
using CE.Assessment.Options;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace CE.Assessment;

internal class ComandLineRunner
{
    private OrderService _orderService;
    private OffersService _offerService;
    private ILogger _logger;

    private const int TableWidth = 70;

    public ComandLineRunner(ILogger<ComandLineRunner> logger, OrderService orderService, OffersService offersService)
    {
        _orderService = orderService;
        _offerService = offersService;
        _logger = logger;
    }

    public void Run(string? arguments)
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
        _logger.LogInformation($"Setting {options.Product}'s stock to {options.Quantity}");

        var response = await _offerService.UpdateProductStock(options.Product, options.Quantity, CancellationToken.None);
        if (!response.Success)
        {
            _logger.LogError(response.Error);
            return;
        }

        _logger.LogInformation($"{options.Product}'s stock updated to {options.Quantity}");
    }

    private async Task RunGetTopProducts(GetTopNoptions options)
    {
        _logger.LogInformation($"Getting top {options.Count} products sold\n");

        var response = await _orderService.GetTopNOrders(options.Count, CancellationToken.None);
        if (!response.Try(out var topNorders, out var error))
        {
            _logger.LogError(error);
            return;
        }

        _logger.LogInformation($"Top {options.Count} products sold\n");

        PrintHeader();
        foreach (var record in topNorders!)
        {
            PrintOrder(record);
        }
        PrintFooter();
    }

    private void PrintHeader()
    {
        var width = TableWidth / 4;
        string row = "|";

        row += CentreText(nameof(ProductSalesRecord.ProductNo), width) + "|";
        row += CentreText(nameof(ProductSalesRecord.Gtin), width) + "|";
        row += CentreText(nameof(ProductSalesRecord.Description), width) + "|";
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
        var width = TableWidth / 4;
        string row = "|";

        row += CentreText(record.ProductNo ?? string.Empty, width) + "|";
        row += CentreText(record.Gtin ?? string.Empty, width) + "|";
        row += CentreText(record.Description ?? string.Empty, width) + "|";
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
