using System;
using System.Collections.Generic;

namespace Funda.ProgrammingAssignment.Domain.Common.Dto
{
    public class GetTopSellersResult
    {
        public IEnumerable<string> SearchTerms { get; set; }
        public int TotalNumberOfObjectsForRequest { get; set; }
        public IEnumerable<RealEstateAgentSalesDto> AgentSales { get; set; }
        public TimeSpan ElapsedTime { get; 
            set; }
    }
}