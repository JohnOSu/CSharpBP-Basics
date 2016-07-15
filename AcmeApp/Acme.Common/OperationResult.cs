namespace Acme.Common
{
    /// <summary>
    /// Provides a generic result flag and message 
    /// useful as a method return type.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class OperationResult<T>
    {
        public OperationResult()
        {
        }

        public OperationResult(T result, string message) : this()
        {
            this.Result = result;
            this.Message = message;
        }

        public T Result { get; set; }
        public string Message { get; set; }
    }
}
