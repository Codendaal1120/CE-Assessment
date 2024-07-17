namespace CE.Assessment.Application;

public static class TryExtensions
{
    public static bool Try<T>(this TryResult<T> res, out T? outVal, out string error)
    {
        outVal = res.Value;
        error = res.Error ?? string.Empty;
        return res.Success;
    }
}
