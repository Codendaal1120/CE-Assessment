using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging;

namespace CE.Assessment.Application.Services;

public class ProductsService
{
    private IChannelEngineClient _client;
    private ILogger _logger;

    public ProductsService(ILogger<ProductsService> logger, IChannelEngineClient client)
    {
        _client = client;
        _logger = logger;
    }

    [Obsolete("Does not work to update stock, use <see cref=\"OffersService\"/> ")]
    public async Task<TryResult> UpdateProductStock(string merchantProductNo, int stock, CancellationToken ct)
    {
        var update = new PatchMerchantProductDto()
        {
            PropertiesToUpdate = new List<string>() { nameof(MerchantProductRequest.Stock) },
            MerchantProductRequestModels = new List<MerchantProductRequest>()
            {
                new MerchantProductRequest() { MerchantProductNo = merchantProductNo, Stock = stock }
            }
        };

        var result = await _client.UpdateProduct(update, ct);

        // check the http call
        if (!result.IsSuccessStatusCode)
        {
            _logger.LogError($"Could not update the product, the http call failed with: {result.Error}");
            return TryResult.Fail($"Http call failed with: {result.Error}");
        }

        // check validation errors
        if (result.Content?.ValidationErrors != null && result.Content.ValidationErrors.Any())
        {
            var error = result.Content.ValidationErrors.AsString();
            _logger.LogError($"Could not update the product, the api returned validation errors: {error}");
            return TryResult.Fail($"Api returned validation errors: {error}");
        }

        // check api response
        if (result.Content?.Content == null)
        {
            _logger.LogError("Could not update the product, the api returned an empty response");
            return TryResult.Fail("Could not update product, the api returned an empty response");
        }

        if (!result.Content.Success)
        {
            // check api response
            _logger.LogError($"Could not update the product, the api returned an error: {result.Content.Message}");
            return TryResult.Fail($"Api returned an error: {result.Content.Message}");
        }

        if (result.Content.Content.RejectedCount > 0)
        {
            // We are only ever updating a single product here, so I assume only a single product message
            _logger.LogError($"Could not update the product, the api rejected the update: {result.Content.Content.ProductMessages?.FirstOrDefault()}");
            return TryResult.Fail($"Api rejected the update: {result.Content.Content.ProductMessages?.FirstOrDefault()}");
        }

        return TryResult.Succeed();
    }
}
