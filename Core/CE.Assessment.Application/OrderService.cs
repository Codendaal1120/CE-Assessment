using CE.Assessment.Application.Model;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging;

namespace CE.Assessment.Application;

public class OrderService
{
    private IChannelEngineClient _client;
    private ILogger _logger;

    public OrderService(ILogger<OrderService> logger, IChannelEngineClient client)
    {
        _client = client;
        _logger = logger;
    }

    /// <summary>
    /// The assement requirement. 
    /// Gets a list of the top 5 products sold (product name, GTIN and total quantity), 
    /// ordered by the total quantity sold in descending order
    /// </summary>
    public async Task<TryResult<IEnumerable<ProductSalesRecord>>> GetTopNOrders(int orderCount, CancellationToken ct)
    {
        var orderResponse = await _client.GetOrders(OrderStatusView.IN_PROGRESS, ct);

        // check the http call
        if (!orderResponse.IsSuccessStatusCode)
        {
            _logger.LogError($"Could not get top 5 sold products, the http call failed with: {orderResponse.Error}");
            return TryResult<IEnumerable<ProductSalesRecord>>.Fail($"Http call failed with: {orderResponse.Error}");
        }

        // check api response
        if (orderResponse.Content?.Content == null)
        {
            _logger.LogError("Could not get top 5 sold products, the api returned an empty response");
            return TryResult<IEnumerable<ProductSalesRecord>>.Fail("Could not get top 5 sold products, the api returned an empty response");
        }

        if (!orderResponse.Content.Success)
        {
            // check api response
            _logger.LogError($"Could not get top 5 sold products, the api returned an error: {orderResponse.Content.Message}");
            return TryResult<IEnumerable<ProductSalesRecord>>.Fail($"Api returned an error: {orderResponse.Content.Message}");
        }

        /*
         * Technically this could be wrong, since the results are paged and we are only reading the first page. 
         * I am assumiung that traversing the pages is beyond the scope of this assesment.
         */

        var rankedOrders = orderResponse.Content.Content
            .Where(o => o.Lines != null)
            .SelectMany(o => o.Lines!)
            .GroupBy(l => new { l.Gtin, l.MerchantProductNo, l.Description, l.Quantity })
            .Select(g => new ProductSalesRecord() { Description = g.Key.Description, Gtin = g.Key.Gtin, ProductNo = g.Key.MerchantProductNo, Quantity = g.Sum(s => s.Quantity) })
            .OrderByDescending(x => x.Quantity)
            .Take(orderCount);

        return TryResult<IEnumerable<ProductSalesRecord>>.Succeed(rankedOrders);
    }
}
