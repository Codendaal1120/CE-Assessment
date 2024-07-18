using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Refit;

namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine;

public interface IChannelEngineClient
{
    [Get("/v2/orders")]
    Task<IApiResponse<ChannelCollectionResponse<MerchantOrderResponse>>> GetOrders(IReadOnlyCollection<OrderStatusView> statuses, CancellationToken ct);

    [Get("/v2/orders")]
    Task<IApiResponse<ChannelCollectionResponse<MerchantOrderResponse>>> GetOrders(OrderStatusView statuses, CancellationToken ct);

    [Patch("/v2/products")]
    Task<IApiResponse<ChannelResponse<ProductCreationResult>>> UpdateProduct(PatchMerchantProductDto update, CancellationToken ct);
}
