namespace Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults
{
    public class ApiResult<T> where T : class
    {
        public bool WasSuccessfull { get; set; }
        public string FailureReason { get; set; }
        public string ResultDescription { get; set; }

        public int ResultCode { get; set; }
        public T Data { get; set; }

        public static ApiResult<T> Failed(int resultCode, string responseStatusDescription,string errorMessage)
        {
            return new ApiResult<T>
            {
                WasSuccessfull = false,
                FailureReason = errorMessage,
                ResultCode = resultCode,
                ResultDescription = responseStatusDescription
            };
        }

        public static ApiResult<T> Success(T data, int resultCode = 200)
        {
            return new ApiResult<T>
            {
                WasSuccessfull = true,
                Data = data,
                ResultCode = resultCode
            };
        }
    }
}