using FluentAssertions;
using FluentAssertions.Common;
using Funda.ProgrammingAssignment.ServiceProxy.Model;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser;
using Xunit;

namespace Funda.ProgrammingAssignment.UnitTests.ServiceProxy
{
    //Those tests are trivial. They would make sense in case of a config file parser or similar. I've included them just to outline the "idea" of the tests. The tests for the AppSettings Api can be done in a similar way, checking that we are inquiring for the right key in config and checking that exceptions are thrown in case of null/invalid values
    public class FixedApiConfigurationParserUnitTests
    {
        private readonly FixedApiConfigurationParser _sut;

        public FixedApiConfigurationParserUnitTests()
        {
            _sut = new FixedApiConfigurationParser();
        }

        [Fact]
        public void ItCreatesANewBasicApiConfiguration()
        {
            var result = _sut.GetBasicConfiguration();
            result.Should().BeOfType<BasicApiConfiguration>();
            result.Should().NotBeNull();
        }

        [Fact]
        public void BaseUrlIsAsConstantValueInClass()
        {
            var result = _sut.GetBasicConfiguration();
            result.BaseUrl.Should().IsSameOrEqualTo(FixedApiConfigurationParser.DefaultBaseUrl);
        }

  

        [Fact]
        public void KeyIsAsConstantValueInClass()
        {
            var result = _sut.GetBasicConfiguration();
            result.Key.Should().IsSameOrEqualTo(FixedApiConfigurationParser.DefaultKey);
        }
    }
}