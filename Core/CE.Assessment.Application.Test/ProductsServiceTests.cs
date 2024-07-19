using CE.Assessment.Application.Services;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Refit;
using System.Collections;
using System.Net;

namespace CE.Assessment.Application.Test;

public class ProductsServiceTests : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    public class ProducTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
                "UpdateProduct_Handles_HttpError",
                "Test that the 'UpdateProduct' handles http failures",
                CreateApiResponse<ChannelResponse<ProductCreationResult>>(null, HttpStatusCode.InternalServerError),
                false)
                .ToTestCase();

            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
               "UpdateProduct_Handles_ApiError",
               "Test that the 'UpdateProduct' handles api failures",
               CreateApiResponse(new ChannelResponse<ProductCreationResult>() { Message = "Tests error", StatusCode = 400, Success = false }),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
                "UpdateProduct_Handles_NullRespoonse",
                "Test that the 'UpdateProduct' handles a null response",
                CreateApiResponse<ChannelResponse<ProductCreationResult>>(null, HttpStatusCode.OK),
                false)
                .ToTestCase();

            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
              "UpdateProduct_Handles_EmptyRespoonse",
              "Test that the 'UpdateProduct' handles an empty response",
              CreateApiResponse(new ChannelResponse<ProductCreationResult>()
              {
                  StatusCode = 200,
                  Success = true,
                  Content = null
              }),
              false)
              .ToTestCase();

            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
               "UpdateProduct_Handles_FailedUpdate",
               "Test that the 'UpdateProduct' handles an update failure",
               CreateApiResponse(new ChannelResponse<ProductCreationResult>()
               {
                   StatusCode = 200,
                   Success = true,
                   Content = new ProductCreationResult() { AcceptedCount = 0, RejectedCount = 1 }
               }),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
              "UpdateProduct_Handles_ValidationErrors",
              "Test that the 'UpdateProduct' handles an validation errors",
              CreateApiResponse(new ChannelResponse<ProductCreationResult>()
              {
                  StatusCode = 200,
                  Success = true,
                  Content = null,
                  ValidationErrors = new Dictionary<string, IReadOnlyCollection<object>> {
                    { "error1", new List<string> { "error-1.1", "error-1.2" } },
                    { "error2", new List<string> { "error-2.1", "error-2.2" } }
                }
              }),
              false)
              .ToTestCase();

            yield return new TestCase<ChannelResponse<ProductCreationResult>>(
             "UpdateProduct_Returns_results",
             "Test that the 'UpdateProduct' correctly updates product",
             CreateApiResponse(new ChannelResponse<ProductCreationResult>()
             {
                 StatusCode = 200,
                 Success = true,
                 Content = new ProductCreationResult() { AcceptedCount = 1, RejectedCount = 0 }
             }),
             true)
             .ToTestCase();
        }
    }

    [TestCaseSource(typeof(ProducTestCases))]
    public async Task<bool> UpdateProductStockTest(ApiResponse<ChannelResponse<ProductCreationResult>> response, string desc)
    {
        // Arrange
        TestContext.Out.WriteLine(desc);

        var client = CreateClient(response);
        var ps = CreateProductsService(client);

        // Act
        var updateResult = await ps.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        return updateResult.Success;
    }

    private ProductsService CreateProductsService(IChannelEngineClient? client = null)
    {
        client ??= Substitute.For<IChannelEngineClient>();

        return new ProductsService(new NullLogger<ProductsService>(), client);
    }

    private IChannelEngineClient CreateClient(ApiResponse<ChannelResponse<ProductCreationResult>> response)
    {
        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProduct(Arg.Any<PatchMerchantProductDto>(), Arg.Any<CancellationToken>()).Returns(response);

        return client;
    }
}