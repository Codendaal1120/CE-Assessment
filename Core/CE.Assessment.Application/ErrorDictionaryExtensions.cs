namespace CE.Assessment.Application.Services;

public static class ErrorDictionaryExtensions
{
    public static string AsString(this Dictionary<string, List<string>> errors)
    {
        var errorList = new List<string>();

        foreach (var kvp in errors)
        {
            var errorText = $"{kvp.Key} errors :";
            errorText += string.Join(',', kvp.Value);
            errorList.Add(errorText);
        }

        return string.Join(';', errorList);
    }

    public static string AsString(this Dictionary<string, IReadOnlyCollection<object>> errors)
    {
        var errorList = new List<string>();

        foreach (var kvp in errors)
        {
            var errorText = $"{kvp.Key} errors :";
            errorText += string.Join(',', kvp.Value);
            errorList.Add(errorText);
        }

        return string.Join(';', errorList);
    }
}
