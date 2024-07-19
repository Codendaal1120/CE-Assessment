using CE.Assessment.Application.Services;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Refit;
using System.Collections;
using System.Net;

namespace CE.Assessment.Application.Test;

public class OfferServiceTests : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    public class OfferTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
                "UpdateProductStock_Handles_HttpError",
                "Test that the 'UpdateProductStock' handles http failures",
                CreateApiResponse<ChannelResponse<Dictionary<string, List<string>>>>(null, HttpStatusCode.InternalServerError),
                false)
                .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
                "UpdateProductStock_Handles_ApiError",
                "Test that the 'UpdateProductStock' handles api failures",
                CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>() { Message = "Tests error", StatusCode = 400, Success = false }),
                false)
                .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
               "UpdateProductStock_Handles_NullRespoonse",
               "Test that the 'UpdateProductStock' handles a null response",
               CreateApiResponse<ChannelResponse<Dictionary<string, List<string>>>>(null, HttpStatusCode.OK),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
               "UpdateProductStock_Handles_EmptyRespoonse",
               "Test that the 'UpdateProductStock' handles an empty response",
               CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
               {
                   StatusCode = 200,
                   Success = true,
                   Content = null
               }),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
               "UpdateProductStock_Handles_FailedUpdate",
               "Test that the 'UpdateProductStock' handles an update failure",
               CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
               {
                   StatusCode = 200,
                   Success = true,
               }),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
               "UpdateProductStock_Handles_ValidationErrors",
               "Test that the 'UpdateProductStock' handles an validation errors",
               CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
               {
                   StatusCode = 200,
                   Success = true,
                   ValidationErrors = new Dictionary<string, IReadOnlyCollection<object>> {
                        { "error1", new List<string> { "error-1.1", "error-1.2" } },
                        { "error2", new List<string> { "error-2.1", "error-2.2" } }
                    }
                }),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
               "UpdateProductStock_Handles_UpdateErrors",
               "Test that the 'UpdateProductStock' handles an update errors",
               CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
               {
                   StatusCode = 200,
                   Success = true,
                   Content = new Dictionary<string, List<string>> { { "1", ["Test message"] } }
               }),
               false)
               .ToTestCase();

            yield return new TestCase<ChannelResponse<Dictionary<string, List<string>>>>(
               "UpdateProductStock_Returns_results",
               "Test that the 'UpdateProductStock' correctly updates product stock",
               CreateApiResponse(new ChannelResponse<Dictionary<string, List<string>>>()
               {
                   StatusCode = 200,
                   Success = true,
                   Content = new Dictionary<string, List<string>>()
               }),
               true)
               .ToTestCase();
        }
    }

    [TestCaseSource(typeof(OfferTestCases))]
    public async Task<bool> UpdateProductStockTest(ApiResponse<ChannelResponse<Dictionary<string, List<string>>>> response, string desc)
    {
        // Arrange
        TestContext.Out.WriteLine(desc);

        var client = CreateClient(response);
        var os = CreateOfferService(client);

        // Act
        var updateResult = await os.UpdateProductStock("1", 5, CancellationToken.None);

        // Assert
        return updateResult.Success;
    }

    /*********/

    private OffersService CreateOfferService(IChannelEngineClient? client = null)
    {
        client ??= Substitute.For<IChannelEngineClient>();

        return new OffersService(new NullLogger<OffersService>(), client);
    }

    private IChannelEngineClient CreateClient(ApiResponse<ChannelResponse<Dictionary<string, List<string>>>> response)
    {
        var client = Substitute.For<IChannelEngineClient>();
        client.UpdateProductStock(Arg.Any<IReadOnlyCollection<MerchantOfferStockUpdateRequest>>(), Arg.Any<CancellationToken>()).Returns(response);

        return client;
    }
}