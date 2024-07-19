using NUnit.Framework;
using Refit;

namespace CE.Assessment.Application.Test;

public class TestCase<T>
{
    public string Description { get; init; }
    public string Name { get; init; }
    public ApiResponse<T> Response { get; init; }
    public bool ExpectedResult { get; init; }

    public TestCase(string name, string description, ApiResponse<T> response, bool expectedResult)
    {
        Description = description;
        Name = name;
        ExpectedResult = expectedResult;
        Response = response;
    }

    public TestCaseData ToTestCase()
    {
        return new TestCaseData(Response, Description).Returns(ExpectedResult).SetArgDisplayNames(Name).SetDescription(Description);
    }
}
