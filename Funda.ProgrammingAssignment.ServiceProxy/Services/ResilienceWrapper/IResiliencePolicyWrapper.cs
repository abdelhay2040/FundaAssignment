using System;
using System.Threading.Tasks;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper
{
    public interface IResiliencePolicyWrapper
    {
        Task<TRes> Execute<TE, TRes>(Func<Task<IRestResponse<TE>>> funcToWrap, Func<IRestResponse<TE>, TRes> funcToExecuteOnSuccess);
    }
}