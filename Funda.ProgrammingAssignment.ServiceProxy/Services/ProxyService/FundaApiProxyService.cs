using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi;
using Funda.ProgrammingAssignment.ServiceProxy.Services.DtoMapper;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestBuilder;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper;
using Funda.ProgrammingAssignment.ServiceProxy.Services.SearchTermsFormatter;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ProxyService
{
    //This class executes the API call to the Funda service, containing all the nitty-gritty details of how it works (serialization, parameter names etc...)
    public class FundaApiProxyService : IFundaProxyApiService
    {
        public const string QueryTypeName = "type";
        public const string KoopTypeName = "koop";
        public const string QueryParametersName = "zo";
        public const string PageQueryStringName = "page";
        public const string PageSizeQueryStringName = "pagesize";

        private readonly IRestRequestsBuilder _requestsBuilder;
        private readonly IResiliencePolicyWrapper _wrapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly ISearchTermsFormatter _searchTermsFormatter;

        public FundaApiProxyService(IRestRequestsBuilder requestsBuilder, IResiliencePolicyWrapper wrapper, IDtoMapper dtoMapper, ISearchTermsFormatter searchTermsFormatter)
        {
            _requestsBuilder = requestsBuilder;
            _wrapper = wrapper;
            _dtoMapper = dtoMapper;
            _searchTermsFormatter = searchTermsFormatter;
        }

        public async Task<PagedApiResult<PropertyDto>> GetPropertiesOnSale(IEnumerable<string> searchTerms, int currentPage, int pageSize)
        {
            var client = _requestsBuilder.BuildDefaultClient();

            var terms = _searchTermsFormatter.Format(searchTerms);
            var request = _requestsBuilder.BuildGet(new List<Parameter>
            {
                new Parameter(QueryTypeName, KoopTypeName,ParameterType.QueryString),
                new Parameter(QueryParametersName, terms, ParameterType.QueryString),
                new Parameter(PageQueryStringName, currentPage, ParameterType.QueryString),
                new Parameter(PageSizeQueryStringName, pageSize, ParameterType.QueryString),
            });

            return await _wrapper.Execute(
                () => client.ExecuteGetTaskAsync<LocatieFeed>(request),
                response => response.IsSuccessful
                    ? _dtoMapper.MapToPropertyDtoApiResult(response.Data, response.StatusCode.GetHashCode(), response.StatusDescription)
                    : PagedApiResult<PropertyDto>.Failed(response.StatusCode.GetHashCode(), response.StatusDescription, response.ErrorMessage));
        }
    }
}
