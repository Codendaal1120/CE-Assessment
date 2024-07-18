using Refit;
using System.Net;

namespace CE.Assessment.Application.Test;

public abstract class TestBase
{
    protected async Task<ApiResponse<T>> CreateApiResponse<T>(T? content, HttpStatusCode? code = null)
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