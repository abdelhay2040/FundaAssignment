using System.Collections.Generic;
using System.Linq;

namespace Funda.ProgrammingAssignment.Domain.Common.Dto
{
    public class RealEstateAgentSalesDto
    {
        public long AgentId { get; set; }
        public string AgentName { get; set; }
        public IEnumerable<PropertyDto> PropertiesOnSale { get; set; }
        public int NumberOfPropertiesInSale => PropertiesOnSale.Count();

        public IEnumerable<string> CitiesWhereIsSelling => PropertiesOnSale.Select(c => c.City).Distinct().OrderBy(c => c);
    }
}