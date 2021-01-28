using System.Collections.Generic;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Domain.Services.ResultMapper
{
    public interface IResultMapper
    {
        IEnumerable<RealEstateAgentSalesDto> MapToAgentSales(IEnumerable<PropertyDto> propertiesToMap);
    }
}