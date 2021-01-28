using System;
using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using Funda.ProgrammingAssignment.ServiceProxy.Model;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestBuilder;
using Moq;
using RestSharp;
using Xunit;
using Parameter = RestSharp.Parameter;

namespace Funda.ProgrammingAssignment.UnitTests.ServiceProxy
{
    public class RestRequestBuilderUnitTests
    {
        private readonly RestRequestsBuilder _sut;
        private readonly IApiConfigurationParser _dummyApiConfigParser;

        public RestRequestBuilderUnitTests()
        {
            var dummyApiConfigParserMock = new Mock<IApiConfigurationParser>();
            dummyApiConfigParserMock
                .Setup(x => x.GetBasicConfiguration())
                .Returns(new Faker<BasicApiConfiguration>()
                    .RuleFor(c => c.BaseUrl, faker => faker.Internet.Url())
                    .Generate());

            _dummyApiConfigParser = dummyApiConfigParserMock.Object;

            _sut = new RestRequestsBuilder(_dummyApiConfigParser);
        }

        [Fact]
        public void WhenAskingToBuildDefaultClient_ItCreatesNewRestClient()
        {
            var result = _sut.BuildDefaultClient();
            result.Should().BeOfType<RestClient>();
            result.Should().NotBeNull();
        }


        [Fact]
        public void WhenAskingToBuildDefaultClient_TheCreatedREstClientHaveTheSameUrlAsTheConfiguration()
        {
            var result = _sut.BuildDefaultClient();
            result.BaseUrl.Should().BeEquivalentTo(new Uri(_dummyApiConfigParser.GetBasicConfiguration().BaseUrl));
        }


        [Fact]
        public void WhenBuildingGetRequest_ItCreatesNewRestRequest()
        {
            var result = _sut.BuildGet(It.IsAny<List<Parameter>>());
            result.Should().BeOfType<RestRequest>();
            result.Should().NotBeNull();
        }

        [Fact]
        public void WhenBuildingGetRequest_ItSetsHttpMethodToGet()
        {
            var result = _sut.BuildGet(It.IsAny<List<Parameter>>());
            result.Method.Should().Be(Method.GET);
        }


        [Fact]
        public void WhenBuildingGetRequest_ResourceIsSetWithKeyAndJson()
        {
            var result = _sut.BuildGet(It.IsAny<List<Parameter>>());
            var resource = result.Resource;

            var resourcePathToCheck =
                $"{_dummyApiConfigParser.GetBasicConfiguration().BaseUrl}/json/{_dummyApiConfigParser.GetBasicConfiguration().Key}/";

            resource.Should().BeEquivalentTo(resourcePathToCheck);
        }


        [Fact]
        public void WhenBuildingGetRequest_OptionalParametersAreAddedToRequest()
        {
            var parametersToAdd = new List<Parameter>()
            {
                new Parameter("Test1", "Value", ParameterType.QueryString),
                new Parameter("Test2", "Value", ParameterType.QueryString)
            };

            var result = _sut.BuildGet(parametersToAdd);
            var parameters = result.Parameters;

            parameters.Should().BeEquivalentTo(parametersToAdd);
        }
    }
}