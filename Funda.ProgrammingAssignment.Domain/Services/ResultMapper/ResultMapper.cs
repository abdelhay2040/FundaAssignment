using System.Collections.Generic;
using System.Linq;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Domain.Services.ResultMapper
{
    public class ResultMapper : IResultMapper
    {
        public IEnumerable<RealEstateAgentSalesDto> MapToAgentSales(IEnumerable<PropertyDto> res)
        {
            return res
                .GroupBy(r => r.RealEstateAgentId)
                .Select(g => new RealEstateAgentSalesDto
                {
                    AgentId = g.Key,
                    AgentName = g.FirstOrDefault()?.RealEstateAgentName,
                    PropertiesOnSale = g.ToList()
                });
        }
    }
}