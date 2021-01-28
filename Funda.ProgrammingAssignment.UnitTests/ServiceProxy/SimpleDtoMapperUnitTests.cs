using System.Linq;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using FluentAssertions.Common;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi;
using Funda.ProgrammingAssignment.ServiceProxy.Services.DtoMapper;
using Xunit;

namespace Funda.ProgrammingAssignment.UnitTests.ServiceProxy
{
    public class SimpleDtoMapperUnitTests
    {
        private readonly SimpleDtoMapper _sut;
        private readonly LocatieFeed _fakeSearchResult;

        private const int _dummyResponseCode = 200;
        private const string _dummyResponseDescription = "TestDescription";

        public SimpleDtoMapperUnitTests()
        {
            _sut = new SimpleDtoMapper();
            _fakeSearchResult = new Faker<LocatieFeed>()
                .RuleFor(r => r.TotaalAantalObjecten, faker => faker.Random.Int(0, 1000))
                .RuleFor(r => r.Metadata, () => new Faker<MetadataInfo>().Generate())
                .RuleFor(r => r.Paging, () => new Faker<PaginationInfo>()
                    .RuleFor(p => p.HuidigePagina, faker => faker.IndexFaker)
                    .RuleFor(p => p.VolgendeUrl, faker => faker.Internet.Url())
                    .RuleFor(p => p.AantalPaginas, faker => faker.Random.Int(1, 30))
                    .Generate())
                .RuleFor(r => r.Objects, () => new Faker<ObjectInfo>()
                    .RuleFor(o => o.Id, faker => faker.Random.Guid())
                    .RuleFor(o => o.Adres, faker => faker.Address.StreetAddress())
                    .RuleFor(o => o.GlobalId, faker => faker.Random.Int())
                    .RuleFor(o => o.Woonplaats, faker => faker.Address.City())
                    .RuleFor(o => o.MakelaarId, faker => faker.Random.Int())
                    .RuleFor(o => o.MakelaarNaam, faker => faker.Person.FullName)
                    .Generate(25))
                .Generate();
        }

        [Fact]
        public void ItCreatesANewPagedApiResultForPropertyDto()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);
            result.Should().BeOfType<PagedApiResult<PropertyDto>>();
            result.Should().NotBeNull();
        }

        [Fact]
        public void ItShouldHaveResultCodeAndDescriptionSet()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.ResultCode.Should().IsSameOrEqualTo(_dummyResponseCode);
            result.ResultDescription.Should().IsSameOrEqualTo(_dummyResponseDescription);
        }

        [Fact]
        public void ItShouldBeMarkedAsSuccessfull()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.WasSuccessfull.Should().BeTrue();
        }

        [Fact]
        public void ItShouldHaveSameNumberOfPages()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.NumberOfPages.Should().IsSameOrEqualTo(_fakeSearchResult.Paging.AantalPaginas);
        }


        [Fact]
        public void ItShouldHaveSameActualPage()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.ActualPage.Should().IsSameOrEqualTo(_fakeSearchResult.Paging.HuidigePagina);
        }


        [Fact]
        public void ItShouldHaveSameNextPage()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.NextPage.Should().IsSameOrEqualTo(_fakeSearchResult.Paging.VolgendeUrl);
        }


        [Fact]
        public void ItShouldHaveSameTotalNumberOfResults()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.TotalNumberOfResults.Should().IsSameOrEqualTo(_fakeSearchResult.TotaalAantalObjecten);
        }

        [Fact]
        public void ItShouldMapObjectsToPropertyDto()
        {
            var result = _sut.MapToPropertyDtoApiResult(_fakeSearchResult, _dummyResponseCode, _dummyResponseDescription);

            result.Data.Count().Should().IsSameOrEqualTo(_fakeSearchResult.Objects.Count());

            foreach (var property in result.Data)
            {
                var compareToFakeResult = _fakeSearchResult.Objects.FirstOrDefault(p => p.Id.ToString() == property.Id);

                compareToFakeResult.Should().NotBeNull();

                property.Id.Should().BeEquivalentTo(compareToFakeResult.Id.ToString());
                property.Address.Should().BeEquivalentTo(compareToFakeResult.Adres);
                property.City.Should().BeEquivalentTo(compareToFakeResult.Woonplaats);
                property.RealEstateAgentId.Should().Be(compareToFakeResult.MakelaarId);
                property.RealEstateAgentName.Should().BeEquivalentTo(compareToFakeResult.MakelaarNaam);
            }
        }
    }
}