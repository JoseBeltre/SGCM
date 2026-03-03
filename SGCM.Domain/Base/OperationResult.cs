namespace SGCM.Domain.Base
{
    public class OperationResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;

        protected OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static OperationResult Success(string message = "Operation completed successfully.")
            => new OperationResult(true, message);

        public static OperationResult Failure(string message)
            => new OperationResult(false, message);
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; private set; }

        private OperationResult(bool isSuccess, string message, T? data)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static OperationResult<T> Success(T data, string message = "Operation completed successfully.")
            => new OperationResult<T>(true, message, data);

        public static OperationResult<T> Failure(string message)
            => new OperationResult<T>(false, message, default);
    }
}