using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi;
using Funda.ProgrammingAssignment.ServiceProxy.Services.DtoMapper;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ProxyService;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestBuilder;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper;
using Funda.ProgrammingAssignment.ServiceProxy.Services.SearchTermsFormatter;
using Moq;
using RestSharp;
using Xunit;

namespace Funda.ProgrammingAssignment.UnitTests.ServiceProxy
{
    public class FundaApiProxyServiceUnitTests
    {
        private readonly FundaApiProxyService _sut;
        private readonly Mock<IRestRequestsBuilder> _mockedRequestsBuilder;
        private readonly Mock<IDtoMapper> _mockedDtoMapper;
        private readonly Mock<IResiliencePolicyWrapper> _mockedResiliencePolicyWrapper;
        private readonly Mock<ISearchTermsFormatter> _searchTermsFormatter;

        public FundaApiProxyServiceUnitTests()
        {
            _mockedDtoMapper = new Mock<IDtoMapper>();
            _mockedRequestsBuilder = new Mock<IRestRequestsBuilder>();
            _mockedResiliencePolicyWrapper = new Mock<IResiliencePolicyWrapper>();
            _searchTermsFormatter = new Mock<ISearchTermsFormatter>();

            _sut = new FundaApiProxyService(_mockedRequestsBuilder.Object, _mockedResiliencePolicyWrapper.Object, _mockedDtoMapper.Object, _searchTermsFormatter.Object);
        }

        [Fact]
        public async Task ItCreatesADefaultClient()
        {
            await _sut.GetPropertiesOnSale(It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>());
            _mockedRequestsBuilder.Verify(builder => builder.BuildDefaultClient(), Times.Once);
        }

        [Fact]
        public async Task TheSearchTermsAreFormatted()
        {
            var dummyTerms = new List<string> { "test1", "test2", "test3" };

            await _sut.GetPropertiesOnSale(dummyTerms, It.IsAny<int>(), It.IsAny<int>());
            _searchTermsFormatter.Verify(f => f.Format(dummyTerms), Times.Once);
        }

        [Fact]
        public async Task ItCreatesARequestWithAllParametersSet()
        {
            var fakeTerms = "thoseAreTheTermsFormatted";
            var dummyCurrentPage = 11;
            var dummyPageSize = 20;
            _searchTermsFormatter.Setup(formatter => formatter.Format(It.IsAny<IEnumerable<string>>()))
                .Returns(fakeTerms);


            await _sut.GetPropertiesOnSale(It.IsAny<IEnumerable<string>>(), dummyCurrentPage, dummyPageSize);

            _mockedRequestsBuilder.Verify(f => f.BuildGet(
                It.Is<List<Parameter>>(list =>
                     list.Any(parameter =>
                            parameter.Name == FundaApiProxyService.QueryTypeName &&
                            parameter.Type == ParameterType.QueryString &&
                            (string)parameter.Value == FundaApiProxyService.KoopTypeName) &&
                        list.Any(parameter =>
                            parameter.Name == FundaApiProxyService.QueryParametersName &&
                            parameter.Type == ParameterType.QueryString && (string)parameter.Value == fakeTerms) &&
                        list.Any(parameter =>
                            parameter.Name == FundaApiProxyService.PageQueryStringName &&
                            parameter.Type == ParameterType.QueryString && (int)parameter.Value == dummyCurrentPage) &&
                        list.Any(parameter =>
                            parameter.Name == FundaApiProxyService.PageSizeQueryStringName &&
                            parameter.Type == ParameterType.QueryString && (int)parameter.Value == dummyPageSize)
                )
                ), Times.Once);
        }
    }
}