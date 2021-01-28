using System.Collections.Generic;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.RequestBuilder
{
    public interface IRestRequestsBuilder
    {
        IRestClient BuildDefaultClient();

        IRestRequest BuildGet(List<Parameter> optionalParameters);

    }
}