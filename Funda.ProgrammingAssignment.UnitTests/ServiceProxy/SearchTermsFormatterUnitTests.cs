using System.Collections.Generic;
using FluentAssertions;
using Funda.ProgrammingAssignment.ServiceProxy.Services.SearchTermsFormatter;
using Xunit;

namespace Funda.ProgrammingAssignment.UnitTests.ServiceProxy
{
    public class SearchTermsFormatterUnitTests
    {
        private readonly SearchTermsFormatter _sut;

        public SearchTermsFormatterUnitTests()
        {
            _sut = new SearchTermsFormatter();
        }

        [Fact]
        public void IfProvidedANullListReturnsAnEmptyString()
        {
            var result = _sut.Format(null);
            result.Should().BeEmpty();
        }

        [Fact]
        public void IfProvidedAnEmptyListReturnsAnEmptyString()
        {
            var result = _sut.Format(new List<string>());
            result.Should().BeEmpty();
        }

        [Fact]
        public void GivingOneValueReturnTheValuePreceededAndSuccedeedBySlash()
        {
            var singletermValue = "ASingleTerm";

            var result = _sut.Format(new List<string>{singletermValue});
            result.Should().StartWith("/");
            result.Should().EndWith("/");
            result.Should().BeEquivalentTo($"/{singletermValue}/");
        }

        [Fact]
        public void GivingMoreThanOneValueItConcatenatesThemAll()
        {
            var first = "Test1";
            var second = "Test2";
            var third = "Test3";

            var result = _sut.Format(new List<string> { first,second,third });
            result.Should().StartWith("/");
            result.Should().EndWith("/");
            result.Should().BeEquivalentTo($"/{first}/{second}/{third}/");
        }
    }
}