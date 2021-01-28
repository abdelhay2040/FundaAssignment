using System;
using System.Net;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.ServiceProxy.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper
{
    //The goal of this class is to mask the "complexity" of the retry policy to the caller. And make the code more testable
    public class PollyResiliencePolicyWrapper : IResiliencePolicyWrapper
    {
        private readonly ILogger _logger;

        public PollyResiliencePolicyWrapper(ILogger logger)
        {
            _logger = logger;
        }

        //In case of exception (i should have used more specific TRANSIENT exceptions of the API, but for simplicity right now i'll just stick to catch them all) or an Unauthorized response code (the one that the API throws in case of over-requests) the resiliency policy will wait for a non-linear amount of time (2,4,8,16,32,and finally 64 seconds) before retrying the request.
        public async Task<TRes> Execute<TE, TRes>(Func<Task<IRestResponse<TE>>> funcToWrap, Func<IRestResponse<TE>, TRes> funcToExecuteOnSuccess)
        {
            var execRes = await Policy
                .HandleResult<IRestResponse<TE>>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                .Or<Exception>()
                .WaitAndRetryAsync(
                    6,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetryAsync: (result, span, retryCount) =>
                    {
                        _logger.LogInformation($"**** Retrying caused by {result?.Result.StatusDescription} - Will try again in {span} seconds. Sorry for the delay!");
                        return Task.CompletedTask;
                    }
                ).ExecuteAndCaptureAsync(funcToWrap);

            if (execRes.Outcome == OutcomeType.Successful)
            {
                var response = execRes.Result;
                return funcToExecuteOnSuccess.Invoke(response);
            }

            throw new RemoteApiInvocationException($"Exception while trying to invoke api method. See inner exception for further details", execRes.FinalException);
        }
    }
}