namespace ResponseWrapperLibrary.Wrappers;

public class ResponseWrapper : IResponseWrapper
{
    public List<string> Messages { get; set; } = [];
    public bool IsSuccessful { get; set; }

    #region Fail Synchronously

    public static IResponseWrapper Fail()
        => new ResponseWrapper { IsSuccessful = false };

    public static IResponseWrapper Fail(string message)
        => new ResponseWrapper { IsSuccessful = false, Messages = [message] };

    public static IResponseWrapper Fail(List<string> messages)
        => new ResponseWrapper { IsSuccessful = false, Messages = messages };

    #endregion

    #region Fail Asynchronously

    public static Task<IResponseWrapper> FailAsync()
        => Task.FromResult(Fail());

    public static Task<IResponseWrapper> FailAsync(string message)
        => Task.FromResult(Fail(message));

    public static Task<IResponseWrapper> FailAsync(List<string> messages)
        => Task.FromResult(Fail(messages));

    #endregion

    #region Success Synchronously

    public static IResponseWrapper Success()
        => new ResponseWrapper { IsSuccessful = true };

    public static IResponseWrapper Success(string message)
        => new ResponseWrapper { IsSuccessful = true, Messages = [message] };

    public static IResponseWrapper Success(List<string> messages)
        => new ResponseWrapper { IsSuccessful = true, Messages = messages };

    #endregion

    #region Success Asynchronously

    public static Task<IResponseWrapper> SuccessAsync()
        => Task.FromResult(Success());

    public static Task<IResponseWrapper> SuccessAsync(string message)
        => Task.FromResult(Success(message));

    public static Task<IResponseWrapper> SuccessAsync(List<string> messages)
        => Task.FromResult(Success(messages));

    #endregion
}

public class ResponseWrapper<T> : ResponseWrapper, IResponseWrapper<T>
{
    public T Data { get; set; }

    public ResponseWrapper()
    {

    }

    #region Fail Synchronously

    public new static ResponseWrapper<T> Fail()
        => new() { IsSuccessful = false };

    public new static ResponseWrapper<T> Fail(string message)
        => new() { IsSuccessful = false, Messages = [message] };

    public new static ResponseWrapper<T> Fail(List<string> messages)
        => new() { IsSuccessful = false, Messages = messages };

    #endregion

    #region Fail Asynchronously

    public new static Task<ResponseWrapper<T>> FailAsync()
        => Task.FromResult(Fail());

    public new static Task<ResponseWrapper<T>> FailAsync(string message)
        => Task.FromResult(Fail(message));

    public new static Task<ResponseWrapper<T>> FailAsync(List<string> messages)
        => Task.FromResult(Fail(messages));

    #endregion

    #region Success Synchronously

    public new static ResponseWrapper<T> Success()
        => new() { IsSuccessful = true };

    public new static ResponseWrapper<T> Success(string message)
        => new() { IsSuccessful = true, Messages = [message] };

    public new static ResponseWrapper<T> Success(List<string> messages)
        => new() { IsSuccessful = true, Messages = messages };

    public static ResponseWrapper<T> Success(T data)
        => new() { Data = data, IsSuccessful = true };

    public static ResponseWrapper<T> Success(T data, string message)
        => new() { Data = data, IsSuccessful = true, Messages = [message] };

    public static ResponseWrapper<T> Success(T data, List<string> messages)
        => new() { Data = data, IsSuccessful = true, Messages = messages };

    #endregion

    #region Success Asynchronously

    public new static Task<ResponseWrapper<T>> SuccessAsync()
        => Task.FromResult(Success());

    public new static Task<ResponseWrapper<T>> SuccessAsync(string message)
        => Task.FromResult(Success(message));

    public new static Task<ResponseWrapper<T>> SuccessAsync(List<string> messages)
        => Task.FromResult(Success(messages));

    public static Task<ResponseWrapper<T>> SuccessAsync(T data)
        => Task.FromResult(Success(data));

    public static Task<ResponseWrapper<T>> SuccessAsync(T data, string message)
        => Task.FromResult(Success(data, message));

    public static Task<ResponseWrapper<T>> SuccessAsync(T data, List<string> messages)
        => Task.FromResult(Success(data, messages));

    #endregion
}
