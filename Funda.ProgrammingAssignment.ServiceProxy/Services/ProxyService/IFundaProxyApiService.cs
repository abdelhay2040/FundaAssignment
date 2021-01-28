using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ProxyService
{
    public interface IFundaProxyApiService
    {
        Task<PagedApiResult<PropertyDto>> GetPropertiesOnSale(IEnumerable<string> searchTerms, int currentPage, int pageSize);
    }
}