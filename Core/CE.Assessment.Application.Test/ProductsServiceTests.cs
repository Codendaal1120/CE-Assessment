using CE.Assessment.Application.Services;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using System.Net;

namespace CE.Assessment.Application.Test;

public class ProductsServiceTests : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Description("Test that the 'UpdateProduct' handles http failures")]
    public async Task UpdateProduct_Handles_HttpError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelResponse<ProductCreationResult>>(null, HttpStatusCode.InternalServerError);

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProduct' handles api failures")]
    public async Task UpdateProduct_Handles_ApiError()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<ProductCreationResult>() { Message = "Tests error", StatusCode = 400, Success = false });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProduct' handles a null response")]
    public async Task UpdateProduct_Handles_NullRespoonse()
    {
        // Arrange
        var apiResponse = await CreateApiResponse<ChannelResponse<ProductCreationResult>>(null, HttpStatusCode.OK);

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProduct' handles an empty response")]
    public async Task UpdateProduct_Handles_EmptyRespoonse()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<ProductCreationResult>() 
        { 
            StatusCode = 200, 
            Success = true,
            Content = null
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProduct' handles an update failure")]
    public async Task UpdateProduct_Handles_FailedUpdate()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<ProductCreationResult>()
        {
            StatusCode = 200,
            Success = true,
            Content = new ProductCreationResult() { AcceptedCount = 0, RejectedCount = 1 }
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(!updateResult.Success);
    }

    [Test, Description("Test that the 'UpdateProduct' correctly updates product")]
    public async Task UpdateProduct_Returns_results()
    {
        // Arrange
        var apiResponse = await CreateApiResponse(new ChannelResponse<ProductCreationResult>()
        {
            StatusCode = 200,
            Success = true,
            Content = new ProductCreationResult() { AcceptedCount = 1, RejectedCount = 0 }
        });

        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(apiResponse);

        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        Assert.That(updateResult.Success);
    }

    private ProductsService CreateProductsService(IChannelEngineClient? client = null)
    {
        client ??= Substitute.For<IChannelEngineClient>();

        return new ProductsService(new NullLogger<ProductsService>(), client);
    }
}