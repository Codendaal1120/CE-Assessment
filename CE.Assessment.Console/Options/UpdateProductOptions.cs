using CommandLine;

namespace CE.Assessment.Options;

[Verb("update", HelpText = "Add file contents to the index.")]
internal class UpdateProductOptions
{
    [Option('q', "quantity", Required = true, HelpText = "Updates the given product stock")]
    public int Quantity { get; set; }
}
