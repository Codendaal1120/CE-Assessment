using CE.Assessment.Application.Services;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using System.Net;

namespace CE.Assessment.Application.Test;

public class OfferServiceTests : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Description("Test that the 'UpdateProductStock' handles http failures")]
    public async Task UpdateProductStock_Handles_HttpError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelResponse<Dictionary<string, List<string>>>>(null, HttpStatusCode.InternalServerError);

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' handles api failures")]
    public async Task UpdateProductStock_Handles_ApiError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>() { Message = "Tests error", StatusCode = 400, Success = false });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' handles a null response")]
    public async Task UpdateProductStock_Handles_NullRespoonse()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelResponse<Dictionary<string, List<string>>>>(null, HttpStatusCode.OK);

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' handles an empty response")]
    public async Task UpdateProductStock_Handles_EmptyRespoonse()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>() 
        { 
            StatusCode = 200, 
            Success = true,
            Content = null
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' handles an update failure")]
    public async Task UpdateProductStock_Handles_FailedUpdate()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
        {
            StatusCode = 200,
            Success = true,
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' handles an validation errors")]
    public async Task UpdateProductStock_Handles_ValidationErrors()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
        {
            StatusCode = 200,
            Success = true,
            ValidationErrors = new Dictionary<string, IReadOnlyCollection<object>> { 
                { "error1", new List<string> { "error-1.1", "error-1.2" } },
                { "error2", new List<string> { "error-2.1", "error-2.2" } }
            }
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' handles an update errors")]
    public async Task UpdateProductStock_Handles_UpdateErrors()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
        {
            StatusCode = 200,
            Success = true,
            Content = new Dictionary<string, List<string>> { { "1", ["Test message"] } }
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProductStock' correctly updates product stock")]
    public async Task UpdateProductStock_Returns_results()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
        {
            StatusCode = 200,
            Success = true,
            Content = new Dictionary<string, List<string>>()
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(updateResult.Success);
    }

    private OffersService CreateOfferService(IChannelEngineClient? client = null)
    {
        client ??= Substitute.For<IChannelEngineClient>();

        return new OffersService(new NullLogger<OffersService>(), client);
    }
}