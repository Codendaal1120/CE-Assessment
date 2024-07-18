namespace CE.Assessment.Application;

public class TryResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }

    public static TryResult Fail(string error)
    {
        return new TryResult { Success = false, Error = error };
    }

    public static TryResult Succeed()
    {
        return new TryResult { Success = true };
    }
}

public class TryResult<T> : TryResult
{
    public T? Value { get; set; }

    public static new TryResult<T> Fail(string error)
    {
        return new TryResult<T> { Success = false, Error = error };
    }

    public static TryResult<T> Succeed(T val)
    {
        return new TryResult<T> { Success = true, Value = val };
    }
}