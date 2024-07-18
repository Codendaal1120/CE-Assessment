using CommandLine;

namespace CE.Assessment.Options;

[Verb("getTopProducts", HelpText = "Gets the top N products sold")]
internal class GetTopNoptions
{
    [Option('c', "count", Required = true, HelpText = "The number of products to list")]
    public int Count { get; set; }
}
