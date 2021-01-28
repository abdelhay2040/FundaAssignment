using System;
using System.Linq;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.DtoMapper
{
    //I could have use Automapper but for a simple scenario like this one i've preferred to stick to good old way ;)
    public class SimpleDtoMapper : IDtoMapper
    {
        public PagedApiResult<PropertyDto> MapToPropertyDtoApiResult(LocatieFeed data, int responseStatusCode, string responseDescription)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return new PagedApiResult<PropertyDto>
            {
                ResultCode = responseStatusCode,
                NumberOfPages = data.Paging.AantalPaginas,
                ActualPage = data.Paging.HuidigePagina,
                NextPage = data.Paging.VolgendeUrl,
                WasSuccessfull = true,
                TotalNumberOfResults = data.TotaalAantalObjecten,
                Data = data.Objects.Select(MapToPropertyDtoApiResult),
                ResultDescription = responseDescription
            };
        }

        public PropertyDto MapToPropertyDtoApiResult(ObjectInfo objectToMap)
        {
            return new PropertyDto()
            {
                Id = objectToMap.Id.ToString(),
                Address = objectToMap.Adres,
                City = objectToMap.Woonplaats,
                RealEstateAgentId = objectToMap.MakelaarId,
                RealEstateAgentName = objectToMap.MakelaarNaam
            };
        }
    }
}
