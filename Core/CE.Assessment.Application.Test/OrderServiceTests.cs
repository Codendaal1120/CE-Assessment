using CE.Assessment.Application.Services;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using System.Net;

namespace CE.Assessment.Application.Test;

public class OrderServiceTests : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Description("Test that the 'GetTopNOrders' handles http failures")]
    public async Task GetTopNOrders_Handles_HttpError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelCollectionResponse<MerchantOrderResponse>>(null, HttpStatusCode.InternalServerError);

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
        var apiResponse = await CreateApiResponse(new ChannelCollectionResponse<MerchantOrderResponse>() { Message = "Tests error", ItemsPerPage = 10, StatusCode = 400, Success = false });

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
        var apiResponse = await CreateApiResponse<ChannelCollectionResponse<MerchantOrderResponse>>(null, HttpStatusCode.OK);

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
        var apiResponse = await CreateApiResponse(new ChannelCollectionResponse<MerchantOrderResponse>() 
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

    [Test, Description("Test that the 'GetTopNOrders' correctly sorts results")]
    public async Task GetTopNOrders_Returns_results()
    {
        // Arrange
        var responseOrders = new[]
        {
            CreateOrderResponse(1, new[] 
            { 
                CreateLineItem(10, "p1"),
                CreateLineItem(10, "p2"), 
                CreateLineItem(10, "p3") 
            }),

             CreateOrderResponse(2, new[]
            {
                CreateLineItem(10, "p2"),
                CreateLineItem(10, "p3"),
                CreateLineItem(10, "p4")
            }),

            CreateOrderResponse(3, new[]
            {
                CreateLineItem(10, "p5"),
                CreateLineItem(10, "p6"),
                CreateLineItem(10, "p1")
            }),

            CreateOrderResponse(4, new[]
            {
                CreateLineItem(10, "p6"),
                CreateLineItem(10, "p3"),
                CreateLineItem(10, "p2")
            }),
            
            CreateOrderResponse(5, new[]
            {
                CreateLineItem(10, "p5"),
                CreateLineItem(10, "p4"),
                CreateLineItem(10, "p1"),
                CreateLineItem(10, "p3")
            }),
        };

        var apiResponse = await CreateApiResponse(new ChannelCollectionResponse<MerchantOrderResponse>()
        {
            Count = 1,
            TotalCount = 1,
            ItemsPerPage = 10,
            StatusCode = 200,
            Success = true,
            Content = responseOrders
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.GetOrders(Arg.Any<OrderStatusView>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOrderService(client);

        // Act
        var orders = await os.GetTopNOrders(5, CancellationToken.None);

        // Assert
        Assert.That(orders.Success);
        Assert.That(orders.Value!.First().ProductNo == $"product-p3", "Product 'p3' should have the most sales (40)");
    }

    private MerchantOrderLineResponse CreateLineItem(int quantity, string nameSuffix)
    {
        return new MerchantOrderLineResponse()
        {
            Id = 1,
            Description = $"desc-{nameSuffix}",
            CancellationRequestedQuantity = 1,
            ChannelProductNo = $"product-{nameSuffix}",
            MerchantProductNo = $"product-{nameSuffix}",
            Condition = Condition.NEW,
            FeeFixed = 0,
            FeeRate = 0,
            IsFulfillmentByMarketplace = false,
            OriginalFeeFixed = 0,
            Quantity = quantity,
            Status = 0,
            StockLocation = new MerchantStockLocationResponse() { Id = 1, Name = "Test location" },
            UnitPriceInclVat = 0,
            VatRate = 0
        };
    }

    private MerchantOrderResponse CreateOrderResponse(int id, IReadOnlyCollection<MerchantOrderLineResponse> lines)
    {
        var address = new MerchantAddressResponse()
        { 
            Gender = Gender.NOT_APPLICABLE
        };

        var mo = new MerchantOrderResponse()
        {
            Id = id,
            ChannelOrderSupport = OrderSupport.NONE,            
            CurrencyCode = "EUR",
            Email = "chris@test.local",
            IsBusinessOrder = true,
            OrderDate = DateTime.UtcNow,
            OrderFee = 0,
            OriginalOrderFee = 0,
            OriginalSubTotalFee = 0,
            OriginalTotalFee = 0,
            Status = OrderStatusView.IN_PROGRESS,
            BillingAddress = address,
            ShippingAddress = address,
            ShippingCostsInclVat = 0,
            SubTotalFee = 0,
            TotalFee = 0,
            TotalInclVat = 0,
            Lines = lines.ToList()
        };

        return mo;
    }

    private OrderService CreateOrderService(IChannelEngineClient? client = null)
    {
        client ??= Substitute.For<IChannelEngineClient>();

        return new OrderService(new NullLogger<OrderService>(), client);
    }

    
}
