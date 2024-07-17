using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Refit;

namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine;

public interface IChannelEngineClient
{
    [Get("/v2/orders")]
    Task<IApiResponse<ChannelResponse<MerchantOrderResponse>>> GetOrders(IReadOnlyCollection<OrderStatusView> statuses, CancellationToken ct);

    [Get("/v2/orders")]
    Task<IApiResponse<ChannelResponse<MerchantOrderResponse>>> GetOrders(OrderStatusView statuses, CancellationToken ct);
}
