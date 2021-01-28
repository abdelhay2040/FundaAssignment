using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Funda.ProgrammingAssignment.Domain.BL;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.Domain.Services.ResultMapper;
using Moq;
using Xunit;

namespace Funda.ProgrammingAssignment.UnitTests.Domain
{
    public class RealEstateAgentsBlTests
    {
        private readonly RealEstateAgentsBl _sut;
        private readonly Mock<IPropertiesRepository> _mockedRepository;
        private readonly Mock<IResultMapper> _mockedResultMapper;

        private int _dummySearchResultNumber = 10;
        private readonly List<string> _dummySearchTerms = new List<string> { "Dummy", "Test" };

        public RealEstateAgentsBlTests()
        {
            _mockedRepository = new Mock<IPropertiesRepository>();
            _mockedResultMapper = new Mock<IResultMapper>();

            _sut = new RealEstateAgentsBl(_mockedRepository.Object, _mockedResultMapper.Object);
        }

        [Fact]
        public async Task InCaseOfEmptySearchTermsThrowsException()
        {

            await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.GetTopSellers(new List<string>(), _dummySearchResultNumber));
            _mockedRepository.Verify(service => service.SearchPropertiesOnSale(null), Times.Never());
        }

        [Fact]
        public async Task InCaseOfNullSearchTermsThrowsException()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.GetTopSellers(null, _dummySearchResultNumber));
            _mockedRepository.Verify(service => service.SearchPropertiesOnSale(null), Times.Never());
        }

        [Fact]
        public async Task InCaseOfLessThanZeroResultNumberThrowsOutOfRangeException()
        {
            var negativeHowMany = -1;
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _sut.GetTopSellers(_dummySearchTerms, negativeHowMany));
            _mockedRepository.Verify(service => service.SearchPropertiesOnSale(null), Times.Never());
        }

        [Fact]
        public async Task ItWasCalledTheRepositoryWithProvidedSearchParameters()
        {
            var fakeSearchParameters = new Faker().Lorem.Words();

            await _sut.GetTopSellers(fakeSearchParameters, _dummySearchResultNumber);
            _mockedRepository.Verify(service => service.SearchPropertiesOnSale(fakeSearchParameters), Times.Once);
        }

        [Fact]
        public async Task ItWasCalledTheResultMapper()
        {
            var fakeSearchParameters = new Faker().Lorem.Words();
            var dummyNumberOfReturnValues = 20;
            var fakeRepoResult = new Faker<PropertyDto>().Generate(5);
            _mockedRepository.Setup(repository => repository.SearchPropertiesOnSale(fakeSearchParameters))
                .ReturnsAsync(() => fakeRepoResult);


            await _sut.GetTopSellers(fakeSearchParameters, dummyNumberOfReturnValues);

            _mockedResultMapper.Verify(service => service.MapToAgentSales(fakeRepoResult), Times.Once);
        }

        [Fact]
        public async Task TheFinalResultIsOrderedByDescendingNumberOfPropertiesInSale()
        {

            var unorderedMappedResult = new Faker<RealEstateAgentSalesDto>()
                .RuleFor(r => r.AgentId, faker => faker.IndexFaker)
                .RuleFor(r => r.PropertiesOnSale, (faker, dto) => new Faker<PropertyDto>().Generate((int)dto.AgentId))
                .Generate(10);

            _mockedResultMapper.Setup(mapper => mapper.MapToAgentSales(It.IsNotNull<IEnumerable<PropertyDto>>())).Returns(unorderedMappedResult);

            var result = await _sut.GetTopSellers(_dummySearchTerms, _dummySearchResultNumber);

            result.AgentSales.Should().BeInDescendingOrder(dto => dto.AgentId);
        }

        [Fact]
        public async Task ItWillTakeTheFirstNOnlyElements()
        {

            var unorderedMappedResult = new Faker<RealEstateAgentSalesDto>()
                .RuleFor(r => r.AgentId, faker => faker.IndexFaker)
                .RuleFor(r => r.PropertiesOnSale, (faker, dto) => new Faker<PropertyDto>().Generate((int)dto.AgentId))
                .Generate(100);

            _mockedResultMapper.Setup(mapper => mapper.MapToAgentSales(It.IsNotNull<IEnumerable<PropertyDto>>())).Returns(unorderedMappedResult);

            var result = await _sut.GetTopSellers(_dummySearchTerms, _dummySearchResultNumber);

            result.AgentSales.Should().HaveCount(_dummySearchResultNumber);
        }
    }

}
