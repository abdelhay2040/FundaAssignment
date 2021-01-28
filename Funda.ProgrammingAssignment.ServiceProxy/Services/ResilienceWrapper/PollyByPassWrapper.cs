using System;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.ServiceProxy.Exceptions;
using Polly;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper
{
    //Just bypass polly. Useful for testing error scenarios
    public class PollyByPassWrapper : IResiliencePolicyWrapper
    {
        public async Task<TRes> Execute<TE, TRes>(Func<Task<IRestResponse<TE>>> funcToWrap, Func<IRestResponse<TE>, TRes> funcToExecuteOnSuccess)
        {
            var execRes = await Policy
                .NoOpAsync()
                .ExecuteAndCaptureAsync(funcToWrap);

            if (execRes.Outcome == OutcomeType.Successful)
            {
                var response = execRes.Result;
                return funcToExecuteOnSuccess.Invoke(response);
            }

            throw new RemoteApiInvocationException($"Exception while trying to invoke api method. See inner exception for further details", execRes.FinalException);
        }
    }
}