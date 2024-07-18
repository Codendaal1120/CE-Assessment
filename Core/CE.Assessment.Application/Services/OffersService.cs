using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging;

namespace CE.Assessment.Application.Services;

public class OffersService
{
    private IChannelEngineClient _client;
    private ILogger _logger;

    public OffersService(ILogger<OffersService> logger, IChannelEngineClient client)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<TryResult> UpdateProductStock(string merchantProductNo, int stock, CancellationToken ct)
    {
        /*
         * Hardcoding stock location 2 (api-dev)
         */
        var update = new MerchantOfferStockUpdateRequest()
        {
            MerchantProductNo = merchantProductNo,
            StockLocations = [ new MerchantStockLocationUpdateRequest() { Stock = stock, StockLocationId = 2 } ]
        };

        var result = await _client.UpdateProductStock([update], ct);

        // check the http call
        if (!result.IsSuccessStatusCode)
        {
            _logger.LogError($"Could not update the product stock, the http call failed with: {result.Error}");
            return TryResult.Fail($"Http call failed with: {result.Error}");
        }

        // check validation errors
        if (result.Content?.ValidationErrors != null && result.Content.ValidationErrors.Any())
        {
            var error = result.Content.ValidationErrors.AsString();
            _logger.LogError($"Could not update the product stock, the api returned validation errors: {error}");
            return TryResult.Fail($"Api returned validation errors: {error}");
        }

        // check api response
        if (result.Content?.Content == null)
        {
            _logger.LogError("Could not update the product stock, the api returned an empty response");
            return TryResult.Fail("Could not update product stock, the api returned an empty response");
        }

        if (!result.Content.Success)
        {
            // check api response
            _logger.LogError($"Could not update the product stock, the api returned an error: {result.Content.Message}");
            return TryResult.Fail($"Api returned an error: {result.Content.Message}");
        }

        if (result.Content.Content.Any())
        {
            var updateErrors = result.Content.Content.AsString();
            _logger.LogError($"Could not update the product, the api rejected the update: {updateErrors}");
            return TryResult.Fail($"Api rejected the update: {updateErrors}");
        }

        return TryResult.Succeed();
    }     
}
