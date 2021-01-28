using System.Collections.Generic;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.SearchTermsFormatter
{
    public interface ISearchTermsFormatter
    {
        string Format(IEnumerable<string> searchTerms);
    }
}