using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;

namespace Funda.ProgrammingAssignment.Domain.Services
{
    public class FakePropertiesRepository : IPropertiesRepository
    {
        public Task<IEnumerable<PropertyDto>> SearchPropertiesOnSale(IEnumerable<string> searchTerms)
        {
            var realEstateAgentFakePool = new Faker<RealEstateAgentSalesDto>()
                .RuleFor(a => a.AgentId, faker => faker.Random.Int(10000, 999999))
                .RuleFor(a => a.AgentName, faker => faker.Person.FullName)
                .Generate(20);

            var citiesFakePool = new[] { "Amsterdam", "Rotterdam", "Utrecht", "Eindhoven", "Groningen", "Breda", "Tilburg" };

            return Task.FromResult(
                new Faker<PropertyDto>()
                    .RuleFor(p => p.Id, faker => faker.Random.Guid().ToString())
                    .RuleFor(p => p.City, faker => faker.PickRandom(citiesFakePool))
                    .RuleFor(p => p.RealEstateAgentId, faker =>
                    {
                        faker.IndexVariable = faker.Random.Int(0, realEstateAgentFakePool.Count - 1);
                        return realEstateAgentFakePool.ElementAt(faker.IndexVariable).AgentId;

                    })
                    .RuleFor(p => p.RealEstateAgentName, faker => realEstateAgentFakePool.ElementAt(faker.IndexVariable).AgentName)
                    .GenerateBetween(10, 1000)
                    .AsEnumerable()
            );
        }
    }
}
