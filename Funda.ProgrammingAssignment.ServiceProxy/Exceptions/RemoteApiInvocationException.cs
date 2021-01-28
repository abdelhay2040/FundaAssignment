namespace Funda.ProgrammingAssignment.ServiceProxy.Exceptions
{
    public class RemoteApiInvocationException : System.Exception
    {
        public string MethodName { get; set; }
        private string Status { get; set; }
        private string ApiErrorMessage { get; set; }

        public RemoteApiInvocationException()
        {
        }

        public RemoteApiInvocationException(string message)
            : base(message)
        {
        }

        public RemoteApiInvocationException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        public RemoteApiInvocationException(string methodName, string status, string apiErrorMessage, System.Exception inner)
            : base($"Exception raised while invoking api method {methodName} - ResponseStatusCode: {status} - ApiMessage: {apiErrorMessage}", inner)
        {
            MethodName = methodName;
            Status = status;
            ApiErrorMessage = apiErrorMessage;
        }

        public RemoteApiInvocationException(string methodName, string status, string apiErrorMessage)
            : base($"Exception raised while invoking api method {methodName} - ResponseStatusCode: {status} - ApiMessage: {apiErrorMessage}")
        {
            MethodName = methodName;
            Status = status;
            ApiErrorMessage = apiErrorMessage;
        }
    }
}