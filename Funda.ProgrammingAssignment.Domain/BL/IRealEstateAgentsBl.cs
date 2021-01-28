using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Domain.BL
{
    public interface IRealEstateAgentsBl
    {
        Task<GetTopSellersResult> GetTopSellers(IEnumerable<string> searchTerms, int howMany);
    }
}