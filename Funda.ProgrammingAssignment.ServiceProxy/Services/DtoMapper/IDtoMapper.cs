using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.ServiceProxy.Model;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.DtoMapper
{
    public interface IDtoMapper
    {
        PagedApiResult<PropertyDto> MapToPropertyDtoApiResult(LocatieFeed responseData, int responseStatusCode, string responseDescription);
    }
}
