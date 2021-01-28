using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.Domain.Services.ResultMapper;

namespace Funda.ProgrammingAssignment.Domain.BL
{
    //This class actually works as "orchestrator" as it takes the result from the repository and orchestrate the mapping of it to the caller
    public class RealEstateAgentsBl : IRealEstateAgentsBl
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IResultMapper _resultMapper;

        public RealEstateAgentsBl(IPropertiesRepository propertiesRepository, IResultMapper resultMapper)
        {
            _propertiesRepository = propertiesRepository;
            _resultMapper = resultMapper;
        }

        public async Task<GetTopSellersResult> GetTopSellers(IEnumerable<string> searchTerms, int howMany)
        {
            if (howMany <= 0) throw new ArgumentOutOfRangeException(nameof(howMany));
            if (searchTerms == null || !searchTerms.Any())
                throw new ArgumentException("Value cannot be an empty collection.", nameof(searchTerms));

            var stopwatch = Stopwatch.StartNew();
            var res = (await _propertiesRepository.SearchPropertiesOnSale(searchTerms)).ToList();
            stopwatch.Stop();
            return new GetTopSellersResult()
            {
                AgentSales = _resultMapper
                    .MapToAgentSales(res)
                    .OrderByDescending(rea => rea.NumberOfPropertiesInSale)
                    .Take(howMany),
                TotalNumberOfObjectsForRequest = res.Count,
                SearchTerms = searchTerms,
                ElapsedTime = stopwatch.Elapsed
            };
        }
    }
}

