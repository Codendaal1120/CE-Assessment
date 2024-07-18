using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Refit;
using System.Net;

namespace CE.Assessment.Application.Test;

public class OrderServiceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Description("Test that the 'GetTopNOrders' handles http failures")]
    public async Task GetTopNOrders_Handles_HttpError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelResponse<MerchantOrderResponse>>(null, HttpStatusCode.InternalServerError);

        var client = Substitute.For<IChannelEngineClient>();
        client.GetOrders(Arg.Any<OrderStatusView>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOrderService(client);

        // Act
        var orders = await os.GetTopNOrders(5, CancellationToken.None);

        // Assert
        Assert.That(!orders.Success);
    }

    [Test, Description("Test that the 'GetTopNOrders' handles api failures")]
    public async Task GetTopNOrders_Handles_ApiError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<MerchantOrderResponse>() { Message = "Tests error", ItemsPerPage = 10, StatusCode = 400, Success = false });

        var client = Substitute.For<IChannelEngineClient>();
        client.GetOrders(Arg.Any<OrderStatusView>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOrderService(client);

        // Act
        var orders = await os.GetTopNOrders(5, CancellationToken.None);

        // Assert
        Assert.That(!orders.Success);
    }

    [Test, Description("Test that the 'GetTopNOrders' handles a null response")]
    public async Task GetTopNOrders_Handles_NullRespoonse()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelResponse<MerchantOrderResponse>>(null, HttpStatusCode.OK);

        var client = Substitute.For<IChannelEngineClient>();
        client.GetOrders(Arg.Any<OrderStatusView>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOrderService(client);

        // Act
        var orders = await os.GetTopNOrders(5, CancellationToken.None);

        // Assert
        Assert.That(!orders.Success);
    }


    [Test, Description("Test that the 'GetTopNOrders' handles an empty response")]
    public async Task GetTopNOrders_Handles_EmptyRespoonse()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<MerchantOrderResponse>() 
        { 
            Count = 1, 
            TotalCount = 1,
            ItemsPerPage = 10, 
            StatusCode = 200, 
            Success = true ,
            Content = Array.Empty<MerchantOrderResponse>()
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.GetOrders(Arg.Any<OrderStatusView>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOrderService(client);

        // Act
        var orders = await os.GetTopNOrders(5, CancellationToken.None);

        // Assert
        Assert.That(orders.Success);
    }

    private MerchantOrderResponse CreateOrderResponse()
    {
        var oder = Substitute.For<MerchantOrderResponse>();
        return oder;
    }

    private OrderService CreateOrderService(IChannelEngineClient? client = null)
    {
        client ??= Substitute.For<IChannelEngineClient>();

        return new OrderService(new NullLogger<OrderService>(), client);
    }

    private async Task<ApiResponse<T>> CreateApiResponse<T>(T? content, HttpStatusCode? code = null)
    {
        code ??= content == null ? HttpStatusCode.NotFound : HttpStatusCode.OK;

        var resp = new HttpResponseMessage(code.Value);
        ApiException? ex = null;

        if (code != HttpStatusCode.OK)
        {
            ex = await ApiException.Create(new HttpRequestMessage(), HttpMethod.Get, resp, new RefitSettings());
        }

        return new ApiResponse<T>(resp, content, new RefitSettings(), ex);
    }
}