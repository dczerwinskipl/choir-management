namespace NEvo.Benchmarks.Monads
{
    public class ServiceResult
    {
        public Exception? Exception { get; }
        public bool Success { get; }

        public ServiceResult()
        {
            Success = true;
        }

        public ServiceResult(Exception exc)
        {
            Exception = exc;
            Success = false;
        }
    }
}


