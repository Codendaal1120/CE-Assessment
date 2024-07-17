namespace CE.Assessment.Application;

public class TryResult<T>
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public T? Value { get; set; }

    public static TryResult<T> Fail(string error)
    {
        return new TryResult<T> { Success = false, Error = error };
    }

    public static TryResult<T> Succeed(T val)
    {
        return new TryResult<T> { Success = true, Value = val };
    }
}
